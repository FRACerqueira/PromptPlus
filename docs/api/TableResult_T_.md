<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## TableResult\<T\> Struct

Represents the result returned by table controls\.

```csharp
public readonly struct TableResult<T>
```
#### Type parameters

<a name='PromptPlusLibrary.TableResult_T_.T'></a>

`T`

Type of selected row value\.
### Constructors

<a name='PromptPlusLibrary.TableResult_T_.TableResult(T,int,int)'></a>

## TableResult\(T, int, int\) Constructor

Represents the result returned by table controls\.

```csharp
public TableResult(T value, int rowIndex, int columnIndex);
```
#### Parameters

<a name='PromptPlusLibrary.TableResult_T_.TableResult(T,int,int).value'></a>

`value` [T](TableResult_T_.md#PromptPlusLibrary.TableResult_T_.T 'PromptPlusLibrary\.TableResult\<T\>\.T')

The selected row value\.

<a name='PromptPlusLibrary.TableResult_T_.TableResult(T,int,int).rowIndex'></a>

`rowIndex` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The selected row index\.

<a name='PromptPlusLibrary.TableResult_T_.TableResult(T,int,int).columnIndex'></a>

`columnIndex` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The selected column index\.
### Properties

<a name='PromptPlusLibrary.TableResult_T_.ColumnIndex'></a>

## TableResult\<T\>\.ColumnIndex Property

Selected column index\.

```csharp
public int ColumnIndex { get; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='PromptPlusLibrary.TableResult_T_.RowIndex'></a>

## TableResult\<T\>\.RowIndex Property

Selected row index\.

```csharp
public int RowIndex { get; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='PromptPlusLibrary.TableResult_T_.Value'></a>

## TableResult\<T\>\.Value Property

Selected row value\.

```csharp
public T Value { get; }
```

#### Property Value
[T](TableResult_T_.md#PromptPlusLibrary.TableResult_T_.T 'PromptPlusLibrary\.TableResult\<T\>\.T')
### Methods

<a name='PromptPlusLibrary.TableResult_T_.Deconstruct(T,int,int)'></a>

## TableResult\<T\>\.Deconstruct\(T, int, int\) Method

Deconstructs the [TableResult&lt;T&gt;](TableResult_T_.md 'PromptPlusLibrary\.TableResult\<T\>') into components\.

```csharp
public void Deconstruct(out T valueResult, out int row, out int column);
```
#### Parameters

<a name='PromptPlusLibrary.TableResult_T_.Deconstruct(T,int,int).valueResult'></a>

`valueResult` [T](TableResult_T_.md#PromptPlusLibrary.TableResult_T_.T 'PromptPlusLibrary\.TableResult\<T\>\.T')

Selected row value\.

<a name='PromptPlusLibrary.TableResult_T_.Deconstruct(T,int,int).row'></a>

`row` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Selected row index\.

<a name='PromptPlusLibrary.TableResult_T_.Deconstruct(T,int,int).column'></a>

`column` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Selected column index\.