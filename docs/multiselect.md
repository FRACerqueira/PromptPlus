# PromptPlus # MultiSelect
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)


## Documentation
Control MultSelect. Generic multi select input IEnumerable/Enum with auto-paginator , tooltips and more.

![](./images/MultSelect.gif)

### Syntax
[**Top**](#promptplus--multiselect)

```csharp
MultiSelect<T>(string prompt = null)
```

 ### Methods
 [**Top**](#promptplus--multiselect)


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
    - Function to extract value with type T from string and displayed.
    - For IEnumerable type, if ommited,the value will be item => item.ToString()
    - For enum value, if ommited,the value will be the \[Display\] attribute if it exists or an enum string.
- ```csharp
  Range(int minvalue, int maxvalue)
    ```
    - Minimum and maximum items seleted
- ```csharp
  AddItem(T value)
    ```
    - Add item to list.
- ```csharp
  AddItems(IEnumerable<T> value)
    ```
    - Add IEnumerable item to list.
- ```csharp
  AddGroup(IEnumerable<T> value, string group)
    ```
    - Add IEnumerable item to list over a group.
- ```csharp
  AddDefault(T value)
    ```
    - Add initial item seleted.
- ```csharp
  AddDefaults(IEnumerable<T> value)
    ```
    - Addd initial IEnumerable items seleted.
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

### Return
[**Top**](#promptplus--multiselect)

```csharp
IControlMultiSelect<T>              //for Control Methods
IPromptControls<IEnumerable<T>>     //for others Base Methods
ResultPromptPlus<IEnumerable<T>>    //for Base Method Run, when execution is direct 
IPromptPipe                         //for Pipe condition 
IFormPlusBase                       //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#promptplus--multiselect)

```csharp
var options = PromptPlus.MultiSelect<string>("Which cities would you like to visit?")
    .AddGroup(new[] { "Seattle", "Boston", "New York" }, "North America")
    .AddGroup(new[] { "Tokyo", "Singapore", "Shanghai" }, "Asia")
    .AddItem("South America (Any)")
    .AddItem("Europe (Any)")
    .DisableItem("Boston")
    .AddDefault("New York")
    .Run(_stopApp);

if (options.IsAborted)
{
    return;
}
if (options.Value.Any())
{
    Console.WriteLine($"You picked {string.Join(", ", options.Value)}");
}
else
{
    Console.WriteLine("You chose nothing!");
}
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)
