// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Controls.Common;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.ChartBar
{
    internal sealed class ChartBarControl : BaseControlPrompt<ChartItem?>, IChartBarControl, IChartBarWidget
    {
        /// <summary>
        /// Total rows the control template reserves around the chart:
        /// prompt+answer line, optional description line, tooltip line, error line,
        /// and pagination footer when active. Chart title and legends are counted
        /// separately in the visible chart area.
        /// Used to derive the maximum visible page size from the available console height.
        /// </summary>
        private const int ReservedTemplateLines = 5;

        private static readonly System.Text.CompositeFormat s_tooltipShowHideFormat =
            CompositeFormat.Parse(PromptPlusResources.TooltipShowHide);
        private static readonly System.Text.CompositeFormat s_tooltipCancelEscFormat =
            CompositeFormat.Parse(PromptPlusResources.TooltipCancelEsc);
        private static readonly System.Text.CompositeFormat s_tooltipChartBarSwitchLayoutFormat =
            CompositeFormat.Parse(PromptPlusResources.TooltipChartBarSwitchLayout);
        private static readonly System.Text.CompositeFormat s_tooltipChartBarSwitchLegendFormat =
            CompositeFormat.Parse(PromptPlusResources.TooltipChartBarSwitchLegend);
        private static readonly System.Text.CompositeFormat s_tooltipChartBarSwitchOrderFormat =
            CompositeFormat.Parse(PromptPlusResources.TooltipChartBarSwitchOrder);

        private readonly Dictionary<ChartBarStyles, Style> _optStyles;
        private CultureInfo _culture;
        private List<ChartItem> _items = [];
        private ChartBarType _chartBarType = ChartBarType.Fill;
        private ChartBarLayout _layout = ChartBarLayout.Standard;
        private ChartBarOrder _order = ChartBarOrder.None;
        private Func<ChartItem, string>? _changeDescription;
        private Func<ChartItem, Task<string>>? _changeDescriptionAsync;
        private Func<ChartItem, (bool, string?)>? _predicateValidSelect;
        private Func<ChartItem, Task<(bool, string?)>>? _predicateValidSelectAsync;
        private bool _hasLegends;
        private bool _showLegends;
        private bool _enableLayoutSwitcher = true;
        private bool _enableOrderingSwitcher = true;
        private byte _width;
        private byte _fractionalDigits = 2;
        private double _totalValue;
        private int _pageSize;
        private int _effectivePageSize;
        private Paginator<ChartItem>? _localPaginator;
        private HideChart _hideChart = HideChart.None;
        private List<string> _toggleTooltips = [];
        private int _indexTooltip;
        private int _sequence;
        // Dedicated counter for auto-generated item ids — separate from _sequence (which only
        // advances when a color is auto-assigned) so every AddItem call gets a stable, insertion-
        // ordered id regardless of whether a color was passed explicitly.
        private int _itemIdSequence;
        private string? _title;
        private TextAlignment _titleAlignment = TextAlignment.Center;
        private char _barOn = ' ';
        private double _ticketStep;
        private int _maxLengthLabel;
        private int _maxLabelDisplayWidth;
        private int _maxShowLengthLabel;


        public ChartBarControl(bool isWidget, IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions)
            : base(isWidget, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<ChartBarStyles>(console.CurrentStyle);
            _culture = ConfigPrompt.DefaultCulture;
            _pageSize = isWidget ? byte.MaxValue : (byte)0;
            _width = ConfigPrompt.ChartWidth;
            _maxShowLengthLabel = 0; // 0 = no truncation (show full labels)
        }

        #region IChartBarControl

        public IChartBarControl Layout(ChartBarLayout layout = ChartBarLayout.Standard)
        {
            _layout = layout;
            return this;
        }

        public IChartBarControl Culture(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            if (!culture.Name.ExistsCulture())
            {
                throw new CultureNotFoundException(culture.Name);
            }
            _culture = culture;
            return this;
        }

        public IChartBarControl BarType(ChartBarType type = ChartBarType.Fill)
        {
            _chartBarType = type;
            return this;
        }

        public IChartBarControl Title(string title, TextAlignment alignment = TextAlignment.Center)
        {
            ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));
            _title = title;
            _titleAlignment = alignment;
            return this;
        }

        public IChartBarControl Width(byte value)
        {
            if (value < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Width must be at least 10.");
            }
            _width = value;
            return this;
        }

        public IChartBarControl Styles(ChartBarStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public IChartBarControl AddItem(string label, double value, Color? colorBar = null, string? id = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(label, nameof(label));

            // Zero-padded so a lexicographic string sort (e.g. ChartBarOrder.LabelAsc-style ties,
            // or any future feature that orders by Id) still matches insertion order regardless of
            // how many items are added — "0000000002" sorts before "0000000010", "2" would not.
            var itemId = id ?? (_itemIdSequence++).ToString("D10", CultureInfo.InvariantCulture);

            if (!colorBar.HasValue)
            {
                colorBar = (Color)(15 - (_sequence % 16));
                _sequence++;
            }

            var item = new ChartItem(itemId, label, value, colorBar);
            _items.Add(item);

            return this;
        }

        public IChartBarControl MaxLengthLabel(byte value = 0)
        {
            // When value is 0, labels are not truncated (show full length)
            // When value > 0, labels are truncated to the specified length
            _maxShowLengthLabel = value;
            return this;
        }



        public IChartBarControl ChangeDescription(Func<ChartItem, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            _changeDescriptionAsync = null;
            return this;
        }

        public IChartBarControl ChangeDescriptionAsync(Func<ChartItem, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescriptionAsync = value;
            _changeDescription = null;
            return this;
        }

        public IChartBarControl Interaction<T>(IEnumerable<T> items, Action<T, IChartBarControl> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);

            foreach (var item in items)
            {
                interactionAction.Invoke(item, this);
            }
            return this;
        }

        public IChartBarControl FractionalDigits(byte value)
        {
            if (value > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "FractionalDigits must be between 0 and 5.");
            }
            _fractionalDigits = value;
            return this;
        }

        public IChartBarControl OrderBy(ChartBarOrder order)
        {
            _order = order;
            return this;
        }

        public IChartBarControl ShowLegends(bool value = true)
        {
            _hasLegends = value;
            _showLegends = value;
            return this;
        }

        public IChartBarControl EnableLayoutSwitcher(bool value = true)
        {
            _enableLayoutSwitcher = value;
            return this;
        }

        public IChartBarControl EnableOrderingSwitcher(bool value = true)
        {
            _enableOrderingSwitcher = value;
            return this;
        }

        public IChartBarControl HideElements(HideChart value)
        {
            _hideChart = value;
            return this;
        }

        public IChartBarControl PageSize(byte value)
        {
            _pageSize = value;
            return this;
        }

        public IChartBarControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        public IChartBarControl PredicateSelected(Func<ChartItem, bool> validSelect)
        {
            ArgumentNullException.ThrowIfNull(validSelect);
            _predicateValidSelect = item => (validSelect(item), null);
            _predicateValidSelectAsync = null;
            return this;
        }

        public IChartBarControl PredicateSelectedAsync(Func<ChartItem, Task<bool>> validSelect)
        {
            ArgumentNullException.ThrowIfNull(validSelect);
            _predicateValidSelectAsync = async item => (await validSelect(item), (string?)null);
            _predicateValidSelect = null;
            return this;
        }

        public IChartBarControl PredicateSelected(Func<ChartItem, (bool, string?)> validSelect)
        {
            ArgumentNullException.ThrowIfNull(validSelect);
            _predicateValidSelect = validSelect;
            _predicateValidSelectAsync = null;
            return this;
        }

        public IChartBarControl PredicateSelectedAsync(Func<ChartItem, Task<(bool, string?)>> validSelect)
        {
            ArgumentNullException.ThrowIfNull(validSelect);
            _predicateValidSelectAsync = validSelect;
            _predicateValidSelect = null;
            return this;
        }

        #endregion

        #region IChartBarWidget

        IChartBarWidget IChartBarWidget.Layout(ChartBarLayout layout)
        {
            Layout(layout);
            return this;
        }

        IChartBarWidget IChartBarWidget.Culture(CultureInfo culture)
        {
            Culture(culture);
            return this;
        }

        IChartBarWidget IChartBarWidget.BarType(ChartBarType type)
        {
            BarType(type);
            return this;
        }

        IChartBarWidget IChartBarWidget.Title(string title, TextAlignment alignment)
        {
            Title(title, alignment);
            return this;
        }

        IChartBarWidget IChartBarWidget.Width(byte value)
        {
            Width(value);
            return this;
        }

        IChartBarWidget IChartBarWidget.Styles(ChartBarStyles styleType, Style style)
        {
            Styles(styleType, style);
            return this;
        }

        IChartBarWidget IChartBarWidget.AddItem(string label, double value, Color? colorBar, string? id)
        {
            AddItem(label, value, colorBar, id);
            return this;
        }

        IChartBarWidget IChartBarWidget.MaxLengthLabel(byte value)
        {
            MaxLengthLabel(value);
            return this;
        }



        IChartBarWidget IChartBarWidget.Interaction<T>(IEnumerable<T> items, Action<T, IChartBarWidget> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);

            foreach (var item in items)
            {
                interactionAction.Invoke(item, this);
            }
            return this;
        }

        IChartBarWidget IChartBarWidget.FractionalDigits(byte value)
        {
            FractionalDigits(value);
            return this;
        }

        IChartBarWidget IChartBarWidget.OrderBy(ChartBarOrder order)
        {
            OrderBy(order);
            return this;
        }

        IChartBarWidget IChartBarWidget.ShowLegends(bool value)
        {
            ShowLegends(value);
            return this;
        }

        IChartBarWidget IChartBarWidget.HideElements(HideChart value)
        {
            HideElements(value);
            return this;
        }

        #endregion

        #region BaseControlPrompt Implementation

        public override void InitControl(CancellationToken cancellationToken)
        {
            // Validate items
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("No items to show in chart. Use AddItem() to add data.");
            }

            // Calculate ticket step for bar rendering
            double maxValue = _items.Max(x => x.Value);
            _ticketStep = maxValue == 0 ? 1 : _width / maxValue;

            // Calculate max label length. MaxLengthLabel's public contract is a count of
            // symbols/runes (documented as "characters"), not display columns — counted by rune so a
            // CJK supplementary-plane surrogate pair is never counted as 2.
            _maxLengthLabel = _items.Max(x => DisplayWidthHelpers.CountRunes(x.Label));

            // Apply label truncation if set (0 = no truncation)
            if (_maxShowLengthLabel > 0 && _maxLengthLabel > _maxShowLengthLabel)
            {
                _maxLengthLabel = _maxShowLengthLabel;
            }
            else if (_maxShowLengthLabel == 0)
            {
                // No truncation - use full label length
                _maxShowLengthLabel = int.MaxValue;
            }

            // Column alignment across items must be based on the DISPLAY WIDTH of each label once
            // truncated to _maxLengthLabel runes, not on _maxLengthLabel itself — an ASCII label and a
            // CJK label truncated to the same rune count can occupy very different terminal columns.
            _maxLabelDisplayWidth = _items.Max(x =>
            {
                string truncated = DisplayWidthHelpers.TruncateToRuneCount(x.Label, _maxLengthLabel);
                return truncated.GetDisplayLength() is { Length: > 0 } d ? d[0] : 0;
            });

            // Set bar character based on type
            _barOn = _chartBarType switch
            {
                ChartBarType.Fill => ' ',
                ChartBarType.Light => GetSymbol(SymbolType.ChartLight)[0],
                ChartBarType.Square => GetSymbol(SymbolType.ChartSquare)[0],
                _ => throw new NotImplementedException($"ChartBarType {_chartBarType} not implemented"),
            };

            // Calculate total value
            _totalValue = _items.Sum(x => Math.Round(x.Value, _fractionalDigits));

            // Order items
            ChangeOrder();

            // Auto-assign colors and calculate percentages
            int indexColor = 15;
            foreach (var item in _items)
            {
                if (!item.Color.HasValue)
                {
                    if (Color.FromInt32(indexColor) == Color.FromConsoleColor(ConsoleHandler.BackgroundColor))
                    {
                        indexColor--;
                        if (indexColor < 0)
                        {
                            indexColor = 15;
                        }
                    }
                    item.Color = Color.FromInt32(indexColor);
                    indexColor--;
                }

                item.Percent = Math.Round((100 * item.Value) / _totalValue, _fractionalDigits);
                item.StyleBar = _chartBarType == ChartBarType.Fill
                    ? new Style(item.Color.Value, item.Color.Value)
                    : ConsoleHandler.CurrentStyle.ForeGround(item.Color.Value);
            }

            // Calculate effective page size
            _effectivePageSize = ComputeEffectivePageSize(ReservedTemplateLines, (byte)_pageSize);

            // Initialize Paginator
            _localPaginator = new Paginator<ChartItem>(
                FilterMode.Disabled,
                _items,
                _effectivePageSize,
                Optional<ChartItem>.Empty(),
                (item1, item2) => item1.Id == item2.Id,
                null,
                null,
                null);

            if (_localPaginator.SelectedItem == null)
            {
                _localPaginator.FirstItem();
            }

            // Load tooltips for interactive mode
            if (!IsWidget)
            {
                LoadTooltipToggle();
            }
        }

        public override bool TryResult(CancellationToken cancellationToken)
        {
            if (IsWidget)
            {
                return false;
            }

            bool oldCursor = ConsoleHandler.CursorVisible;
            ConsoleHandler.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    KeyPressResult press = ReadNextKey(true, cancellationToken);

                    if (press.IsResize || press.IsCancelled)
                    {
                        if (press.IsCancelled)
                        {
                            _indexTooltip = 0;
                            ResultCtrl = new ResultPrompt<ChartItem?>(_localPaginator?.SelectedItem, true);
                        }
                        break;
                    }

                    ConsoleKeyInfo keyInfo = press.Key;

                    // Abort key
                    if (IsAbortKeyPress(keyInfo))
                    {
                        _indexTooltip = 0;
                        ResultCtrl = new ResultPrompt<ChartItem?>(_localPaginator?.SelectedItem, true);
                        break;
                    }

                    // Enter to select
                    if (keyInfo.IsPressEnterKey())
                    {
                        _indexTooltip = 0;

                        // Validate selection
                        (bool ok, string? message) = ValidateSelection(_localPaginator?.SelectedItem!).Result;

                        if (!ok)
                        {
                            SetError(message ?? PromptPlusResources.PredicateSelectInvalid);
                            break;
                        }

                        ResultCtrl = new ResultPrompt<ChartItem?>(_localPaginator?.SelectedItem, false);
                        break;
                    }

                    // Tooltip toggle
                    if (IsTooltipToggerKeyPress(keyInfo))
                    {
                        _indexTooltip++;
                        if (_indexTooltip > _toggleTooltips.Count)
                        {
                            _indexTooltip = 0;
                        }
                        break;
                    }

                    // Tooltip show/hide
                    if (CheckTooltipShowHideKeyPress(keyInfo))
                    {
                        break;
                    }

                    // Switch Layout
                    if (ConfigPrompt.HotKeyChartBarSwitchLayout.Equals(keyInfo) && _enableLayoutSwitcher)
                    {
                        ChartBarLayout targetLayout = _layout == ChartBarLayout.Standard ? ChartBarLayout.Stacked : ChartBarLayout.Standard;

                        // Only switch to Stacked if there's enough space
                        if (targetLayout == ChartBarLayout.Stacked && !CanRenderStackedLayout())
                        {
                            // Cannot switch to stacked layout - not enough width
                            _indexTooltip = 0;
                            break;
                        }

                        _layout = targetLayout;
                        _localPaginator?.FirstItem();
                        _indexTooltip = 0;
                        break;
                    }

                    // Switch Legend
                    if (ConfigPrompt.HotKeyChartBarSwitchLegend.Equals(keyInfo) && _hasLegends)
                    {
                        _showLegends = !_showLegends;
                        _indexTooltip = 0;
                        break;
                    }

                    // Switch Order
                    if (ConfigPrompt.HotKeyChartBarSwitchOrder.Equals(keyInfo) && _enableOrderingSwitcher)
                    {
                        int intOrder = (int)_order;
                        intOrder++;
                        if (!Enum.IsDefined(typeof(ChartBarOrder), intOrder))
                        {
                            intOrder = 0;
                        }
                        _order = (ChartBarOrder)intOrder;
                        ChangeOrder();
                        _indexTooltip = 0;
                        break;
                    }

                    // Ctrl+Home
                    if (keyInfo.IsPressCtrlHomeKey())
                    {
                        if (!_localPaginator!.Home())
                        {
                            continue;
                        }
                        _indexTooltip= 0;
                        break;
                    }

                    // Ctrl+End
                    if (keyInfo.IsPressCtrlEndKey())
                    {
                        if (!_localPaginator!.End())
                        {
                            continue;
                        }
                        _indexTooltip = 0;
                        break;
                    }

                    // Navigation: Up arrow / Left arrow  when stacked 
                    if (keyInfo.IsPressUpArrowKey() || (keyInfo.IsPressLeftArrowKey() && _layout == ChartBarLayout.Stacked))
                    {
                        if (_localPaginator!.IsFirstPageItem)
                        {
                            _localPaginator!.PreviousPage(IndexOption.LastItem);
                        }
                        else
                        {
                            _localPaginator!.PreviousItem();
                        }
                        _indexTooltip = 0;
                        break;
                    }

                    // Navigation: Down arrow / Right arrow  when stacked
                    if (keyInfo.IsPressDownArrowKey() || (keyInfo.IsPressRightArrowKey() && _layout == ChartBarLayout.Stacked))
                    {
                        if (_localPaginator!.IsLastPageItem)
                        {
                            _localPaginator.NextPage(IndexOption.FirstItem);
                        }
                        else
                        {
                            _localPaginator.NextItem();
                        }
                        _indexTooltip = 0;
                        break;
                    }

                    // Page Up 
                    if (keyInfo.IsPressPageUpKey())
                    {
                        if (_localPaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                        {
                            _indexTooltip = 0;
                            break;
                        }
                    }

                    // Page Down 
                    if (keyInfo.IsPressPageDownKey())
                    {
                        if (_localPaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                        {
                            _indexTooltip = 0;
                            break;
                        }
                    }
                }
            }
            finally
            {
                ConsoleHandler.CursorVisible = oldCursor;
            }

            return ResultCtrl != null;
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            // Re-evaluate the effective page size every frame so the visible items count
            // stays in sync with the current console height (after any terminal resize).
            int targetPageSize = ComputeEffectivePageSize(ReservedTemplateLines, (byte)_pageSize);
            if (targetPageSize != _effectivePageSize)
            {
                _effectivePageSize = targetPageSize;
                _localPaginator?.UpdatePageSize(_effectivePageSize);
            }

            if (!IsWidget)
            {
                WritePrompt(screenBuffer, _optStyles[ChartBarStyles.Prompt]);
                WriteAnswer(screenBuffer);
                WriteDescription(screenBuffer);
            }

            WriteChart(screenBuffer);

            if (!IsWidget)
            {
                WriteTooltip(screenBuffer);
                WriteError(screenBuffer, _optStyles[ChartBarStyles.Error]);
            }
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer, _optStyles[ChartBarStyles.Prompt]);

            string answer = ResultCtrl!.Value.IsAborted
                ? OptionsControl.EnabledAbortKeyValue ? PromptPlusResources.CanceledKey : string.Empty
                : ResultCtrl.Value.Content?.Label ?? string.Empty;

            screenBuffer.WriteLine(answer, _optStyles[ChartBarStyles.Answer]);

            return true;
        }

        public override void FinalizeControl()
        {
            // Cleanup if needed
        }

        #endregion

        #region Helper Methods

        private void ChangeOrder()
        {
            // Preserve current item before reordering
            ChartItem? currentItem = _localPaginator?.SelectedItem;

            _items = _order switch
            {
                // Documented as "No sorting applied; items appear in insertion order" — must be a
                // true no-op. Sorting by Id here was a real bug: auto-generated ids used to be
                // random GUIDs (fixed alongside this, see AddItem), so "None" silently randomized
                // item order on every run instead of preserving insertion order.
                ChartBarOrder.None => _items,
                ChartBarOrder.Smallest => [.. _items.OrderBy(x => x.Value)],
                ChartBarOrder.Highest => [.. _items.OrderByDescending(x => x.Value)],
                ChartBarOrder.LabelAsc => [.. _items.OrderBy(x => x.Label)],
                ChartBarOrder.LabelDesc => [.. _items.OrderByDescending(x => x.Label)],
                _ => throw new NotImplementedException($"ChartBarOrder {_order} not implemented"),
            };

            // Reinitialize paginator with reordered items
            if (_localPaginator != null)
            {
                Optional<ChartItem> defaultValue = currentItem != null
                    ? Optional<ChartItem>.Set(currentItem)
                    : Optional<ChartItem>.Empty();
                
                _localPaginator.UpdateCollection(_items, defaultValue);

                if (_localPaginator.SelectedItem == null)
                {
                    _localPaginator.FirstItem();
                }
            }
        }

        private async Task<(bool, string?)> ValidateSelection(ChartItem item)
        {
            if (_predicateValidSelectAsync != null)
            {
                return await _predicateValidSelectAsync(item);
            }
            if (_predicateValidSelect != null)
            {
                return _predicateValidSelect(item);
            }
            return (true, null);
        }

        private static string GetTooltipMain()
        {
            StringBuilder tooltip = new();
            tooltip.Append(PromptPlusResources.TooltipEnterFinish);
            tooltip.Append('.');
            tooltip.Append(PromptPlusResources.TooltipBaseNavegate);
            tooltip.Append('.');
            return tooltip.ToString();
        }

        /// <summary>
        /// Validates if the console has enough width to render the stacked layout.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the console width is sufficient to render all chart items 
        /// in stacked layout; otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// The stacked layout requires a minimum console width calculated as the maximum value 
        /// between the chart width (<see cref="_width"/>) and the number of items (<see cref="_items"/>.Count), 
        /// plus a margin of 2 characters. If the current console width is insufficient, 
        /// the layout switch to stacked mode will be prevented to avoid rendering issues.
        /// </remarks>
        private bool CanRenderStackedLayout()
        {
            // Stacked layout requires enough console width to render all items
            // The minimum required width is the chart width (_width) plus some margin
            // Also need to ensure we have at least one character per item
            int minimumWidth = Math.Max(_width, _items.Count);

            // Add some buffer for margins and ensure we don't exceed console width
            return ConsoleHandler.Width >= minimumWidth + 2; // +2 for minimal margins
        }

        private void LoadTooltipToggle()
        {
            List<string> lsttooltips =
            [
                GetTooltipMain()
            ];

            // Navigation tooltip
            lsttooltips.Add(PromptPlusResources.TooltipPages);

            // Enter to select
            lsttooltips.Add(PromptPlusResources.TooltipEnterSelect);

            // Switch Layout (if enabled)
            if (_enableLayoutSwitcher)
            {
                lsttooltips.Add(string.Format(_culture, s_tooltipChartBarSwitchLayoutFormat, ConfigPrompt.HotKeyChartBarSwitchLayout));
            }

            // Switch Legend (if has legends)
            if (_hasLegends)
            {
                lsttooltips.Add(string.Format(_culture, s_tooltipChartBarSwitchLegendFormat, ConfigPrompt.HotKeyChartBarSwitchLegend));
            }

            // Switch Order (if enabled)
            if (_enableOrderingSwitcher)
            {
                lsttooltips.Add(string.Format(_culture, s_tooltipChartBarSwitchOrderFormat, ConfigPrompt.HotKeyChartBarSwitchOrder));
            }

            // Abort key (if enabled)
            if (OptionsControl.EnabledAbortKeyValue)
            {
                lsttooltips.Add(string.Format(_culture, s_tooltipCancelEscFormat, ConfigPrompt.HotKeyAbortKeyPress));
            }

            // Tooltip toggle key
            lsttooltips.Add(string.Format(_culture, s_tooltipShowHideFormat, ConfigPrompt.HotKeyTooltipShowHide));

            _toggleTooltips = lsttooltips;
        }

        private string ValueToString(double value)
        {
            return Math.Round(value, _fractionalDigits).ToString($"F{_fractionalDigits}", _culture);
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            ChartItem? currentItem = _localPaginator?.SelectedItem;
            if (currentItem == null)
            {
                screenBuffer.WriteLine(string.Empty, _optStyles[ChartBarStyles.Answer]);
                return;
            }

            string answer = currentItem.Label;

            // Add value when applicable (not hidden)
            if (!_hideChart.HasFlag(HideChart.Values))
            {
                answer = $"{answer}: {ValueToString(currentItem.Value)}";
            }

            // Add percentage when applicable (not hidden)
            if (!_hideChart.HasFlag(HideChart.Percentage))
            {
                answer = $"{answer} ({ValueToString(currentItem.Percent)}%)";
            }

            if (_layout != ChartBarLayout.Standard && !_hasLegends)
            {
                var stylemarkcolor = (currentItem.StyleBar ?? ConsoleHandler.CurrentStyle).Background(ConsoleHandler.CurrentStyle.Background);
                screenBuffer.Write(GetSymbol(SymbolType.ChartLabel), stylemarkcolor);
                screenBuffer.Write(" ", ConsoleHandler.CurrentStyle);
            }
            screenBuffer.Write(answer, _optStyles[ChartBarStyles.Answer]);
            screenBuffer.SavePromptCursor();
            screenBuffer.WriteLine(string.Empty, _optStyles[ChartBarStyles.Answer]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            ChartItem? currentItem = _localPaginator?.SelectedItem;
            string? desc = OptionsControl.DescriptionValue;
            if (_changeDescriptionAsync is not null && currentItem is not null)
            {
                desc = _changeDescriptionAsync.Invoke(currentItem)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            else if (_changeDescription is not null && currentItem is not null)
            {
                desc = _changeDescription.Invoke(currentItem);
            }
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[ChartBarStyles.Prompt]);
            }
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            if (_indexTooltip >= _toggleTooltips.Count)
            {
                _indexTooltip = 0;
            }
            string? tooltip = _indexTooltip < _toggleTooltips.Count ? _toggleTooltips[_indexTooltip] : null;
            if (!string.IsNullOrEmpty(tooltip))
            {
                tooltip = $"{ConfigPrompt.HotKeyTooltip}:{PromptPlusResources.TooltipBase}.{tooltip}";
                if (!tooltip.EndsWith('.'))
                {
                    tooltip = $"{tooltip}.";
                }
                screenBuffer.WriteLine(tooltip, _optStyles[ChartBarStyles.Prompt]);
            }
        }

        // Helper method to write a line with alignment.
        private void WriteLineAlign(BufferScreen screenBuffer, string text, TextAlignment alignment, Style style)
        {
            screenBuffer.WriteLine(DisplayWidthHelpers.AlignLine(text, _width, alignment), style);
        }

        private void WriteChart(BufferScreen screenBuffer)
        {
            // Write title if set
            if (!string.IsNullOrEmpty(_title) && !_hideChart.HasFlag(HideChart.Title))
            {
                WriteLineAlign(screenBuffer, _title, _titleAlignment, _optStyles[ChartBarStyles.ChartTitle]);
            }

            // Get visible items for current page from Paginator
            IEnumerable<ChartItem> visibleItems;
            if (_localPaginator != null && !IsWidget)
            {
                visibleItems = _localPaginator.GetPageData();
            }
            else
            {
                visibleItems = _items;
            }

            // Render based on layout
            if (_layout == ChartBarLayout.Standard)
            {
                WriteStandardChart(screenBuffer, visibleItems);
            }
            else
            {
                WriteStackedChart(screenBuffer, visibleItems);
            }

            // Write legends if enabled
            if (_showLegends)
            {
                WriteLegends(screenBuffer);
            }

            // Write pagination info if applicable
            if (_localPaginator != null && !IsWidget && _localPaginator.PageCount > 0 && _layout == ChartBarLayout.Standard)
            {
                string template = ConfigPrompt.PaginationTemplateValue(
                    _localPaginator.TotalCountValid,
                    _localPaginator.SelectedPage + 1,
                    _localPaginator.PageCount
                )!;
                screenBuffer.WriteLine(template, _optStyles[ChartBarStyles.Pagination]);
            }
        }

        private void WriteStandardChart(BufferScreen screenBuffer, IEnumerable<ChartItem> items)
        {
            foreach (var item in items)
            {
                bool isSelected = !IsWidget && item == _localPaginator?.SelectedItem;
                Style labelStyle = isSelected ? _optStyles[ChartBarStyles.Selected] : _optStyles[ChartBarStyles.ChartLabel];

                // Truncate to _maxLengthLabel runes (retention, char-count contract preserved), then
                // pad to _maxLabelDisplayWidth columns (alignment, computed from real display width)
                // so ASCII and CJK labels line up their bars on the same column.
                string label = DisplayWidthHelpers.TruncateToRuneCount(item.Label, _maxLengthLabel);
                int labelDisplayWidth = label.GetDisplayLength() is { Length: > 0 } ld ? ld[0] : 0;
                if (labelDisplayWidth < _maxLabelDisplayWidth)
                {
                    label += new string(' ', _maxLabelDisplayWidth - labelDisplayWidth);
                }

                // Write label
                if (!_showLegends)
                {
                    if (isSelected)
                    {
                        screenBuffer.Write(GetSymbol(SymbolType.Selector), labelStyle);
                        screenBuffer.Write(" ", ConsoleHandler.CurrentStyle);
                    }
                    else
                    {
                        screenBuffer.Write("  ", ConsoleHandler.CurrentStyle);
                    }
                    screenBuffer.Write(label, labelStyle);
                    screenBuffer.Write(" ", labelStyle);
                }

                // Calculate bar width
                int barWidth = (int)(item.Value * _ticketStep);
                if (barWidth > _width) barWidth = _width;

                // Write bar
                if (barWidth > 0)
                {
                    string bar = new(_barOn, barWidth);
                    screenBuffer.Write(bar, item.StyleBar ?? ConsoleHandler.CurrentStyle);
                }

                if (!_showLegends)
                {
                    // Write value if not hidden
                    if (!_hideChart.HasFlag(HideChart.Values))
                    {
                        screenBuffer.Write($" {ValueToString(item.Value)}", _optStyles[ChartBarStyles.ChartValue]);
                    }

                    // Write percentage if not hidden
                    if (!_hideChart.HasFlag(HideChart.Percentage))
                    {
                        screenBuffer.Write($" ({ValueToString(item.Percent)}%)", _optStyles[ChartBarStyles.ChartPercent]);
                    }
                }
                screenBuffer.WriteLine("", labelStyle);
            }
        }

        private void WriteStackedChart(BufferScreen screenBuffer, IEnumerable<ChartItem> items)
        {
            double tkt = _width / _totalValue;

            foreach (ChartItem item in _items)
            {
                int length = (int)(tkt * item.Value);
                if (tkt == 0)
                {
                    tkt = 1;
                }
                screenBuffer.Write(new string(_barOn, length), item.StyleBar!.Value);
            }
            screenBuffer.WriteLine("", ConsoleHandler.CurrentStyle);
        }

        private void WriteLegends(BufferScreen screenBuffer)
        {
            screenBuffer.WriteLine("", ConsoleHandler.CurrentStyle);
            foreach (var item in _items)
            {
                bool isSelected = (_localPaginator?.SelectedItem.Id == item.Id);
                if (isSelected)
                {
                    screenBuffer.Write(GetSymbol(SymbolType.Selector), _optStyles[ChartBarStyles.Selected]);
                    screenBuffer.Write(" ", ConsoleHandler.CurrentStyle);
                }
                else
                {
                    screenBuffer.Write("  ", ConsoleHandler.CurrentStyle);
                }
                screenBuffer.Write(GetSymbol(SymbolType.ChartLabel), item.StyleBar ?? ConsoleHandler.CurrentStyle);
                screenBuffer.Write(" ", ConsoleHandler.CurrentStyle);
                screenBuffer.Write($"{item.Label}: ", _optStyles[ChartBarStyles.ChartLabel]);
                screenBuffer.Write($"{ValueToString(item.Value)} ", _optStyles[ChartBarStyles.ChartValue]);
                screenBuffer.WriteLine($"({ValueToString(item.Percent)}%)", _optStyles[ChartBarStyles.ChartPercent]);
            }
        }

        #endregion
    }
}
