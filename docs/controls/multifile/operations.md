<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiFile — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [MultiFile — Styles →](styles.md)

---

How the `MultiFile` control behaves while it is running: keyboard, tree navigation, checking, cascade
and recursive marking, the count range, validation, history, and the values it returns.

---

## Anatomy of the control

```
Check files or folders: Program.cs  1.2 KB ← prompt + live answer (path + size, follows the cursor)
Space to check, Enter to confirm         ← description (optional)
C:\Projects                              ← root folder
+- ▼ [x] src                             ← expanded, checked folder
│    › [x] Program.cs      1.2 KB        ← focused, checked file + size
│      [ ] Startup.cs      3.4 KB        ← unchecked file
+- ▶ [ ] docs                            ← collapsed folder
+- [x] README.md           0.8 KB
Checked: 3                               ← tagged info (count)
Page 1/2                                 ← pagination
Space: check  Enter: confirm  Esc: cancel ← tooltip
```

The live answer shows the size next to the path for a focused **file** (omitted for folders, and
omitted entirely when [`HideSize`](methods.md#hidesize) is set), reachable via
`Home`/`End`/`←`/`→` scrolling if the path is too long to fit. The final answer after **Enter**
shows the checked-items summary (paths only, no size).

Every region can be recolored — see [Styles](styles.md).

---

## Tree navigation

`MultiFile` is a **lazy** tree browser: a folder's children are read only when you expand it, and
released when you collapse it. Checked entries are tracked by their full path, so a selection
survives collapsing and re-expanding its branch.

| Key | Action |
|---|---|
| `↑` / `↓` | Move focus up / down |
| `→` / `+` | Expand the focused folder |
| `←` / `-` | Collapse the focused folder |
| `Space` | Check / uncheck (recursive on folders by default — see below) |
| `Ctrl+Space` | Recursive folder mark, when [`RecursiveMarkWithCtrlSpace`](methods.md#recursivemarkwithctrlspace) is on |
| `F2` | Toggle all (check / uncheck everything currently loaded) |
| `F4` | Wildcard / recursive select under the focused folder |
| `Page Up` / `Page Down` | Jump one page |
| `Home` / `End` | First / last visible item |
| `Enter` | Confirm the checked entries (subject to the range) |
| `Esc` | Abort → `IsAborted == true` |
| `Shift+F3` | Toggle the summary between full path and short name |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

---

## Checking, cascade & recursive marking

How **Space** behaves on a folder depends on two settings:

| `CascadeCheck` | `RecursiveMarkWithCtrlSpace` | `Space` on a folder | `Ctrl+Space` on a folder |
|---|---|---|---|
| `true` (default) | `false` (default) | Recursively checks the folder and everything under it | — |
| `true` | `true` | Toggles only the folder itself | Recursively checks the folder and everything under it |
| `false` | — | Toggles only the folder itself | — |

- [`CascadeCheck`](methods.md#cascadecheck) governs whether folder state propagates to descendants.
- [`RecursiveMarkWithCtrlSpace`](methods.md#recursivemarkwithctrlspace) moves the recursive action to
  **Ctrl+Space** and keeps plain **Space** for single-item toggling.
- **F4** performs a wildcard/recursive select under the focused folder, and **F2** toggles all
  currently loaded entries at once.

On files, **Space** always toggles just that file.

---

## What can be checked

- [`SearchPattern`](methods.md#searchpattern) filters **files** by wildcard; directories always show.
- [`OnlyFolders`](methods.md#onlyfolders) hides files entirely.
- [`SelectFilesOnly`](methods.md#selectfilesonly) lets folders be expanded and browsed but not
  checked — only files enter the result.
- [`ShowHidden`](methods.md#showhidden) / [`ShowSystem`](methods.md#showsystem) add Hidden/System
  entries; both off by default.

---

## The count range

[`Range(min, max?)`](methods.md#range) enforces how many items must be checked:

- **Enter** is blocked until at least `min` items are checked.
- If `max` is set, checking beyond it is prevented (or reported).
- Omit `max` (pass `null`) for a minimum-only rule.

---

## Validation with a predicate

[`PredicateChecked`](methods.md#predicatechecked) /
[`PredicateCheckedAsync`](methods.md#predicatecheckedasync) decides whether an item may be checked:

- For an **individual** toggle, a rejection shows the optional message as an error line and the item
  stays unchecked.
- During a **mass** selection (recursive folder, F4 wildcard, F2 check-all), rejected items are
  **silently skipped** — no error is shown.
- The async predicate is awaited synchronously (blocking) on the UI thread for a single toggle, but
  runs on a **background thread** during recursive selection — keep it thread-safe and UI-free.

---

## Full-path display

The summary shows either each entry's short name or its full path. [`ShowFullPath`](methods.md#showfullpath)
sets the initial choice; the user can flip it with **Shift+F3**. This affects only the *display* —
each `FileItem.FullPath` always carries the complete path.

---

## Initial values & history

- [`Default(fullPaths)`](methods.md#default) pre-checks the given paths and expands the tree to the
  first one under the [`Root`](methods.md#root).
- With [`EnableHistory`](methods.md#enablehistory), the confirmed paths are stored on disk; on the
  next run they become the defaults and the tree expands to the first.
- History options (`MinPrefixLength`, `MaxItems`, `ExpirationTime`, `FilterType`, `PageSize`) match
  the [Input history options](../input/methods.md#enablehistory).

---

## The `FileItem` results

`Run()` returns `ResultPrompt<FileItem[]>`. Each `FileItem` is a sealed class (not
`System.IO.FileInfo` — there is no `FullName`):

| Member | Type | Meaning |
|---|---|---|
| `FullPath` | `string` | Full path on disk (`ToString()` returns this) |
| `Name` | `string` | Display name of the entry |
| `IsDirectory` | `bool` | `true` for folders |
| `Length` | `long` | Size in bytes; `0` for directories |
| `LastWriteTime` | `DateTime` | Last write timestamp |

```csharp
var result = PromptPlus.Controls.MultiFile("Check").Root(root).Run();
if (!result.IsAborted)
{
    foreach (var f in result.Content)
        PromptPlus.Console.WriteLine($"{f.FullPath} (dir: {f.IsDirectory}, size: {f.Length})");
}
```

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `MultiFile` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must confirm |
| `HideAfterFinish(true)` | Erases the tree after confirm — the whole control is erased, not just the interactive part |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

`PageSize` can be set per control ([`PageSize`](methods.md#pagesize)) or globally
(`PromptPlus.Config.PageSize`).

---

## Edge cases & gotchas

- **Aborted results** carry an empty `.Content` array — iterating is still safe, but branch on
  `IsAborted` when it matters.
- **`FileItem` is not `FileInfo`** — use `FullPath`, not `FullName`.
- **`Length` is `0` for directories** — check `IsDirectory` before treating it as a file size.
- **Recursive marking may touch many entries** — a predicate keeps unwanted files out silently.
- **Large roots are fine** — lazy loading means only expanded folders are read.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key reference
- [File → Operations](../file/operations.md) — the single-selection sibling
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
