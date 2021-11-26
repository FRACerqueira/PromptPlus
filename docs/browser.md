# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # Browser
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ResultBrowser**](resultbrowser) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)


## Documentation
Control Browser. Browser files/folder with auto-paginator and tooltips.

![](./images/Browser.gif)

### Syntax
[**Top**](#-promptplus--browser)

```csharp
 Browser(string prompt, string description = null)
 ```

### Methods
[**Top**](#-promptplus--browser)

- ```csharp
  Prompt(string value, string description = null)
  ``` 
  - set prompt message and optional description

- ```csharp
  Filter(BrowserFilter value)
    ```
    - Fiter result to only type folder. Default value = BrowserFilter.None (All types)   

- ```csharp
  Default(string value)
    ```
    - full path of file/folder initial selected

- ```csharp
  PrefixExtension(string value)
    ```
    - Prefix to be added to the end of item (only new file/folder)

- ```csharp
  AllowNotSelected(bool value)
    ```
    - Accept not seleted item

- ```csharp
  Root(string value)
    ```
    - Root for dry discovery

- ```csharp
  SearchPattern(string value)
    ```
    - Specifies what to search for by the browser

- ```csharp
  PageSize(int value)
    ```
    - Maximum item per page. If the value is ommited, the value will be calculated according to the screen size 

- ```csharp
   SupressHidden(bool value)
    ```
    - Supress file/folder with attribute hidden/system

- ```csharp
  PromptCurrentPath(bool value)
    ```
    - Split fullpath of seleted item and show then

- ```csharp
  promptSearchPattern(bool value)
    ```
    - Show/Hide searchPattern in prompt message

- ```csharp
  Config(Action<IPromptConfig> context)
  ``` 
  - For access [**base methods**](basemethods) common to all controls.

- ```csharp
   PipeCondition(Func<ResultPipe[], object, bool> condition)
  ``` 
  - Set condition to run pipe.

- ```csharp
   ToPipe(string id, string title, object state = null)
  ``` 
  - Transform control to IFormPlusBase.
  - It is mandatory to use this method to use with the Pipeline control. See examples in [**PipeLine Control**](pipeline)

- ```csharp
  ResultPromptPlus<ResultBrowser> Run(CancellationToken? value = null)
  ``` 
	- Control execution

### Return
[**Top**](#-promptplus--browser)

```csharp
IControlBrowser                     //for Control Methods
ResultPromptPlus<ResultBrowser>     //After execute method Run
IPromptPipe                         //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase                       //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#-promptplus--browser)

```csharp
var file = PromptPlus.Browser("Select/New file")
    .PageSize(10)
    .AllowNotSelected(true)
    .PrefixExtension(".cs")
    .Run(_stopApp);

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
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)
