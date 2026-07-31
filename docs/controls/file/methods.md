<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **File — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [File — Operations →](operations.md)

---

Every fluent method on `IFileControl`. Each returns the same control instance, so calls chain in any
order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.File(string prompt = "", string? description = null)`, which
> returns `IFileControl`.

**Quick jump:**
[Root](#root) ·
[SearchPattern](#searchpattern) ·
[OnlyFolders](#onlyfolders) ·
[ShowHidden](#showhidden) ·
[ShowSystem](#showsystem) ·
[SelectFilesOnly](#selectfilesonly) ·
[HideSize](#hidesize) ·
[ShowFullPath](#showfullpath) ·
[PageSize](#pagesize) ·
[Default](#default) ·
[EnableHistory](#enablehistory) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Choosing the root

### `Root`

```csharp
IFileControl Root(string path)
```

Sets the folder the tree starts at. When not set, the current directory
(`Directory.GetCurrentDirectory()`) is used. Point it at a drive root (e.g. `C:\`) to browse a whole
volume.

```csharp
PromptPlus.Controls.File("Pick")
    .Root(@"C:\Projects")
    .Run();
```

> Throws `ArgumentNullException` if `path` is `null`.

---

## Filtering what is listed

### `SearchPattern`

```csharp
IFileControl SearchPattern(string pattern)
```

Filters **files** by a wildcard pattern; directories are always listed so the tree can be navigated.
Default is `*` (all files).

```csharp
PromptPlus.Controls.File("Pick a C# file")
    .Root(root)
    .SearchPattern("*.cs")
    .Run();
```

> Throws `ArgumentNullException` if `pattern` is `null`.

---

### `OnlyFolders`

```csharp
IFileControl OnlyFolders(bool value = true)
```

Lists directories only, hiding all files. Useful when the user should pick a folder.

```csharp
PromptPlus.Controls.File("Pick a folder").Root(root).OnlyFolders().Run();
```

---

### `ShowHidden`

```csharp
IFileControl ShowHidden(bool value = true)
```

Includes entries marked with the Hidden attribute. Hidden by default.

---

### `ShowSystem`

```csharp
IFileControl ShowSystem(bool value = true)
```

Includes entries marked with the System attribute. Hidden by default.

```csharp
PromptPlus.Controls.File("Browse (incl. hidden/system)")
    .Root(root)
    .ShowHidden()
    .ShowSystem()
    .Run();
```

---

## Selection rule

### `SelectFilesOnly`

```csharp
IFileControl SelectFilesOnly(bool value = true)
```

Restricts the returned entry to files. Folders can still be expanded and browsed, but pressing
**Enter** on a folder does not confirm it.

```csharp
PromptPlus.Controls.File("Pick a file")
    .Root(root)
    .SearchPattern("*.cs")
    .SelectFilesOnly()
    .Run();
```

---

## Layout & display

### `HideSize`

```csharp
IFileControl HideSize(bool value = true)
```

Hides the file-size column shown next to files.

```csharp
PromptPlus.Controls.File("Pick").Root(root).HideSize().Run();
```

---

### `ShowFullPath`

```csharp
IFileControl ShowFullPath(bool value = true)
```

Sets whether the answer/summary shows the full path or just the entry name for the selected item.
Default is to show only the name. The user can flip this at runtime with the full-path hotkey
(**Shift+F3**) regardless of the initial value.

```csharp
PromptPlus.Controls.File("Select (answer shows short name)")
    .Root(root)
    .ShowFullPath(false)
    .Run();
```

---

### `PageSize`

```csharp
IFileControl PageSize(byte value)
```

Rows visible at once (valid range 0–255). `0` (default) auto-fits to the console height, reserving
lines for the header, footer, and pagination.

```csharp
PromptPlus.Controls.File("Pick").Root(root).PageSize(12).Run();
```

---

## Initial value

### `Default`

```csharp
IFileControl Default(string fullPath, bool useDefaultHistory = true)
```

Pre-selects a file or directory, expanding the tree down to it when the path lies under the root.
When `useDefaultHistory` is `true` and [history](#enablehistory) is enabled, a stored value may
override this default.

```csharp
PromptPlus.Controls.File("Pick")
    .Root(root)
    .Default(@"C:\Projects\App\Program.cs")
    .Run();
```

> Throws `ArgumentNullException` if `fullPath` is `null`.

---

## History

### `EnableHistory`

```csharp
IFileControl EnableHistory(string filename, Action<IHistoryOptions>? options = null)
```

Persists the confirmed path to `filename` and can restore it as the default on the next run (the tree
expands to it). The `IHistoryOptions` builder is identical to the one documented for
[Input → EnableHistory](../input/methods.md#enablehistory) (`MinPrefixLength`, `MaxItems`,
`ExpirationTime`, `FilterType`, `PageSize`).

```csharp
PromptPlus.Controls.File("Pick a file (remembered)")
    .Root(root)
    .EnableHistory("file-history")
    .Run();
```

> Throws `ArgumentNullException` if `filename` is `null`.

---

## Appearance & behavior

### `Styles`

```csharp
IFileControl Styles(FileStyles styleType, Style style)
```

Recolors one visual region of this control. See the region list and examples on the
[Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here
PromptPlus.Controls.File("Pick").Root(root)
    .Styles(FileStyles.FileTypeFolder, new Style(Color.Cyan, Color.Default))
    .Run();
```

---

### `Options`

```csharp
IFileControl Options(Action<IControlOptions> options)
```

Overrides global behaviors for this one control (prompt/description text, abort key, tooltip,
hide-after-finish). See
[Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions).

```csharp
PromptPlus.Controls.File("Pick").Root(root)
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
ResultPrompt<FileItem?> Run(CancellationToken token = default)
```

Renders the tree and blocks until the user confirms (**Enter**) or aborts (**Esc**). Returns
[`ResultPrompt<FileItem?>`](../../architecture.md#resultpromptt); `.Content` is `null` when aborted.

```csharp
var result = PromptPlus.Controls.File("Pick").Root(root).Run();
if (!result.IsAborted)
    PromptPlus.Console.WriteLine(result.Content?.FullPath);
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `FileStyles` regions
- [Index](index.md) — overview and method map
- [MultiFile → Methods](../multifile/methods.md) — the multiple-selection sibling
