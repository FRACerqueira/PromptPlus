# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # MultiSelect
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control MultSelect. Generic multi select input IEnumerable/Enum with auto-paginator , tooltips and more.

![](./images/MultSelect.gif)

### Syntax
[**Top**](#-promptplus--multiselect)

```csharp
MultiSelect<T>(string prompt, string description = null)
```

 ### Methods
[**Top**](#-promptplus--multiselect)

- ```csharp
  Prompt(string value, string description = null)
  ``` 
  - set prompt message and optional description

- ```csharp
  PageSize(int value)
    ```
    - Maximum item per page. If the value is ommited, the value will be calculated according to the screen size

- ```csharp
  TextSelector(Func<T, string> value)
    ```
    - Function to extract value with type T to string and displayed.
    - For IEnumerable type, if ommited,the value will be item => item.ToString()
    - For enum value, if ommited,the value will be the \[Display\] attribute if it exists or an enum string.

- ```csharp
  DescriptionSelector(Func<T, string> value)
    ```
    - Function to extract the value with type T to string and displayed in the description line.

- ```csharp
  ShowGroupOnDescription(string noGroupMessage)
    ```
    - Show the description line with the group name or use the noGroupMessage parameter when there is no group.
    - Must have declared method DescriptionSelector

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
  ResultPromptPlus<IEnumerable<T>> Run(CancellationToken? value = null)
  ``` 
	- Control execution

### Return
[**Top**](#-promptplus--multiselect)

```csharp
IControlMultiSelect<T>              //for Control Methods
ResultPromptPlus<IEnumerable<T>>    //After execute method Run
IPromptPipe                         //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase                       //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#-promptplus--multiselect)

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
