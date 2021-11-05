# PromptPlus # Select
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control Select. Generic select IEnumerable/Enum with auto-paginator and tooltips and more.

![](./images/Select.gif)

### Syntax
[**Top**](#promptplus--select)

```csharp
 Select<T>(string prompt = null)
```

### Methods
[**Top**](#promptplus--select)

 - ```csharp
 Prompt(string value)
  ``` 
  - set prompt message
 - ```csharp
 Default(T value)
 ``` 
  - initial item seleted.
- ```csharp
  PageSize(int value)
    ```
    - Maximum item per page. If the value is ommited, the value will be calculated according to the screen size
- ```csharp
  TextSelector(Func<T, string> value)
    ```
    - Function to extract value with type T from string and displayed.
    - For IEnumerable type, if ommited,the value will be item => item.ToString()
    - For enum value, if ommited,the value will be the \[Display\] attribute if it exists or an enum string.
- ```csharp
  AddItem(T value)
    ```
    - Add item to list.
- ```csharp
  AddItems(IEnumerable<T> value)
    ```
    - Add IEnumerable item to list.
- ```csharp
  HideItem(T value)
    ```
    - Hide item in list.
- ```csharp
  HideItems(IEnumerable<T> value)
    ```
    - Hide IEnumerable items in list.
- ```csharp
  DisableItem(T value)
    ```
    - Tick disabled item in list .
- ```csharp
  DisableItems(IEnumerable<T> value)
    ```
    - Tick disabled IEnumerable items in list.
- ```csharp
  AutoSelectIfOne()
    ```
    - Automatically selects the item when there is only a single filtered item

### Return
[**Top**](#promptplus--select)

```csharp
IControlSelect<T>      //for Control Methods
IPromptControls<T>     //for others Base Methods
ResultPromptPlus<T>    //for Base Method Run, when execution is direct 
IPromptPipe            //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase          //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#promptplus--select)

```csharp
var city = PromptPlus.Select<string>("Select your city")
    .AddItems(new[] { "1 - Seattle", "2 - London", "3 - Tokyo", "4 - New York", "5 - Singapore", "6 - Shanghai" })
    .PageSize(3)
    .AutoSelectIfOne()
    .Run(_stopApp);

if (city.IsAborted)
{
    return;
}
if (!string.IsNullOrEmpty(city.Value))
{
    PromptPlus.WriteLine($"Hello, {city.Value}!");
}
else
{
    PromptPlus.WriteLine("You chose nothing!");
}
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)


