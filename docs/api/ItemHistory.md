<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ItemHistory Struct

Represents the history of an item with a timeout\.

```csharp
public struct ItemHistory
```
### Constructors

<a name='PromptPlusLibrary.ItemHistory.ItemHistory()'></a>

## ItemHistory\(\) Constructor

Initializes a new instance of the [ItemHistory](ItemHistory.md 'PromptPlusLibrary\.ItemHistory') struct with default values\.

```csharp
public ItemHistory();
```

<a name='PromptPlusLibrary.ItemHistory.ItemHistory(string,long)'></a>

## ItemHistory\(string, long\) Constructor

Initializes a new instance of the [ItemHistory](ItemHistory.md 'PromptPlusLibrary\.ItemHistory') struct with the specified history and timeout ticks\.

```csharp
public ItemHistory(string history, long dateTicks);
```
#### Parameters

<a name='PromptPlusLibrary.ItemHistory.ItemHistory(string,long).history'></a>

`history` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The history data to associate with the item\. Cannot be null\.

<a name='PromptPlusLibrary.ItemHistory.ItemHistory(string,long).dateTicks'></a>

`dateTicks` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The timeout ticks for the item\.
### Fields

<a name='PromptPlusLibrary.ItemHistory.Separator'></a>

## ItemHistory\.Separator Field

Separator character used in the string representation\.

```csharp
public const char Separator = '';
```

#### Field Value
[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')
### Properties

<a name='PromptPlusLibrary.ItemHistory.History'></a>

## ItemHistory\.History Property

The history data associated with the item\.

```csharp
public readonly string History { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='PromptPlusLibrary.ItemHistory.TimeOutTicks'></a>

## ItemHistory\.TimeOutTicks Property

Gets the timeout duration, in ticks, for the associated operation\.

```csharp
public readonly long TimeOutTicks { get; }
```

#### Property Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')
### Methods

<a name='PromptPlusLibrary.ItemHistory.CreateItemHistory(string,System.TimeSpan)'></a>

## ItemHistory\.CreateItemHistory\(string, TimeSpan\) Method

Creates a new instance of the ItemHistory class with the specified history and a timeout applied to its
expiration\.

```csharp
public static PromptPlusLibrary.ItemHistory CreateItemHistory(string history, System.TimeSpan timeout);
```
#### Parameters

<a name='PromptPlusLibrary.ItemHistory.CreateItemHistory(string,System.TimeSpan).history'></a>

`history` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The history data to associate with the item\. Cannot be null\.

<a name='PromptPlusLibrary.ItemHistory.CreateItemHistory(string,System.TimeSpan).timeout'></a>

`timeout` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The duration to add to the current time to determine the item's expiration\.

#### Returns
[ItemHistory](ItemHistory.md 'PromptPlusLibrary\.ItemHistory')  
The same [ItemHistory](ItemHistory.md 'PromptPlusLibrary\.ItemHistory') instance for chaining\.

<a name='PromptPlusLibrary.ItemHistory.ToString()'></a>

## ItemHistory\.ToString\(\) Method

Returns the fully qualified type name of this instance\.

```csharp
public override readonly string ToString();
```

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The fully qualified type name\.