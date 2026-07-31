<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IHistoryOptions Interface

Provides a fluent API for configuring persisted history behavior, including filtering, limits, expiration, and paging\.

```csharp
public interface IHistoryOptions
```
### Methods

<a name='PromptPlusLibrary.IHistoryOptions.ExpirationTime(System.TimeSpan)'></a>

## IHistoryOptions\.ExpirationTime\(TimeSpan\) Method

Sets the expiration duration applied to newly added history entries\.
The default value is 365 days\.

```csharp
PromptPlusLibrary.IHistoryOptions ExpirationTime(System.TimeSpan value);
```
#### Parameters

<a name='PromptPlusLibrary.IHistoryOptions.ExpirationTime(System.TimeSpan).value'></a>

`value` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

A positive [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') that defines when entries expire\.

#### Returns
[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')  
The current [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions') instance for chaining\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [value](IHistoryOptions.md#PromptPlusLibrary.IHistoryOptions.ExpirationTime(System.TimeSpan).value 'PromptPlusLibrary\.IHistoryOptions\.ExpirationTime\(System\.TimeSpan\)\.value') is less than one second\.

<a name='PromptPlusLibrary.IHistoryOptions.FilterType(PromptPlusLibrary.FilterMode)'></a>

## IHistoryOptions\.FilterType\(FilterMode\) Method

Sets the filtering strategy for history suggestions\.
Default is [Contains](FilterMode.md#PromptPlusLibrary.FilterMode.Contains 'PromptPlusLibrary\.FilterMode\.Contains'), which matches entries containing the typed prefix\.

```csharp
PromptPlusLibrary.IHistoryOptions FilterType(PromptPlusLibrary.FilterMode value);
```
#### Parameters

<a name='PromptPlusLibrary.IHistoryOptions.FilterType(PromptPlusLibrary.FilterMode).value'></a>

`value` [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode')

The filtering strategy to apply\.

#### Returns
[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')  
The current [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions') instance for chaining\.

<a name='PromptPlusLibrary.IHistoryOptions.MaxItems(byte)'></a>

## IHistoryOptions\.MaxItems\(byte\) Method

Sets the maximum number of entries retained in history\.
The default value is 255\.

```csharp
PromptPlusLibrary.IHistoryOptions MaxItems(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IHistoryOptions.MaxItems(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The maximum number of items\. Must be greater than or equal to 1\.

#### Returns
[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')  
The current [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions') instance for chaining\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [value](IHistoryOptions.md#PromptPlusLibrary.IHistoryOptions.MaxItems(byte).value 'PromptPlusLibrary\.IHistoryOptions\.MaxItems\(byte\)\.value') less than 1\.

<a name='PromptPlusLibrary.IHistoryOptions.MinPrefixLength(byte)'></a>

## IHistoryOptions\.MinPrefixLength\(byte\) Method

Sets the minimum number of typed characters required before history suggestions are shown\.
The default value is 0\.

```csharp
PromptPlusLibrary.IHistoryOptions MinPrefixLength(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IHistoryOptions.MinPrefixLength(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The minimum prefix length\. Must be greater than or equal to 1\.

#### Returns
[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')  
The current [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions') instance for chaining\.

<a name='PromptPlusLibrary.IHistoryOptions.PageSize(byte)'></a>

## IHistoryOptions\.PageSize\(byte\) Method

Sets the number of history entries displayed per page during history navigation\.
The default value is 5\.

```csharp
PromptPlusLibrary.IHistoryOptions PageSize(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IHistoryOptions.PageSize(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The page size\. Must be greater than or equal to 1\.

#### Returns
[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')  
The current [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions') instance for chaining\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [value](IHistoryOptions.md#PromptPlusLibrary.IHistoryOptions.PageSize(byte).value 'PromptPlusLibrary\.IHistoryOptions\.PageSize\(byte\)\.value') less than 1\.