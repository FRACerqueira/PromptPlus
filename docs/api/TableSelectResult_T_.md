<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## TableSelectResult\<T\> Struct

Represents the result returned by the [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') control\.

```csharp
public readonly struct TableSelectResult<T>
```
#### Type parameters

<a name='PromptPlusLibrary.TableSelectResult_T_.T'></a>

`T`

Type of selected row value\.
### Constructors

<a name='PromptPlusLibrary.TableSelectResult_T_.TableSelectResult(T,int,int)'></a>

## TableSelectResult\(T, int, int\) Constructor

Represents the result returned by the [ITableSelectControl&lt;T&gt;](ITableSelectControl_T_.md 'PromptPlusLibrary\.ITableSelectControl\<T\>') control\.

```csharp
public TableSelectResult(T value, int rowIndex, int columnIndex);
```
#### Parameters

<a name='PromptPlusLibrary.TableSelectResult_T_.TableSelectResult(T,int,int).value'></a>

`value` [T](TableSelectResult_T_.md#PromptPlusLibrary.TableSelectResult_T_.T 'PromptPlusLibrary\.TableSelectResult\<T\>\.T')

The selected row value\.

<a name='PromptPlusLibrary.TableSelectResult_T_.TableSelectResult(T,int,int).rowIndex'></a>

`rowIndex` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The selected row index\.

<a name='PromptPlusLibrary.TableSelectResult_T_.TableSelectResult(T,int,int).columnIndex'></a>

`columnIndex` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The selected column index\.
### Properties

<a name='PromptPlusLibrary.TableSelectResult_T_.ColumnIndex'></a>

## TableSelectResult\<T\>\.ColumnIndex Property

Selected column index\.

```csharp
public int ColumnIndex { get; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='PromptPlusLibrary.TableSelectResult_T_.RowIndex'></a>

## TableSelectResult\<T\>\.RowIndex Property

Selected row index\.

```csharp
public int RowIndex { get; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='PromptPlusLibrary.TableSelectResult_T_.Value'></a>

## TableSelectResult\<T\>\.Value Property

Selected row value\.

```csharp
public T Value { get; }
```

#### Property Value
[T](TableSelectResult_T_.md#PromptPlusLibrary.TableSelectResult_T_.T 'PromptPlusLibrary\.TableSelectResult\<T\>\.T')
### Methods

<a name='PromptPlusLibrary.TableSelectResult_T_.Deconstruct(T,int,int)'></a>

## TableSelectResult\<T\>\.Deconstruct\(T, int, int\) Method

Deconstructs the [TableSelectResult&lt;T&gt;](TableSelectResult_T_.md 'PromptPlusLibrary\.TableSelectResult\<T\>') into components\.

```csharp
public void Deconstruct(out T valueResult, out int row, out int column);
```
#### Parameters

<a name='PromptPlusLibrary.TableSelectResult_T_.Deconstruct(T,int,int).valueResult'></a>

`valueResult` [T](TableSelectResult_T_.md#PromptPlusLibrary.TableSelectResult_T_.T 'PromptPlusLibrary\.TableSelectResult\<T\>\.T')

Selected row value\.

<a name='PromptPlusLibrary.TableSelectResult_T_.Deconstruct(T,int,int).row'></a>

`row` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Selected row index\.

<a name='PromptPlusLibrary.TableSelectResult_T_.Deconstruct(T,int,int).column'></a>

`column` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Selected column index\.