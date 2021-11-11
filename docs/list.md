# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # List
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control List. Create Generic IEnumerable with auto-paginator, tooptip , input validator, message error by type/format and more.

![](./images/List.gif)

### Syntax
[**Top**](#-promptplus--list)

```csharp
List<T>(string prompt = null)
````

### Methods
[**Top**](#-promptplus--list)

- ```csharp
  Prompt(string value)
  ``` 
  - set prompt message 
- ```csharp
  PageSize(int value)
    ```
    - Maximum item per page. If the value is ommited, the value will be calculated according to the screen size 
- ```csharp
   TextSelector(Func<T, string> value)
    ```
    - Function to extract value with type T from string and displayed. If not defined, the value will be item => item.ToString()
- ```csharp
  Range(int minvalue, int maxvalue)
    ```
    - Minimum and maximum items added
- ```csharp
  UpperCase(bool value)
    ```
    - UpperCase input
- ```csharp
  AllowDuplicate(bool value)
    ```
    - Allow duplicate input
- ```csharp
  AddValidator(Func<object, ValidationResult> validator);
  ``` 
    - item of input validator
- ```csharp
  AddValidators(IEnumerable<Func<object, ValidationResult>> validators)
  ``` 
    - List of input validator
- ```csharp
  ValidateOnDemand()
  ``` 
    - Run the validators on each interaction

### Return
[**Top**](#-promptplus--list)

```csharp
IControlList<T>                     //for Control Methods
IPromptControls<ResultBrowser>      //for others Base Methods
ResultPromptPlus<ResultBrowser>     //for Base Method Run, when execution is direct 
IPromptPipe                         //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase                       //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#-promptplus--list)

```csharp
var lst = PromptPlus.List<string>("Please add item(s)")
    .PageSize(3)
    .UpperCase(true)
    .Run(_stopApp);

if (lst.IsAborted)
{
    return;
}
Console.WriteLine($"You picked {string.Join(", ", lst.Value)}");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)
