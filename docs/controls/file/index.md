<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **File**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [File — Methods →](methods.md)

---

> A lazy-loaded **file-system tree** where the user browses folders and picks **one** file or
> folder, confirming with **Enter**.

`File` renders the file system as an expandable/collapsible tree rooted at a folder you choose. It
loads each directory's contents only when the folder is expanded (and releases them when collapsed),
so memory stays proportional to what is on screen — not to the size of the drive. You can filter
files by pattern, list folders only, restrict selection to files, show or hide hidden/system
entries, and pre-select a path.

> ☑️ Need to pick **several** files or folders at once? Use the [**MultiFile**](../multifile/index.md)
> control — same tree browser, with checkboxes.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, first examples, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Keyboard, tree navigation, filtering, selection rules, history |
| [Styles](styles.md) | The `FileStyles` regions and how to recolor them |

---

## When to use it

| Use `File` when… | Consider instead… |
|---|---|
| The user picks one file or folder from disk | — |
| The user may pick several files/folders | [MultiFile](../multifile/index.md) |
| The data is an arbitrary hierarchy (not the file system) | [Tree](../tree/index.md) |
| The choice is from an in-memory list | [Select](../select/index.md) |
| The data is tabular (multiple columns) | [Table](../table/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .File("Select a file or folder")
    .Root(@"C:\Projects")
    .Run();

if (!result.IsAborted)
    PromptPlus.Console.WriteLine($"You chose {result.Content?.FullPath}");
```

- `File("Select a file or folder")` creates the browser; the argument is the prompt text.
- `.Root(...)` sets the folder the tree starts at. Without it, the current directory is used.
- `.Run()` renders the tree and blocks until **Enter** (confirm) or **Esc** (abort), returning a
  [`ResultPrompt<FileItem?>`](../../architecture.md#resultpromptt).

---

## Reading the result

`Run()` returns `ResultPrompt<FileItem?>`. `FileItem` is a small sealed class — **not**
`System.IO.FileInfo` — describing the chosen entry:

```csharp
var result = PromptPlus.Controls.File("Pick a file").Root(root).Run();

if (!result.IsAborted && result.Content is not null)
{
    var f = result.Content;
    PromptPlus.Console.WriteLine(f.FullPath);        // full path on disk
    PromptPlus.Console.WriteLine(f.Name);            // display name
    PromptPlus.Console.WriteLine($"{f.IsDirectory}"); // folder or file?
    PromptPlus.Console.WriteLine($"{f.Length} bytes");// 0 for directories
    PromptPlus.Console.WriteLine($"{f.LastWriteTime}");
}
```

`FileItem.ToString()` returns `FullPath`, so the item prints as its path.

---

## A richer example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .File("Select a C# file", "Right/+ expand, Left/- collapse, Enter to select")
    .Root(@"C:\Projects")
    .SearchPattern("*.cs")   // only *.cs files are listed (folders always show)
    .SelectFilesOnly()       // folders can be expanded but not returned
    .HideSize()              // drop the size column
    .PageSize(12)            // 12 visible rows
    .Run();
```

This combines a **file filter**, a **selection rule**, and **paging** — see
[Operations](operations.md) for how they behave together.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Choose the root | `Root` |
| Filter what is listed | `SearchPattern`, `OnlyFolders`, `ShowHidden`, `ShowSystem` |
| Selection rule | `SelectFilesOnly` |
| Layout & display | `HideSize`, `ShowFullPath`, `PageSize` |
| Initial value | `Default` |
| History | `EnabledHistory` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`File` returns `ResultPrompt<FileItem?>` — the single chosen entry.

| Member | Meaning |
|---|---|
| `.Content` | The selected [`FileItem`](#reading-the-result) (`null` if aborted) |
| `.IsAborted` | `true` when the user pressed Esc |

```csharp
var (item, aborted) = PromptPlus.Controls.File("Pick").Root(root).Run();
if (!aborted) PromptPlus.Console.WriteLine(item?.FullPath);
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — keyboard, tree navigation, filtering, history
- [Styles](styles.md) — recolor the tree regions
- [MultiFile](../multifile/index.md) — multiple-selection sibling
- [Tree](../tree/index.md) — the generic hierarchy picker behind this control
