<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## StateProgress Struct

Represents the final state of a Progress Bar control execution\.

```csharp
public readonly struct StateProgress
```
### Constructors

<a name='PromptPlusLibrary.StateProgress.StateProgress(double,string,double,double,System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception)'></a>

## StateProgress\(double, string, double, double, TimeSpan, IReadOnlyDictionary\<string,object\>, Exception\) Constructor

Represents the final state of a Progress Bar control execution\.

```csharp
public StateProgress(double value, string? valuetext, double minvalue, double maxvalue, System.TimeSpan elapsedtime, System.Collections.Generic.IReadOnlyDictionary<string,object?>? resultcontext=null, System.Exception? error=null);
```
#### Parameters

<a name='PromptPlusLibrary.StateProgress.StateProgress(double,string,double,double,System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Final numeric value\.

<a name='PromptPlusLibrary.StateProgress.StateProgress(double,string,double,double,System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).valuetext'></a>

`valuetext` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Final display text\.

<a name='PromptPlusLibrary.StateProgress.StateProgress(double,string,double,double,System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).minvalue'></a>

`minvalue` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Configured minimum value\.

<a name='PromptPlusLibrary.StateProgress.StateProgress(double,string,double,double,System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).maxvalue'></a>

`maxvalue` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Configured maximum value\.

<a name='PromptPlusLibrary.StateProgress.StateProgress(double,string,double,double,System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).elapsedtime'></a>

`elapsedtime` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

Total elapsed execution time\.

<a name='PromptPlusLibrary.StateProgress.StateProgress(double,string,double,double,System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).resultcontext'></a>

`resultcontext` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

Optional output context values\.

<a name='PromptPlusLibrary.StateProgress.StateProgress(double,string,double,double,System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).error'></a>

`error` [System\.Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception 'System\.Exception')

Captured exception, if any\.
### Properties

<a name='PromptPlusLibrary.StateProgress.ElapsedTime'></a>

## StateProgress\.ElapsedTime Property

Gets the total elapsed time\.

```csharp
public System.TimeSpan ElapsedTime { get; }
```

#### Property Value
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

<a name='PromptPlusLibrary.StateProgress.ExceptionProgress'></a>

## StateProgress\.ExceptionProgress Property

Gets the captured exception, if one occurred\.

```csharp
public System.Exception? ExceptionProgress { get; }
```

#### Property Value
[System\.Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception 'System\.Exception')

<a name='PromptPlusLibrary.StateProgress.FinishedText'></a>

## StateProgress\.FinishedText Property

Gets the final display text\.

```csharp
public string? FinishedText { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='PromptPlusLibrary.StateProgress.FinishedValue'></a>

## StateProgress\.FinishedValue Property

Gets the final numeric value\.

```csharp
public System.Nullable<double> FinishedValue { get; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='PromptPlusLibrary.StateProgress.MaxValue'></a>

## StateProgress\.MaxValue Property

Gets the configured maximum value\.

```csharp
public double MaxValue { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='PromptPlusLibrary.StateProgress.MinValue'></a>

## StateProgress\.MinValue Property

Gets the configured minimum value\.

```csharp
public double MinValue { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='PromptPlusLibrary.StateProgress.OutputContext'></a>

## StateProgress\.OutputContext Property

Gets optional output context values\.

```csharp
public System.Collections.Generic.IReadOnlyDictionary<string,object?>? OutputContext { get; }
```

#### Property Value
[System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')
### Methods

<a name='PromptPlusLibrary.StateProgress.GetOutput_T_(string,bool)'></a>

## StateProgress\.GetOutput\<T\>\(string, bool\) Method

Tries to read an output context value and cast it to [T](StateProgress.md#PromptPlusLibrary.StateProgress.GetOutput_T_(string,bool).T 'PromptPlusLibrary\.StateProgress\.GetOutput\<T\>\(string, bool\)\.T')\.

```csharp
public T GetOutput<T>(string key, out bool found);
```
#### Type parameters

<a name='PromptPlusLibrary.StateProgress.GetOutput_T_(string,bool).T'></a>

`T`

Expected value type\.
#### Parameters

<a name='PromptPlusLibrary.StateProgress.GetOutput_T_(string,bool).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Output context key\.

<a name='PromptPlusLibrary.StateProgress.GetOutput_T_(string,bool).found'></a>

`found` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` when the key exists and the value matches [T](StateProgress.md#PromptPlusLibrary.StateProgress.GetOutput_T_(string,bool).T 'PromptPlusLibrary\.StateProgress\.GetOutput\<T\>\(string, bool\)\.T'); otherwise, `false`\.

#### Returns
[T](StateProgress.md#PromptPlusLibrary.StateProgress.GetOutput_T_(string,bool).T 'PromptPlusLibrary\.StateProgress\.GetOutput\<T\>\(string, bool\)\.T')  
The typed value when found; otherwise, `default`\.