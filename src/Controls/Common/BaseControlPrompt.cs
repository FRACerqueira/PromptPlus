// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Buffers;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace PromptPlusLibrary.Controls.Common
{
    /// <summary>
    /// Represents the base class for all control prompts.
    /// </summary>
    /// <typeparam name="T">The type of the control prompt, allowing for specific implementations to define 
    /// their own types while still inheriting common functionality from this base class.
    /// </typeparam>
    /// <param name="isWidget">Indicates whether the control is a widget.</param>
    /// <param name="console">The console instance to operate on.</param>
    /// <param name="promptConfig">The configuration for the prompt.</param>
    /// <param name="baseControlOptions">The options for the base control.</param>
    internal abstract class BaseControlPrompt<T>(bool isWidget, IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions)
    {
        private string? _errorMessage;
        private bool _showTooltipValue = true;
        private (int StartLeft, int StartTop) _screenPosition = (0, 0);
        private readonly BufferScreen _bufferScreen = new();
        private readonly CultureInfo _cultureInfo = Thread.CurrentThread.CurrentCulture;
        private static readonly FrozenDictionary<SymbolType, (string value, string unicode)> _symbols = InitSymbols();
        // Instance-level (NOT static): both hold resource strings localized per PromptConfig.
        // DefaultCulture, which is set per control instance (see PromptConfig.DefaultCulture ->
        // PromptPlusResources.Culture). A static cache would freeze whichever culture the FIRST
        // control of this closed generic type happened to run under, and every later control
        // (even one configured for a different culture) would keep reading that stale value.
        private string[]? _EmacsTooltips;
        private string[]? _EmacsTooltipsReadonly;
        private CompositeFormat? _resizeWarningFormat;
        private int _lastKnownWidth;
        private int _lastKnownHeight;
        private int _renderedPhysicalRows;
        private int _lastCursorTopOffset;
        private int _lastRenderWidth;
        private int _lastRenderHeight;
        private (int StartLeft, int StartTop) _anchorScreenPosition = (0, 0);
        private volatile bool _pendingResize;

        // Base-owned read-only answer viewport: an emacs buffer plus the last loaded text, reloaded
        // only when the text changes so horizontal navigation is preserved between frames. The last
        // effective viewport width and terminal height are tracked as well so a terminal resize
        // (width OR height, since either can move/re-anchor the frame) re-anchors the horizontal
        // scroll (ToHome) without reloading the content when only the geometry changed.
        private EmacsConsoleBuffer? _answerViewportBuffer;
        private string? _answerViewportLastText;
        private int _answerViewportLastWidth = -1;
        private int _answerViewportLastHeight = -1;
        private bool _resizePositionHandled;

        /// <summary>
        /// Minimum terminal height (in rows) required by <see cref="RenderBuffer"/>
        /// before the small-terminal safeguard kicks in.
        /// </summary>
        private const int MinSafeRenderHeight = 10;

        /// <summary>
        /// Minimum terminal width (in columns) required by <see cref="RenderBuffer"/>
        /// before the small-terminal safeguard kicks in.
        /// </summary>
        private const int MinSafeRenderWidth = 80;

        /// <summary>
        /// Ellipsis marker rendered when the terminal supports Unicode.
        /// </summary>
        public const string UnicodeEllipsis = "…";

        /// <summary>
        /// Ellipsis marker rendered when the terminal does not support Unicode.
        /// </summary>
        public const string AsciiEllipsis = "_";

        /// <summary>
        /// Polling interval, in milliseconds, used by <see cref="ShowResizeWarningAndWait"/>
        /// while the terminal is too small to render. The value is OS-blocking
        /// (so it costs effectively zero CPU between ticks) and short enough
        /// that any size change is reflected within one human-perceivable
        /// frame (~60 checks per second).
        /// </summary>
        private const int ResizeWarningPollIntervalMs = 16;

        /// <summary>
        /// Gets whether a terminal resize is pending and the control should re-render.
        /// When processing keys inside <see cref="TryResult"/>, prefer <see cref="ReadNextKey"/>
        /// which packages this flag directly in the returned <see cref="KeyPressResult"/>.
        /// </summary>
        protected bool IsPendingResize => _pendingResize;

        /// <summary>
        /// Indicates whether this is a "Live" control that renders automatically by firing
        /// simulated key events (e.g. ProgressBar spinner/progress updates) instead of only
        /// rendering in response to real user input.
        /// </summary>
        /// <remarks>
        /// Live controls render continuously, so they can attempt an automatic render in the
        /// short window between the terminal physically changing size and the <c>SizeChanged</c>
        /// event flipping <see cref="IsPendingResize"/>. During that window the cached anchor
        /// (<c>_screenPosition</c>) is stale, and rendering there leaves artifacts on screen.
        /// When this returns <c>true</c>, the main render loop polls the terminal dimensions
        /// every iteration and routes any width/height change through the full resize relayout
        /// (which clears the previous footprint), regardless of event timing. Interactive
        /// controls keep the default (<c>false</c>) and are unaffected.
        /// </remarks>
        protected virtual bool IsLiveAutoRenderControl => false;

        /// <summary>
        /// Requests a full resize relayout on the next render-loop iteration. Intended for
        /// "Live" controls (see <see cref="IsLiveAutoRenderControl"/>) that detect a terminal
        /// size change by polling before the <c>SizeChanged</c> event arrives. Setting this
        /// flag makes <see cref="ReadNextKey"/> report <see cref="KeyPressResult.IsResize"/>
        /// so the control's <c>TryResult</c> loop breaks and the main loop performs the
        /// relayout that clears the previous footprint.
        /// </summary>
        protected void RequestResizeRelayout()
        {
            _pendingResize = true;
        }

        /// <summary>
        /// Carries the complete outcome of a single <see cref="ReadNextKey"/> call so that
        /// derived controls can handle resize, cancellation, and the actual key in one place
        /// without polling multiple separate flags.
        /// </summary>
        /// <param name="Key">The key that was pressed, or <c>default</c> when <see cref="IsResize"/> or <see cref="IsCancelled"/> is <c>true</c>.</param>
        /// <param name="IsResize">True when a pending terminal resize caused <see cref="WaitKeypress"/> to return early. The key value must be ignored.</param>
        /// <param name="IsCancelled">True when the <see cref="CancellationToken"/> was signalled. The key value must be ignored.</param>
        protected readonly record struct KeyPressResult(ConsoleKeyInfo Key, bool IsResize, bool IsCancelled);

        /// <summary>
        /// Gets a value indicating whether the control is a widget.
        /// </summary>
        public bool IsWidget => isWidget;

        /// <summary>
        /// Gets the console handler used by the console.
        /// </summary>
        public IConsole ConsoleHandler => console;

        /// <summary>
        /// Gets the prompt configuration associated with the control.
        /// </summary>
        public PromptConfig ConfigPrompt => promptConfig;

        /// <summary>
        /// Gets the options properties associated with the control.
        /// </summary>  
        public BaseControlOptions OptionsControl => baseControlOptions;

        /// <summary>
        /// Gets or sets the result of the control prompt, which may be null if the prompt has not yet completed.
        /// </summary>
        public ResultPrompt<T>? ResultCtrl { get; set; }

        /// <summary>
        /// Gets the validation error message, or an empty string if there is no error.
        /// </summary>
        public string ValidateError => _errorMessage ?? string.Empty;

        /// <summary>
        /// Gets a value indicating whether the tooltip is shown.
        /// </summary>  
        public bool IsShowTooltip => _showTooltipValue;

        /// <summary>
        /// Shows the control prompt and blocks until it completes, returning the result.
        /// </summary>
        public void Show()
        {
            Run(CancellationToken.None);
        }

        /// <summary>
        /// Runs the control prompt and blocks until it completes, returning the result.
        /// </summary>
        /// <param name="stoptoken">A cancellation token to stop the prompt.</param>
        /// <returns>The result of the control prompt.</returns>
        public ResultPrompt<T> Run(CancellationToken stoptoken = default)
        {
            _showTooltipValue = OptionsControl.ShowTooltipValue;


            using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(stoptoken, console.CancelToken);

            bool oldcursor = console.CursorVisible;
            Color oldforecolor = console.ForegroundColor;
            Color oldbackcolor = console.BackgroundColor;
            Thread.CurrentThread.CurrentCulture = ConfigPrompt.DefaultCulture;
            console.CursorVisible = false;
            try
            {

                console.SizeChanged += ConsoleHandler_SizeChanged;

                InitControl(cts.Token);

                _screenPosition = console.GetCursorPosition();
                _anchorScreenPosition = _screenPosition;
                _lastKnownWidth = console.Width;
                _lastKnownHeight = console.Height;
                _renderedPhysicalRows = 0;
                _lastCursorTopOffset = 0;
                _lastRenderWidth = console.Width;
                _lastRenderHeight = console.Height;

                do
                {
                    bool templateReady = false;

                    // Live controls (auto-render via simulated key events) can reach this point
                    // in the brief window AFTER the terminal physically changed size but BEFORE
                    // the SizeChanged event flips _pendingResize. Rendering there uses a stale
                    // anchor and leaves artifacts (especially on height changes). Detect the size
                    // change proactively here and force the full resize relayout below. Interactive
                    // controls opt out (IsLiveAutoRenderControl == false) and are unaffected.
                    if (IsLiveAutoRenderControl
                        && !_pendingResize
                        && (console.Width != _lastKnownWidth || console.Height != _lastKnownHeight))
                    {
                        _pendingResize = true;
                    }

                    if (_pendingResize)
                    {
                        _pendingResize = false;

                        // -- Resize handling that preserves external content ---------------
                        //
                        // CRITICAL CONSTRAINT: The control must NEVER touch content that was
                        // written BEFORE the control started (terminal history, other output).
                        // Only the rows the control itself rendered into may be cleared.
                        //
                        // Challenges after a terminal resize:
                        //   • Width SHRINK ? previously-rendered logical lines that fit on one
                        //     row now WRAP into multiple physical rows (footprint GROWS).
                        //   • Width GROW   ? previously-wrapped lines collapse back (footprint
                        //     SHRINKS). The leftover rows below were already cleared by reflow
                        //     but may contain shifted text fragments.
                        //   • Height SHRINK ? terminal auto-scrolls content UP.
                        //   • Height GROW  ? content stays put.
                        //
                        // Order of operations (do NOT reorder):
                        //   1. Capture old state (top, dims, left, rendered physical rows).
                        //   2. Compute the OLD frame's footprint reflowed at the NEW width
                        //      using the buffer's previously rendered content (OriginalBuffer)
                        //      and the OLD start column. This is how many rows the stale
                        //      content occupies on screen NOW.
                        //   3. Determine the post-resize position of the control's frame.
                        //      PRIMARY: ask the terminal where the cursor is (it was placed
                        //      inside our frame by the last RenderBuffer at a KNOWN offset).
                        //      FALLBACK: heuristic over heightShrink when the cursor reading
                        //      is unavailable or out of range.
                        //   4. Build the new template to know the new line count.
                        //   5. Decide the new anchored position.
                        //   6. Clear the UNION of old-reflowed footprint and new footprint
                        //      (both bounded to the control's own area).
                        //   7. Render at the new position.

                        int oldStartTop = _screenPosition.StartTop;
                        int oldStartLeft = _screenPosition.StartLeft;
                        int oldHeight = _lastKnownHeight;
                        int oldPhysicalRows = _renderedPhysicalRows;
                        int newHeight = console.Height;
                        int newWidth = console.Width;

                        // === 1. Compute OLD frame's footprint reflowed at NEW width ===
                        // Use the OLD start column (which is where each logical line begins).
                        // This is the number of physical rows the stale content occupies
                        // right now, after the terminal has performed its own reflow.
                        int oldFrameRowsReflowed = CalculateReflowedRows(
                            _bufferScreen, oldStartLeft, newWidth);
                        if (oldFrameRowsReflowed <= 0)
                        {
                            oldFrameRowsReflowed = Math.Max(1, oldPhysicalRows);
                        }

                        // === 2. Compute terminal scroll-up due to height shrink ===
                        // When height decreases, the terminal scrolls content up to keep the
                        // cursor visible. We must track that movement so the anchor follows.
                        //
                        // PRIMARY SOURCE OF TRUTH: ask the terminal where the cursor is NOW.
                        // The cursor was placed inside the frame by the last RenderBuffer at
                        // (savedcursor.Left, oldStartTop + _lastCursorTopOffset). After the
                        // resize the terminal has moved that physical row by whatever amount
                        // its own scroll/reflow logic decided. Reading the cursor back gives
                        // us the EXACT post-resize position of that specific frame row, which
                        // is far more reliable than any heuristic over heightShrink. Subtract
                        // the offset to obtain the real frame top.
                        //
                        // FALLBACK: only when the cursor reading is clearly invalid (terminal
                        // returned a negative/out-of-range value, e.g. when GetCursorPosition
                        // is not supported) do we fall back to the original heuristic.
                        int scrollUp = 0;
                        int currentFrameTop = oldStartTop;

                        bool cursorAnchorUsable = false;
                        bool heightShrank = newHeight < oldHeight;
                        if (console.Profile.IsTerminal)
                        {
                            // Determine the frame's REAL current top by reading the parked cursor,
                            // for BOTH grow and shrink. This independence from the height-delta is
                            // essential for RAPID resizes: when the terminal cycles shrink?grow
                            // faster than we process frames, the intermediate sizes are skipped and
                            // we only observe the size BEFORE and AFTER. In between, the terminal may
                            // have scrolled the frame UP (shrink) and then pulled it back DOWN from
                            // scrollback (grow). Classifying by the net delta (grow ? "frame didn't
                            // move") is then wrong: it renders the new frame at a stale row and leaves
                            // the previous one behind as a DUPLICATE.
                            //
                            // The cursor was parked at a KNOWN offset inside the frame after the last
                            // render (frame top-left / offset 0 for Live controls, PromptCursor for
                            // interactive controls), so reading it back yields the true frame top no
                            // matter what path the terminal took to reach the new size.
                            (int _, int actualCursorTop) = console.GetCursorPosition();
                            int candidateFrameTop = actualCursorTop - _lastCursorTopOffset;
                            // Sanity: the recovered top must be within the visible viewport
                            // and non-negative. If the terminal returned garbage we ignore it.
                            if (actualCursorTop >= 0 && actualCursorTop < newHeight
                                && candidateFrameTop >= 0 && candidateFrameTop < newHeight)
                            {
                                currentFrameTop = candidateFrameTop;
                                scrollUp = Math.Max(0, oldStartTop - currentFrameTop);
                                cursorAnchorUsable = true;
                            }
                        }

                        if (!cursorAnchorUsable)
                        {
                            // Deterministic recovery (Live controls + fallback): on a height shrink
                            // the terminal absorbs the delta first from the blank rows BELOW the
                            // frame, and only scrolls the frame up for the remainder. This matches
                            // observed terminal behavior and does not depend on an (unreliable)
                            // cursor read, so the frame stays anchored instead of drifting.
                            scrollUp = 0;
                            if (heightShrank)
                            {
                                int heightShrink = oldHeight - newHeight;
                                // Rows that were below the bottom of the OLD frame in the OLD view
                                int oldFrameBottom = oldStartTop + oldPhysicalRows - 1;
                                int rowsBelowFrame = Math.Max(0, (oldHeight - 1) - oldFrameBottom);
                                // The terminal absorbs the shrink first from the area below, then
                                // by scrolling content up.
                                scrollUp = Math.Max(0, heightShrink - rowsBelowFrame);
                                // Cannot scroll more than the anchor's distance from the top.
                                scrollUp = Math.Min(scrollUp, oldStartTop);
                            }
                            currentFrameTop = Math.Max(0, oldStartTop - scrollUp);
                        }

                        // === 4. Rebuild template at the new dimensions ===
                        _bufferScreen.Reset();
                        BufferTemplate(_bufferScreen);
                        templateReady = true;

                        int newLineCount = Math.Max(0, _bufferScreen.CurrentLineCount);

                        // === 5. Decide where to render ===
                        // CRITICAL: use _screenPosition (the control's CURRENT real on-screen
                        // position) as the base, NOT _anchorScreenPosition.
                        //
                        // The "anchor" can be stale because RenderBuffer performs its own
                        // auto-scroll when content overflows the bottom of the terminal during
                        // normal rendering (see the !_resizePositionHandled branch in
                        // RenderBuffer). That scroll moves _screenPosition up but does not
                        // touch the anchor. As a result, after a height GROW following an
                        // overflow-scroll, the anchor would be BELOW the real frame top and
                        // the next render would land below the existing frame, leaving the
                        // previous content stranded above.
                        //
                        // currentFrameTop already encodes (oldStartTop - scrollUp), which is
                        // exactly where the control's frame lives on screen right now.
                        int newStartLeft = oldStartLeft;
                        if (newStartLeft >= newWidth)
                        {
                            newStartLeft = 0;
                        }

                        // Physical rows the NEW frame will occupy at the new width
                        int newFrameRows = CalculateReflowedRowsFromCurrent(
                            _bufferScreen, newStartLeft, newWidth);
                        if (newFrameRows <= 0)
                        {
                            newFrameRows = Math.Max(1, newLineCount);
                        }

                        int newStartTop = currentFrameTop;
                        bool didPreScroll = false;
                        if (newStartTop + newFrameRows > newHeight)
                        {
                            // Frame too tall to fit from currentFrameTop downward.
                            //
                            // DO NOT clamp newStartTop upward — that would land the control
                            // inside rows that may belong to EXTERNAL content (terminal
                            // history above the control), and the subsequent clear loop
                            // would erase it.
                            //
                            // CORRECT STRATEGY: pre-scroll the terminal by emitting newlines
                            // at the bottom of the new viewport. The terminal scrolls
                            // EVERYTHING up by that many rows — external content moves into
                            // scrollback (preserved by the terminal itself, not overwritten),
                            // the stale frame moves up too, and our anchor follows.
                            //
                            // IMPORTANT: when this path runs, we are in the combined
                            // width+height shrink scenario where the OLD frame's exact
                            // current position on screen CANNOT be determined accurately
                            // (the unknown reflow of external content above the control
                            // shifts the stale frame by an unknown amount). After the
                            // pre-scroll we therefore deliberately AVOID trying to clear
                            // the stale frame at its assumed location — we only clear the
                            // rows we are about to render into. Any stale fragment that
                            // survives elsewhere will at worst be visible on screen, but
                            // we will NEVER overwrite external content we don't own.
                            int totalScrollNeeded = (newStartTop + newFrameRows) - newHeight;
                            int maxScroll = newStartTop;
                            int resizeScroll = Math.Min(totalScrollNeeded, maxScroll);

                            if (resizeScroll > 0 && console.Profile.IsTerminal)
                            {
                                console.SetCursorPosition(0, newHeight - 1);
                                for (int i = 0; i < resizeScroll; i++)
                                {
                                    console.WriteRaw(Environment.NewLine, console.CurrentStyle, true);
                                }
                                newStartTop -= resizeScroll;
                                didPreScroll = true;
                            }
                            else if (resizeScroll == 0)
                            {
                                // Cannot scroll at all (frame already at row 0). Truncation
                                // is unavoidable — leave newStartTop at 0.
                                newStartTop = 0;
                            }
                        }
                        newStartTop = Math.Max(0, newStartTop);

                        _screenPosition = (newStartLeft, newStartTop);
                        // Keep the anchor in sync with the real position so future logic that
                        // may consult it does not see a stale value.
                        _anchorScreenPosition = _screenPosition;

                        // === 6. Clear strategy ===
                        // Two paths, chosen to never touch external content:
                        //
                        // (a) NORMAL (no pre-scroll): the stale frame is at currentFrameTop
                        //     and the new frame is at newStartTop (typically the same row).
                        //     We can safely clear the UNION of the two footprints because
                        //     both are control-owned regions.
                        //
                        // (b) PRE-SCROLLED: we just emitted newlines from the bottom of the
                        //     viewport to scroll the terminal. The stale frame's exact
                        //     post-scroll position is UNKNOWN when external content above
                        //     the control reflowed horizontally (that reflow shifts the
                        //     stale frame by an unknown amount). To be safe, clear ONLY the
                        //     rows we will render into — never speculate about the stale
                        //     position because guessing wrong erases external content.
                        int clearTop;
                        int clearBottom;
                        if (didPreScroll)
                        {
                            clearTop = newStartTop;
                            clearBottom = Math.Min(newHeight - 1, newStartTop + newFrameRows - 1);
                        }
                        else
                        {
                            int oldRegionTop = currentFrameTop;
                            int oldRegionBottom = currentFrameTop + oldFrameRowsReflowed - 1;
                            int newRegionTop = newStartTop;
                            int newRegionBottom = newStartTop + newFrameRows - 1;

                            clearTop = Math.Max(0, Math.Min(oldRegionTop, newRegionTop));
                            clearBottom = Math.Min(newHeight - 1,
                                                   Math.Max(oldRegionBottom, newRegionBottom));

                            // For Live auto-render controls, the recovered frame top can be wrong
                            // when GetCursorPosition() is unreliable (common in Windows Terminal /
                            // IDE-integrated terminals). In that case the stale frame sits at the
                            // CACHED anchor (oldStartTop) which the recovered region may exclude,
                            // leaving it on screen as garbage. Extend the clear to also cover the
                            // cached footprint. Both regions belong to this control, so widening
                            // the clear here never touches external content.
                            if (IsLiveAutoRenderControl)
                            {
                                int cachedTop = Math.Max(0, oldStartTop);
                                int cachedBottom = Math.Min(newHeight - 1,
                                                            oldStartTop + Math.Max(1, oldPhysicalRows) - 1);
                                clearTop = Math.Min(clearTop, cachedTop);
                                clearBottom = Math.Max(clearBottom, cachedBottom);
                            }
                        }

                        for (int row = clearTop; row <= clearBottom; row++)
                        {
                            console.SetCursorPosition(0, row);
                            console.WriteRaw("", console.CurrentStyle, true);
                        }

                        // === 7. Position cursor for rendering ===
                        console.SetCursorPosition(newStartLeft, newStartTop);

                        // === Update tracking state ===
                        _lastKnownWidth = newWidth;
                        _lastKnownHeight = newHeight;
                        _renderedPhysicalRows = newFrameRows;

                        // Signal that resize already handled positioning - RenderBuffer
                        // should not perform additional scroll adjustments this frame
                        _resizePositionHandled = true;
                    }

                    if (!templateReady)
                    {
                        BufferTemplate(_bufferScreen);
                    }
                    RenderBuffer(_bufferScreen);
                    // Snapshot the terminal dimensions ONCE after rendering. Each console.Width/
                    // console.Height access takes a lock and performs an OS syscall, and they are
                    // consumed several times below; reading them once avoids redundant syscalls and
                    // guarantees a consistent view of the size for this frame's tracking state.
                    int renderedWidth = console.Width;
                    int renderedHeight = console.Height;
                    // Update the physical-rows tracking AFTER rendering so the next resize
                    // knows the actual on-screen footprint of the control's frame.
                    // After RenderBuffer, CurrentBuffer is swapped into OriginalBuffer.
                    _renderedPhysicalRows = CalculateReflowedRows(
                        _bufferScreen, _screenPosition.StartLeft, renderedWidth);
                    // Capture the cursor's offset from the frame top: RenderBuffer ends by
                    // placing the cursor at (savedcursor.Left, _screenPosition.StartTop +
                    // savedcursor.Top). We need that offset on the next resize to recover
                    // the real frame top via GetCursorPosition() - offset, regardless of
                    // any terminal scroll or external reflow that happened in between.
                    _lastCursorTopOffset = _bufferScreen.PromptCursor?.Top ?? 0;
                    if (IsLiveAutoRenderControl)
                    {
                        // Live controls have no user-facing cursor. Park it at the frame TOP-LEFT
                        // with a KNOWN offset (0). This makes GetCursorPosition() on the next resize
                        // return the frame's REAL current row no matter how the terminal scrolled or
                        // shifted content (bottom-absorb on shrink, scrollback-pull on grow, etc.),
                        // so the recovered frame top is always accurate — proven necessary because
                        // instrumentation showed the default cursor (frame bottom) drifts by the
                        // frame height with a recorded offset of 0.
                        console.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop);
                        _lastCursorTopOffset = 0;
                    }
                    _resizePositionHandled = false;
                    // Remember the dimensions this frame was actually rendered at, so the finish
                    // path can tell whether a resize happened AFTER the last render (and only then
                    // realign). Without this, an unconditional realign would relocate the frame on
                    // a normal finish and re-introduce the gap/duplication bugs.
                    _lastRenderWidth = renderedWidth;
                    _lastRenderHeight = renderedHeight;

                    if (cts.Token.IsCancellationRequested)
                    {
                        ResultCtrl = new ResultPrompt<T>(default!, true);
                        break;
                    }
                } while (!cts.Token.IsCancellationRequested && !isWidget && (!TryResult(cts.Token) || _pendingResize));

                if (!cts.Token.IsCancellationRequested && !isWidget)
                {
                    // Live controls (auto-render via simulated key events) can reach the finish
                    // path at the very moment a terminal resize happened but before the render
                    // loop processed it (e.g. progress hits 100% during a height shrink). The
                    // cached _screenPosition can then point at a stale row while the terminal has
                    // already scrolled the frame — the finish frame draws at the wrong place and
                    // the previous frame is left behind as garbage.
                    //
                    //
                    // A resize can land between the last render and the finish (e.g. progress hits
                    // 100% at the same instant). If so, realign to the frame's real on-screen
                    // position and clear the previous footprint before drawing the finish frame.
                    // Gate on dimensions actually changing SINCE THE LAST RENDER (tracked in
                    // _lastRenderWidth/Height) — an unconditional realign would relocate the frame
                    // on a normal finish and re-introduce the blank-gap / duplicated-frame bugs.
                    if (IsLiveAutoRenderControl
                        && (console.Width != _lastRenderWidth || console.Height != _lastRenderHeight))
                    {
                        RealignFrameAfterResize();
                        // The old footprint was erased on screen; reset the buffer so the finish
                        // render repaints every line fresh instead of diffing against content we
                        // just cleared.
                        _bufferScreen.Reset();
                    }
                    else if (IsLiveAutoRenderControl)
                    {
                        // No resize since the last render: the frame is already at the correct,
                        // known position (_screenPosition). Prevent RenderBuffer's overflow
                        // pre-scroll from relocating the finish frame based on the taller finish
                        // template — instrumentation showed this pushed the finish frame down by
                        // its own row count, producing the "finish drawn below with a gap" bug.
                        _resizePositionHandled = true;
                    }

                    for (int row = 0; row <= _bufferScreen.OriginalLineCount; row++)
                    {
                        console.SetCursorPosition(0, _screenPosition.StartTop + row);
                        console.WriteRaw("", console.CurrentStyle, true);
                    }
                    console.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop);
                    if (FinishTemplate(_bufferScreen))
                    {
                        RenderBuffer(_bufferScreen);
                    }
                }
                else if (cts.Token.IsCancellationRequested && !isWidget && IsLiveAutoRenderControl)
                {
                    // The run was cancelled externally (CancellationToken/Ctrl+C) so the finish
                    // block above is skipped. Live controls can render a tall multi-line frame
                    // (e.g. a paginated task list); leaving that footprint on screen produces
                    // leftover rows and interferes with terminal auto-scroll. Clear the control's
                    // own rendered area and park the cursor at its top so subsequent output starts
                    // cleanly. Interactive controls keep their previous cancel behavior.
                    for (int row = 0; row <= _bufferScreen.OriginalLineCount; row++)
                    {
                        console.SetCursorPosition(0, _screenPosition.StartTop + row);
                        console.WriteRaw("", console.CurrentStyle, true);
                    }
                    console.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop);
                }
                FinalizeControl();
            }
            finally
            {
                console.SizeChanged -= ConsoleHandler_SizeChanged;
                console.CursorVisible = oldcursor;
                console.ForegroundRgbColor = oldforecolor;
                console.BackgroundRgbColor = oldbackcolor;
                Thread.CurrentThread.CurrentCulture = _cultureInfo;
            }

            if (cts.Token.IsCancellationRequested && (ResultCtrl is null || !ResultCtrl.Value.IsAborted))
            {
                ResultCtrl = new ResultPrompt<T>(default!, true);
            }
            return ResultCtrl ?? new ResultPrompt<T>(default!, true);
        }


        /// <summary>
        /// Handles the console size changed event. If the control is not a widget, it signals that a resize is pending and updates the last known width.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ConsoleSizeChangedEventArgs"/> instance containing the event data.</param>
        private void ConsoleHandler_SizeChanged(object? sender, ConsoleSizeChangedEventArgs e)
        {
            if (isWidget)
            {
                return;
            }

            // Signal the main render loop to do a full recalibrate + clear + rebuild.
            // ALL console writes stay on the main thread — no direct writes here.
            //
            // React to BOTH width and height changes:
            //   • Width changes may reflow wrapped rows
            //   • Height changes (especially shrink) may require repositioning
            if (e.Width != _lastKnownWidth || e.Height != _lastKnownHeight)
            {
                _pendingResize = true;
            }
        }

        /// <summary>
        /// Attempts to retrieve the result of the control prompt. 
        /// Derived controls must implement this method to provide their specific logic for determining 
        /// when the prompt has completed and what the result should be.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the result.</param>
        /// <returns><c>true</c> if the prompt has completed and the result is available; otherwise, <c>false</c>.</returns>
        public abstract bool TryResult(CancellationToken cancellationToken);

        /// <summary>
        /// Initializes the control prompt. 
        /// Derived controls must implement this method to perform any necessary setup before the prompt is displayed.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe while initializing the control.</param>
        public abstract void InitControl(CancellationToken cancellationToken);

        /// <summary>
        /// Buffers the template for the control prompt.
        /// </summary>
        /// <param name="screenBuffer">The screen buffer to use for rendering the template.</param>
        public abstract void BufferTemplate(BufferScreen screenBuffer);

        /// <summary>
        /// Finishes the template for the control prompt.
        /// </summary>
        /// <param name="screenBuffer">The screen buffer to use for rendering the template.</param>
        /// <returns><c>true</c> if the template was successfully finished; otherwise, <c>false</c>.</returns>
        public abstract bool FinishTemplate(BufferScreen screenBuffer);

        /// <summary>
        /// Finalizes the control prompt. 
        /// Derived controls must implement this method to perform any necessary cleanup after the prompt has completed.
        /// </summary>
        public abstract void FinalizeControl();

        /// <summary>
        /// Reads the next key via <see cref="WaitKeypress"/> and packages the result together
        /// with resize and cancellation flags into a <see cref="KeyPressResult"/>.
        /// Derived controls must call this instead of <see cref="WaitKeypress"/> directly
        /// inside their <see cref="TryResult"/> loops; the resize guard is then always enforced
        /// and there is no need to check <see cref="IsPendingResize"/> separately.
        /// </summary>

        public int GetPromptDisplayWidth()
        {
            string prompt = OptionsControl.PromptValue ?? string.Empty;
            string suffix = OptionsControl.SufixAfterPromptValue ?? string.Empty;
            string fullPrompt = prompt + suffix;

            return fullPrompt.GetDisplayLength() is { Length: > 0 } lengths
                ? lengths.Sum()
                : 0;
        }

        protected KeyPressResult ReadNextKey(bool intercept, CancellationToken token)
        {
            ConsoleKeyInfo key = WaitKeypress(intercept, token);
            return new KeyPressResult(key, _pendingResize, token.IsCancellationRequested);
        }

        /// <summary>
        /// Waits for a key press on the console, returning <c>default</c> immediately when a
        /// pending resize or cancellation is detected. Override this method to add custom
        /// behaviour such as a spinner animation, idle timeout, or key pre-filtering.
        /// </summary>
        /// <remarks>
        /// Overrides must honour both <paramref name="token"/> and <see cref="IsPendingResize"/>:
        /// they should return <c>default</c> in either case so the render loop can react
        /// correctly. Callers should use <see cref="ReadNextKey"/> rather than this method
        /// directly so that the <see cref="KeyPressResult"/> flags are populated.
        /// </remarks>
        public virtual ConsoleKeyInfo WaitKeypress(bool intercept, CancellationToken token)
        {
            while (!console.KeyAvailable && !token.IsCancellationRequested)
            {
                if (_pendingResize)
                {
                    // A resize is pending. Return a default key so TryResult unblocks
                    // immediately; the do-while loop condition will detect _pendingResize
                    // and skip TryResult's result, looping back for a full re-render.
                    return default;
                }
                token.WaitHandle.WaitOne(16);
            }
            return console.KeyAvailable && !token.IsCancellationRequested ? console.ReadKey(intercept) : default;
        }

        public virtual void WritePrompt(BufferScreen screenBuffer, Style style)
        {
            if (!string.IsNullOrEmpty(OptionsControl.PromptValue))
            {
                screenBuffer.Write(OptionsControl.PromptValue, style);
                if (OptionsControl.SufixAfterPromptValue is not null)
                {
                    screenBuffer.Write(OptionsControl.SufixAfterPromptValue, style);
                }
            }
        }

        public virtual void WriteError(BufferScreen screenBuffer, Style style)
        {
            if (!string.IsNullOrEmpty(ValidateError))
            {
                screenBuffer.WriteLine(ValidateError, style);
                ClearError();
            }
        }

        /// <summary>
        /// Clears the current validation error message, if any. This method can be called to reset the error state of the control prompt.
        /// </summary>
        public void ClearError()
        {
            _errorMessage = null;
        }

        /// <summary>
        /// Sets the validation error message for the control prompt. 
        /// This method can be called to indicate that a validation error has occurred 
        /// and provide a corresponding error message.
        /// </summary>
        /// <param name="errorMessage">The error message to set.</param>
        public void SetError(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        /// <summary>
        /// Attempts to validate the specified input using the provided list of validators.
        /// </summary>
        /// <param name="input">The input to validate.</param>
        /// <param name="validators">The list of validators to apply.</param>
        /// <returns><c>true</c> if the input is valid; otherwise, <c>false</c>.</returns>
        public bool TryValidate(object input, IList<Func<object, ValidationResult>> validators)
        {
            foreach (Func<object, ValidationResult> validator in validators)
            {
                ValidationResult result = validator(input);

                if (result != ValidationResult.Success)
                {
                    _errorMessage = result.ErrorMessage;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether the specified key press corresponds to the tooltip toggle key defined in the configuration.
        /// </summary>
        /// <param name="keyInfo"></param>
        /// <returns></returns>
        public bool IsTooltipToggerKeyPress(ConsoleKeyInfo keyInfo)
        {
            return OptionsControl.ShowTooltipValue && ConfigPrompt.HotKeyTooltip.Equals(keyInfo);
        }

        /// <summary>
        /// Determines whether the specified key press corresponds to the tooltip show/hide key defined in the configuration.
        /// </summary>
        /// <param name="keyInfo"></param>
        /// <returns></returns>
        public bool CheckTooltipShowHideKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (OptionsControl.ShowTooltipValue && ConfigPrompt.HotKeyTooltipShowHide.Equals(keyInfo))
            {
                _showTooltipValue = !_showTooltipValue;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified key press corresponds to the abort key defined in the configuration.
        /// </summary>
        /// <param name="keyInfo">The key press to check.</param>
        /// <returns><c>true</c> if the key press corresponds to the abort key; otherwise, <c>false</c>.</returns>
        public bool IsAbortKeyPress(ConsoleKeyInfo keyInfo)
        {
            return OptionsControl.EnabledAbortKeyValue && ConfigPrompt.HotKeyAbortKeyPress.Equals(keyInfo);
        }

        /// <summary>
        /// Gets the Emacs tooltips based on the specified read-only state.
        /// </summary>
        /// <param name="isreadonly">Indicates whether the tooltips should be for read-only mode.</param>
        /// <returns>An array of Emacs tooltips.</returns>
        public string[] GetEmacsTooltips(bool isreadonly)
        {
            if (!ConsoleHandler.EnabledEmacs)
            {
                return [];
            }
            if (isreadonly)
            {
                _EmacsTooltipsReadonly ??=
                [
                    PromptPlusResources.Emac_ctrl_a,
                    PromptPlusResources.Emac_ctrl_b,
                    PromptPlusResources.Emac_ctrl_e,
                    PromptPlusResources.Emac_ctrl_f,
                    PromptPlusResources.Emac_alt_b,
                    PromptPlusResources.Emac_alt_f
                ];
                return _EmacsTooltipsReadonly;
            }
            _EmacsTooltips ??=
            [
                PromptPlusResources.Emac_Insert,
                PromptPlusResources.Emac_ctrl_a,
                PromptPlusResources.Emac_ctrl_b,
                PromptPlusResources.Emac_ctrl_d,
                PromptPlusResources.Emac_ctrl_e,
                PromptPlusResources.Emac_ctrl_f,
                PromptPlusResources.Emac_ctrl_h,
                PromptPlusResources.Emac_ctrl_k,
                PromptPlusResources.Emac_ctrl_l,
                PromptPlusResources.Emac_ctrl_t,
                PromptPlusResources.Emac_ctrl_u,
                PromptPlusResources.Emac_ctrl_w,
                PromptPlusResources.Emac_alt_b,
                PromptPlusResources.Emac_alt_c,
                PromptPlusResources.Emac_alt_d,
                PromptPlusResources.Emac_alt_f,
                PromptPlusResources.Emac_alt_l,
                PromptPlusResources.Emac_alt_u
            ];
            return _EmacsTooltips;
        }

        /// <summary>
        /// Computes the visible left and right slices of <paramref name="buffer"/> for a
        /// viewport whose width equals the remaining screen columns after the prompt.
        /// The viewport window slides so the cursor is always inside the visible area.
        /// When slicing is active, delimiter characters are added conditionally:
        /// <list type="bullet">
        ///   <item>The ellipsis string is prepended only when text is hidden <em>before</em> the visible slice (<c>start &gt; 0</c>).</item>
        ///   <item>The ellipsis string is appended when text is still hidden <em>after</em> the right edge of the viewport.</item>
        ///   <item>No suffix is added when the viewport reaches the actual end of the text.</item>
        /// </list>
        /// </summary>
        /// <param name="buffer">The Emacs buffer whose content is to be sliced.</param>
        /// <param name="promptWidth">The number of columns already consumed by the prompt on the same line.</param>
        /// <returns>
        /// A tuple of (visibleLeft, visibleRight) where <c>visibleLeft</c> is the text
        /// before the cursor that fits in the viewport, and <c>visibleRight</c> is the
        /// text from the cursor to the end of the viewport.
        /// </returns>
        public (string VisibleLeft, string VisibleRight) ViewportSlice(EmacsConsoleBuffer buffer, int promptWidth)
        {
            return ViewportSlice(buffer.ToString(), buffer.Position, promptWidth);
        }

        /// <summary>
        /// Renders a read-only, horizontally-scrollable answer line into <paramref name="screenBuffer"/>.
        /// The base owns an internal <see cref="EmacsConsoleBuffer"/> and only reloads it when
        /// <paramref name="text"/> changes, so the cursor position (and therefore the horizontal
        /// scroll and the left/right ellipsis) is preserved while the user navigates with
        /// Home/End/Left/Right. When the text does not fit the available width, the tail of the text
        /// stays visible with an ellipsis on the left.
        /// </summary>
        /// <param name="screenBuffer">The screen buffer to write into.</param>
        /// <param name="text">The current answer text.</param>
        /// <param name="style">The style used to render the answer.</param>
        public void WriteAnswerViewport(BufferScreen screenBuffer, string text, Style style)
        {
            _answerViewportBuffer ??= new EmacsConsoleBuffer(true, CaseOptions.Any, ConsoleHandler.EnabledEmacs, static (_) => true);

            int promptWidth = GetPromptDisplayWidth();
            int viewportWidth = Math.Max(0, ConsoleHandler.Width - promptWidth);
            int terminalHeight = ConsoleHandler.Height;

            // Reload the buffer only when the text changes (ToHome anchors at the start, like
            // Select). Anchoring at the start keeps the recorded cursor column small, so the answer
            // never fills the full width nor triggers a pending-wrap that duplicates the line on
            // resize. Reloading only on change preserves horizontal navigation between frames.
            bool textChanged = !string.Equals(text, _answerViewportLastText, StringComparison.Ordinal);

            // A terminal resize changes the geometry; re-anchor the horizontal scroll so the
            // read-only answer stays predictably aligned at the start. Track BOTH width and height:
            // a width change alters the viewport slice directly, while a height change can move and
            // re-anchor the frame on screen (scroll/relayout), so either warrants a re-anchor. Only
            // re-anchor (ToHome) here instead of reloading, since the content did not change.
            bool geometryChanged = viewportWidth != _answerViewportLastWidth
                                   || terminalHeight != _answerViewportLastHeight;

            if (textChanged)
            {
                _answerViewportBuffer.LoadPrintable(text ?? string.Empty);
                _answerViewportBuffer.ToHome();
                _answerViewportLastText = text;
            }
            else if (geometryChanged)
            {
                _answerViewportBuffer.ToHome();
            }

            _answerViewportLastWidth = viewportWidth;
            _answerViewportLastHeight = terminalHeight;

            // Slice directly from the cached text and the buffer's cursor position, reusing the
            // viewportWidth already computed above. The buffer content always equals
            // _answerViewportLastText (navigation keys only move the cursor, never mutate the
            // content), so this avoids both an extra buffer.ToString() allocation and a redundant
            // ConsoleHandler.Width read (each Width access costs a lock + syscall) per frame.
            (string visibleLeft, string visibleRight) = ViewportSliceCore(
                _answerViewportLastText ?? string.Empty, _answerViewportBuffer.Position, viewportWidth);
            screenBuffer.Write(visibleLeft, style);
            screenBuffer.SavePromptCursor();
            screenBuffer.WriteLine(visibleRight, style);
        }

        /// <summary>
        /// Forwards a non-printable readline key (Home/End/Left/Right/...) to the internal answer
        /// viewport buffer so the read-only answer scrolls horizontally. Returns <c>true</c> when
        /// the key was consumed by the buffer.
        /// </summary>
        /// <param name="keyInfo">The key to forward.</param>
        /// <returns><c>true</c> when the key moved the answer cursor; otherwise, <c>false</c>.</returns>
        public bool TryAnswerViewportNavigation(ConsoleKeyInfo keyInfo)
        {
            if (_answerViewportBuffer is null)
            {
                return false;
            }
            return !_answerViewportBuffer.IsPrintable(keyInfo.KeyChar)
                   && _answerViewportBuffer.TryAcceptedReadlineConsoleKey(keyInfo);
        }

        /// <summary>
        /// Computes the visible left and right slices for a
        /// viewport whose width equals the remaining screen columns after the prompt.
        /// The viewport window slides so the cursor is always inside the visible area.
        /// When slicing is active, delimiter characters are added conditionally:
        /// <list type="bullet">
        ///   <item>The ellipsis string is prepended only when text is hidden <em>before</em> the visible slice (<c>start &gt; 0</c>).</item>
        ///   <item>The ellipsis string is appended when text is still hidden <em>after</em> the right edge of the viewport.</item>
        ///   <item>No suffix is added when the viewport reaches the actual end of the text.</item>
        /// </list>
        /// </summary>
        /// <param name="fullText">The full text content to be sliced.</param>
        /// <param name="cursorPos">The current cursor position within the text.</param>
        /// <param name="promptWidth">The number of columns already consumed by the prompt on the same line.</param>
        /// <returns>
        /// A tuple of (visibleLeft, visibleRight) where <c>visibleLeft</c> is the text
        /// before the cursor that fits in the viewport, and <c>visibleRight</c> is the
        /// text from the cursor to the end of the viewport.
        /// </returns>
        public (string VisibleLeft, string VisibleRight) ViewportSlice(string fullText, int cursorPos, int promptWidth)
        {
            int viewportWidth = Math.Max(0, ConsoleHandler.Width - promptWidth);
            return ViewportSliceCore(fullText, cursorPos, viewportWidth);
        }

        /// <summary>
        /// Core slicing routine shared by the public <see cref="ViewportSlice(string, int, int)"/>
        /// overload and the hot-path answer viewport render. Receives the already-computed
        /// <paramref name="viewportWidth"/> so callers on the per-frame render path avoid an extra
        /// <see cref="IConsole.Width"/> read (a lock + syscall).
        /// </summary>
        /// <param name="fullText">The full text content to be sliced.</param>
        /// <param name="cursorPos">The current cursor position within the text.</param>
        /// <param name="viewportWidth">The available viewport width in display columns.</param>
        private (string VisibleLeft, string VisibleRight) ViewportSliceCore(string fullText, int cursorPos, int viewportWidth)
        {
            // Rune width classification lives once in ConsolePlusLibrary.StringExtensions.GetRuneWidth
            // (public extension on Rune) — do not re-duplicate the wide-rune ranges here; a local copy
            // would silently drift from the canonical one used by GetDisplayLength/TruncateToDisplayWidth.
            static int RuneDisplayWidth(Rune rune) => rune.GetRuneWidth();

            static int SliceDisplayWidth(string source, int start, int end)
            {
                if (end <= start)
                {
                    return 0;
                }

                int width = 0;
                foreach (Rune rune in source.AsSpan(start, end - start).EnumerateRunes())
                {
                    width += RuneDisplayWidth(rune);
                }

                return width;
            }

            static bool FitsInWidth(string value, int maxWidth)
            {
                int width = 0;
                foreach (Rune rune in value.EnumerateRunes())
                {
                    width += RuneDisplayWidth(rune);
                    if (width > maxWidth)
                    {
                        return false;
                    }
                }

                return true;
            }

            static bool TryTrimRightRune(string source, int start, ref int end, ref int width)
            {
                if (end <= start)
                {
                    return false;
                }

                ReadOnlySpan<char> span = source.AsSpan(start, end - start);
                OperationStatus status = Rune.DecodeLastFromUtf16(span, out Rune rune, out int charsConsumed);
                if (status != OperationStatus.Done || charsConsumed <= 0)
                {
                    charsConsumed = 1;
                    rune = new Rune(span[^1]);
                }

                end -= charsConsumed;
                width -= RuneDisplayWidth(rune);
                if (width < 0)
                {
                    width = SliceDisplayWidth(source, start, end);
                }

                return true;
            }

            static bool TryTrimLeftRune(string source, ref int start, int end, ref int width)
            {
                if (end <= start)
                {
                    return false;
                }

                ReadOnlySpan<char> span = source.AsSpan(start, end - start);
                OperationStatus status = Rune.DecodeFromUtf16(span, out Rune rune, out int charsConsumed);
                if (status != OperationStatus.Done || charsConsumed <= 0)
                {
                    charsConsumed = 1;
                    rune = new Rune(span[0]);
                }

                start += charsConsumed;
                width -= RuneDisplayWidth(rune);
                if (width < 0)
                {
                    width = SliceDisplayWidth(source, start, end);
                }

                return true;
            }

            static void TrimToBudget(
                string source,
                ref int leftStart,
                int leftEnd,
                ref int leftWidth,
                int rightStart,
                ref int rightEnd,
                ref int rightWidth,
                int budget)
            {
                while (leftWidth + rightWidth > budget)
                {
                    if (TryTrimRightRune(source, rightStart, ref rightEnd, ref rightWidth))
                    {
                        continue;
                    }

                    if (TryTrimLeftRune(source, ref leftStart, leftEnd, ref leftWidth))
                    {
                        continue;
                    }

                    break;
                }
            }

            int totalLen = fullText.Length;

            cursorPos = Math.Max(0, Math.Min(cursorPos, totalLen));

            if (viewportWidth <= 0)
            {
                return (string.Empty, string.Empty);
            }

            if (totalLen == 0)
            {
                return (string.Empty, string.Empty);
            }

            if (FitsInWidth(fullText, viewportWidth))
            {
                return (fullText[..cursorPos], fullText[cursorPos..]);
            }

            string ellipsisStr = ConsoleHandler.SupportsUnicode
                ? UnicodeEllipsis
                : AsciiEllipsis;
            int ellipsisWidth = SliceDisplayWidth(ellipsisStr, 0, ellipsisStr.Length);

            // When the cursor sits at the absolute end of the text, nothing is rendered to its
            // right, so it needs its own free column to land on. Without this reservation the
            // window below fills every column of the viewport and the cursor ends up clamped
            // onto the same column as the last visible character instead of past it.
            bool cursorAtTextEnd = cursorPos == totalLen;
            int usableWidth = cursorAtTextEnd ? Math.Max(0, viewportWidth - 1) : viewportWidth;

            int start = Math.Max(0, cursorPos - Math.Max(0, usableWidth - 1));
            int end = Math.Min(totalLen, start + usableWidth);
            start = Math.Max(0, end - usableWidth);

            bool showPrefix = start > 0 && ellipsisWidth > 0 && ellipsisWidth <= usableWidth;
            bool showSuffix = end < totalLen && ellipsisWidth > 0 && ellipsisWidth <= usableWidth;

            if (showPrefix && showSuffix && (2 * ellipsisWidth > usableWidth))
            {
                showPrefix = false;
            }

            cursorPos = Math.Max(start, Math.Min(cursorPos, end));
            int leftStart = start;
            int leftEnd = cursorPos;
            int rightStart = cursorPos;
            int rightEnd = end;

            int contentLeftWidth = SliceDisplayWidth(fullText, leftStart, leftEnd);
            int contentRightWidth = SliceDisplayWidth(fullText, rightStart, rightEnd);

            // Trimming to fit the budget can hide characters that were not originally flagged as
            // hidden (e.g. content sacrificed purely to make room for the opposite side's
            // ellipsis). Re-check both sides against the ACTUAL post-trim bounds and, whenever
            // that reveals a newly-hidden side, reserve its ellipsis and trim again. Each flag can
            // only flip from false to true (trimming never un-hides content), so this converges.
            while (true)
            {
                int contentBudget = Math.Max(0, usableWidth
                    - (showPrefix ? ellipsisWidth : 0)
                    - (showSuffix ? ellipsisWidth : 0));

                TrimToBudget(
                    fullText,
                    ref leftStart,
                    leftEnd,
                    ref contentLeftWidth,
                    rightStart,
                    ref rightEnd,
                    ref contentRightWidth,
                    contentBudget);

                bool needsPrefix = leftStart > 0 && ellipsisWidth > 0 && ellipsisWidth <= usableWidth;
                bool needsSuffix = rightEnd < totalLen && ellipsisWidth > 0 && ellipsisWidth <= usableWidth;

                if (needsPrefix == showPrefix && needsSuffix == showSuffix)
                {
                    break;
                }

                showPrefix = needsPrefix;
                showSuffix = needsSuffix;
            }

            int visibleLeftWidth = contentLeftWidth + (showPrefix ? ellipsisWidth : 0);
            int visibleRightWidth = contentRightWidth + (showSuffix ? ellipsisWidth : 0);

            while (visibleLeftWidth + visibleRightWidth > usableWidth)
            {
                if (rightEnd > rightStart)
                {
                    TryTrimRightRune(fullText, rightStart, ref rightEnd, ref contentRightWidth);
                    visibleRightWidth = contentRightWidth + (showSuffix ? ellipsisWidth : 0);
                    continue;
                }

                if (leftEnd > leftStart)
                {
                    TryTrimLeftRune(fullText, ref leftStart, leftEnd, ref contentLeftWidth);
                    visibleLeftWidth = contentLeftWidth + (showPrefix ? ellipsisWidth : 0);
                    continue;
                }

                break;
            }

            string contentLeft = fullText[leftStart..leftEnd];
            string contentRight = fullText[rightStart..rightEnd];
            string visibleLeft = showPrefix ? ellipsisStr + contentLeft : contentLeft;
            string visibleRight = showSuffix ? contentRight + ellipsisStr : contentRight;

            return (visibleLeft, visibleRight);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "ByDesign")]
        public bool TryDeserializeHistoryValue<T1>(string? history, out T1 value)
        {
            value = default!;
            if (string.IsNullOrWhiteSpace(history))
            {
                return false;
            }

            if (TryDeserialize<T1>(history, out value))
            {
                return true;
            }

            if (history.Length >= 2 && history[0] == '"' && history[^1] == '"')
            {
                try
                {
                    string? decoded = JsonSerializer.Deserialize<string>(history);
                    if (!string.IsNullOrWhiteSpace(decoded) && TryDeserialize(decoded, out value))
                    {
                        return true;
                    }
                }
                catch (JsonException)
                {
                    // ignore malformed legacy payload
                }
            }

            return false;
        }

        private static bool TryDeserialize<T1>(string payload, out T1 value)
        {
            value = default!;
            try
            {
                T1? parsed = JsonSerializer.Deserialize<T1>(payload);
                if (parsed is null)
                {
                    return false;
                }
                value = parsed;
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }

        /// <summary>
        /// Computes the effective page size for a control prompt, taking into account the reserved template lines and the available rows in the console.
        /// </summary>
        /// <param name="reservedTemplateLines">The number of lines reserved for the template.</param>
        /// <param name="pageSize">The requested page size.</param>
        /// <returns>The effective page size.</returns>
        public int ComputeEffectivePageSize(in int reservedTemplateLines, byte pageSize)
        {
            if (pageSize <= 0)
            {
                pageSize = byte.MaxValue;
            }
            int availableRows = Math.Max(1, ConsoleHandler.Height - reservedTemplateLines);
            return Math.Min(pageSize, availableRows);
        }

        /// <summary>
        /// Recovers the control frame's real on-screen top after a terminal resize that was
        /// NOT yet processed by the main render loop, clears the old (pre-resize) footprint,
        /// and updates <see cref="_screenPosition"/> so the next render lands at the correct
        /// location. Used by the finish path for Live auto-render controls where progress can
        /// complete at the exact moment of a resize (e.g. height shrink) and would otherwise
        /// draw the finish frame at a stale anchor, leaving the previous frame as garbage.
        /// </summary>
        private void RealignFrameAfterResize()
        {
            int oldStartTop = _screenPosition.StartTop;
            int oldStartLeft = _screenPosition.StartLeft;
            int oldHeight = _lastKnownHeight;
            int oldPhysicalRows = _renderedPhysicalRows;
            int newHeight = console.Height;
            int newWidth = console.Width;

            // Physical rows the OLD frame occupies NOW, reflowed at the new width.
            int oldFrameRowsReflowed = CalculateReflowedRows(_bufferScreen, oldStartLeft, newWidth);
            if (oldFrameRowsReflowed <= 0)
            {
                oldFrameRowsReflowed = Math.Max(1, oldPhysicalRows);
            }

            // Recover the frame top by reading the parked cursor for BOTH grow and shrink. The
            // cursor is a reliable anchor now (interactive controls park it via PromptCursor, Live
            // controls park it at the frame top-left with offset 0 after each render), so the
            // subtraction yields the true frame top regardless of how the terminal moved content.
            // Reading unconditionally (not just on shrink) is required for RAPID shrink?grow
            // cycles where a grow pulls the frame back down from scrollback: assuming the frame
            // stayed put on grow would render at a stale row and duplicate the frame.
            int currentFrameTop = oldStartTop;
            bool cursorAnchorUsable = false;
            bool heightShrank = newHeight < oldHeight;
            if (console.Profile.IsTerminal)
            {
                (int _, int actualCursorTop) = console.GetCursorPosition();
                int candidateFrameTop = actualCursorTop - _lastCursorTopOffset;
                if (actualCursorTop >= 0 && actualCursorTop < newHeight
                    && candidateFrameTop >= 0 && candidateFrameTop < newHeight)
                {
                    currentFrameTop = candidateFrameTop;
                    cursorAnchorUsable = true;
                }
            }

            if (!cursorAnchorUsable)
            {
                int scrollUp = 0;
                if (heightShrank)
                {
                    int heightShrink = oldHeight - newHeight;
                    int oldFrameBottom = oldStartTop + oldPhysicalRows - 1;
                    int rowsBelowFrame = Math.Max(0, (oldHeight - 1) - oldFrameBottom);
                    scrollUp = Math.Max(0, heightShrink - rowsBelowFrame);
                    scrollUp = Math.Min(scrollUp, oldStartTop);
                }
                currentFrameTop = Math.Max(0, oldStartTop - scrollUp);
            }

            int newStartLeft = oldStartLeft;
            if (newStartLeft >= newWidth)
            {
                newStartLeft = 0;
            }

            // Clear the UNION of:
            //   • the CACHED footprint  (oldStartTop .. oldStartTop + oldPhysicalRows - 1), and
            //   • the RECOVERED footprint (currentFrameTop .. + oldFrameRowsReflowed - 1).
            //
            // Both regions belong to THIS control (never external content). Clearing the union
            // guarantees the previous frame is erased regardless of whether the terminal scrolled
            // it (currentFrameTop != oldStartTop) or the cached anchor drifted. This is what makes
            // the fix deterministic instead of relying on a single (possibly wrong) position.
            int cachedTop = Math.Max(0, oldStartTop);
            int cachedBottom = Math.Min(newHeight - 1, oldStartTop + Math.Max(1, oldPhysicalRows) - 1);
            int recoveredTop = Math.Max(0, currentFrameTop);
            int recoveredBottom = Math.Min(newHeight - 1, currentFrameTop + oldFrameRowsReflowed - 1);

            int clearTop = Math.Min(cachedTop, recoveredTop);
            int clearBottom = Math.Max(cachedBottom, recoveredBottom);
            for (int row = clearTop; row <= clearBottom; row++)
            {
                console.SetCursorPosition(0, row);
                console.WriteRaw("", console.CurrentStyle, true);
            }

            _screenPosition = (newStartLeft, Math.Max(0, currentFrameTop));
            _anchorScreenPosition = _screenPosition;
            _lastKnownWidth = newWidth;
            _lastKnownHeight = newHeight;
            _renderedPhysicalRows = oldFrameRowsReflowed;
            // The finish render rebuilds and repositions from _screenPosition; prevent an extra
            // auto-scroll adjustment based on stale state this frame.
            _resizePositionHandled = true;
            console.SetCursorPosition(newStartLeft, _screenPosition.StartTop);
        }

        private void RenderBuffer(BufferScreen bufferScreen)
        {
            bool oldcursor = console.CursorVisible;
            console.CursorVisible = false;

            // -- Small-terminal safeguard --------------------------------------
            // When the terminal is too small to safely render the control, abort
            // this frame and display a one-line warning INSIDE the control's own
            // anchor row (never touching content above/below the footprint).
            // Wait until the user resizes back to a usable size, then force a
            // full relayout on the next iteration of the main render loop.
            if (!isWidget
                && console.Profile.IsTerminal
                && (console.Height < MinSafeRenderHeight || console.Width < MinSafeRenderWidth))
            {
                ShowResizeWarningAndWait(bufferScreen);
                console.CursorVisible = oldcursor;
                return;
            }

            // Before rendering, check whether the content would overflow the bottom
            // of the visible screen. If it does, pre-scroll the terminal by emitting
            // newlines and adjust StartTop so all subsequent SetCursorPosition calls
            // land on visible rows.
            //
            // IMPORTANT: Skip this adjustment when _resizePositionHandled is true,
            // because the resize block already calculated the correct position
            // accounting for the frame size. Double-adjusting causes off-by-one errors.
            if (console.Profile.IsTerminal && !_resizePositionHandled)
            {
                int newLineCount = bufferScreen.CurrentLineCount;
                if (newLineCount > 0)
                {
                    int lastContentRow = _screenPosition.StartTop + newLineCount - 1;
                    if (lastContentRow >= console.Height)
                    {
                        int scrollNeeded = lastContentRow - (console.Height - 1);
                        console.SetCursorPosition(_screenPosition.StartLeft, console.Height - 1);
                        for (int i = 0; i < scrollNeeded; i++)
                        {
                            console.WriteRaw(Environment.NewLine, console.CurrentStyle, true);
                        }
                        _screenPosition = (_screenPosition.StartLeft,
                            Math.Max(0, _screenPosition.StartTop - scrollNeeded));
                        // Keep the anchor consistent with the real screen position so a
                        // subsequent resize does not anchor below the actual frame top.
                        _anchorScreenPosition = (_anchorScreenPosition.StartLeft,
                            Math.Max(0, _anchorScreenPosition.StartTop - scrollNeeded));
                    }
                }
            }

            (int Left, int Top)? savedcursor = bufferScreen.PromptCursor;

            // When the line count shrinks, clear only the trailing rows that no longer exist.
            // Do NOT clear all rows here, otherwise unchanged rows would be erased and not
            // redrawn by the diff pass.
            int oldLineCount = bufferScreen.OriginalLineCount;
            int newLineCountAfterTemplate = bufferScreen.CurrentLineCount;
            if (oldLineCount > newLineCountAfterTemplate)
            {
                for (int i = newLineCountAfterTemplate; i < oldLineCount; i++)
                {
                    console.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop + i);
                    console.WriteRaw("", console.CurrentStyle, true);
                }
            }

            foreach (BufferLineDiff diff in bufferScreen.UpdateBufferDiff())
            {
                console.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop + diff.Row);
                // Ensure tails from previously longer content are removed when a line shrinks.
                console.WriteRaw("", console.CurrentStyle, true);
                console.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop + diff.Row);
                WriteLineSegments(diff.Line);
            }

            if (savedcursor.HasValue)
            {
                console.SetCursorPosition(savedcursor.Value.Left, _screenPosition.StartTop + savedcursor.Value.Top);
            }
            console.CursorVisible = oldcursor;
        }

        /// <summary>
        /// Displays a one-line "resize terminal" warning at the control's anchor
        /// row and blocks until the terminal grows back to a usable size
        /// (Height &gt;= <see cref="MinSafeRenderHeight"/> and Width &gt;= <see cref="MinSafeRenderWidth"/>).
        /// </summary>
        /// <remarks>
        /// External content above and below the control's footprint is fully
        /// preserved: only a single row inside the control's own area is
        /// written, and that row is cleared on exit. The pending stale buffer
        /// is dropped so the next iteration of the main render loop rebuilds
        /// the template from scratch at the new size.
        /// </remarks>
        /// <param name="bufferScreen">The buffer screen whose pending current
        /// frame must be discarded so the next render rebuilds from scratch.</param>
        private void ShowResizeWarningAndWait(BufferScreen bufferScreen)
        {
            _resizeWarningFormat ??= CompositeFormat.Parse(PromptPlusResources.ResizeTerminalWarning);

            // Always render once on entry, then refresh only when the terminal
            // is actually resized (signalled by console.SizeChanged via
            // _pendingResize). This avoids flicker from redrawing every tick.
            bool needsRedraw = true;

            // Number of PHYSICAL rows the previous warning occupied after the
            // terminal wrapped it. Tracked so we can erase every wrapped row
            // before the next redraw — clearrestofline only clears the row
            // where the cursor sits, so a longer wrapped tail from a previous
            // frame would otherwise remain visible underneath a shorter one.
            int previousRowsUsed = 0;

            while (!console.CancelToken.IsCancellationRequested
                   && (console.Height < MinSafeRenderHeight || console.Width < MinSafeRenderWidth))
            {
                if (needsRedraw)
                {
                    string message = string.Format(
                        CultureInfo.InvariantCulture,
                        _resizeWarningFormat,
                        MinSafeRenderHeight,
                        MinSafeRenderWidth,
                        console.Height,
                        console.Width);

                    // Always read the current anchor — _screenPosition may have
                    // been shifted up by a previous pre-scroll inside this loop.
                    int anchorLeft = _screenPosition.StartLeft;
                    int anchorTop = _screenPosition.StartTop;

                    // (1) Erase any wrapped tail from the PREVIOUS frame so a
                    //     shorter new message does not leave stale text behind.
                    EraseResizeWarning(anchorLeft, anchorTop, previousRowsUsed);

                    // (2) Compute the wrapped row count for the NEW message and,
                    //     if it would extend past the bottom of the viewport,
                    //     pre-scroll the terminal — same overflow-safe approach
                    //     RenderBuffer uses — and shift the anchor up so the
                    //     warning lands on visible rows without destroying any
                    //     external content above the control.
                    int newRowsNeeded = ComputeWrappedRowCount(message, anchorLeft, console.Width);
                    if (console.Profile.IsTerminal && newRowsNeeded > 0)
                    {
                        int lastContentRow = anchorTop + newRowsNeeded - 1;
                        if (lastContentRow >= console.Height)
                        {
                            int scrollNeeded = lastContentRow - (console.Height - 1);
                            console.SetCursorPosition(anchorLeft, console.Height - 1);
                            for (int i = 0; i < scrollNeeded; i++)
                            {
                                console.WriteRaw(Environment.NewLine, console.CurrentStyle, true);
                            }
                            anchorTop = Math.Max(0, anchorTop - scrollNeeded);
                            _screenPosition = (anchorLeft, anchorTop);
                            _anchorScreenPosition = (_anchorScreenPosition.StartLeft,
                                Math.Max(0, _anchorScreenPosition.StartTop - scrollNeeded));
                        }
                    }

                    // (3) Write the warning at the (possibly adjusted) anchor.
                    console.SetCursorPosition(anchorLeft, anchorTop);
                    console.WriteRaw(message, console.CurrentStyle.ForeGround(Color.Yellow).Overflow(Overflow.None), true);

                    previousRowsUsed = newRowsNeeded;
                    console.SetCursorPosition(anchorLeft, anchorTop);
                    needsRedraw = false;
                }

                // Block on the cancellation handle. WaitHandle.WaitOne is an
                // OS-level wait, so CPU usage is effectively zero between
                // ticks; the wait returns early when the token is cancelled.
                // The interval is short enough to be imperceptible (~5
                // checks per second) but long enough to keep CPU at idle.
                console.CancelToken.WaitHandle.WaitOne(ResizeWarningPollIntervalMs);
                if (_pendingResize)
                {
                    _pendingResize = false;
                    needsRedraw = true;
                }
            }

            // Erase every physical row the last warning occupied so the next
            // full render starts on a clean region. Use the LIVE anchor since
            // a pre-scroll may have shifted it during the loop.
            EraseResizeWarning(_screenPosition.StartLeft, _screenPosition.StartTop, previousRowsUsed);
            console.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop);

            // Drop any half-built current frame and force the main loop to
            // perform a full resize-style relayout at the new dimensions.
            bufferScreen.Clear();
            _pendingResize = true;
        }

        /// <summary>
        /// Clears <paramref name="rowsUsed"/> physical rows starting at the
        /// control's anchor (<paramref name="left"/>, <paramref name="top"/>),
        /// staying inside the visible viewport. Used by
        /// <see cref="ShowResizeWarningAndWait"/> to remove any wrapped tail
        /// from a previous warning before redrawing.
        /// </summary>
        private void EraseResizeWarning(int left, int top, int rowsUsed)
        {
            if (rowsUsed <= 0 || top < 0 || top >= console.Height)
            {
                return;
            }
            int maxRow = Math.Min(console.Height - 1, top + rowsUsed - 1);
            for (int row = top; row <= maxRow; row++)
            {
                console.SetCursorPosition(row == top ? left : 0, row);
                console.WriteRaw("", console.CurrentStyle, true);
            }
        }

        /// <summary>
        /// Calculates how many physical terminal rows <paramref name="text"/>
        /// occupies when emitted at column <paramref name="startLeft"/> in a
        /// viewport of width <paramref name="width"/>. The first physical row
        /// has only <c>width - startLeft</c> usable cells; subsequent rows use
        /// the full width.
        /// </summary>
        private static int ComputeWrappedRowCount(string text, int startLeft, int width)
        {
            if (string.IsNullOrEmpty(text) || width <= 0)
            {
                return 0;
            }
            int displayLen = text.GetDisplayLength() is { Length: > 0 } d ? d[0] : 0;
            if (displayLen <= 0)
            {
                return 0;
            }
            int firstRowCells = Math.Max(1, width - startLeft);
            if (displayLen <= firstRowCells)
            {
                return 1;
            }
            int remaining = displayLen - firstRowCells;
            return 1 + (int)Math.Ceiling(remaining / (double)width);
        }

        /// <summary>
        /// Writes all non-empty segments of a <see cref="LineScreen"/> to the console.
        /// Emits a clear-line signal when the line has no content.
        /// </summary>
        /// <param name="line">The line screen to write.</param>
        private void WriteLineSegments(LineScreen line)
        {
            if (line.Content.Count == 0)
            {
                console.WriteRaw("", console.CurrentStyle, true);
                return;
            }
            var first = true;
            foreach (Fragment item in line.Content)
            {
                if (item.Type == FragmentKind.ClearRestofline || item.Text.Length == 0)
                {
                    continue;
                }
                // Fast-path: most fragments contain no CR/LF, so skip the three allocating
                // Replace calls entirely and write the original string. Only strip line breaks
                // when they are actually present.
                string text = item.Text;
                if (item.Text.AsSpan().IndexOfAny('\r', '\n') >= 0)
                {
                    text = item.Text
                        .Replace("\r", string.Empty, StringComparison.Ordinal)
                        .Replace("\n", string.Empty, StringComparison.Ordinal);
                }
                var ctrloverflow = (item.Style ?? console.CurrentStyle).Overflow(Overflow.Ellipsis);
                console.WriteRaw(text, ctrloverflow, first);
                first = false;
            }
        }


        /// <summary>
        /// Gets the symbol for the specified <see cref="SymbolType"/>.
        /// </summary>
        /// <param name="type">The type of symbol to retrieve.</param>
        /// <param name="useUnicode">Indicates whether to use the Unicode representation of the symbol if supported.</param>
        /// <returns>The symbol as a string.</returns>
        /// <exception cref="ArgumentException">Thrown if the symbol type is not found in the dictionary.</exception>   
        public string GetSymbol(SymbolType type, bool useUnicode = true)
        {
            if (!_symbols.TryGetValue(type, out var symbol))
            {
                throw new ArgumentException($"SymbolType '{type}' not found in the symbol dictionary.", nameof(type));
            }
            if (useUnicode && console.SupportsUnicode && CanEncode(symbol.unicode))
            {
                return symbol.unicode;
            }
            return symbol.value;
        }

        /// <summary>
        /// Determines whether the console output encoding can represent every character
        /// in <paramref name="text"/> without resorting to a replacement (fallback) character.
        /// This guards against cases where <see cref="IConsole.SupportsUnicode"/> is <c>true</c>
        /// but the active output encoding would still emit a '?' for a specific glyph.
        /// </summary>
        /// <param name="text">The Unicode text to validate against the current output encoding.</param>
        /// <returns><c>true</c> if the text can be encoded losslessly; otherwise, <c>false</c>.</returns>
        private bool CanEncode(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }
            try
            {
                var encoding = console.OutputEncoding;
                if (encoding is null)
                {
                    return true;
                }
                // A strict encoder throws EncoderFallbackException instead of emitting a
                // replacement character, letting us detect that a fallback would occur.
                var strict = Encoding.GetEncoding(
                    encoding.CodePage,
                    EncoderFallback.ExceptionFallback,
                    DecoderFallback.ExceptionFallback);
                _ = strict.GetBytes(text);
                return true;
            }
            catch (EncoderFallbackException)
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes the dictionary of symbols used in the control prompts. 
        /// Each symbol type is associated with a tuple containing its ASCII representation and its Unicode representation.
        /// </summary>
        /// <returns>A dictionary mapping symbol types to their ASCII and Unicode representations.</returns>
        private static FrozenDictionary<SymbolType, (string value, string unicode)> InitSymbols()
        {
            return new Dictionary<SymbolType, (string value, string unicode)>
            {
                { SymbolType.Done, ("V", "√") },
                { SymbolType.Error, ("!", "‼") },
                { SymbolType.Canceled, ("x", "×") },
                { SymbolType.Selector, (">", "►") },
                { SymbolType.Selected, ("[x]", "[■]") },
                { SymbolType.NotSelect, ("[ ]", "[ ]") },
                { SymbolType.PartialSelect, ("[?]", "[▪]") },
                { SymbolType.WaitProcess, ("[*]", "[◔]") },
                { SymbolType.Expanded, ("[-]", "[▼]") },
                { SymbolType.Collapsed, ("[+]", "[►]") },
                { SymbolType.IndentGroup, ("|-", "├─") },
                { SymbolType.IndentEndGroup, ("|_", "└─") },
                { SymbolType.TreeLinecross, (" |-", " ├─") },
                { SymbolType.TreeLinecorner, (" |_", " └─") },
                { SymbolType.TreeLinevertical, (" | ", " │ ") },
                { SymbolType.TreeLinespace, ("   ", "   ") },
                { SymbolType.DoubleBorder, ("=", "═") },
                { SymbolType.SingleBorder, ("-", "─") },
                { SymbolType.HeavyBorder, ("*", "━") },
                { SymbolType.ProgressBarLight, ("-", "─") },
                { SymbolType.ProgressBarDoubleLight, ("=", "═") },
                { SymbolType.ProgressBarSquare, ("#", "█") },
                { SymbolType.ProgressBarBar, ("|", "▌") },
                { SymbolType.ProgressBarDot, (".", "∙") },
                { SymbolType.SliderBarLight, ("-", "─") },
                { SymbolType.SliderBarDoubleLight, ("=", "═") },
                { SymbolType.SliderBarSquare, ("#", "█") },
                { SymbolType.SliderBarDot, (".", "∙") },
                { SymbolType.ChartLabel, ("#", "■") },
                { SymbolType.ChartLight, ("-", "─") },
                { SymbolType.ChartSquare, ("#", "█") },
                { SymbolType.GridSingleTopLeft, ("+", "┌") },
                { SymbolType.GridSingleTopCenter, ("+", "┬") },
                { SymbolType.GridSingleTopRight, ("+", "┐") },
                { SymbolType.GridSingleMiddleLeft, ("|", "├") },
                { SymbolType.GridSingleMiddleCenter, ("+", "┼") },
                { SymbolType.GridSingleMiddleRight, ("|", "┤") },
                { SymbolType.GridSingleBottomLeft, ("+", "└") },
                { SymbolType.GridSingleBottomCenter, ("+", "┴") },
                { SymbolType.GridSingleBottomRight, ("+", "┘") },
                { SymbolType.GridSingleBorderLeft, ("|", "│") },
                { SymbolType.GridSingleBorderRight, ("|", "│") },
                { SymbolType.GridSingleBorderTop, ("-", "─") },
                { SymbolType.GridSingleBorderBottom, ("-", "─") },
                { SymbolType.GridSingleDividerY, ("|", "│") },
                { SymbolType.GridSingleDividerX, ("-", "─") },
                { SymbolType.GridDoubleTopLeft, ("+", "╔") },
                { SymbolType.GridDoubleTopCenter, ("+", "╦") },
                { SymbolType.GridDoubleTopRight, ("+", "╗") },
                { SymbolType.GridDoubleMiddleLeft, ("|", "╠") },
                { SymbolType.GridDoubleMiddleCenter, ("+", "╬") },
                { SymbolType.GridDoubleMiddleRight, ("|", "╣") },
                { SymbolType.GridDoubleBottomLeft, ("+", "╚") },
                { SymbolType.GridDoubleBottomCenter, ("+", "╩") },
                { SymbolType.GridDoubleBottomRight, ("+", "╝") },
                { SymbolType.GridDoubleBorderLeft, ("|", "║") },
                { SymbolType.GridDoubleBorderRight, ("|", "║") },
                { SymbolType.GridDoubleBorderTop, ("=", "═") },
                { SymbolType.GridDoubleBorderBottom, ("=", "═") },
                { SymbolType.GridDoubleDividerY, ("|", "║") },
                { SymbolType.GridDoubleDividerX, ("=", "═") },
                { SymbolType.CalendarNote, ("*", "∙") },
                { SymbolType.CalendarNoteHighlight, ("#", "♦") },
                { SymbolType.CalendarHighlight, ("!", "*") },
                { SymbolType.CalendarTodayLeft, ("<", "◄") },
                { SymbolType.CalendarTodayRight, (">", "►") },
                { SymbolType.TaskWaiting, (" ", "○") },
                { SymbolType.TaskRunning, (">", "◐") },
                { SymbolType.TaskSuccess, ("v", "●") },
                { SymbolType.TaskFailed, ("x", "✗") },
                { SymbolType.FilterableStatus, ("*", "⌕")}
            }.ToFrozenDictionary();
        }

        /// <summary>
        /// Calculates how many physical terminal rows the CURRENT buffer (just-built template)
        /// will occupy when rendered at <paramref name="width"/> starting at column <paramref name="startLeft"/>.
        /// Each logical line wraps to ceil((startLeft + lineWidth) / width) physical rows
        /// (minimum 1 per logical line so empty lines still occupy a row).
        /// </summary>
        private static int CalculateReflowedRowsFromCurrent(BufferScreen buffer, int startLeft, int width)
            => SumPhysicalRows(buffer.CurrentBuffer(), buffer.CurrentLineCount, startLeft, width);

        /// <summary>
        /// Calculates how many physical terminal rows the PREVIOUSLY-RENDERED frame
        /// (OriginalBuffer) would occupy after reflow at <paramref name="newWidth"/>.
        /// Used during resize to know how many rows on screen the stale content occupies.
        /// </summary>
        private static int CalculateReflowedRows(BufferScreen buffer, int startLeft, int newWidth)
            => SumPhysicalRows(buffer.OriginalBuffer(), buffer.OriginalLineCount, startLeft, newWidth);

        /// <summary>
        /// Common helper: sums the physical row count for a sequence of <see cref="LineScreen"/>
        /// using <c>ceil((startLeft + contentLen) / width)</c> per line, with a minimum of one
        /// physical row per logical line. Returns <paramref name="logicalLineCountFallback"/>
        /// when <paramref name="width"/> is non-positive so callers receive at least the
        /// logical line count in degenerate cases (terminal width unknown / zero).
        /// </summary>
        private static int SumPhysicalRows(LineScreen[] lines, int logicalLineCountFallback, int startLeft, int width)
        {
            if (width <= 0)
            {
                return Math.Max(0, logicalLineCountFallback);
            }
            if (startLeft < 0)
            {
                startLeft = 0;
            }
            int total = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                int contentLen = Math.Max(0, lines[i].ContentSize);
                int cells = startLeft + contentLen;
                int rows = cells <= 0 ? 1 : (cells + width - 1) / width;
                total += rows < 1 ? 1 : rows;
            }
            return total;
        }

    }
}
