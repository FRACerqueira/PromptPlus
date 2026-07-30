<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **File — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [File — Styles →](styles.md)

---

How the `File` control behaves while it is running: keyboard, tree navigation, filtering, selection
rules, history, and the values it returns.

---

## Anatomy of the control

```
Select a file or folder: index.md  1.2 KB ← prompt + live answer (path + size, follows the cursor)
Right/+ expand, Left/- collapse          ← description (optional)
C:\Projects                              ← root folder
├─ ▶ src                                 ← collapsed folder (expand symbol)
├─ ▼ docs                                ← expanded folder
│    › index.md          1.2 KB          ← focused item + size column
│      styles.md         3.4 KB
└─ README.md             0.8 KB
Page 1/2                                 ← pagination
Enter: select  Esc: cancel               ← tooltip
```

The answer line shows the size next to the path for a focused **file** (omitted for folders, and
omitted entirely when [`HideSize`](methods.md#hidesize) is set) — the same information as the list
row, but reachable via `Home`/`End`/`←`/`→` scrolling if the path is too long to fit.

Every region can be recolored — see [Styles](styles.md).

---

## Tree navigation

`File` is a **lazy** tree browser: a folder's children are read only when you expand it, and released
when you collapse it. This keeps memory proportional to what is visible, so it is safe to point
[`Root`](methods.md#root) at a large drive.

| Key | Action |
|---|---|
| `↑` / `↓` | Move focus up / down |
| `→` / `+` | Expand the focused folder |
| `←` / `-` | Collapse the focused folder |
| `Page Up` / `Page Down` | Jump one page |
| `Home` / `End` | First / last visible item |
| `Enter` | Confirm the focused entry (subject to the selection rule) |
| `Esc` | Abort → `IsAborted == true` |
| `Shift+F3` | Toggle the answer between full path and short name |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

---

## What is listed

- [`SearchPattern`](methods.md#searchpattern) filters **files** by wildcard (e.g. `*.cs`);
  directories are always shown so you can navigate into them.
- [`OnlyFolders`](methods.md#onlyfolders) hides files entirely — a folder picker.
- [`ShowHidden`](methods.md#showhidden) and [`ShowSystem`](methods.md#showsystem) add entries that
  carry the Hidden/System attribute; both are off by default.
- [`HideSize`](methods.md#hidesize) removes the size column shown next to files.

---

## Selection rules

Pressing **Enter** confirms the focused entry and closes the control — but what counts as a valid
selection depends on the configuration:

- By default, either a **file or a folder** can be confirmed.
- With [`SelectFilesOnly`](methods.md#selectfilesonly), folders can be expanded and browsed, but only
  a **file** can be confirmed.
- With [`OnlyFolders`](methods.md#onlyfolders), files are not listed at all, so only folders are
  available.

The confirmed entry is returned as a [`FileItem`](#the-fileitem-result).

---

## Full-path display

The answer line (and, when enabled, the summary) shows either the entry's short name or its full
path. [`ShowFullPath`](methods.md#showfullpath) sets the initial choice; the user can flip it at any
time with **Shift+F3**. This affects only the *display* — `.Content.FullPath` always carries the
complete path.

---

## Initial selection & history

- [`Default(fullPath)`](methods.md#default) pre-selects an entry and expands the tree down to it when
  the path lies under the [`Root`](methods.md#root).
- With [`EnableHistory`](methods.md#enablehistory), the confirmed path is stored on disk; on the
  next run it becomes the default and the tree expands to it (unless overridden by an explicit
  `Default` with `useDefaultHistory: false`).
- History options (`MinPrefixLength`, `MaxItems`, `ExpirationTime`, `FilterType`, `PageSize`) match
  the [Input history options](../input/methods.md#enablehistory).

---

## The `FileItem` result

`Run()` returns `ResultPrompt<FileItem?>`. `FileItem` is a sealed class (not `System.IO.FileInfo` —
there is no `FullName`):

| Member | Type | Meaning |
|---|---|---|
| `FullPath` | `string` | Full path on disk (`ToString()` returns this) |
| `Name` | `string` | Display name of the entry |
| `IsDirectory` | `bool` | `true` for folders |
| `Length` | `long` | Size in bytes; `0` for directories |
| `LastWriteTime` | `DateTime` | Last write timestamp |

```csharp
var result = PromptPlus.Controls.File("Pick").Root(root).Run();
if (!result.IsAborted && result.Content is not null)
{
    var f = result.Content;
    PromptPlus.Console.WriteLine($"{f.FullPath} (dir: {f.IsDirectory}, size: {f.Length})");
}
```

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `File` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must choose |
| `HideAfterFinish(true)` | Erases the tree after confirm; only the answer remains |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

`PageSize` can be set per control ([`PageSize`](methods.md#pagesize)) or globally
(`PromptPlus.Config.PageSize`).

---

## Edge cases & gotchas

- **Aborted results** carry `.Content == null`. Always branch on `IsAborted` before dereferencing.
- **`FileItem` is not `FileInfo`** — use `FullPath`, not `FullName`.
- **`Length` is `0` for directories** — check `IsDirectory` before treating it as a file size.
- **`SearchPattern` filters files only** — folders always appear so the tree remains navigable.
- **Large roots are fine** — lazy loading means only expanded folders are read.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key reference
- [MultiFile → Operations](../multifile/operations.md) — the multiple-selection sibling
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
