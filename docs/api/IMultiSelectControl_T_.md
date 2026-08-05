<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IMultiSelectControl\<T\> Interface

Provides a fluent API for configuring and running a multi\-selection list control\.

```csharp
public interface IMultiSelectControl<T>
```
#### Type parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.T'></a>

`T`

The type of items shown in the list\.

### Remarks
The control renders a scrollable, optionally grouped list where the user navigates items
with the arrow keys, toggles individual checks with `Space`, and confirms the entire
selection with `Enter`\. Features include inline filtering \([Filter\(FilterMode\)](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Filter(PromptPlusLibrary.FilterMode) 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Filter\(PromptPlusLibrary\.FilterMode\)')\),
optional grouped layout with header separators, history persistence \([EnableHistory\(string, Action&lt;IHistoryOptions&gt;\)](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_) 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.EnableHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)')\),
range constraints \([Range\(int, Nullable&lt;int&gt;\)](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Range(int,System.Nullable_int_) 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Range\(int, System\.Nullable\<int\>\)')\), and view\-only mode \([ViewOnly\(bool\)](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.ViewOnly(bool) 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.ViewOnly\(bool\)')\)\.
Every configuration method returns the same [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance
so calls can be chained \(fluent style\)\. Call [Run\(CancellationToken\)](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Run\(System\.Threading\.CancellationToken\)') last\.
### Methods

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItem(string,T,bool,bool)'></a>

## IMultiSelectControl\<T\>\.AddGroupedItem\(string, T, bool, bool\) Method

Adds an item to a specific group in the list\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> AddGroupedItem(string group, T value, bool ischecked=false, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItem(string,T,bool,bool).group'></a>

`group` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the group\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItem(string,T,bool,bool).value'></a>

`value` [T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')

The item to add\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItem(string,T,bool,bool).ischecked'></a>

`ischecked` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the item is initially checked; otherwise, it is unchecked\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItem(string,T,bool,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the item is disabled and cannot be selected\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [group](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItem(string,T,bool,bool).group 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.AddGroupedItem\(string, T, bool, bool\)\.group') is `null`\.

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItem(string,T,bool,bool).value 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.AddGroupedItem\(string, T, bool, bool\)\.value') is `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItems(string,System.Collections.Generic.IEnumerable_T_,bool,bool)'></a>

## IMultiSelectControl\<T\>\.AddGroupedItems\(string, IEnumerable\<T\>, bool, bool\) Method

Adds a collection of items to a specific group in the list\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> AddGroupedItems(string group, System.Collections.Generic.IEnumerable<T> values, bool ischecked=false, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItems(string,System.Collections.Generic.IEnumerable_T_,bool,bool).group'></a>

`group` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the group\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItems(string,System.Collections.Generic.IEnumerable_T_,bool,bool).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of items to add\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItems(string,System.Collections.Generic.IEnumerable_T_,bool,bool).ischecked'></a>

`ischecked` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the items are initially checked; otherwise, they are unchecked\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItems(string,System.Collections.Generic.IEnumerable_T_,bool,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, all items are disabled and cannot be selected\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [group](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItems(string,System.Collections.Generic.IEnumerable_T_,bool,bool).group 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.AddGroupedItems\(string, System\.Collections\.Generic\.IEnumerable\<T\>, bool, bool\)\.group') is `null`\.

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [values](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItems(string,System.Collections.Generic.IEnumerable_T_,bool,bool).values 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.AddGroupedItems\(string, System\.Collections\.Generic\.IEnumerable\<T\>, bool, bool\)\.values') is `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddItem(T,bool,bool)'></a>

## IMultiSelectControl\<T\>\.AddItem\(T, bool, bool\) Method

Adds an item to the list\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> AddItem(T value, bool ischecked=false, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddItem(T,bool,bool).value'></a>

`value` [T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')

The item to add\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddItem(T,bool,bool).ischecked'></a>

`ischecked` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the item is initially checked; otherwise, it is unchecked\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddItem(T,bool,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the item is disabled and cannot be selected\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.AddItem(T,bool,bool).value 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.AddItem\(T, bool, bool\)\.value') is `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool,bool)'></a>

## IMultiSelectControl\<T\>\.AddItems\(IEnumerable\<T\>, bool, bool\) Method

Adds a collection of items to the list\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> AddItems(System.Collections.Generic.IEnumerable<T> values, bool ischecked=false, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool,bool).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of items to add\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool,bool).ischecked'></a>

`ischecked` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the item is initially checked; otherwise, it is unchecked\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, all items are disabled and cannot be selected\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [values](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.AddItems(System.Collections.Generic.IEnumerable_T_,bool,bool).values 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.AddItems\(System\.Collections\.Generic\.IEnumerable\<T\>, bool, bool\)\.values') is `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddSeparator(PromptPlusLibrary.SeparatorLine,System.Nullable_char_)'></a>

## IMultiSelectControl\<T\>\.AddSeparator\(SeparatorLine, Nullable\<char\>\) Method

Adds a visual separator line to the list\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> AddSeparator(PromptPlusLibrary.SeparatorLine separatorLine=PromptPlusLibrary.SeparatorLine.SingleLine, System.Nullable<char> value=null);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddSeparator(PromptPlusLibrary.SeparatorLine,System.Nullable_char_).separatorLine'></a>

`separatorLine` [SeparatorLine](SeparatorLine.md 'PromptPlusLibrary\.SeparatorLine')

The type of separator line\. Default is [SingleLine](SeparatorLine.md#PromptPlusLibrary.SeparatorLine.SingleLine 'PromptPlusLibrary\.SeparatorLine\.SingleLine')\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.AddSeparator(PromptPlusLibrary.SeparatorLine,System.Nullable_char_).value'></a>

`value` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The character to use for the separator line\. Only used when [separatorLine](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.AddSeparator(PromptPlusLibrary.SeparatorLine,System.Nullable_char_).separatorLine 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.AddSeparator\(PromptPlusLibrary\.SeparatorLine, System\.Nullable\<char\>\)\.separatorLine') is [UserChar](SeparatorLine.md#PromptPlusLibrary.SeparatorLine.UserChar 'PromptPlusLibrary\.SeparatorLine\.UserChar')\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.ChangeDescription(System.Func_T,string_)'></a>

## IMultiSelectControl\<T\>\.ChangeDescription\(Func\<T,string\>\) Method

Dynamically updates the control description based on the currently selected item\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> ChangeDescription(System.Func<T,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.ChangeDescription(System.Func_T,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current item and returns the description\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.ChangeDescription(System.Func_T,string_).value 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.ChangeDescription\(System\.Func\<T,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## IMultiSelectControl\<T\>\.ChangeDescriptionAsync\(Func\<T,Task\<string\>\>\) Method

Dynamically updates the control description based on the currently selected item using an asynchronous callback\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> ChangeDescriptionAsync(System.Func<T,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that asynchronously receives the current item and returns the description\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.ChangeDescriptionAsync\(System\.Func\<T,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Default(System.Collections.Generic.IEnumerable_T_,bool)'></a>

## IMultiSelectControl\<T\>\.Default\(IEnumerable\<T\>, bool\) Method

Sets the initial selected item and checked items for the MultiSelect control\.
The selected item is the first item in the collection that matches any provided value, and checked items are those that match the provided values\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> Default(System.Collections.Generic.IEnumerable<T> values, bool useDefaultHistory=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Default(System.Collections.Generic.IEnumerable_T_,bool).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The initial values\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Default(System.Collections.Generic.IEnumerable_T_,bool).useDefaultHistory'></a>

`useDefaultHistory` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, uses values from history \(selected item and checked items\) when history is enabled; otherwise, uses [values](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Default(System.Collections.Generic.IEnumerable_T_,bool).values 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Default\(System\.Collections\.Generic\.IEnumerable\<T\>, bool\)\.values')\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [values](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Default(System.Collections.Generic.IEnumerable_T_,bool).values 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Default\(System\.Collections\.Generic\.IEnumerable\<T\>, bool\)\.values') is `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.DefaultMatchBy(System.Func_T,T,bool_)'></a>

## IMultiSelectControl\<T\>\.DefaultMatchBy\(Func\<T,T,bool\>\) Method

Sets a custom item comparator for determining item equality\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> DefaultMatchBy(System.Func<T,T,bool> comparer);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.DefaultMatchBy(System.Func_T,T,bool_).comparer'></a>

`comparer` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

A function that compares two items and returns `true` if they are equal\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [comparer](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.DefaultMatchBy(System.Func_T,T,bool_).comparer 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.DefaultMatchBy\(System\.Func\<T,T,bool\>\)\.comparer') is `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_)'></a>

## IMultiSelectControl\<T\>\.EnableHistory\(string, Action\<IHistoryOptions\>\) Method

Enables history and applies custom options to the history feature\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> EnableHistory(string filename, System.Action<PromptPlusLibrary.IHistoryOptions>? options=null);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename'></a>

`filename` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the file to store history\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action to configure [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')\. Optional\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [filename](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.EnableHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)\.filename') is `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.ExtraInfo(System.Func_T,string_)'></a>

## IMultiSelectControl\<T\>\.ExtraInfo\(Func\<T,string\>\) Method

Registers a callback that returns an additional informational line rendered below the
highlighted item\. Useful for displaying metadata without cluttering the list\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> ExtraInfo(System.Func<T,string?> extraInfoNode);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.ExtraInfo(System.Func_T,string_).extraInfoNode'></a>

`extraInfoNode` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the focused item and returns the extra text, or `null` to show nothing\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.ExtraInfoAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## IMultiSelectControl\<T\>\.ExtraInfoAsync\(Func\<T,Task\<string\>\>\) Method

Asynchronous counterpart of [ExtraInfo\(Func&lt;T,string&gt;\)](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.ExtraInfo(System.Func_T,string_) 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.ExtraInfo\(System\.Func\<T,string\>\)')\. The task is awaited
synchronously \(blocking\) on the UI thread each time the cursor moves\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> ExtraInfoAsync(System.Func<T,System.Threading.Tasks.Task<string?>> extraInfoNode);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.ExtraInfoAsync(System.Func_T,System.Threading.Tasks.Task_string__).extraInfoNode'></a>

`extraInfoNode` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An async function that receives the focused item and returns the extra text\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Filter(PromptPlusLibrary.FilterMode)'></a>

## IMultiSelectControl\<T\>\.Filter\(FilterMode\) Method

Sets the filter strategy for filtering items in the collection\. Default is [Disabled](FilterMode.md#PromptPlusLibrary.FilterMode.Disabled 'PromptPlusLibrary\.FilterMode\.Disabled')\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> Filter(PromptPlusLibrary.FilterMode value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Filter(PromptPlusLibrary.FilterMode).value'></a>

`value` [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode')

The [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode') to apply\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.HideTipGroup(bool)'></a>

## IMultiSelectControl\<T\>\.HideTipGroup\(bool\) Method

Hides the group name tip\. Default is `false`\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> HideTipGroup(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.HideTipGroup(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, hides the group name tip; otherwise, shows it\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiSelectControl_T__)'></a>

## IMultiSelectControl\<T\>\.Interaction\<T1\>\(IEnumerable\<T1\>, Action\<T1,IMultiSelectControl\<T\>\>\) Method

Iterates [items](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiSelectControl_T__).items 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiSelectControl\<T\>\>\)\.items') and invokes [interactionAction](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiSelectControl_T__).interactionAction 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiSelectControl\<T\>\>\)\.interactionAction') for each element,
giving the caller a chance to call [AddItem\(T, bool, bool\)](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.AddItem(T,bool,bool) 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.AddItem\(T, bool, bool\)') or [AddGroupedItem\(string, T, bool, bool\)](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.AddGroupedItem(string,T,bool,bool) 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.AddGroupedItem\(string, T, bool, bool\)') programmatically\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> Interaction<T1>(System.Collections.Generic.IEnumerable<T1> items, System.Action<T1,PromptPlusLibrary.IMultiSelectControl<T>> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiSelectControl_T__).T1'></a>

`T1`

The type of elements in the input sequence\.
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiSelectControl_T__).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T1](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiSelectControl_T__).T1 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiSelectControl\<T\>\>\)\.T1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The input sequence to iterate\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiSelectControl_T__).interactionAction'></a>

`interactionAction` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[T1](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiSelectControl_T__).T1 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiSelectControl\<T\>\>\)\.T1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')

The action invoked for each element\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [interactionAction](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiSelectControl_T__).interactionAction 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiSelectControl\<T\>\>\)\.interactionAction') is `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiSelectControl_T_,System.Threading.Tasks.Task_)'></a>

## IMultiSelectControl\<T\>\.InteractionAsync\<T1\>\(IEnumerable\<T1\>, Func\<T1,IMultiSelectControl\<T\>,Task\>\) Method

Asynchronous counterpart of [Interaction&lt;T1&gt;\(IEnumerable&lt;T1&gt;, Action&lt;T1,IMultiSelectControl&lt;T&gt;&gt;\)](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiSelectControl_T__) 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiSelectControl\<T\>\>\)')\. The task returned by
[interactionAction](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiSelectControl_T_,System.Threading.Tasks.Task_).interactionAction 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.IMultiSelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.interactionAction') is awaited synchronously \(blocking\)\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> InteractionAsync<T1>(System.Collections.Generic.IEnumerable<T1> items, System.Func<T1,PromptPlusLibrary.IMultiSelectControl<T>,System.Threading.Tasks.Task> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiSelectControl_T_,System.Threading.Tasks.Task_).T1'></a>

`T1`

The type of elements in the input sequence\.
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiSelectControl_T_,System.Threading.Tasks.Task_).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T1](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiSelectControl_T_,System.Threading.Tasks.Task_).T1 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.IMultiSelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.T1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The input sequence to iterate\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiSelectControl_T_,System.Threading.Tasks.Task_).interactionAction'></a>

`interactionAction` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T1](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiSelectControl_T_,System.Threading.Tasks.Task_).T1 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.IMultiSelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.T1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

The asynchronous action invoked for each element\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [interactionAction](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiSelectControl_T_,System.Threading.Tasks.Task_).interactionAction 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.IMultiSelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.interactionAction') is `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IMultiSelectControl\<T\>\.Options\(Action\<IControlOptions\>\) Method

Applies custom options to the control\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action to configure [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [options](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.PageSize(byte)'></a>

## IMultiSelectControl\<T\>\.PageSize\(byte\) Method

Sets the maximum number of items to display per page\. Default value is 0\.
Valid range is 0\-255\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> PageSize(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.PageSize(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The maximum number of items per page\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

### Remarks
A value of 0 automatically calculates the page size based on screen height, reserving lines for header, footer, and pagination\.
If the provided value is greater than the available screen height \(minus reserved lines\), it is coerced to the maximum allowed value\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.PredicateChecked(System.Func_T,bool_)'></a>

## IMultiSelectControl\<T\>\.PredicateChecked\(Func\<T,bool\>\) Method

Sets a synchronous validation predicate executed when the user attempts to check an item\.
Returns `false` to reject the check and show a generic error\. Never evaluated when
unchecking an item — unchecking is always allowed for non\-disabled items\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> PredicateChecked(System.Func<T,bool> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.PredicateChecked(System.Func_T,bool_).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A predicate that returns `true` when the item can be checked\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.PredicateCheckedAsync(System.Func_T,System.Threading.Tasks.Task_bool__)'></a>

## IMultiSelectControl\<T\>\.PredicateCheckedAsync\(Func\<T,Task\<bool\>\>\) Method

Asynchronous counterpart of [PredicateChecked\(Func&lt;T,bool&gt;\)](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.PredicateChecked(System.Func_T,bool_) 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.PredicateChecked\(System\.Func\<T,bool\>\)')\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> PredicateCheckedAsync(System.Func<T,System.Threading.Tasks.Task<bool>> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.PredicateCheckedAsync(System.Func_T,System.Threading.Tasks.Task_bool__).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An asynchronous predicate that returns `true` when the item can be checked\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

### Remarks
The asynchronous predicate is evaluated synchronously \(blocking\) on the UI thread; it does not run in parallel\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Range(int,System.Nullable_int_)'></a>

## IMultiSelectControl\<T\>\.Range\(int, Nullable\<int\>\) Method

Defines the valid range for the number of selected items\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> Range(int minvalue, System.Nullable<int> maxvalue=null);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Range(int,System.Nullable_int_).minvalue'></a>

`minvalue` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The minimum number of items that must be selected\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Range(int,System.Nullable_int_).maxvalue'></a>

`maxvalue` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional maximum number of items that can be selected\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for method chaining\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [minvalue](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Range(int,System.Nullable_int_).minvalue 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Range\(int, System\.Nullable\<int\>\)\.minvalue') is less than 0 or when [maxvalue](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Range(int,System.Nullable_int_).maxvalue 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Range\(int, System\.Nullable\<int\>\)\.maxvalue') is specified and is less than [minvalue](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Range(int,System.Nullable_int_).minvalue 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Range\(int, System\.Nullable\<int\>\)\.minvalue')\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Run(System.Threading.CancellationToken)'></a>

## IMultiSelectControl\<T\>\.Run\(CancellationToken\) Method

Displays the multi\-select list and blocks until the user confirms or cancels,
returning the array of checked items\.

```csharp
PromptPlusLibrary.ResultPrompt<T[]> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the operation\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') containing the checked items as a `T[]`, or an empty array when cancelled\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Styles(PromptPlusLibrary.MultiSelectStyles,ConsolePlusLibrary.Style)'></a>

## IMultiSelectControl\<T\>\.Styles\(MultiSelectStyles, Style\) Method

Overrides styles for the MultiSelect control\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> Styles(PromptPlusLibrary.MultiSelectStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Styles(PromptPlusLibrary.MultiSelectStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [MultiSelectStyles](MultiSelectStyles.md 'PromptPlusLibrary\.MultiSelectStyles')

The [MultiSelectStyles](MultiSelectStyles.md 'PromptPlusLibrary\.MultiSelectStyles') to apply\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.Styles(PromptPlusLibrary.MultiSelectStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to use\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.TextSelector(System.Func_T,string_)'></a>

## IMultiSelectControl\<T\>\.TextSelector\(Func\<T,string\>\) Method

Sets the function used to display item text in the list\. By default, `ToString()` is used\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> TextSelector(System.Func<T,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.TextSelector(System.Func_T,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that returns the display text for each item\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.TextSelector(System.Func_T,string_).value 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.TextSelector\(System\.Func\<T,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.TextSelectorAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## IMultiSelectControl\<T\>\.TextSelectorAsync\(Func\<T,Task\<string\>\>\) Method

Sets an asynchronous function used to display item text in the list\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> TextSelectorAsync(System.Func<T,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.TextSelectorAsync(System.Func_T,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that asynchronously returns the display text for each item\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.TextSelectorAsync(System.Func_T,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.TextSelectorAsync\(System\.Func\<T,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.UseDefaultHistory()'></a>

## IMultiSelectControl\<T\>\.UseDefaultHistory\(\) Method

Instructs the control to initialize its checked set from the most recent history entry,
overriding any values supplied by [Default\(IEnumerable&lt;T&gt;, bool\)](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.Default(System.Collections.Generic.IEnumerable_T_,bool) 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.Default\(System\.Collections\.Generic\.IEnumerable\<T\>, bool\)')\.
Has no effect when history is not enabled via [EnableHistory\(string, Action&lt;IHistoryOptions&gt;\)](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_) 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.EnableHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)')\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> UseDefaultHistory();
```

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiSelectControl_T_.ViewOnly(bool)'></a>

## IMultiSelectControl\<T\>\.ViewOnly\(bool\) Method

Configures the control to view\-only mode, where items can be viewed but not selected\. Default is `false`\.

```csharp
PromptPlusLibrary.IMultiSelectControl<T> ViewOnly(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiSelectControl_T_.ViewOnly(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, enables view\-only mode; otherwise, item selection is enabled\.

#### Returns
[PromptPlusLibrary\.IMultiSelectControl&lt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')[T](IMultiSelectControl_T_.md#PromptPlusLibrary.IMultiSelectControl_T_.T 'PromptPlusLibrary\.IMultiSelectControl\<T\>\.T')[&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>')  
The current [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') instance for chaining\.