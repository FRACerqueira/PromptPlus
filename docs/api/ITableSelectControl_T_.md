<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ITableSelectControl\<T\> Interface

Provides a fluent API for configuring and running the TableSelect control — a single\-row\-selection table\.

```csharp
public interface ITableSelectControl<T>
```
#### Type parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.T'></a>

`T`

The type of items displayed as table rows\.

### Remarks
The control renders item data as a paginated table with named columns, optional row
filtering, history persistence, and view\-only mode\. The user navigates rows with the
arrow keys and confirms with `Enter`\. At least one column \([AddColumn\(string, Func&lt;T,object&gt;, Func&lt;object,string&gt;, Nullable&lt;int&gt;, ColumnAlignment, bool\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.AddColumn\(string, System\.Func\<T,object\>, System\.Func\<object,string\>, System\.Nullable\<int\>, PromptPlusLibrary\.ColumnAlignment, bool\)')\)
and one item \([AddItem\(T, bool\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.AddItem(T,bool) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.AddItem\(T, bool\)') or [AddItems\(IEnumerable&lt;T&gt;, bool\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.AddItems\(System\.Collections\.Generic\.IEnumerable\<T\>, bool\)')\)
must be configured before [Run\(CancellationToken\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Run\(System\.Threading\.CancellationToken\)') is called, otherwise a
[System\.ComponentModel\.DataAnnotations\.ValidationException](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationexception 'System\.ComponentModel\.DataAnnotations\.ValidationException') is thrown\.
Every configuration method returns the same [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance so
calls can be chained \(fluent style\)\. Call [Run\(CancellationToken\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Run\(System\.Threading\.CancellationToken\)') last\.
### Methods

<a name='PromptPlusLibrary.ITableSelectControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool)'></a>

## ITableSelectControl\<T\>\.AddColumn\(string, Func\<T,object\>, Func\<object,string\>, Nullable\<int\>, ColumnAlignment, bool\) Method

Adds a column definition to the table\. At least one column must be added before [Run\(CancellationToken\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Run\(System\.Threading\.CancellationToken\)')\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> AddColumn(string header, System.Func<T,object> selector, System.Func<object,string>? formatter=null, System.Nullable<int> width=null, PromptPlusLibrary.ColumnAlignment alignment=PromptPlusLibrary.ColumnAlignment.Left, bool isFilterable=false);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).header'></a>

`header` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Column header text\. Cannot be [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null'), empty, or whitespace\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).selector'></a>

`selector` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

Function that extracts the cell value from a row item\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).formatter'></a>

`formatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

Optional function that converts the raw cell value to its display string\.
When [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null'), the raw value's `ToString()` result is used\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).width'></a>

`width` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Fixed column width in characters\. When [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') \(default\), the width is
automatically calculated from the header text and all cell values at [Run\(CancellationToken\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Run\(System\.Threading\.CancellationToken\)') time\.
Must be greater than zero when specified\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).alignment'></a>

`alignment` [ColumnAlignment](ColumnAlignment.md 'PromptPlusLibrary\.ColumnAlignment')

Cell content alignment\. Default is [Left](ColumnAlignment.md#PromptPlusLibrary.ColumnAlignment.Left 'PromptPlusLibrary\.ColumnAlignment\.Left')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).isFilterable'></a>

`isFilterable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool'), cell values of this column participate in filter matching\.
Default is [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [header](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).header 'PromptPlusLibrary\.ITableSelectControl\<T\>\.AddColumn\(string, System\.Func\<T,object\>, System\.Func\<object,string\>, System\.Nullable\<int\>, PromptPlusLibrary\.ColumnAlignment, bool\)\.header') or [selector](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).selector 'PromptPlusLibrary\.ITableSelectControl\<T\>\.AddColumn\(string, System\.Func\<T,object\>, System\.Func\<object,string\>, System\.Nullable\<int\>, PromptPlusLibrary\.ColumnAlignment, bool\)\.selector') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when [header](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).header 'PromptPlusLibrary\.ITableSelectControl\<T\>\.AddColumn\(string, System\.Func\<T,object\>, System\.Func\<object,string\>, System\.Nullable\<int\>, PromptPlusLibrary\.ColumnAlignment, bool\)\.header') is empty or whitespace\.

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [width](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool).width 'PromptPlusLibrary\.ITableSelectControl\<T\>\.AddColumn\(string, System\.Func\<T,object\>, System\.Func\<object,string\>, System\.Nullable\<int\>, PromptPlusLibrary\.ColumnAlignment, bool\)\.width') is specified and is not greater than zero\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.AddItem(T,bool)'></a>

## ITableSelectControl\<T\>\.AddItem\(T, bool\) Method

Adds a single row item to the table\. At least one item must be added before [Run\(CancellationToken\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Run\(System\.Threading\.CancellationToken\)')\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> AddItem(T value, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.AddItem(T,bool).value'></a>

`value` [T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')

The row value\. Cannot be [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.AddItem(T,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') the row is shown but cannot be selected\.
Default is [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.AddItem(T,bool).value 'PromptPlusLibrary\.ITableSelectControl\<T\>\.AddItem\(T, bool\)\.value') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool)'></a>

## ITableSelectControl\<T\>\.AddItems\(IEnumerable\<T\>, bool\) Method

Adds multiple row items to the table\. At least one item must be added before [Run\(CancellationToken\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Run\(System\.Threading\.CancellationToken\)')\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> AddItems(System.Collections.Generic.IEnumerable<T> values, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The row values\. Cannot be [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') all rows in [values](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool).values 'PromptPlusLibrary\.ITableSelectControl\<T\>\.AddItems\(System\.Collections\.Generic\.IEnumerable\<T\>, bool\)\.values') are shown but cannot be selected\.
Default is [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [values](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool).values 'PromptPlusLibrary\.ITableSelectControl\<T\>\.AddItems\(System\.Collections\.Generic\.IEnumerable\<T\>, bool\)\.values') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.ChangeDescription(System.Func_T,string_)'></a>

## ITableSelectControl\<T\>\.ChangeDescription\(Func\<T,string\>\) Method

Registers a synchronous callback that provides the description text shown below
the table whenever the cursor moves to a different row\.
Replaces any previously registered asynchronous description callback\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> ChangeDescription(System.Func<T,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.ChangeDescription(System.Func_T,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the currently highlighted row value and returns the description string\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.ChangeDescription(System.Func_T,string_).value 'PromptPlusLibrary\.ITableSelectControl\<T\>\.ChangeDescription\(System\.Func\<T,string\>\)\.value') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## ITableSelectControl\<T\>\.ChangeDescriptionAsync\(Func\<T,Task\<string\>\>\) Method

Registers an asynchronous callback that provides the description text shown below
the table whenever the cursor moves to a different row\.
Replaces any previously registered synchronous description callback\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> ChangeDescriptionAsync(System.Func<T,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the currently highlighted row value and returns a
[System\.Threading\.Tasks\.Task&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1') of [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String') with the description\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.ITableSelectControl\<T\>\.ChangeDescriptionAsync\(System\.Func\<T,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.Default(T,bool)'></a>

## ITableSelectControl\<T\>\.Default\(T, bool\) Method

Pre\-selects a row as the initial cursor position\.
The row is matched against the item list using the comparer set by [DefaultMatchBy\(Func&lt;T,T,bool&gt;\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.DefaultMatchBy(System.Func_T,T,bool_) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.DefaultMatchBy\(System\.Func\<T,T,bool\>\)')
\(default: [System\.Collections\.Generic\.EqualityComparer&lt;&gt;\.Default](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.equalitycomparer-1.default 'System\.Collections\.Generic\.EqualityComparer\`1\.Default')\)\.
Disabled rows and rows rejected by a selection predicate are not pre\-selected\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> Default(T value, bool useDefaultHistory=true);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.Default(T,bool).value'></a>

`value` [T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')

The value to pre\-select\. Cannot be [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.Default(T,bool).useDefaultHistory'></a>

`useDefaultHistory` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') \(default\) and history is enabled via [EnableHistory\(string, Action&lt;IHistoryOptions&gt;\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.EnableHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)'),
the most recent history entry overrides this value as the initial selection\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Default(T,bool).value 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Default\(T, bool\)\.value') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.DefaultMatchBy(System.Func_T,T,bool_)'></a>

## ITableSelectControl\<T\>\.DefaultMatchBy\(Func\<T,T,bool\>\) Method

Overrides the equality comparer used to locate the default row and match history values\.
Default is [System\.Collections\.Generic\.EqualityComparer&lt;&gt;\.Default](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.equalitycomparer-1.default 'System\.Collections\.Generic\.EqualityComparer\`1\.Default')\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> DefaultMatchBy(System.Func<T,T,bool> comparer);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.DefaultMatchBy(System.Func_T,T,bool_).comparer'></a>

`comparer` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

A function that returns [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when two row values are considered equal\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [comparer](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.DefaultMatchBy(System.Func_T,T,bool_).comparer 'PromptPlusLibrary\.ITableSelectControl\<T\>\.DefaultMatchBy\(System\.Func\<T,T,bool\>\)\.comparer') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_)'></a>

## ITableSelectControl\<T\>\.EnableHistory\(string, Action\<IHistoryOptions\>\) Method

Enables persistent history stored in the specified file, and optionally customises
the history behaviour via [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> EnableHistory(string filename, System.Action<PromptPlusLibrary.IHistoryOptions>? options=null);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename'></a>

`filename` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Path or name of the file used to persist history entries\. Cannot be [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null'),
empty, or whitespace\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

Optional action to further configure the history feature \(max entries, expiry, etc\.\)\.
When [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null'), default history settings are used\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [filename](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename 'PromptPlusLibrary\.ITableSelectControl\<T\>\.EnableHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)\.filename') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when [filename](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename 'PromptPlusLibrary\.ITableSelectControl\<T\>\.EnableHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)\.filename') is empty or whitespace\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.Filter(PromptPlusLibrary.FilterMode,PromptPlusLibrary.FilterTableMode)'></a>

## ITableSelectControl\<T\>\.Filter\(FilterMode, FilterTableMode\) Method

Enables and configures the row filter feature\.
Default is [Disabled](FilterMode.md#PromptPlusLibrary.FilterMode.Disabled 'PromptPlusLibrary\.FilterMode\.Disabled') with [Answer](FilterTableMode.md#PromptPlusLibrary.FilterTableMode.Answer 'PromptPlusLibrary\.FilterTableMode\.Answer')\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> Filter(PromptPlusLibrary.FilterMode value, PromptPlusLibrary.FilterTableMode filterby=PromptPlusLibrary.FilterTableMode.Answer);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.Filter(PromptPlusLibrary.FilterMode,PromptPlusLibrary.FilterTableMode).value'></a>

`value` [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode')

The [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode') to apply\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.Filter(PromptPlusLibrary.FilterMode,PromptPlusLibrary.FilterTableMode).filterby'></a>

`filterby` [FilterTableMode](FilterTableMode.md 'PromptPlusLibrary\.FilterTableMode')

Determines which data the filter is matched against\.
Default is [Answer](FilterTableMode.md#PromptPlusLibrary.FilterTableMode.Answer 'PromptPlusLibrary\.FilterTableMode\.Answer')\.
Only columns marked with `isFilterable = true` in [AddColumn\(string, Func&lt;T,object&gt;, Func&lt;object,string&gt;, Nullable&lt;int&gt;, ColumnAlignment, bool\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.AddColumn(string,System.Func_T,object_,System.Func_object,string_,System.Nullable_int_,PromptPlusLibrary.ColumnAlignment,bool) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.AddColumn\(string, System\.Func\<T,object\>, System\.Func\<object,string\>, System\.Nullable\<int\>, PromptPlusLibrary\.ColumnAlignment, bool\)')
participate when [FilterTableMode](FilterTableMode.md 'PromptPlusLibrary\.FilterTableMode') targets column content\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.HideElements(PromptPlusLibrary.HideTable)'></a>

## ITableSelectControl\<T\>\.HideElements\(HideTable\) Method

Specifies which border regions of the table are hidden\.
Default is [None](HideTable.md#PromptPlusLibrary.HideTable.None 'PromptPlusLibrary\.HideTable\.None') \(all borders visible\)\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> HideElements(PromptPlusLibrary.HideTable borders);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.HideElements(PromptPlusLibrary.HideTable).borders'></a>

`borders` [HideTable](HideTable.md 'PromptPlusLibrary\.HideTable')

A [HideTable](HideTable.md 'PromptPlusLibrary\.HideTable') flags value that identifies the regions to hide\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.HorizontalScroll(PromptPlusLibrary.HorizontalScrollMode)'></a>

## ITableSelectControl\<T\>\.HorizontalScroll\(HorizontalScrollMode\) Method

Configures how columns are scrolled horizontally when they do not all fit on screen\.
Default is [Full](HorizontalScrollMode.md#PromptPlusLibrary.HorizontalScrollMode.Full 'PromptPlusLibrary\.HorizontalScrollMode\.Full')\.
When all columns fit within the console width, horizontal scrolling is inactive and
column\-navigation keys \(Tab / Shift\+Tab\) are ignored\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> HorizontalScroll(PromptPlusLibrary.HorizontalScrollMode mode);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.HorizontalScroll(PromptPlusLibrary.HorizontalScrollMode).mode'></a>

`mode` [HorizontalScrollMode](HorizontalScrollMode.md 'PromptPlusLibrary\.HorizontalScrollMode')

The desired [HorizontalScrollMode](HorizontalScrollMode.md 'PromptPlusLibrary\.HorizontalScrollMode')\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITableSelectControl_T__)'></a>

## ITableSelectControl\<T\>\.Interaction\<T1\>\(IEnumerable\<T1\>, Action\<T1,ITableSelectControl\<T\>\>\) Method

Iterates synchronously over [items](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITableSelectControl_T__).items 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITableSelectControl\<T\>\>\)\.items'), invoking [interactionAction](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITableSelectControl_T__).interactionAction 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITableSelectControl\<T\>\>\)\.interactionAction')
for each element to allow programmatic configuration of the control \(e\.g\. bulk [AddItem\(T, bool\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.AddItem(T,bool) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.AddItem\(T, bool\)') calls\)\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> Interaction<T1>(System.Collections.Generic.IEnumerable<T1> items, System.Action<T1,PromptPlusLibrary.ITableSelectControl<T>> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITableSelectControl_T__).T1'></a>

`T1`

Type of elements in the input sequence\.
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITableSelectControl_T__).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T1](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITableSelectControl_T__).T1 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITableSelectControl\<T\>\>\)\.T1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The input sequence to iterate\. Cannot be [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITableSelectControl_T__).interactionAction'></a>

`interactionAction` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[T1](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITableSelectControl_T__).T1 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITableSelectControl\<T\>\>\)\.T1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')

The action invoked for each element, receiving the element and the current
[ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance\. Cannot be [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [items](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITableSelectControl_T__).items 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITableSelectControl\<T\>\>\)\.items') or [interactionAction](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITableSelectControl_T__).interactionAction 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITableSelectControl\<T\>\>\)\.interactionAction') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITableSelectControl_T_,System.Threading.Tasks.Task_)'></a>

## ITableSelectControl\<T\>\.InteractionAsync\<T1\>\(IEnumerable\<T1\>, Func\<T1,ITableSelectControl\<T\>,Task\>\) Method

Iterates over [items](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITableSelectControl_T_,System.Threading.Tasks.Task_).items 'PromptPlusLibrary\.ITableSelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ITableSelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.items'), invoking the asynchronous [interactionAction](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITableSelectControl_T_,System.Threading.Tasks.Task_).interactionAction 'PromptPlusLibrary\.ITableSelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ITableSelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.interactionAction')
for each element \(awaited synchronously\) to allow programmatic configuration of the control\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> InteractionAsync<T1>(System.Collections.Generic.IEnumerable<T1> items, System.Func<T1,PromptPlusLibrary.ITableSelectControl<T>,System.Threading.Tasks.Task> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITableSelectControl_T_,System.Threading.Tasks.Task_).T1'></a>

`T1`

Type of elements in the input sequence\.
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITableSelectControl_T_,System.Threading.Tasks.Task_).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T1](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITableSelectControl_T_,System.Threading.Tasks.Task_).T1 'PromptPlusLibrary\.ITableSelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ITableSelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.T1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The input sequence to iterate\. Cannot be [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITableSelectControl_T_,System.Threading.Tasks.Task_).interactionAction'></a>

`interactionAction` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T1](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITableSelectControl_T_,System.Threading.Tasks.Task_).T1 'PromptPlusLibrary\.ITableSelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ITableSelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.T1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

The async function invoked for each element, receiving the element and the current
[ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance\. Cannot be [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [items](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITableSelectControl_T_,System.Threading.Tasks.Task_).items 'PromptPlusLibrary\.ITableSelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ITableSelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.items') or [interactionAction](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITableSelectControl_T_,System.Threading.Tasks.Task_).interactionAction 'PromptPlusLibrary\.ITableSelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ITableSelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.interactionAction') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.LayoutMode(PromptPlusLibrary.TableLayoutMode)'></a>

## ITableSelectControl\<T\>\.LayoutMode\(TableLayoutMode\) Method

Sets the table layout mode that controls the box\-drawing characters used for borders\.
Default is [SingleBox](TableLayoutMode.md#PromptPlusLibrary.TableLayoutMode.SingleBox 'PromptPlusLibrary\.TableLayoutMode\.SingleBox')\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> LayoutMode(PromptPlusLibrary.TableLayoutMode mode);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.LayoutMode(PromptPlusLibrary.TableLayoutMode).mode'></a>

`mode` [TableLayoutMode](TableLayoutMode.md 'PromptPlusLibrary\.TableLayoutMode')

The desired [TableLayoutMode](TableLayoutMode.md 'PromptPlusLibrary\.TableLayoutMode')\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## ITableSelectControl\<T\>\.Options\(Action\<IControlOptions\>\) Method

Applies global control options via the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') fluent API\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action that configures the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') instance\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [options](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.PageSize(byte)'></a>

## ITableSelectControl\<T\>\.PageSize\(byte\) Method

Sets the maximum number of rows per page\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> PageSize(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.PageSize(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

Maximum rows per page\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.PredicateSelected(System.Func_T,bool_)'></a>

## ITableSelectControl\<T\>\.PredicateSelected\(Func\<T,bool\>\) Method

Sets a synchronous predicate that determines whether the currently highlighted row
can be confirmed\. Replaces any previously registered asynchronous predicate\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> PredicateSelected(System.Func<T,bool> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.PredicateSelected(System.Func_T,bool_).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A callback that receives the row value and returns [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when the row
is a valid selection; otherwise [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [validselect](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.PredicateSelected(System.Func_T,bool_).validselect 'PromptPlusLibrary\.ITableSelectControl\<T\>\.PredicateSelected\(System\.Func\<T,bool\>\)\.validselect') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__)'></a>

## ITableSelectControl\<T\>\.PredicateSelectedAsync\(Func\<T,Task\<bool\>\>\) Method

Sets an asynchronous predicate that determines whether the currently highlighted row
can be confirmed\. Replaces any previously registered synchronous predicate\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> PredicateSelectedAsync(System.Func<T,System.Threading.Tasks.Task<bool>> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A callback that receives the row value and returns a [System\.Threading\.Tasks\.Task&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1') of
[bool](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/bool') — [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when the row is a valid selection\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [validselect](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__).validselect 'PromptPlusLibrary\.ITableSelectControl\<T\>\.PredicateSelectedAsync\(System\.Func\<T,System\.Threading\.Tasks\.Task\<bool\>\>\)\.validselect') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.Run(System.Threading.CancellationToken)'></a>

## ITableSelectControl\<T\>\.Run\(CancellationToken\) Method

Runs the table control, blocking until the user confirms or cancels\.

```csharp
PromptPlusLibrary.ResultPrompt<PromptPlusLibrary.TableSelectResult<T>> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Cancellation token\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[PromptPlusLibrary\.TableSelectResult&lt;](TableSelectResult_T_.md 'PromptPlusLibrary\.TableSelectResult\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](TableSelectResult_T_.md 'PromptPlusLibrary\.TableSelectResult\<T\>')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') wrapping a [TableSelectResult&lt;T&gt;](TableSelectResult_T_.md 'PromptPlusLibrary\.TableSelectResult\<T\>') that contains
the confirmed row value and its table coordinates\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.Styles(PromptPlusLibrary.TableSelectStyles,ConsolePlusLibrary.Style)'></a>

## ITableSelectControl\<T\>\.Styles\(TableSelectStyles, Style\) Method

Overrides a specific visual style used by the table control\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> Styles(PromptPlusLibrary.TableSelectStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.Styles(PromptPlusLibrary.TableSelectStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [TableSelectStyles](TableSelectStyles.md 'PromptPlusLibrary\.TableSelectStyles')

The [TableSelectStyles](TableSelectStyles.md 'PromptPlusLibrary\.TableSelectStyles') element whose style is overridden\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.Styles(PromptPlusLibrary.TableSelectStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.TextSelector(System.Func_T,string_)'></a>

## ITableSelectControl\<T\>\.TextSelector\(Func\<T,string\>\) Method

Registers a synchronous callback that converts a row value to the answer text displayed
after the control completes\. Replaces any previously registered asynchronous callback\.
When neither [TextSelector\(Func&lt;T,string&gt;\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.TextSelector(System.Func_T,string_) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.TextSelector\(System\.Func\<T,string\>\)') nor [TextSelectorAsync\(Func&lt;T,Task&lt;string&gt;&gt;\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.TextSelectorAsync(System.Func_T,System.Threading.Tasks.Task_string__) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.TextSelectorAsync\(System\.Func\<T,System\.Threading\.Tasks\.Task\<string\>\>\)') is set,
the answer text falls back to `value.ToString()`\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> TextSelector(System.Func<T,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.TextSelector(System.Func_T,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the confirmed row value and returns its answer string\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.TextSelectorAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## ITableSelectControl\<T\>\.TextSelectorAsync\(Func\<T,Task\<string\>\>\) Method

Registers an asynchronous callback that converts a row value to the answer text displayed
after the control completes\. Replaces any previously registered synchronous callback\.
When neither [TextSelectorAsync\(Func&lt;T,Task&lt;string&gt;&gt;\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.TextSelectorAsync(System.Func_T,System.Threading.Tasks.Task_string__) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.TextSelectorAsync\(System\.Func\<T,System\.Threading\.Tasks\.Task\<string\>\>\)') nor [TextSelector\(Func&lt;T,string&gt;\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.TextSelector(System.Func_T,string_) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.TextSelector\(System\.Func\<T,string\>\)') is set,
the answer text falls back to `value.ToString()`\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> TextSelectorAsync(System.Func<T,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.TextSelectorAsync(System.Func_T,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the confirmed row value and returns a [System\.Threading\.Tasks\.Task&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')
of [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String') with the answer text\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.UseDefaultHistory()'></a>

## ITableSelectControl\<T\>\.UseDefaultHistory\(\) Method

Sets the most recent history entry as the initial cursor position, clearing any
value previously set by [Default\(T, bool\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Default(T,bool) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Default\(T, bool\)')\.
Has no effect when history is not enabled via [EnableHistory\(string, Action&lt;IHistoryOptions&gt;\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.EnableHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)')\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> UseDefaultHistory();
```

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ITableSelectControl_T_.ViewOnly(bool)'></a>

## ITableSelectControl\<T\>\.ViewOnly\(bool\) Method

Enables view\-only mode: the user can navigate the table freely but cannot change the selection\.
When confirmed \(Enter\), the control always returns the item that was initially highlighted
at startup \(set via [Default\(T, bool\)](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.Default(T,bool) 'PromptPlusLibrary\.ITableSelectControl\<T\>\.Default\(T, bool\)') or the first row\), regardless of where the user browsed\.
In this mode, selection predicates and disabled\-row restrictions are not enforced\.
Default is [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') \(normal selection mode\)\.

```csharp
PromptPlusLibrary.ITableSelectControl<T> ViewOnly(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.ITableSelectControl_T_.ViewOnly(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') to enable view\-only mode; [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') to restore normal selection\.
            Default is [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

#### Returns
[PromptPlusLibrary\.ITableSelectControl&lt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')[T](ITableSelectControl_T_.md#PromptPlusLibrary.ITableSelectControl_T_.T 'PromptPlusLibrary\.ITableSelectControl\<T\>\.T')[&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>')  
The current [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') instance for chaining\.