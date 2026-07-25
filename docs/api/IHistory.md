<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IHistory Interface

Provides a fluent API for adding, reading, and managing persisted history entries\.

```csharp
public interface IHistory
```
### Methods

<a name='PromptPlusLibrary.IHistory.AddHistory(string,System.Nullable_System.TimeSpan_)'></a>

## IHistory\.AddHistory\(string, Nullable\<TimeSpan\>\) Method

Adds an entry to history\.

```csharp
PromptPlusLibrary.IHistory AddHistory(string value, System.Nullable<System.TimeSpan> timeout=null);
```
#### Parameters

<a name='PromptPlusLibrary.IHistory.AddHistory(string,System.Nullable_System.TimeSpan_).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The value to store in history\.

<a name='PromptPlusLibrary.IHistory.AddHistory(string,System.Nullable_System.TimeSpan_).timeout'></a>

`timeout` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional lifetime for the entry\. After the timeout expires, the entry may be removed
depending on the implementation\. If `null`, no expiration is applied\.

#### Returns
[IHistory](IHistory.md 'PromptPlusLibrary\.IHistory')  
The current [IHistory](IHistory.md 'PromptPlusLibrary\.IHistory') instance for fluent chaining\.

<a name='PromptPlusLibrary.IHistory.ReadHistory_T_()'></a>

## IHistory\.ReadHistory\<T\>\(\) Method

Reads all history entries and deserializes them to the specified type\.

```csharp
System.Collections.Generic.IList<T> ReadHistory<T>();
```
#### Type parameters

<a name='PromptPlusLibrary.IHistory.ReadHistory_T_().T'></a>

`T`

The type used to deserialize history entries\.

#### Returns
[System\.Collections\.Generic\.IList&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1 'System\.Collections\.Generic\.IList\`1')[T](IHistory.md#PromptPlusLibrary.IHistory.ReadHistory_T_().T 'PromptPlusLibrary\.IHistory\.ReadHistory\<T\>\(\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1 'System\.Collections\.Generic\.IList\`1')  
A list of deserialized history entries\.

<a name='PromptPlusLibrary.IHistory.Remove()'></a>

## IHistory\.Remove\(\) Method

Removes persisted history, such as deleting the backing store or clearing all entries\.

```csharp
void Remove();
```

<a name='PromptPlusLibrary.IHistory.Save()'></a>

## IHistory\.Save\(\) Method

Persists in\-memory history entries to durable storage\.

```csharp
void Save();
```