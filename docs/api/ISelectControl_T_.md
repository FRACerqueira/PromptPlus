<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ISelectControl\<T\> Interface

Provides a fluent API for configuring and running a single\-selection list control\.

```csharp
public interface ISelectControl<T>
```
#### Type parameters

<a name='PromptPlusLibrary.ISelectControl_T_.T'></a>

`T`

The type of items shown in the list\.

### Remarks
The control renders a scrollable, optionally grouped list where the user moves the cursor
with the arrow keys and confirms with `Enter`\. Features include inline filtering
\([Filter\(FilterMode\)](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.Filter(PromptPlusLibrary.FilterMode) 'PromptPlusLibrary\.ISelectControl\<T\>\.Filter\(PromptPlusLibrary\.FilterMode\)')\), optional grouped layout, history persistence \([EnabledHistory\(string, Action&lt;IHistoryOptions&gt;\)](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_) 'PromptPlusLibrary\.ISelectControl\<T\>\.EnabledHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)')\),
auto\-select when only one item matches the filter \([AutoSelect\(bool\)](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.AutoSelect(bool) 'PromptPlusLibrary\.ISelectControl\<T\>\.AutoSelect\(bool\)')\), and view\-only
mode \([ViewOnly\(bool\)](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.ViewOnly(bool) 'PromptPlusLibrary\.ISelectControl\<T\>\.ViewOnly\(bool\)')\)\. Every configuration method returns the same
[ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance so calls can be chained \(fluent style\)\.
Call [Run\(CancellationToken\)](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.ISelectControl\<T\>\.Run\(System\.Threading\.CancellationToken\)') last\.
### Methods

<a name='PromptPlusLibrary.ISelectControl_T_.AddGroupedItem(string,T,bool)'></a>

## ISelectControl\<T\>\.AddGroupedItem\(string, T, bool\) Method

Adds an item to a specific group in the list\.

```csharp
PromptPlusLibrary.ISelectControl<T> AddGroupedItem(string group, T value, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.AddGroupedItem(string,T,bool).group'></a>

`group` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the group\. Cannot be `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.AddGroupedItem(string,T,bool).value'></a>

`value` [T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')

The item to add\. Cannot be `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.AddGroupedItem(string,T,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the item is disabled and cannot be selected\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [group](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.AddGroupedItem(string,T,bool).group 'PromptPlusLibrary\.ISelectControl\<T\>\.AddGroupedItem\(string, T, bool\)\.group') is `null`\.

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.AddGroupedItem(string,T,bool).value 'PromptPlusLibrary\.ISelectControl\<T\>\.AddGroupedItem\(string, T, bool\)\.value') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.AddGroupedItems(string,System.Collections.Generic.IEnumerable_T_,bool)'></a>

## ISelectControl\<T\>\.AddGroupedItems\(string, IEnumerable\<T\>, bool\) Method

Adds a collection of items to a specific group in the list\.

```csharp
PromptPlusLibrary.ISelectControl<T> AddGroupedItems(string group, System.Collections.Generic.IEnumerable<T> values, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.AddGroupedItems(string,System.Collections.Generic.IEnumerable_T_,bool).group'></a>

`group` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the group\. Cannot be `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.AddGroupedItems(string,System.Collections.Generic.IEnumerable_T_,bool).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of items to add\. Cannot be `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.AddGroupedItems(string,System.Collections.Generic.IEnumerable_T_,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, all items are disabled and cannot be selected\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [group](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.AddGroupedItems(string,System.Collections.Generic.IEnumerable_T_,bool).group 'PromptPlusLibrary\.ISelectControl\<T\>\.AddGroupedItems\(string, System\.Collections\.Generic\.IEnumerable\<T\>, bool\)\.group') is `null`\.

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [values](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.AddGroupedItems(string,System.Collections.Generic.IEnumerable_T_,bool).values 'PromptPlusLibrary\.ISelectControl\<T\>\.AddGroupedItems\(string, System\.Collections\.Generic\.IEnumerable\<T\>, bool\)\.values') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.AddItem(T,bool)'></a>

## ISelectControl\<T\>\.AddItem\(T, bool\) Method

Adds a single item to the list\.

```csharp
PromptPlusLibrary.ISelectControl<T> AddItem(T value, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.AddItem(T,bool).value'></a>

`value` [T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')

The item to add\. Cannot be `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.AddItem(T,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the item is disabled and cannot be selected\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.AddItem(T,bool).value 'PromptPlusLibrary\.ISelectControl\<T\>\.AddItem\(T, bool\)\.value') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool)'></a>

## ISelectControl\<T\>\.AddItems\(IEnumerable\<T\>, bool\) Method

Adds multiple items to the list\.

```csharp
PromptPlusLibrary.ISelectControl<T> AddItems(System.Collections.Generic.IEnumerable<T> values, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of items to add\. Cannot be `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, all items are disabled and cannot be selected\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [values](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool).values 'PromptPlusLibrary\.ISelectControl\<T\>\.AddItems\(System\.Collections\.Generic\.IEnumerable\<T\>, bool\)\.values') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.AddSeparator(PromptPlusLibrary.SeparatorLine,System.Nullable_char_)'></a>

## ISelectControl\<T\>\.AddSeparator\(SeparatorLine, Nullable\<char\>\) Method

Adds a visual separator line to the list\.

```csharp
PromptPlusLibrary.ISelectControl<T> AddSeparator(PromptPlusLibrary.SeparatorLine separatorLine=PromptPlusLibrary.SeparatorLine.SingleLine, System.Nullable<char> value=null);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.AddSeparator(PromptPlusLibrary.SeparatorLine,System.Nullable_char_).separatorLine'></a>

`separatorLine` [SeparatorLine](SeparatorLine.md 'PromptPlusLibrary\.SeparatorLine')

The type of separator line\. Default is [SingleLine](SeparatorLine.md#PromptPlusLibrary.SeparatorLine.SingleLine 'PromptPlusLibrary\.SeparatorLine\.SingleLine')\.

<a name='PromptPlusLibrary.ISelectControl_T_.AddSeparator(PromptPlusLibrary.SeparatorLine,System.Nullable_char_).value'></a>

`value` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The character to use for the separator line\. Only used when [separatorLine](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.AddSeparator(PromptPlusLibrary.SeparatorLine,System.Nullable_char_).separatorLine 'PromptPlusLibrary\.ISelectControl\<T\>\.AddSeparator\(PromptPlusLibrary\.SeparatorLine, System\.Nullable\<char\>\)\.separatorLine') is [UserChar](SeparatorLine.md#PromptPlusLibrary.SeparatorLine.UserChar 'PromptPlusLibrary\.SeparatorLine\.UserChar')\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ISelectControl_T_.AutoSelect(bool)'></a>

## ISelectControl\<T\>\.AutoSelect\(bool\) Method

Automatically selects and confirms the item when filtering leaves a single selectable result\.

```csharp
PromptPlusLibrary.ISelectControl<T> AutoSelect(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.AutoSelect(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, enables auto\-selection; otherwise, disables it\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ISelectControl_T_.ChangeDescription(System.Func_T,string_)'></a>

## ISelectControl\<T\>\.ChangeDescription\(Func\<T,string\>\) Method

Dynamically updates the prompt description based on the currently selected item\.

```csharp
PromptPlusLibrary.ISelectControl<T> ChangeDescription(System.Func<T,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.ChangeDescription(System.Func_T,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current item and returns the description\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.ChangeDescription(System.Func_T,string_).value 'PromptPlusLibrary\.ISelectControl\<T\>\.ChangeDescription\(System\.Func\<T,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## ISelectControl\<T\>\.ChangeDescriptionAsync\(Func\<T,Task\<string\>\>\) Method

Dynamically updates the prompt description based on the currently selected item using an asynchronous callback\.

```csharp
PromptPlusLibrary.ISelectControl<T> ChangeDescriptionAsync(System.Func<T,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that asynchronously receives the current item and returns the description\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.ISelectControl\<T\>\.ChangeDescriptionAsync\(System\.Func\<T,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.Default(T,bool)'></a>

## ISelectControl\<T\>\.Default\(T, bool\) Method

Sets the initial selected item for the select control\.

```csharp
PromptPlusLibrary.ISelectControl<T> Default(T value, bool useDefaultHistory=true);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.Default(T,bool).value'></a>

`value` [T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')

The initial value\. Cannot be `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.Default(T,bool).useDefaultHistory'></a>

`useDefaultHistory` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, uses the value from history when enabled; otherwise, uses [value](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.Default(T,bool).value 'PromptPlusLibrary\.ISelectControl\<T\>\.Default\(T, bool\)\.value')\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.Default(T,bool).value 'PromptPlusLibrary\.ISelectControl\<T\>\.Default\(T, bool\)\.value') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.DefaultMatchBy(System.Func_T,T,bool_)'></a>

## ISelectControl\<T\>\.DefaultMatchBy\(Func\<T,T,bool\>\) Method

Sets a custom item comparator for determining item equality\.

```csharp
PromptPlusLibrary.ISelectControl<T> DefaultMatchBy(System.Func<T,T,bool> comparer);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.DefaultMatchBy(System.Func_T,T,bool_).comparer'></a>

`comparer` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

A function that compares two items and returns `true` if they are equal\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [comparer](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.DefaultMatchBy(System.Func_T,T,bool_).comparer 'PromptPlusLibrary\.ISelectControl\<T\>\.DefaultMatchBy\(System\.Func\<T,T,bool\>\)\.comparer') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_)'></a>

## ISelectControl\<T\>\.EnabledHistory\(string, Action\<IHistoryOptions\>\) Method

Enables history and applies custom configuration to the history feature\.

```csharp
PromptPlusLibrary.ISelectControl<T> EnabledHistory(string filename, System.Action<PromptPlusLibrary.IHistoryOptions>? options=null);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename'></a>

`filename` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the file to store history\. Cannot be `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action to configure [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')\. Optional\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [filename](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename 'PromptPlusLibrary\.ISelectControl\<T\>\.EnabledHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)\.filename') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.ExtraInfo(System.Func_T,string_)'></a>

## ISelectControl\<T\>\.ExtraInfo\(Func\<T,string\>\) Method

Configures the control to display additional information for each item\.

```csharp
PromptPlusLibrary.ISelectControl<T> ExtraInfo(System.Func<T,string?> extraInfoNode);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.ExtraInfo(System.Func_T,string_).extraInfoNode'></a>

`extraInfoNode` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that takes an item of type [T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T') and returns extra information\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ISelectControl_T_.ExtraInfoAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## ISelectControl\<T\>\.ExtraInfoAsync\(Func\<T,Task\<string\>\>\) Method

Configures the control to display additional information for each item asynchronously\.

```csharp
PromptPlusLibrary.ISelectControl<T> ExtraInfoAsync(System.Func<T,System.Threading.Tasks.Task<string?>> extraInfoNode);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.ExtraInfoAsync(System.Func_T,System.Threading.Tasks.Task_string__).extraInfoNode'></a>

`extraInfoNode` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that takes an item of type [T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T') and asynchronously returns extra information\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ISelectControl_T_.Filter(PromptPlusLibrary.FilterMode)'></a>

## ISelectControl\<T\>\.Filter\(FilterMode\) Method

Sets the filtering strategy used for items in the collection\. The default is [Disabled](FilterMode.md#PromptPlusLibrary.FilterMode.Disabled 'PromptPlusLibrary\.FilterMode\.Disabled')\.

```csharp
PromptPlusLibrary.ISelectControl<T> Filter(PromptPlusLibrary.FilterMode value);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.Filter(PromptPlusLibrary.FilterMode).value'></a>

`value` [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode')

The [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode') to apply\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ISelectControl_T_.HideTipGroup(bool)'></a>

## ISelectControl\<T\>\.HideTipGroup\(bool\) Method

Hides the group name hint\. The default is `false`\.

```csharp
PromptPlusLibrary.ISelectControl<T> HideTipGroup(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.HideTipGroup(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, hides the group name tip; otherwise, shows it\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ISelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ISelectControl_T__)'></a>

## ISelectControl\<T\>\.Interaction\<T1\>\(IEnumerable\<T1\>, Action\<T1,ISelectControl\<T\>\>\) Method

Executes a synchronous interaction for each item in the collection\.

```csharp
PromptPlusLibrary.ISelectControl<T> Interaction<T1>(System.Collections.Generic.IEnumerable<T1> items, System.Action<T1,PromptPlusLibrary.ISelectControl<T>> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.ISelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ISelectControl_T__).T1'></a>

`T1`

The type of items in the collection\.
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ISelectControl_T__).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T1](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ISelectControl_T__).T1 'PromptPlusLibrary\.ISelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ISelectControl\<T\>\>\)\.T1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of items to interact with\.

<a name='PromptPlusLibrary.ISelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ISelectControl_T__).interactionAction'></a>

`interactionAction` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[T1](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ISelectControl_T__).T1 'PromptPlusLibrary\.ISelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ISelectControl\<T\>\>\)\.T1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')

The action to perform on each item\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [interactionAction](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ISelectControl_T__).interactionAction 'PromptPlusLibrary\.ISelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ISelectControl\<T\>\>\)\.interactionAction') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ISelectControl_T_,System.Threading.Tasks.Task_)'></a>

## ISelectControl\<T\>\.InteractionAsync\<T1\>\(IEnumerable\<T1\>, Func\<T1,ISelectControl\<T\>,Task\>\) Method

Executes an asynchronous interaction for each item in the collection\.

```csharp
PromptPlusLibrary.ISelectControl<T> InteractionAsync<T1>(System.Collections.Generic.IEnumerable<T1> items, System.Func<T1,PromptPlusLibrary.ISelectControl<T>,System.Threading.Tasks.Task> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.ISelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ISelectControl_T_,System.Threading.Tasks.Task_).T1'></a>

`T1`

The type of items in the collection\.
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ISelectControl_T_,System.Threading.Tasks.Task_).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T1](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ISelectControl_T_,System.Threading.Tasks.Task_).T1 'PromptPlusLibrary\.ISelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ISelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.T1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of items to interact with\.

<a name='PromptPlusLibrary.ISelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ISelectControl_T_,System.Threading.Tasks.Task_).interactionAction'></a>

`interactionAction` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T1](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ISelectControl_T_,System.Threading.Tasks.Task_).T1 'PromptPlusLibrary\.ISelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ISelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.T1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

The asynchronous action to perform on each item\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [interactionAction](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ISelectControl_T_,System.Threading.Tasks.Task_).interactionAction 'PromptPlusLibrary\.ISelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ISelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.interactionAction') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## ISelectControl\<T\>\.Options\(Action\<IControlOptions\>\) Method

Applies custom options to the control\.

```csharp
PromptPlusLibrary.ISelectControl<T> Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action to configure [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [options](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.ISelectControl\<T\>\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.PageSize(byte)'></a>

## ISelectControl\<T\>\.PageSize\(byte\) Method

Sets the maximum number of items displayed per page\. The default value is 0\.
Valid range is 0\-255\.

```csharp
PromptPlusLibrary.ISelectControl<T> PageSize(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.PageSize(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The maximum number of items per page\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

### Remarks
A value of 0 automatically computes the page size based on screen height, reserving lines for header, footer, and pagination\.
If the provided value exceeds the available screen height \(minus reserved lines\), it is coerced to the maximum allowed value\.

<a name='PromptPlusLibrary.ISelectControl_T_.PredicateSelected(System.Func_T,bool_)'></a>

## ISelectControl\<T\>\.PredicateSelected\(Func\<T,bool\>\) Method

Sets a validation predicate that determines whether the selected item is valid\.

```csharp
PromptPlusLibrary.ISelectControl<T> PredicateSelected(System.Func<T,bool> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.PredicateSelected(System.Func_T,bool_).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A predicate that returns `true` when an item is valid and can be selected\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ISelectControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__)'></a>

## ISelectControl\<T\>\.PredicateSelectedAsync\(Func\<T,Task\<bool\>\>\) Method

Sets an asynchronous validation predicate that determines whether the selected item is valid\.

```csharp
PromptPlusLibrary.ISelectControl<T> PredicateSelectedAsync(System.Func<T,System.Threading.Tasks.Task<bool>> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An asynchronous predicate that returns `true` when an item is valid and can be selected\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

### Remarks
The asynchronous predicate is evaluated synchronously \(blocking\) on the UI thread; it does not run in parallel\.

<a name='PromptPlusLibrary.ISelectControl_T_.Run(System.Threading.CancellationToken)'></a>

## ISelectControl\<T\>\.Run\(CancellationToken\) Method

Displays the selection list and blocks until the user confirms or cancels,
returning the highlighted item\.

```csharp
PromptPlusLibrary.ResultPrompt<T> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the operation\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') containing the selected item, or an aborted result if cancelled\.

<a name='PromptPlusLibrary.ISelectControl_T_.Styles(PromptPlusLibrary.SelectStyles,ConsolePlusLibrary.Style)'></a>

## ISelectControl\<T\>\.Styles\(SelectStyles, Style\) Method

Overrides visual styles for the select control\.

```csharp
PromptPlusLibrary.ISelectControl<T> Styles(PromptPlusLibrary.SelectStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.Styles(PromptPlusLibrary.SelectStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [SelectStyles](SelectStyles.md 'PromptPlusLibrary\.SelectStyles')

The [SelectStyles](SelectStyles.md 'PromptPlusLibrary\.SelectStyles') to apply\.

<a name='PromptPlusLibrary.ISelectControl_T_.Styles(PromptPlusLibrary.SelectStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to use\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [style](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.Styles(PromptPlusLibrary.SelectStyles,ConsolePlusLibrary.Style).style 'PromptPlusLibrary\.ISelectControl\<T\>\.Styles\(PromptPlusLibrary\.SelectStyles, ConsolePlusLibrary\.Style\)\.style') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.TextSelector(System.Func_T,string_)'></a>

## ISelectControl\<T\>\.TextSelector\(Func\<T,string\>\) Method

Sets the function used to generate item text in the list\. By default, `ToString()` is used\.

```csharp
PromptPlusLibrary.ISelectControl<T> TextSelector(System.Func<T,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.TextSelector(System.Func_T,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that returns the display text for each item\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.TextSelector(System.Func_T,string_).value 'PromptPlusLibrary\.ISelectControl\<T\>\.TextSelector\(System\.Func\<T,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.TextSelectorAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## ISelectControl\<T\>\.TextSelectorAsync\(Func\<T,Task\<string\>\>\) Method

Sets an asynchronous function used to display item text in the list\.

```csharp
PromptPlusLibrary.ISelectControl<T> TextSelectorAsync(System.Func<T,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.TextSelectorAsync(System.Func_T,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that asynchronously returns the display text for each item\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.TextSelectorAsync(System.Func_T,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.ISelectControl\<T\>\.TextSelectorAsync\(System\.Func\<T,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ISelectControl_T_.UseDefaultHistory()'></a>

## ISelectControl\<T\>\.UseDefaultHistory\(\) Method

Sets the initial selected item from history \(when enabled\)\.

```csharp
PromptPlusLibrary.ISelectControl<T> UseDefaultHistory();
```

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.ISelectControl_T_.ViewOnly(bool)'></a>

## ISelectControl\<T\>\.ViewOnly\(bool\) Method

Configures the control for view\-only mode, where items can be viewed but not selected\. The default is `false`\.

```csharp
PromptPlusLibrary.ISelectControl<T> ViewOnly(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.ISelectControl_T_.ViewOnly(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, enables view\-only mode; otherwise, item selection is enabled\.

#### Returns
[PromptPlusLibrary\.ISelectControl&lt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')[T](ISelectControl_T_.md#PromptPlusLibrary.ISelectControl_T_.T 'PromptPlusLibrary\.ISelectControl\<T\>\.T')[&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>')  
The current [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') instance for chaining\.