# PromptPlus # Browser
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ResultBrowser**](resultbrowser) |
[**Browser Options**](browseroptions) |
[**BaseOptions**](baseoptions)

## Documentation
Control Browser. Browser files/folder with auto-paginator and tooltips.

![](./images/Browser.gif)

### Options

[**Browser Options**](browseroptions)

### Syntax
[**Top**](#promptplus--browser)

```csharp
Browser(BrowserOptions options, CancellationToken? cancellationToken = null)
Browser(Action<BrowserOptions> configure, CancellationToken? cancellationToken = null)
```

```csharp
Browser(BrowserFilter fileBrowserChoose, string message, string defaultValue = null, string prefixExtension = null, bool allowNotSelected = false, string rootFolder = null, string searchPattern = null, int? pageSize = null, bool supressHidden = true, bool promptCurrentPath = true, bool promptSearchPattern = true, CancellationToken? cancellationToken = null)
```

**Highlighted parameters**
- defaultValue = full path of file/folder initial selected
- prefixExtension = prefix to be added to the end of the selected item
- allowNotSelected = accept not seleted item
- rootFolder = root for discovery
- searchPattern = Specifies what to search for by the browser
- supressHidden = supress file/folder with attribute hidden/system
- promptCurrentPath = split fullpath of seleted item and show then
- promptSearchPattern = show/hide searchPattern in prompt message
- pageSize = maximum item per page. Tf the value is null, the value will be calculated according to the screen size 

### Return
[**Top**](#promptplus--browser)

```csharp
ResultPromptPlus<ResultBrowser>
```

### Sample
[**Top**](#promptplus--browser)

```csharp
var file = PromptPlus.Browser(BrowserFilter.None, 
    "Select/New file", 
    cancellationToken: _stopApp, 
    pageSize: 10, 
    allowNotSelected: true,
    prefixExtension: ".cs");
if (file.IsAborted)
{
   return;
}
if (string.IsNullOrEmpty(file.Value.SelectedValue))
{
   Console.WriteLine("You chose nothing!");
}
else
{
   var filefound = file.Value.NotFound ? "not found" : "found";
   Console.WriteLine($"You picked, {Path.Combine(file.Value.PathValue, file.Value.SelectedValue)} and {filefound}");
}
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ResultBrowser**](resultbrowser) |
[**Browser Options**](browseroptions) |
[**BaseOptions**](baseoptions)
