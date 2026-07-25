<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ChartItem Class

Represents a single item in a chart bar visualization with label, value, color, and calculated properties\.

```csharp
public sealed class ChartItem
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → ChartItem
### Constructors

<a name='PromptPlusLibrary.ChartItem.ChartItem(string,string,double,System.Nullable_ConsolePlusLibrary.Color_)'></a>

## ChartItem\(string, string, double, Nullable\<Color\>\) Constructor

Represents a single item in a chart bar visualization with label, value, color, and calculated properties\.

```csharp
public ChartItem(string id, string label, double value, System.Nullable<ConsolePlusLibrary.Color> color);
```
#### Parameters

<a name='PromptPlusLibrary.ChartItem.ChartItem(string,string,double,System.Nullable_ConsolePlusLibrary.Color_).id'></a>

`id` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Unique identifier for the item\.

<a name='PromptPlusLibrary.ChartItem.ChartItem(string,string,double,System.Nullable_ConsolePlusLibrary.Color_).label'></a>

`label` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Display label for the item\.

<a name='PromptPlusLibrary.ChartItem.ChartItem(string,string,double,System.Nullable_ConsolePlusLibrary.Color_).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Numeric value of the item\.

<a name='PromptPlusLibrary.ChartItem.ChartItem(string,string,double,System.Nullable_ConsolePlusLibrary.Color_).color'></a>

`color` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[ConsolePlusLibrary\.Color](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.color 'ConsolePlusLibrary\.Color')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional color for the bar representation\.
### Properties

<a name='PromptPlusLibrary.ChartItem.Color'></a>

## ChartItem\.Color Property

Gets or sets the color used to render the bar for this item\.

```csharp
public System.Nullable<ConsolePlusLibrary.Color> Color { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[ConsolePlusLibrary\.Color](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.color 'ConsolePlusLibrary\.Color')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='PromptPlusLibrary.ChartItem.Id'></a>

## ChartItem\.Id Property

Gets the unique identifier for this chart item\.

```csharp
public string Id { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='PromptPlusLibrary.ChartItem.Label'></a>

## ChartItem\.Label Property

Gets the display label for this chart item\.

```csharp
public string Label { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='PromptPlusLibrary.ChartItem.Percent'></a>

## ChartItem\.Percent Property

Gets or sets the calculated percentage this item represents of the total\.

```csharp
public double Percent { get; set; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='PromptPlusLibrary.ChartItem.StyleBar'></a>

## ChartItem\.StyleBar Property

Gets or sets the style to use when rendering the bar for this item\.

```csharp
public System.Nullable<ConsolePlusLibrary.Style> StyleBar { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='PromptPlusLibrary.ChartItem.Value'></a>

## ChartItem\.Value Property

Gets the numeric value associated with this chart item\.

```csharp
public double Value { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')