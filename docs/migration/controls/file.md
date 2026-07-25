# Migration v5.x → v6.x: File and MultiFile

> Back to [Migration Overview](../../migration-v5-to-v6.md)

## Renamed controls

| v5.x | v6.x |
|---|---|
| `FileSelect()` | `File()` |
| `FileMultiSelect()` | `MultiFile()` |

## Renamed public methods

`MultiFile` **renamed** its confirmation validator (v5.x `FileMultiSelect` had `PredicateSelected`).
The single-selection `File` control has **no** such validator in v6.x.

| Control | v5.x member | v6.x member |
|---|---|---|
| MultiFile | `PredicateSelected(Func<FileItem, bool>)` | `PredicateChecked(Func<FileItem, bool>)` |
| MultiFile | `PredicateSelected(Func<FileItem, (bool, string?)>)` | `PredicateChecked(Func<FileItem, (bool, string?)>)` |

> The async overload follows the same name: `PredicateCheckedAsync` (new in v6.x).

## Item type changed

The selected item type changed and so did its members:

| | v5.x (`ItemFile`) | v6.x (`FileItem`) |
|---|---|---|
| Name | `Name` | `Name` |
| Full path | `FullPath` | `FullPath` |
| Is a folder | `IsFolder` | `IsDirectory` |
| Size in bytes | `Length` | `Length` |
| Last write time | — | `LastWriteTime` |

> ⚠️ `FileItem` has **no** `Size`, `IsFolder`, `FullName`, or `Extension` members. Use `Length`, `IsDirectory`, `FullPath`; for an extension, use `Path.GetExtension(item.FullPath)`.

`File().Run()` returns `ResultPrompt<FileItem?>`; `MultiFile().Run()` returns `ResultPrompt<FileItem[]>` (read via `.Content`).

---

## Breaking Changes

### 1. `AcceptHiddenAttributes` → `ShowHidden`

```csharp
// v5.x — true made hidden files accepted/visible
.AcceptHiddenAttributes(true)

// v6.x — true shows hidden entries (same effect)
.ShowHidden(true)
```

> Renamed only; the effect is the same (`true` makes hidden entries visible).

### 2. `AcceptSystemAttributes` → `ShowSystem`

```csharp
// v5.x
.AcceptSystemAttributes(true)

// v6.x
.ShowSystem(true)
```

### 3. `HideSizeInfo` → `HideSize`

```csharp
// v5.x
.HideSizeInfo(true)

// v6.x
.HideSize(true)
```

### 4. `HideZeroEntries` — removed with no equivalent

```csharp
// v5.x only — empty folders are always shown in v6.x
.HideZeroEntries(true)
```

### 5. `HideFilesBySize` — removed with no equivalent

**Before (v5.x):**
```csharp
PromptPlus.Controls.FileSelect("File:")
    .HideFilesBySize(0, 1024)   // hide files larger than 1KB
    .Run();
```

**After (v6.x):**
```csharp
// No size filter. For MultiFile you can block confirmation with PredicateChecked:
PromptPlus.Controls.MultiFile("Files:")
    .PredicateChecked(item =>
        item.IsDirectory || item.Length <= 1024
            ? (true, null)
            : (false, "File is larger than 1KB"))
    .Run();
```

> ⚠️ `File` (single-select) has **no** `PredicateChecked` in v6.x — only `MultiFile` does. There is no built-in size filter for `File`.

### 6. `EnabledSearchFilter` — removed with no equivalent (File and MultiFile)

**Before (v5.x):**
```csharp
.EnabledSearchFilter(FilterMode.Contains)
```

**After (v6.x):**
```csharp
// Use SearchPattern to filter by name/extension pattern
PromptPlus.Controls.File("File:")
    .SearchPattern("*.txt")
    .Run();
```

### 7. `PredicateDisabled` — removed with no equivalent (MultiFile)

**Before (v5.x):**
```csharp
PromptPlus.Controls.FileMultiSelect("Files:")
    .PredicateDisabled(item => Path.GetExtension(item.FullPath) == ".exe")
    .Run();
```

**After (v6.x):**
```csharp
// Use PredicateChecked to block confirmation of invalid items
PromptPlus.Controls.MultiFile("Files:")
    .PredicateChecked(item =>
        Path.GetExtension(item.FullPath) != ".exe"
            ? (true, null)
            : (false, ".exe files are not allowed"))
    .Run();
```

> ⚠️ Difference: `PredicateDisabled` prevented navigating to the item; `PredicateChecked` only blocks confirmation after selection.

### 8. `HideCountSelected` — removed (MultiFile)

```csharp
// v5.x only — the selected count is always shown in v6.x
.HideCountSelected(true)
```

### 9. `DefaultHistory(bool)` — removed (File)

```csharp
// v5.x
.EnabledHistory("file_history").DefaultHistory(true)

// v6.x — default history is active whenever EnabledHistory is set
.EnabledHistory("file_history")
```

### 10. `MaxWidth(byte)` — removed (File and MultiFile)

```csharp
// v5.x only — remove during migration
.MaxWidth(60)
```

---

## What's new in v6.x

### `SelectFilesOnly` / `ShowFullPath` (File and MultiFile)
```csharp
PromptPlus.Controls.File("File:")
    .SelectFilesOnly(true)   // exclude folders from the final selection
    .ShowFullPath(true)      // show the full path in the answer area
    .Run();
```

### `CascadeCheck` / `RecursiveMarkWithCtrlSpace` (MultiFile)
```csharp
PromptPlus.Controls.MultiFile("Files:")
    .CascadeCheck(true)                 // checking a folder checks its children
    .RecursiveMarkWithCtrlSpace(true)   // Ctrl+Space marks recursively
    .Run();
```

### `PredicateCheckedAsync` (MultiFile)
```csharp
PromptPlus.Controls.MultiFile("Files:")
    .PredicateCheckedAsync(async item =>
    {
        bool allowed = await CheckPermissionAsync(item.FullPath);
        return (allowed, allowed ? null : "No access permission");
    })
    .Run();
```

---

## Full API reference

### File — v5.x vs v6.x

| Method | v5.x (FileSelect) | v6.x (File) | Change |
|---|---|---|---|
| Factory | `FileSelect()` | `File()` | Renamed |
| `AcceptHiddenAttributes(bool)` | ✅ | ❌ | → `ShowHidden(bool)` |
| `AcceptSystemAttributes(bool)` | ✅ | ❌ | → `ShowSystem(bool)` |
| `HideSizeInfo(bool)` | ✅ | ❌ | → `HideSize(bool)` |
| `HideZeroEntries(bool)` | ✅ | ❌ | Removed |
| `HideFilesBySize(long, long)` | ✅ | ❌ | Removed |
| `EnabledSearchFilter(FilterMode)` | ✅ | ❌ | Removed |
| `PredicateDisabled(Func<ItemFile,bool>)` | ✅ | ❌ | Removed |
| `PredicateSelected` (x2) | ✅ | ❌ | Removed (only on `MultiFile` in v6.x) |
| `MaxWidth(byte)` | ✅ | ❌ | Removed |
| `DefaultHistory(bool)` | ✅ | ❌ | Removed |
| `Root(string)` · `SearchPattern(string)` · `OnlyFolders(bool)` · `PageSize(byte)` · `Default(string, bool)` · `EnabledHistory` | ✅ | ✅ | Unchanged |
| `SelectFilesOnly(bool)` · `ShowFullPath(bool)` | ❌ | ✅ | New |
| `Run()` | `ResultPrompt<ItemFile>` | `ResultPrompt<FileItem?>` | Item type changed |

### MultiFile — v5.x vs v6.x

| Method | v5.x (FileMultiSelect) | v6.x (MultiFile) | Change |
|---|---|---|---|
| Factory | `FileMultiSelect()` | `MultiFile()` | Renamed |
| `AcceptHiddenAttributes` / `AcceptSystemAttributes` / `HideSizeInfo` | ✅ | ❌ | → `ShowHidden` / `ShowSystem` / `HideSize` |
| `EnabledSearchFilter(FilterMode)` | ✅ | ❌ | Removed |
| `PredicateDisabled(Func<ItemFile,bool>)` | ✅ | ❌ | Removed |
| `HideCountSelected(bool)` | ✅ | ❌ | Removed |
| `HideZeroEntries` / `HideFilesBySize` | ✅ | ❌ | Removed |
| `MaxWidth(byte)` | ✅ | ❌ | Removed |
| `Range(int, int?)` · `Root` · `SearchPattern` · `OnlyFolders` · `PageSize` | ✅ | ✅ | Unchanged |
| `PredicateSelected` (x2) | ✅ | ❌ | Renamed to `PredicateChecked` (x2) |
| `CascadeCheck` · `RecursiveMarkWithCtrlSpace` · `SelectFilesOnly` · `ShowFullPath` · `PredicateChecked` (x2) · `PredicateCheckedAsync` (x2) | ❌ | ✅ | New |
| `Run()` | `ResultPrompt<ItemFile[]>` | `ResultPrompt<FileItem[]>` | Item type changed |
