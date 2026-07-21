<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IControls Interface

Defines a factory interface for creating interactive PromptPlus controls\.

```csharp
public interface IControls
```
### Methods

<a name='PromptPlusLibrary.IControls.Calendar(string,string)'></a>

## IControls\.Calendar\(string, string\) Method

Creates an Calendar control with the specified prompt\.

```csharp
PromptPlusLibrary.ICalendarControl Calendar(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.Calendar(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.Calendar(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
An [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.ChartBar(string,string)'></a>

## IControls\.ChartBar\(string, string\) Method

Creates an interactive chart bar control for visualizing data as horizontal bars\.

```csharp
PromptPlusLibrary.IChartBarControl ChartBar(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.ChartBar(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.ChartBar(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
An [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance for further configuration and execution\.

<a name='PromptPlusLibrary.IControls.Confirm(string,string,bool)'></a>

## IControls\.Confirm\(string, string, bool\) Method

Creates an KeyPress control with the specified prompt and in yes/no mode\.

```csharp
PromptPlusLibrary.IKeyPressControl Confirm(string prompt="", string? description=null, bool showresult=false);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.Confirm(string,string,bool).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.Confirm(string,string,bool).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

<a name='PromptPlusLibrary.IControls.Confirm(string,string,bool).showresult'></a>

`showresult` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, shown KeyPress result; otherwise, they will be hidden after finish\.

#### Returns
[IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl')  
An [IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.File(string,string)'></a>

## IControls\.File\(string, string\) Method

Creates a file control that browses the file system as an expandable/collapsible tree
of directories and files, loading contents lazily to keep memory usage low\.

```csharp
PromptPlusLibrary.IFileControl File(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.File(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.File(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context\.

#### Returns
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl')  
An [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance for further configuration and execution\.

<a name='PromptPlusLibrary.IControls.History(string)'></a>

## IControls\.History\(string\) Method

Creates a history object for managing persisted history operations\.

```csharp
PromptPlusLibrary.IHistory History(string filename);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.History(string).filename'></a>

`filename` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The history file name\. Cannot be `null`\.

#### Returns
[IHistory](IHistory.md 'PromptPlusLibrary\.IHistory')  
An [IHistory](IHistory.md 'PromptPlusLibrary\.IHistory') instance for managing persisted history operations\.

<a name='PromptPlusLibrary.IControls.Input(string,string)'></a>

## IControls\.Input\(string, string\) Method

Creates an input control with the specified prompt\.

```csharp
PromptPlusLibrary.IInputControl Input(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.Input(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.Input(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context for the input\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
An [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for further configuration and execution\.

<a name='PromptPlusLibrary.IControls.KeyPress(string,string,bool)'></a>

## IControls\.KeyPress\(string, string, bool\) Method

Creates an KeyPress control with the specified prompt\.

```csharp
PromptPlusLibrary.IKeyPressControl KeyPress(string prompt="", string? description=null, bool showresult=false);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.KeyPress(string,string,bool).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.KeyPress(string,string,bool).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

<a name='PromptPlusLibrary.IControls.KeyPress(string,string,bool).showresult'></a>

`showresult` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, shown KeyPress result; otherwise, they will be hidden after finish\.

#### Returns
[IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl')  
An [IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.MaskDate(string,string)'></a>

## IControls\.MaskDate\(string, string\) Method

Creates an MaskEdit\(DateTime\) control \(date only\) with the specified prompt\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<System.DateTime> MaskDate(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.MaskDate(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.MaskDate(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
An [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.MaskDateOnly(string,string)'></a>

## IControls\.MaskDateOnly\(string, string\) Method

Creates an MaskEdit\(DateOnly\) control with the specified prompt\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<System.DateOnly> MaskDateOnly(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.MaskDateOnly(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.MaskDateOnly(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
An [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.MaskDateTime(string,string)'></a>

## IControls\.MaskDateTime\(string, string\) Method

Creates an MaskEdit\(DateTime\) control \(date and time\) with the specified prompt\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<System.DateTime> MaskDateTime(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.MaskDateTime(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.MaskDateTime(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
An [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.MaskDecimal(string,string)'></a>

## IControls\.MaskDecimal\(string, string\) Method

Creates an MaskEdit\(decimal\) control with the specified prompt\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<decimal> MaskDecimal(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.MaskDecimal(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.MaskDecimal(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
An [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.MaskDecimalCurrency(string,string)'></a>

## IControls\.MaskDecimalCurrency\(string, string\) Method

Creates an MaskEdit\(decimal\) currency control with the specified prompt\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<decimal> MaskDecimalCurrency(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.MaskDecimalCurrency(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.MaskDecimalCurrency(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
An [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.MaskDouble(string,string)'></a>

## IControls\.MaskDouble\(string, string\) Method

Creates an MaskEdit\(double\) control with the specified prompt\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<double> MaskDouble(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.MaskDouble(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.MaskDouble(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
An [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.MaskDoubleCurrency(string,string)'></a>

## IControls\.MaskDoubleCurrency\(string, string\) Method

Creates an MaskEdit\(double\) currency control with the specified prompt\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<double> MaskDoubleCurrency(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.MaskDoubleCurrency(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.MaskDoubleCurrency(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
An [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.MaskEdit(string,string)'></a>

## IControls\.MaskEdit\(string, string\) Method

Creates an MaskEdit\(string\) control with the specified prompt\.

```csharp
PromptPlusLibrary.IMaskEditStringControl<string> MaskEdit(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.MaskEdit(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.MaskEdit(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[PromptPlusLibrary\.IMaskEditStringControl&lt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')  
An [IMaskEditStringControl&lt;T&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.MaskInteger(string,string)'></a>

## IControls\.MaskInteger\(string, string\) Method

Creates an MaskEdit\(int\) control with the specified prompt\.

```csharp
PromptPlusLibrary.IMaskEditNumberControl<int> MaskInteger(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.MaskInteger(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.MaskInteger(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[PromptPlusLibrary\.IMaskEditNumberControl&lt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')  
An [IMaskEditNumberControl&lt;T&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.MaskLong(string,string)'></a>

## IControls\.MaskLong\(string, string\) Method

Creates an MaskEdit\(long\) control with the specified prompt\.

```csharp
PromptPlusLibrary.IMaskEditNumberControl<long> MaskLong(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.MaskLong(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.MaskLong(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[PromptPlusLibrary\.IMaskEditNumberControl&lt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')  
An [IMaskEditNumberControl&lt;T&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.MaskTime(string,string)'></a>

## IControls\.MaskTime\(string, string\) Method

Creates an MaskEdit\(DateTime\) control \(time only\) with the specified prompt\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<System.DateTime> MaskTime(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.MaskTime(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.MaskTime(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
An [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.MaskTimeOnly(string,string)'></a>

## IControls\.MaskTimeOnly\(string, string\) Method

Creates an MaskEdit\(TimeOnly\) control with the specified prompt\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<System.TimeOnly> MaskTimeOnly(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.MaskTimeOnly(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.MaskTimeOnly(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
An [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.MultiFile(string,string)'></a>

## IControls\.MultiFile\(string, string\) Method

Creates a multi\-file control that browses the file system as an expandable/collapsible tree
of directories and files, allowing multiple files and/or folders to be checked and returned
at once, loading contents lazily to keep memory usage low\.

```csharp
PromptPlusLibrary.IMultiFileControl MultiFile(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.MultiFile(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.MultiFile(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
An [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for further configuration and execution\.

<a name='PromptPlusLibrary.IControls.MultiSelect_T_(string,string)'></a>

## IControls\.MultiSelect\<T\>\(string, string\) Method

Creates a multi\-select control for choosing multiple options from a list\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> MultiSelect<T>(string prompt="", string? description=null);
```
#### Type parameters

<a name='PromptPlusLibrary.IControls.MultiSelect_T_(string,string).T'></a>

`T`

The type of the items in the selection list\.
#### Parameters

<a name='PromptPlusLibrary.IControls.MultiSelect_T_(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.MultiSelect_T_(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context for the selection\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IControls.md#PromptPlusLibrary.IControls.MultiSelect_T_(string,string).T 'PromptPlusLibrary\.IControls\.MultiSelect\<T\>\(string, string\)\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
An [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for further configuration and execution\.

<a name='PromptPlusLibrary.IControls.MultiTable_T_(string,string)'></a>

## IControls\.MultiTable\<T\>\(string, string\) Method

Creates a multi\-table control for navigating a table and selecting multiple rows\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> MultiTable<T>(string prompt="", string? description=null);
```
#### Type parameters

<a name='PromptPlusLibrary.IControls.MultiTable_T_(string,string).T'></a>

`T`

The type of the items in the table rows\.
#### Parameters

<a name='PromptPlusLibrary.IControls.MultiTable_T_(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.MultiTable_T_(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context for the table interaction\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IControls.md#PromptPlusLibrary.IControls.MultiTable_T_(string,string).T 'PromptPlusLibrary\.IControls\.MultiTable\<T\>\(string, string\)\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
An [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for further configuration and execution\.

<a name='PromptPlusLibrary.IControls.MultiTasks(string,string)'></a>

## IControls\.MultiTasks\(string, string\) Method

Creates a multi\-tasks control that runs several tasks \(sequentially or in parallel\),
presenting a paginated execution list with waiting/running/success/failure indicators\.

```csharp
PromptPlusLibrary.IMultiTasksControl MultiTasks(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.MultiTasks(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.MultiTasks(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
An [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for further configuration and execution\.

<a name='PromptPlusLibrary.IControls.MultiTree_T_(string,string)'></a>

## IControls\.MultiTree\<T\>\(string, string\) Method

Creates a generic multi\-selection tree control that browses a hierarchy of user items
of type [T](IControls.md#PromptPlusLibrary.IControls.MultiTree_T_(string,string).T 'PromptPlusLibrary\.IControls\.MultiTree\<T\>\(string, string\)\.T') with tri\-state checkboxes \(unchecked / checked / indeterminate\)\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> MultiTree<T>(string prompt="", string? description=null);
```
#### Type parameters

<a name='PromptPlusLibrary.IControls.MultiTree_T_(string,string).T'></a>

`T`

The type of items in the tree\.
#### Parameters

<a name='PromptPlusLibrary.IControls.MultiTree_T_(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.MultiTree_T_(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context\.

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IControls.md#PromptPlusLibrary.IControls.MultiTree_T_(string,string).T 'PromptPlusLibrary\.IControls\.MultiTree\<T\>\(string, string\)\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')  
An [IMultiTreeControl&lt;T&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>') instance for further configuration and execution\.

<a name='PromptPlusLibrary.IControls.ProgressBar(string,string)'></a>

## IControls\.ProgressBar\(string, string\) Method

Creates an Progress Bar control with the specified prompt\.

```csharp
PromptPlusLibrary.IProgressBarControl ProgressBar(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.ProgressBar(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.ProgressBar(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
An [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for further configuration and wait progress\.

<a name='PromptPlusLibrary.IControls.Secret(string,string)'></a>

## IControls\.Secret\(string, string\) Method

Creates a secret \(masked\) input control with the specified prompt\.

```csharp
PromptPlusLibrary.IInputSecretControl Secret(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.Secret(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.Secret(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context for the input\.

#### Returns
[IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl')  
An [IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl') instance for further configuration and execution\.

<a name='PromptPlusLibrary.IControls.Select_T_(string,string)'></a>

## IControls\.Select\<T\>\(string, string\) Method

Creates a select control for choosing a single option from a list\.

```csharp
PromptPlusLibrary.ISelectControl<T> Select<T>(string prompt="", string? description=null);
```
#### Type parameters

<a name='PromptPlusLibrary.IControls.Select_T_(string,string).T'></a>

`T`

The type of the items in the selection list\.
#### Parameters

<a name='PromptPlusLibrary.IControls.Select_T_(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.Select_T_(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context for the selection\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](IControls.md#PromptPlusLibrary.IControls.Select_T_(string,string).T 'PromptPlusLibrary\.IControls\.Select\<T\>\(string, string\)\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
An [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for further configuration and execution\.

<a name='PromptPlusLibrary.IControls.Slider(string,string)'></a>

## IControls\.Slider\(string, string\) Method

Creates an Slider control with the specified prompt\.

```csharp
PromptPlusLibrary.ISliderControl Slider(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.Slider(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt\.

<a name='PromptPlusLibrary.IControls.Slider(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description for input

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
An [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance for further configuration and reading input\.

<a name='PromptPlusLibrary.IControls.Switch(string,string)'></a>

## IControls\.Switch\(string, string\) Method

Creates a switch control for toggling a boolean value\.

```csharp
PromptPlusLibrary.ISwitchControl Switch(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.Switch(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.Switch(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context for the switch\.

#### Returns
[ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl')  
An [ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl') instance for further configuration and execution\.

<a name='PromptPlusLibrary.IControls.Table_T_(string,string)'></a>

## IControls\.Table\<T\>\(string, string\) Method

Creates a table control for navigating and selecting tabular rows/cells\.

```csharp
PromptPlusLibrary.ITableControl<T> Table<T>(string prompt="", string? description=null);
```
#### Type parameters

<a name='PromptPlusLibrary.IControls.Table_T_(string,string).T'></a>

`T`

The type of the items in the table rows\.
#### Parameters

<a name='PromptPlusLibrary.IControls.Table_T_(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.Table_T_(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context for the table interaction\.

#### Returns
[PromptPlusLibrary\.ITableControl&lt;](ITableControl_T_.md 'PromptPlusLibrary\.ITableControl\<T\>')[T](IControls.md#PromptPlusLibrary.IControls.Table_T_(string,string).T 'PromptPlusLibrary\.IControls\.Table\<T\>\(string, string\)\.T')[&gt;](ITableControl_T_.md 'PromptPlusLibrary\.ITableControl\<T\>')  
An [ITableControl&lt;T&gt;](ITableControl_T_.md 'PromptPlusLibrary\.ITableControl\<T\>') instance for further configuration and execution\.

<a name='PromptPlusLibrary.IControls.Task(string,string)'></a>

## IControls\.Task\(string, string\) Method

Creates a task control that runs a synchronous or asynchronous action/function and waits
for it to complete, optionally displaying elapsed time and an animated spinner\.

```csharp
PromptPlusLibrary.ITaskControl Task(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.Task(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.Task(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context for the task\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
An [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for further configuration and execution\.

<a name='PromptPlusLibrary.IControls.Time(string,string)'></a>

## IControls\.Time\(string, string\) Method

Creates a time control that suspends execution for a fixed duration while displaying a live countdown\.

```csharp
PromptPlusLibrary.ITimeControl Time(string prompt="", string? description=null);
```
#### Parameters

<a name='PromptPlusLibrary.IControls.Time(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.Time(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context for the countdown\.

#### Returns
[ITimeControl](ITimeControl.md 'PromptPlusLibrary\.ITimeControl')  
An [ITimeControl](ITimeControl.md 'PromptPlusLibrary\.ITimeControl') instance for further configuration and execution\.

<a name='PromptPlusLibrary.IControls.Tree_T_(string,string)'></a>

## IControls\.Tree\<T\>\(string, string\) Method

Creates a generic tree control that browses a hierarchy of user items of type
[T](IControls.md#PromptPlusLibrary.IControls.Tree_T_(string,string).T 'PromptPlusLibrary\.IControls\.Tree\<T\>\(string, string\)\.T') as an expandable/collapsible tree, loading children lazily\.

```csharp
PromptPlusLibrary.ITreeControl<T> Tree<T>(string prompt="", string? description=null);
```
#### Type parameters

<a name='PromptPlusLibrary.IControls.Tree_T_(string,string).T'></a>

`T`

The type of items in the tree\.
#### Parameters

<a name='PromptPlusLibrary.IControls.Tree_T_(string,string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text prompt displayed to the user\.

<a name='PromptPlusLibrary.IControls.Tree_T_(string,string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional description providing additional context\.

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](IControls.md#PromptPlusLibrary.IControls.Tree_T_(string,string).T 'PromptPlusLibrary\.IControls\.Tree\<T\>\(string, string\)\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')  
An [ITreeControl&lt;T&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>') instance for further configuration and execution\.