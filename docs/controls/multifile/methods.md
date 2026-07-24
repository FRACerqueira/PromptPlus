<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiFile — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[? Back to Home](../../../README.md) • **Next:** [MultiFile — Operations ?](operations.md)

---

Every fluent method on `IMultiFileControl`. Each returns the same control instance, so calls chain in
any order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.MultiFile(string prompt = "", string? description = null)`,
> which returns `IMultiFileControl`.

**Quick jump:**
[Root](#root) ·
[SearchPattern](#searchpattern) ·
[OnlyFolders](#onlyfolders) ·
[ShowHidden](#showhidden) ·
[ShowSystem](#showsystem) ·
[SelectFilesOnly](#selectfilesonly) ·
[CascadeCheck](#cascadecheck) ·
[RecursiveMarkWithCtrlSpace](#recursivemarkwithctrlspace) ·
[Range](#range) ·
[PredicateChecked](#predicatechecked) ·
[PredicateCheckedAsync](#predicatecheckedasync) ·
[HideSize](#hidesize) ·
[ShowFullPath](#showfullpath) ·
[PageSize](#pagesize) ·
[Default](#default) ·
[EnabledHistory](#enabledhistory) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Choosing the root

### `Root`

```csharp
IMultiFileControl Root(string path)
```

Sets the folder the tree starts at. When not set, the current directory
(`Directory.GetCurrentDirectory()`) is used. Point it at a drive root (e.g. `C:\`) to browse a whole
volume.

```csharp
PromptPlus.Controls.MultiFile("Check").Root(@"C:\Projects").Run();
```

> Throws `ArgumentNullException` if `path` is `null`.

---

## Filtering what is listed

### `SearchPattern`

```csharp
IMultiFileControl SearchPattern(string pattern)
```

Filters **files** by a wildcard pattern; directories are always listed so the tree can be navigated.
Default is `*` (all files).

```csharp
PromptPlus.Controls.MultiFile("Check C# files")
    .Root(root)
    .SearchPattern("*.cs")
    .Run();
```

> Throws `ArgumentNullException` if `pattern` is `null`.

---

### `OnlyFolders`

```csharp
IMultiFileControl OnlyFolders(bool value = true)
```

Lists directories only, hiding all files.

```csharp
PromptPlus.Controls.MultiFile("Check folders").Root(root).OnlyFolders().Run();
```

---

### `ShowHidden`

```csharp
IMultiFileControl ShowHidden(bool value = true)
```

Includes entries marked with the Hidden attribute. Hidden by default.

---

### `ShowSystem`

```csharp
IMultiFileControl ShowSystem(bool value = true)
```

Includes entries marked with the System attribute. Hidden by default.

---

## Check rules

### `SelectFilesOnly`

```csharp
IMultiFileControl SelectFilesOnly(bool value = true)
```

Restricts checking to files. Folders can still be expanded and browsed, but they cannot be checked.

```csharp
PromptPlus.Controls.MultiFile("Check files")
    .Root(root)
    .SearchPattern("*.cs")
    .SelectFilesOnly()
    .Run();
```

---

### `CascadeCheck`

```csharp
IMultiFileControl CascadeCheck(bool value = true)
```

When `true` (default), checking or unchecking a folder propagates the new state to **all its
descendants** (files and subfolders). When `false`, only the folder itself is toggled. Works together
with [`RecursiveMarkWithCtrlSpace`](#recursivemarkwithctrlspace) to decide whether recursive marking
is available and which key triggers it.

```csharp
PromptPlus.Controls.MultiFile("Check (no cascade)")
    .Root(root)
    .CascadeCheck(false)   // Space marks folders as single items
    .Run();
```

---

### `RecursiveMarkWithCtrlSpace`

```csharp
IMultiFileControl RecursiveMarkWithCtrlSpace(bool value = true)
```

Moves the recursive folder marking to **Ctrl+Space**. When enabled, plain **Space** only toggles the
checked state of the focused entry; the recursive select/unselect-everything-under-the-folder action
happens on **Ctrl+Space**. When disabled (default), plain **Space** performs the recursive selection
on folders (if [`CascadeCheck`](#cascadecheck) is `true`).

```csharp
PromptPlus.Controls.MultiFile("Space=item, Ctrl+Space=recursive")
    .Root(root)
    .RecursiveMarkWithCtrlSpace()
    .Run();
```

---

### `Range`

```csharp
IMultiFileControl Range(int minvalue, int? maxvalue = null)
```

Sets the minimum and (optionally) maximum number of items that must be checked before **Enter**
confirms. Pass `null` for `maxvalue` to leave the upper bound unlimited.

```csharp
PromptPlus.Controls.MultiFile("Check between 2 and 4 items")
    .Root(root)
    .Range(2, 4)
    .Run();
```

> Throws `ArgumentOutOfRangeException` when `minvalue` is greater than `maxvalue`.

---

## Validating a check

A predicate decides whether an individual [`FileItem`](operations.md#the-fileitem-results) may be
checked. For a single toggle a rejection shows the optional message as an error; during mass
selections (recursive folder, wildcard, or check-all) rejected items are silently skipped. Setting a
predicate replaces any previously set one (sync or async).

### `PredicateChecked`

```csharp
IMultiFileControl PredicateChecked(Func<FileItem, bool> validselect)
IMultiFileControl PredicateChecked(Func<FileItem, (bool, string?)> validselect)
```

| Overload | Return | Behavior |
|---|---|---|
| `Func<FileItem, bool>` | `true` = checkable | Default message on rejection |
| `Func<FileItem, (bool, string?)>` | `(isValid, message)` | Custom `message` on rejection |

```csharp
PromptPlus.Controls.MultiFile("Check large files only")
    .Root(root)
    .PredicateChecked(f => f.IsDirectory || f.Length > 1024
        ? (true, null)
        : (false, "File is too small"))
    .Run();
```

> Throws `ArgumentNullException` if `validselect` is `null`.

---

### `PredicateCheckedAsync`

```csharp
IMultiFileControl PredicateCheckedAsync(Func<FileItem, Task<bool>> validselect)
IMultiFileControl PredicateCheckedAsync(Func<FileItem, Task<(bool, string?)>> validselect)
```

Asynchronous counterparts.

> ?? For an individual toggle the predicate is awaited **synchronously (blocking) on the UI thread**.
> During a recursive folder (wildcard) selection it runs on a **background thread** while enumerating
> the subtree, so it must be thread-safe and must not touch UI state.

---

## Layout & display

### `HideSize`

```csharp
IMultiFileControl HideSize(bool value = true)
```

Hides the file-size column shown next to files.

---

### `ShowFullPath`

```csharp
IMultiFileControl ShowFullPath(bool value = true)
```

Sets whether the summary shows the full path or just the entry name for each checked item. Default is
to show only the name. The user can flip this at runtime with the full-path hotkey (**Shift+F3**).

```csharp
PromptPlus.Controls.MultiFile("Check (short names)")
    .Root(root)
    .ShowFullPath(false)
    .Run();
```

---

### `PageSize`

```csharp
IMultiFileControl PageSize(byte value)
```

Rows visible at once (valid range 0–255). `0` (default) auto-fits to the console height, reserving
lines for the header, footer, and pagination.

```csharp
PromptPlus.Controls.MultiFile("Check").Root(root).PageSize(12).Run();
```

---

## Initial values

### `Default`

```csharp
IMultiFileControl Default(IEnumerable<string> fullPaths, bool useDefaultHistory = true)
```

Pre-checks the supplied paths, expanding the tree down to the first one when it lies under the root.
When `useDefaultHistory` is `true` and [history](#enabledhistory) is enabled, stored values may
override these defaults.

```csharp
PromptPlus.Controls.MultiFile("Pre-checked defaults")
    .Root(root)
    .Default([@"C:\Projects\a.cs", @"C:\Projects\b.cs"])
    .Run();
```

> Throws `ArgumentNullException` if `fullPaths` is `null`.

---

## History

### `EnabledHistory`

```csharp
IMultiFileControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
```

Persists the confirmed paths to `filename` and can restore them as the defaults on the next run (the
tree expands to the first). The `IHistoryOptions` builder is identical to the one documented for
[Input ? EnabledHistory](../input/methods.md#enabledhistory) (`MinPrefixLength`, `MaxItems`,
`ExpirationTime`, `FilterType`, `PageSize`).

```csharp
PromptPlus.Controls.MultiFile("Check (remembered)")
    .Root(root)
    .EnabledHistory("multifile-history")
    .Run();
```

> Throws `ArgumentNullException` if `filename` is `null`.

---

## Appearance & behavior

### `Styles`

```csharp
IMultiFileControl Styles(MultiFileStyles styleType, Style style)
```

Recolors one visual region of this control. See the region list and examples on the
[Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here
PromptPlus.Controls.MultiFile("Check").Root(root)
    .Styles(MultiFileStyles.FileTypeFolder, new Style(Color.Cyan, Color.Default))
    .Run();
```

---

### `Options`

```csharp
IMultiFileControl Options(Action<IControlOptions> options)
```

Overrides global behaviors for this one control (prompt/description text, abort key, tooltip,
hide-after-finish). See
[Global Behaviors ? Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions).

```csharp
PromptPlus.Controls.MultiFile("Check").Root(root)
    .Options(opt =>
    {
        opt.ShowTooltip(true);
        opt.EnabledAbortKey(true);
        opt.HideAfterFinish(false);
    })
    .Run();
```

> Throws `ArgumentNullException` if `options` is `null`.

---

## Running the control

### `Run`

```csharp
ResultPrompt<FileItem[]> Run(CancellationToken token = default)
```

Renders the tree and blocks until the user confirms (**Enter**) or aborts (**Esc**). Returns
[`ResultPrompt<FileItem[]>`](../../architecture.md#resultpromptt); `.Content` is an empty array when
aborted.

```csharp
var result = PromptPlus.Controls.MultiFile("Check").Root(root).Run();
if (!result.IsAborted)
    foreach (var f in result.Content)
        PromptPlus.Console.WriteLine(f.FullPath);
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `MultiFileStyles` regions
- [Index](index.md) — overview and method map
- [File ? Methods](../file/methods.md) — the single-selection sibling
