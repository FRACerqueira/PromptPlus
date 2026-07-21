<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IWidgets Interface

Provides methods for rendering visual widgets \(banner, dash lines, chart bar, slider, and others\)\.

```csharp
public interface IWidgets
```
### Methods

<a name='PromptPlusLibrary.IWidgets.Banner(string,string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions)'></a>

## IWidgets\.Banner\(string, string, Nullable\<Style\>, DashOptions\) Method

Renders a banner widget as FIGlet \(ASCII art\) text using a specific FIGlet font file\.

```csharp
void Banner(string? value, string pathfontFiglet, System.Nullable<ConsolePlusLibrary.Style> style=null, ConsolePlusLibrary.DashOptions dashOptions=ConsolePlusLibrary.DashOptions.None);
```
#### Parameters

<a name='PromptPlusLibrary.IWidgets.Banner(string,string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Text to render\.

<a name='PromptPlusLibrary.IWidgets.Banner(string,string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions).pathfontFiglet'></a>

`pathfontFiglet` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Path to the FIGlet font file\.

<a name='PromptPlusLibrary.IWidgets.Banner(string,string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions).style'></a>

`style` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional style override; if `null`, current console style is used\.

<a name='PromptPlusLibrary.IWidgets.Banner(string,string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions).dashOptions'></a>

`dashOptions` [ConsolePlusLibrary\.DashOptions](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.dashoptions 'ConsolePlusLibrary\.DashOptions')

Dash style \(default: [ConsolePlusLibrary\.DashOptions\.None](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.dashoptions.none 'ConsolePlusLibrary\.DashOptions\.None')\)\.

<a name='PromptPlusLibrary.IWidgets.Banner(string,System.IO.Stream,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions)'></a>

## IWidgets\.Banner\(string, Stream, Nullable\<Style\>, DashOptions\) Method

Renders a banner widget as FIGlet \(ASCII art\) text using a specific FIGlet font stream\.

```csharp
void Banner(string? value, System.IO.Stream streamFontFiglet, System.Nullable<ConsolePlusLibrary.Style> style=null, ConsolePlusLibrary.DashOptions dashOptions=ConsolePlusLibrary.DashOptions.None);
```
#### Parameters

<a name='PromptPlusLibrary.IWidgets.Banner(string,System.IO.Stream,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Text to render\.

<a name='PromptPlusLibrary.IWidgets.Banner(string,System.IO.Stream,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions).streamFontFiglet'></a>

`streamFontFiglet` [System\.IO\.Stream](https://learn.microsoft.com/en-us/dotnet/api/system.io.stream 'System\.IO\.Stream')

Stream containing the FIGlet font data\.

<a name='PromptPlusLibrary.IWidgets.Banner(string,System.IO.Stream,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions).style'></a>

`style` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional style override; if `null`, current console style is used\.

<a name='PromptPlusLibrary.IWidgets.Banner(string,System.IO.Stream,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions).dashOptions'></a>

`dashOptions` [ConsolePlusLibrary\.DashOptions](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.dashoptions 'ConsolePlusLibrary\.DashOptions')

Dash style \(default: [ConsolePlusLibrary\.DashOptions\.None](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.dashoptions.none 'ConsolePlusLibrary\.DashOptions\.None')\)\.

<a name='PromptPlusLibrary.IWidgets.Banner(string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions)'></a>

## IWidgets\.Banner\(string, Nullable\<Style\>, DashOptions\) Method

Renders a banner widget as FIGlet \(ASCII art\) text\.

```csharp
void Banner(string? value, System.Nullable<ConsolePlusLibrary.Style> style=null, ConsolePlusLibrary.DashOptions dashOptions=ConsolePlusLibrary.DashOptions.None);
```
#### Parameters

<a name='PromptPlusLibrary.IWidgets.Banner(string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Text to render\.

<a name='PromptPlusLibrary.IWidgets.Banner(string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions).style'></a>

`style` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional style override; if `null`, current console style is used\.

<a name='PromptPlusLibrary.IWidgets.Banner(string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions).dashOptions'></a>

`dashOptions` [ConsolePlusLibrary\.DashOptions](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.dashoptions 'ConsolePlusLibrary\.DashOptions')

Dash style \(default: [ConsolePlusLibrary\.DashOptions\.None](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.dashoptions.none 'ConsolePlusLibrary\.DashOptions\.None')\)\.

<a name='PromptPlusLibrary.IWidgets.Calendar(System.DateTime)'></a>

## IWidgets\.Calendar\(DateTime\) Method

Creates a calendar widget for the month and year referenced by [dateref](IWidgets.md#PromptPlusLibrary.IWidgets.Calendar(System.DateTime).dateref 'PromptPlusLibrary\.IWidgets\.Calendar\(System\.DateTime\)\.dateref')\.

```csharp
PromptPlusLibrary.ICalendarWidget Calendar(System.DateTime dateref);
```
#### Parameters

<a name='PromptPlusLibrary.IWidgets.Calendar(System.DateTime).dateref'></a>

`dateref` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

Date whose month/year will be rendered \(day component is ignored\)\.

#### Returns
[ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget')  
An [ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget') instance for further configuration and rendering\.

<a name='PromptPlusLibrary.IWidgets.ChartBar()'></a>

## IWidgets\.ChartBar\(\) Method

Creates a chart bar widget for displaying data as horizontal bars\.

```csharp
PromptPlusLibrary.IChartBarWidget ChartBar();
```

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
An [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance for further configuration and rendering\.

<a name='PromptPlusLibrary.IWidgets.Dash(string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions,int,bool)'></a>

## IWidgets\.Dash\(string, Nullable\<Style\>, DashOptions, int, bool\) Method

Writes a styled text line followed by a dash border line\.

```csharp
void Dash(string? value, System.Nullable<ConsolePlusLibrary.Style> style=null, ConsolePlusLibrary.DashOptions dashOptions=ConsolePlusLibrary.DashOptions.SingleBorder, int extralines=0, bool applycolorbackground=false);
```
#### Parameters

<a name='PromptPlusLibrary.IWidgets.Dash(string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions,int,bool).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Text to write\.

<a name='PromptPlusLibrary.IWidgets.Dash(string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions,int,bool).style'></a>

`style` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional style for text and dash rendering\. If `null`, default styling is used\.

<a name='PromptPlusLibrary.IWidgets.Dash(string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions,int,bool).dashOptions'></a>

`dashOptions` [ConsolePlusLibrary\.DashOptions](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.dashoptions 'ConsolePlusLibrary\.DashOptions')

Dash style \(default: [ConsolePlusLibrary\.DashOptions\.SingleBorder](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.dashoptions.singleborder 'ConsolePlusLibrary\.DashOptions\.SingleBorder')\)\.

<a name='PromptPlusLibrary.IWidgets.Dash(string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions,int,bool).extralines'></a>

`extralines` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Extra blank lines appended after the dash line \(default: 0\)\.

<a name='PromptPlusLibrary.IWidgets.Dash(string,System.Nullable_ConsolePlusLibrary.Style_,ConsolePlusLibrary.DashOptions,int,bool).applycolorbackground'></a>

`applycolorbackground` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, applies background color across the full line \(default: `false`\)\.

<a name='PromptPlusLibrary.IWidgets.DoubleDash(string,ConsolePlusLibrary.DashOptions,int,System.Nullable_ConsolePlusLibrary.Style_,bool)'></a>

## IWidgets\.DoubleDash\(string, DashOptions, int, Nullable\<Style\>, bool\) Method

Writes a styled text line framed by two dash border lines \(above and below\)\.

```csharp
void DoubleDash(string value, ConsolePlusLibrary.DashOptions dashOptions=ConsolePlusLibrary.DashOptions.AsciiSingleBorder, int extraLines=0, System.Nullable<ConsolePlusLibrary.Style> style=null, bool applyColorBackground=false);
```
#### Parameters

<a name='PromptPlusLibrary.IWidgets.DoubleDash(string,ConsolePlusLibrary.DashOptions,int,System.Nullable_ConsolePlusLibrary.Style_,bool).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Text to write\.

<a name='PromptPlusLibrary.IWidgets.DoubleDash(string,ConsolePlusLibrary.DashOptions,int,System.Nullable_ConsolePlusLibrary.Style_,bool).dashOptions'></a>

`dashOptions` [ConsolePlusLibrary\.DashOptions](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.dashoptions 'ConsolePlusLibrary\.DashOptions')

Dash style \(default: [ConsolePlusLibrary\.DashOptions\.AsciiSingleBorder](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.dashoptions.asciisingleborder 'ConsolePlusLibrary\.DashOptions\.AsciiSingleBorder')\)\.

<a name='PromptPlusLibrary.IWidgets.DoubleDash(string,ConsolePlusLibrary.DashOptions,int,System.Nullable_ConsolePlusLibrary.Style_,bool).extraLines'></a>

`extraLines` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Extra blank lines appended after the bottom dash line \(default: 0\)\.

<a name='PromptPlusLibrary.IWidgets.DoubleDash(string,ConsolePlusLibrary.DashOptions,int,System.Nullable_ConsolePlusLibrary.Style_,bool).style'></a>

`style` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional style for text and dash rendering\. If `null`, default styling is used\.

<a name='PromptPlusLibrary.IWidgets.DoubleDash(string,ConsolePlusLibrary.DashOptions,int,System.Nullable_ConsolePlusLibrary.Style_,bool).applyColorBackground'></a>

`applyColorBackground` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, applies background color across each full line \(default: `false`\)\.

<a name='PromptPlusLibrary.IWidgets.SingleDash(string,ConsolePlusLibrary.DashOptions,int,System.Nullable_ConsolePlusLibrary.Style_,bool)'></a>

## IWidgets\.SingleDash\(string, DashOptions, int, Nullable\<Style\>, bool\) Method

Writes a styled text line followed by a single dash border line\.

```csharp
void SingleDash(string value, ConsolePlusLibrary.DashOptions dashOptions=ConsolePlusLibrary.DashOptions.AsciiSingleBorder, int extraLines=0, System.Nullable<ConsolePlusLibrary.Style> style=null, bool applyColorBackground=false);
```
#### Parameters

<a name='PromptPlusLibrary.IWidgets.SingleDash(string,ConsolePlusLibrary.DashOptions,int,System.Nullable_ConsolePlusLibrary.Style_,bool).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Text to write\.

<a name='PromptPlusLibrary.IWidgets.SingleDash(string,ConsolePlusLibrary.DashOptions,int,System.Nullable_ConsolePlusLibrary.Style_,bool).dashOptions'></a>

`dashOptions` [ConsolePlusLibrary\.DashOptions](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.dashoptions 'ConsolePlusLibrary\.DashOptions')

Dash style \(default: [ConsolePlusLibrary\.DashOptions\.AsciiSingleBorder](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.dashoptions.asciisingleborder 'ConsolePlusLibrary\.DashOptions\.AsciiSingleBorder')\)\.

<a name='PromptPlusLibrary.IWidgets.SingleDash(string,ConsolePlusLibrary.DashOptions,int,System.Nullable_ConsolePlusLibrary.Style_,bool).extraLines'></a>

`extraLines` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Extra blank lines appended after the dash line \(default: 0\)\.

<a name='PromptPlusLibrary.IWidgets.SingleDash(string,ConsolePlusLibrary.DashOptions,int,System.Nullable_ConsolePlusLibrary.Style_,bool).style'></a>

`style` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional style for text and dash rendering\. If `null`, default styling is used\.

<a name='PromptPlusLibrary.IWidgets.SingleDash(string,ConsolePlusLibrary.DashOptions,int,System.Nullable_ConsolePlusLibrary.Style_,bool).applyColorBackground'></a>

`applyColorBackground` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, applies background color across the full line \(default: `false`\)\.

<a name='PromptPlusLibrary.IWidgets.Slider(double,double,double,byte)'></a>

## IWidgets\.Slider\(double, double, double, byte\) Method

Creates a slider widget for displaying a numeric value within a range\.

```csharp
PromptPlusLibrary.ISliderWidget Slider(double value, double minvalue=0.0, double maxvalue=100.0, byte fractionalDigits=2);
```
#### Parameters

<a name='PromptPlusLibrary.IWidgets.Slider(double,double,double,byte).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Initial value to display\.

<a name='PromptPlusLibrary.IWidgets.Slider(double,double,double,byte).minvalue'></a>

`minvalue` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Minimum permitted value \(default: 0\)\.

<a name='PromptPlusLibrary.IWidgets.Slider(double,double,double,byte).maxvalue'></a>

`maxvalue` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Maximum permitted value \(default: 100\)\.

<a name='PromptPlusLibrary.IWidgets.Slider(double,double,double,byte).fractionalDigits'></a>

`fractionalDigits` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

Number of fractional digits to show \(default: 2, maximum: 5\)\.

#### Returns
[ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget')  
An [ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget') for further customization\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when:
           [value](IWidgets.md#PromptPlusLibrary.IWidgets.Slider(double,double,double,byte).value 'PromptPlusLibrary\.IWidgets\.Slider\(double, double, double, byte\)\.value') is less than [minvalue](IWidgets.md#PromptPlusLibrary.IWidgets.Slider(double,double,double,byte).minvalue 'PromptPlusLibrary\.IWidgets\.Slider\(double, double, double, byte\)\.minvalue') or greater than [maxvalue](IWidgets.md#PromptPlusLibrary.IWidgets.Slider(double,double,double,byte).maxvalue 'PromptPlusLibrary\.IWidgets\.Slider\(double, double, double, byte\)\.maxvalue'),
           [minvalue](IWidgets.md#PromptPlusLibrary.IWidgets.Slider(double,double,double,byte).minvalue 'PromptPlusLibrary\.IWidgets\.Slider\(double, double, double, byte\)\.minvalue') is greater than or equal to [maxvalue](IWidgets.md#PromptPlusLibrary.IWidgets.Slider(double,double,double,byte).maxvalue 'PromptPlusLibrary\.IWidgets\.Slider\(double, double, double, byte\)\.maxvalue'),
           [fractionalDigits](IWidgets.md#PromptPlusLibrary.IWidgets.Slider(double,double,double,byte).fractionalDigits 'PromptPlusLibrary\.IWidgets\.Slider\(double, double, double, byte\)\.fractionalDigits') is greater than 5\.

<a name='PromptPlusLibrary.IWidgets.Switch(bool)'></a>

## IWidgets\.Switch\(bool\) Method

Creates a switch widget for displaying a boolean on/off value\.

```csharp
PromptPlusLibrary.ISwitchWidget Switch(bool value);
```
#### Parameters

<a name='PromptPlusLibrary.IWidgets.Switch(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Initial value to display\.

#### Returns
[ISwitchWidget](ISwitchWidget.md 'PromptPlusLibrary\.ISwitchWidget')  
An [ISwitchWidget](ISwitchWidget.md 'PromptPlusLibrary\.ISwitchWidget') for further customization\.