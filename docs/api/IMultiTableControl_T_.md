<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IMultiTableControl\<T\> Interface

Defines the fluent API used to configure and run a multi\-table prompt control\.
The MultiTable control displays items as a navigable table and allows the user
to mark/unmark multiple rows for selection, returning the checked rows as an array\.

```csharp
public interface IMultiTableControl<T>
```
#### Type parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.T'></a>

`T`

The type of items displayed as table rows\.

### Remarks
At least one column \([AddColumn\(string, Func&lt;T,object&gt;, Func&lt;object,string&gt;, Nullable&lt;int&gt;, ColumnAlignment, bool\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.AddColumn\(string, System\.Func\<T,object\>, System\.Func\<object,string\>, System\.Nullable\<int\>, PromptPlusLibrary\.ColumnAlignment, bool\)')\) and one item \([AddItem\(T, bool, bool\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.AddItem(T,bool,bool) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.AddItem\(T, bool, bool\)') or
[AddItems\(IEnumerable&lt;T&gt;, bool, bool\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool,bool) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.AddItems\(System\.Collections\.Generic\.IEnumerable\<T\>, bool, bool\)')\) must be configured before [Run\(CancellationToken\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.Run\(System\.Threading\.CancellationToken\)') is called;
otherwise a [System\.ComponentModel\.DataAnnotations\.ValidationException](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationexception 'System\.ComponentModel\.DataAnnotations\.ValidationException') is thrown\.
### Methods

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool)'></a>

## IMultiTableControl\<T\>\.AddColumn\(string, Func\<T,object\>, Func\<object,string\>, Nullable\<int\>, ColumnAlignment, bool\) Method

Adds a column definition to the table\. At least one column must be added before [Run\(CancellationToken\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.Run\(System\.Threading\.CancellationToken\)')\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> AddColumn(string header, System.Func<T,object> selector, System.Func<object,string>? formatter=null, System.Nullable<int> width=null, PromptPlusLibrary.ColumnAlignment alignment=PromptPlusLibrary.ColumnAlignment.Left, bool isFilterable=false);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).header'></a>

`header` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Column header text\. Cannot be [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null'), empty, or whitespace\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).selector'></a>

`selector` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

Function that extracts the cell value from a row item\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).formatter'></a>

`formatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

Optional function that converts the raw cell value to its display string\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).width'></a>

`width` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Fixed column width in characters\. [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') = auto\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).alignment'></a>

`alignment` [ColumnAlignment](ColumnAlignment.md 'PromptPlusLibrary\.ColumnAlignment')

Cell content alignment\. Default is [Left](ColumnAlignment.md#PromptPlusLibrary.ColumnAlignment.Left 'PromptPlusLibrary\.ColumnAlignment\.Left')\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).isFilterable'></a>

`isFilterable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool'), cell values participate in filter matching\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddItem(T,bool,bool)'></a>

## IMultiTableControl\<T\>\.AddItem\(T, bool, bool\) Method

Adds a single row item to the table\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> AddItem(T value, bool ischecked=false, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddItem(T,bool,bool).value'></a>

`value` [T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')

The row value\. Cannot be [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddItem(T,bool,bool).ischecked'></a>

`ischecked` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') the row starts pre\-checked\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddItem(T,bool,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') the row is shown but cannot be toggled\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool,bool)'></a>

## IMultiTableControl\<T\>\.AddItems\(IEnumerable\<T\>, bool, bool\) Method

Adds multiple row items to the table\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> AddItems(System.Collections.Generic.IEnumerable<T> values, bool ischecked=false, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool,bool).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The row values\. Cannot be [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool,bool).ischecked'></a>

`ischecked` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') all rows start pre\-checked\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') all rows are shown but cannot be toggled\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.ChangeDescription(System.Func_T,string_)'></a>

## IMultiTableControl\<T\>\.ChangeDescription\(Func\<T,string\>\) Method

Registers a synchronous callback that provides the description text shown below
the table whenever the cursor moves to a different row\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> ChangeDescription(System.Func<T,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.ChangeDescription(System.Func_T,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the focused row and returns the description text\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## IMultiTableControl\<T\>\.ChangeDescriptionAsync\(Func\<T,Task\<string\>\>\) Method

Asynchronous counterpart of [ChangeDescription\(Func&lt;T,string&gt;\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.ChangeDescription(System.Func_T,string_) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.ChangeDescription\(System\.Func\<T,string\>\)')\.
The task is awaited synchronously \(blocking\) on the UI thread\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> ChangeDescriptionAsync(System.Func<T,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An asynchronous function that receives the focused row and returns the description text\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.Default(System.Collections.Generic.IEnumerable_T_)'></a>

## IMultiTableControl\<T\>\.Default\(IEnumerable\<T\>\) Method

Pre\-marks all matching items as checked and positions the cursor on the first match\.
Any item previously added with `ischecked = true` retains its state when not
present in [values](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.Default(System.Collections.Generic.IEnumerable_T_).values 'PromptPlusLibrary\.IMultiTableControl\<T\>\.Default\(System\.Collections\.Generic\.IEnumerable\<T\>\)\.values')\. Values in this list take precedence: items
are marked checked regardless of `ischecked` at [AddItem\(T, bool, bool\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.AddItem(T,bool,bool) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.AddItem\(T, bool, bool\)') time\.
Disabled items matching the list are also marked checked \(read\-only visual\)\.
Has no effect when [values](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.Default(System.Collections.Generic.IEnumerable_T_).values 'PromptPlusLibrary\.IMultiTableControl\<T\>\.Default\(System\.Collections\.Generic\.IEnumerable\<T\>\)\.values') is empty\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> Default(System.Collections.Generic.IEnumerable<T> values);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.Default(System.Collections.Generic.IEnumerable_T_).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The values to pre\-check\. Matched via the comparer set by [DefaultMatchBy\(Func&lt;T,T,bool&gt;\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.DefaultMatchBy(System.Func_T,T,bool_) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.DefaultMatchBy\(System\.Func\<T,T,bool\>\)')\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.DefaultMatchBy(System.Func_T,T,bool_)'></a>

## IMultiTableControl\<T\>\.DefaultMatchBy\(Func\<T,T,bool\>\) Method

Overrides the equality comparer used to match default and history values against the
loaded items\. Default is [System\.Collections\.Generic\.EqualityComparer&lt;&gt;\.Default](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.equalitycomparer-1.default 'System\.Collections\.Generic\.EqualityComparer\`1\.Default')\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> DefaultMatchBy(System.Func<T,T,bool> comparer);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.DefaultMatchBy(System.Func_T,T,bool_).comparer'></a>

`comparer` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

A function that returns `true` when two items are considered equal\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_)'></a>

## IMultiTableControl\<T\>\.EnableHistory\(string, Action\<IHistoryOptions\>\) Method

Enables persistent history stored in the specified file\.
The checked array is serialized as JSON and restored on the next run\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> EnableHistory(string filename, System.Action<PromptPlusLibrary.IHistoryOptions>? options=null);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename'></a>

`filename` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The history file name\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An optional callback to configure [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.Filter(PromptPlusLibrary.FilterMode,PromptPlusLibrary.FilterTableMode)'></a>

## IMultiTableControl\<T\>\.Filter\(FilterMode, FilterTableMode\) Method

Enables and configures the row filter feature\.
Default is [Disabled](FilterMode.md#PromptPlusLibrary.FilterMode.Disabled 'PromptPlusLibrary\.FilterMode\.Disabled') with [Answer](FilterTableMode.md#PromptPlusLibrary.FilterTableMode.Answer 'PromptPlusLibrary\.FilterTableMode\.Answer')\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> Filter(PromptPlusLibrary.FilterMode value, PromptPlusLibrary.FilterTableMode filterby=PromptPlusLibrary.FilterTableMode.Answer);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.Filter(PromptPlusLibrary.FilterMode,PromptPlusLibrary.FilterTableMode).value'></a>

`value` [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode')

The [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode') to apply\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.Filter(PromptPlusLibrary.FilterMode,PromptPlusLibrary.FilterTableMode).filterby'></a>

`filterby` [FilterTableMode](FilterTableMode.md 'PromptPlusLibrary\.FilterTableMode')

Determines which data the filter is matched against\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.HideElements(PromptPlusLibrary.HideTable)'></a>

## IMultiTableControl\<T\>\.HideElements\(HideTable\) Method

Specifies which regions of the table are hidden\.
Default is [None](HideTable.md#PromptPlusLibrary.HideTable.None 'PromptPlusLibrary\.HideTable\.None') \(all regions visible\)\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> HideElements(PromptPlusLibrary.HideTable borders);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.HideElements(PromptPlusLibrary.HideTable).borders'></a>

`borders` [HideTable](HideTable.md 'PromptPlusLibrary\.HideTable')

A [HideTable](HideTable.md 'PromptPlusLibrary\.HideTable') flags value that identifies the regions to hide\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.HorizontalScroll(PromptPlusLibrary.HorizontalScrollMode)'></a>

## IMultiTableControl\<T\>\.HorizontalScroll\(HorizontalScrollMode\) Method

Configures how columns are scrolled horizontally when they do not all fit on screen\.
Default is [Full](HorizontalScrollMode.md#PromptPlusLibrary.HorizontalScrollMode.Full 'PromptPlusLibrary\.HorizontalScrollMode\.Full')\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> HorizontalScroll(PromptPlusLibrary.HorizontalScrollMode mode);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.HorizontalScroll(PromptPlusLibrary.HorizontalScrollMode).mode'></a>

`mode` [HorizontalScrollMode](HorizontalScrollMode.md 'PromptPlusLibrary\.HorizontalScrollMode')

The desired [HorizontalScrollMode](HorizontalScrollMode.md 'PromptPlusLibrary\.HorizontalScrollMode')\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTableControl_T__)'></a>

## IMultiTableControl\<T\>\.Interaction\<T1\>\(IEnumerable\<T1\>, Action\<T1,IMultiTableControl\<T\>\>\) Method

Iterates synchronously over [items](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTableControl_T__).items 'PromptPlusLibrary\.IMultiTableControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiTableControl\<T\>\>\)\.items'), invoking
[interactionAction](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTableControl_T__).interactionAction 'PromptPlusLibrary\.IMultiTableControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiTableControl\<T\>\>\)\.interactionAction') for each element, giving the caller a chance
to add rows programmatically via [AddItem\(T, bool, bool\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.AddItem(T,bool,bool) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.AddItem\(T, bool, bool\)') or [AddItems\(IEnumerable&lt;T&gt;, bool, bool\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool,bool) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.AddItems\(System\.Collections\.Generic\.IEnumerable\<T\>, bool, bool\)')\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> Interaction<T1>(System.Collections.Generic.IEnumerable<T1> items, System.Action<T1,PromptPlusLibrary.IMultiTableControl<T>> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTableControl_T__).T1'></a>

`T1`

The type of elements in the input sequence\.
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTableControl_T__).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T1](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTableControl_T__).T1 'PromptPlusLibrary\.IMultiTableControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiTableControl\<T\>\>\)\.T1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The input sequence to iterate\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTableControl_T__).interactionAction'></a>

`interactionAction` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[T1](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTableControl_T__).T1 'PromptPlusLibrary\.IMultiTableControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiTableControl\<T\>\>\)\.T1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')

The action invoked for each element\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiTableControl_T_,System.Threading.Tasks.Task_)'></a>

## IMultiTableControl\<T\>\.InteractionAsync\<T1\>\(IEnumerable\<T1\>, Func\<T1,IMultiTableControl\<T\>,Task\>\) Method

Asynchronous counterpart of [Interaction&lt;T1&gt;\(IEnumerable&lt;T1&gt;, Action&lt;T1,IMultiTableControl&lt;T&gt;&gt;\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTableControl_T__) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiTableControl\<T\>\>\)')\.
Each task returned by [interactionAction](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiTableControl_T_,System.Threading.Tasks.Task_).interactionAction 'PromptPlusLibrary\.IMultiTableControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.IMultiTableControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.interactionAction') is awaited synchronously \(blocking\)\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> InteractionAsync<T1>(System.Collections.Generic.IEnumerable<T1> items, System.Func<T1,PromptPlusLibrary.IMultiTableControl<T>,System.Threading.Tasks.Task> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiTableControl_T_,System.Threading.Tasks.Task_).T1'></a>

`T1`

The type of elements in the input sequence\.
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiTableControl_T_,System.Threading.Tasks.Task_).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T1](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiTableControl_T_,System.Threading.Tasks.Task_).T1 'PromptPlusLibrary\.IMultiTableControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.IMultiTableControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.T1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The input sequence to iterate\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiTableControl_T_,System.Threading.Tasks.Task_).interactionAction'></a>

`interactionAction` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T1](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiTableControl_T_,System.Threading.Tasks.Task_).T1 'PromptPlusLibrary\.IMultiTableControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.IMultiTableControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.T1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

The asynchronous action invoked for each element\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.LayoutMode(PromptPlusLibrary.TableLayoutMode)'></a>

## IMultiTableControl\<T\>\.LayoutMode\(TableLayoutMode\) Method

Sets the table layout mode that controls the box\-drawing characters used for borders\.
Default is [SingleBox](TableLayoutMode.md#PromptPlusLibrary.TableLayoutMode.SingleBox 'PromptPlusLibrary\.TableLayoutMode\.SingleBox')\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> LayoutMode(PromptPlusLibrary.TableLayoutMode mode);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.LayoutMode(PromptPlusLibrary.TableLayoutMode).mode'></a>

`mode` [TableLayoutMode](TableLayoutMode.md 'PromptPlusLibrary\.TableLayoutMode')

The desired [TableLayoutMode](TableLayoutMode.md 'PromptPlusLibrary\.TableLayoutMode')\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IMultiTableControl\<T\>\.Options\(Action\<IControlOptions\>\) Method

Applies global control options via the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') fluent API\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action that configures the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') instance\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [options](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.IMultiTableControl\<T\>\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.PageSize(byte)'></a>

## IMultiTableControl\<T\>\.PageSize\(byte\) Method

Sets the maximum number of rows per page\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> PageSize(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.PageSize(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

Maximum rows per page\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.PredicateChecked(System.Func_T,bool_)'></a>

## IMultiTableControl\<T\>\.PredicateChecked\(Func\<T,bool\>\) Method

Sets a synchronous predicate that determines whether a row can be checked\.
Returns `false` to prevent checking and show a generic error message\.
Only evaluated when marking a row as checked — unchecking an already\-checked row is
always allowed \(subject only to it not being disabled\) and never runs this predicate\.
Replaces any previously registered asynchronous predicate\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> PredicateChecked(System.Func<T,bool> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.PredicateChecked(System.Func_T,bool_).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A predicate that returns `true` when the row can be checked\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.PredicateCheckedAsync(System.Func_T,System.Threading.Tasks.Task_bool__)'></a>

## IMultiTableControl\<T\>\.PredicateCheckedAsync\(Func\<T,Task\<bool\>\>\) Method

Asynchronous counterpart of [PredicateChecked\(Func&lt;T,bool&gt;\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.PredicateChecked(System.Func_T,bool_) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.PredicateChecked\(System\.Func\<T,bool\>\)')\.
The predicate is evaluated synchronously \(blocking\) on the UI thread\.
Replaces any previously registered synchronous predicate\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> PredicateCheckedAsync(System.Func<T,System.Threading.Tasks.Task<bool>> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.PredicateCheckedAsync(System.Func_T,System.Threading.Tasks.Task_bool__).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An asynchronous predicate that returns `true` when the row can be checked\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.Range(int,System.Nullable_int_)'></a>

## IMultiTableControl\<T\>\.Range\(int, Nullable\<int\>\) Method

Constrains the number of checked items at confirmation time\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> Range(int minvalue, System.Nullable<int> maxvalue=null);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.Range(int,System.Nullable_int_).minvalue'></a>

`minvalue` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Minimum number of checked items required\. Must be \>= 0\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.Range(int,System.Nullable_int_).maxvalue'></a>

`maxvalue` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Maximum number of checked items allowed\. [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') = unlimited\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.Run(System.Threading.CancellationToken)'></a>

## IMultiTableControl\<T\>\.Run\(CancellationToken\) Method

Runs the multi\-table control, blocking until the user confirms or cancels\.

```csharp
PromptPlusLibrary.ResultPrompt<T[]> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Cancellation token\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') wrapping a `T[]` that contains all checked row values\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.Styles(PromptPlusLibrary.MultiTableStyles,ConsolePlusLibrary.Style)'></a>

## IMultiTableControl\<T\>\.Styles\(MultiTableStyles, Style\) Method

Overrides a specific visual style used by the multi\-table control\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> Styles(PromptPlusLibrary.MultiTableStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.Styles(PromptPlusLibrary.MultiTableStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [MultiTableStyles](MultiTableStyles.md 'PromptPlusLibrary\.MultiTableStyles')

The [MultiTableStyles](MultiTableStyles.md 'PromptPlusLibrary\.MultiTableStyles') element whose style is overridden\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.Styles(PromptPlusLibrary.MultiTableStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.TextSelector(System.Func_T,string_)'></a>

## IMultiTableControl\<T\>\.TextSelector\(Func\<T,string\>\) Method

Registers a synchronous callback that converts a row value to the answer text
displayed in the header after the control completes\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> TextSelector(System.Func<T,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.TextSelector(System.Func_T,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the row value and returns its display text\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.TextSelectorAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## IMultiTableControl\<T\>\.TextSelectorAsync\(Func\<T,Task\<string\>\>\) Method

Asynchronous counterpart of [TextSelector\(Func&lt;T,string&gt;\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.TextSelector(System.Func_T,string_) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.TextSelector\(System\.Func\<T,string\>\)')\.
The task is awaited synchronously \(blocking\) on the UI thread\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> TextSelectorAsync(System.Func<T,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.TextSelectorAsync(System.Func_T,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An asynchronous function that receives the row value and returns its display text\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTableControl_T_.UseDefaultHistory()'></a>

## IMultiTableControl\<T\>\.UseDefaultHistory\(\) Method

Loads the most recent history entry as the initial checked set, clearing any
value previously set by [Default\(IEnumerable&lt;T&gt;\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.Default(System.Collections.Generic.IEnumerable_T_) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.Default\(System\.Collections\.Generic\.IEnumerable\<T\>\)')\.
Has no effect when history is not enabled via [EnableHistory\(string, Action&lt;IHistoryOptions&gt;\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.EnableHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)')\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> UseDefaultHistory();
```

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')

<a name='PromptPlusLibrary.IMultiTableControl_T_.ViewOnly(bool)'></a>

## IMultiTableControl\<T\>\.ViewOnly\(bool\) Method

Enables view\-only mode: the user can navigate rows but cannot toggle checkboxes\.
Items marked via [Default\(IEnumerable&lt;T&gt;\)](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.Default(System.Collections.Generic.IEnumerable_T_) 'PromptPlusLibrary\.IMultiTableControl\<T\>\.Default\(System\.Collections\.Generic\.IEnumerable\<T\>\)') are still pre\-checked \(read\-only visual\)\.
Default is `false`\.

```csharp
PromptPlusLibrary.IMultiTableControl<T> ViewOnly(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTableControl_T_.ViewOnly(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to enable view\-only mode; otherwise, `false`\.

#### Returns
[PromptPlusLibrary\.IMultiTableControl&lt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')[T](IMultiTableControl_T_.md#PromptPlusLibrary.IMultiTableControl_T_.T 'PromptPlusLibrary\.IMultiTableControl\<T\>\.T')[&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>')  
The current [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') instance for chaining\.