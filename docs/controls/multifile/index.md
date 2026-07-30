<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiFile**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [MultiFile — Methods →](methods.md)

---

> A lazy-loaded **file-system tree** where the user checks **several** files and/or folders and
> confirms with **Enter**.

`MultiFile` is the multiple-selection sibling of [`File`](../file/index.md). It renders the file
system as an expandable/collapsible tree rooted at a folder you choose, loading each directory's
contents only when it is expanded. Checked entries are tracked by their full path, so a selection
survives collapsing and re-expanding the branch that contains it. Beyond the shared browsing
options, it adds cascade checking, recursive folder marking, a min/max count range, and a check
predicate.

> 🔘 Only need **one** file or folder? Use the [**File**](../file/index.md) control — same tree
> browser, single selection.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, first examples, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Keyboard, checking, cascade/recursive marking, range, history |
| [Styles](styles.md) | The `MultiFileStyles` regions and how to recolor them |

---

## When to use it

| Use `MultiFile` when… | Consider instead… |
|---|---|
| The user checks several files/folders from disk | — |
| The user picks exactly one file/folder | [File](../file/index.md) |
| The data is an arbitrary hierarchy (not the file system) | [Tree](../tree/index.md) |
| The choices are an in-memory list | [MultiSelect](../multiselect/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .MultiFile("Check files or folders")
    .Root(@"C:\Projects")
    .Run();

if (!result.IsAborted)
{
    foreach (var f in result.Content)
        PromptPlus.Console.WriteLine(f.FullPath);
}
```

- `MultiFile("Check files or folders")` creates the browser; the argument is the prompt text.
- `.Root(...)` sets the folder the tree starts at. Without it, the current directory is used.
- `.Run()` renders the tree and blocks until **Enter** (confirm) or **Esc** (abort), returning a
  [`ResultPrompt<FileItem[]>`](../../architecture.md#resultpromptt).

---

## Reading the result

`Run()` returns `ResultPrompt<FileItem[]>` — an array of the checked entries. `FileItem` is a small
sealed class (**not** `System.IO.FileInfo`):

```csharp
var result = PromptPlus.Controls.MultiFile("Check files").Root(root).Run();

if (!result.IsAborted)
{
    PromptPlus.Console.WriteLine($"Checked {result.Content.Length} item(s):");
    foreach (var f in result.Content)
        PromptPlus.Console.WriteLine($"  {f.FullPath} (dir: {f.IsDirectory}, size: {f.Length})");
}
```

When aborted, `.Content` is an empty array. `FileItem.ToString()` returns `FullPath`.

---

## A richer example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .MultiFile("Check C# files", "Space to check, Enter to confirm")
    .Root(@"C:\Projects")
    .SearchPattern("*.cs")   // only *.cs files are listed (folders always show)
    .SelectFilesOnly()       // folders can be expanded but not checked
    .Range(2, 4)             // require between 2 and 4 checked items
    .Run();
```

This combines a **file filter**, a **check rule**, and a **count range** — see
[Operations](operations.md) for how they behave together.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Choose the root | `Root` |
| Filter what is listed | `SearchPattern`, `OnlyFolders`, `ShowHidden`, `ShowSystem` |
| Check rules | `SelectFilesOnly`, `CascadeCheck`, `RecursiveMarkWithCtrlSpace`, `Range` |
| Validate a check | `PredicateChecked`, `PredicateCheckedAsync` |
| Layout & display | `HideSize`, `ShowFullPath`, `PageSize` |
| Initial values | `Default` |
| History | `EnableHistory` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`MultiFile` returns `ResultPrompt<FileItem[]>` — the checked entries.

| Member | Meaning |
|---|---|
| `.Content` | The checked [`FileItem`](#reading-the-result) array (empty if aborted) |
| `.IsAborted` | `true` when the user pressed Esc |

```csharp
var (items, aborted) = PromptPlus.Controls.MultiFile("Check").Root(root).Run();
if (!aborted)
    foreach (var f in items) PromptPlus.Console.WriteLine(f.FullPath);
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — keyboard, checking, cascade/recursive marking, range
- [Styles](styles.md) — recolor the tree regions
- [File](../file/index.md) — single-selection sibling
- [Tree](../tree/index.md) — the generic hierarchy picker behind this control
