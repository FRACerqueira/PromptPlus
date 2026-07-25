<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ProgressBarEvent Class

Represents the mutable state used by ProgressBar update callbacks\.

```csharp
public sealed class ProgressBarEvent
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → ProgressBarEvent
### Constructors

<a name='PromptPlusLibrary.ProgressBarEvent.ProgressBarEvent(double,double,double,System.Collections.Generic.IDictionary_string,object_)'></a>

## ProgressBarEvent\(double, double, double, IDictionary\<string,object\>\) Constructor

Initializes a new [ProgressBarEvent](ProgressBarEvent.md 'PromptPlusLibrary\.ProgressBarEvent') instance\.

```csharp
public ProgressBarEvent(double value, double min, double max, System.Collections.Generic.IDictionary<string,object?>? paramcontext=null);
```
#### Parameters

<a name='PromptPlusLibrary.ProgressBarEvent.ProgressBarEvent(double,double,double,System.Collections.Generic.IDictionary_string,object_).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Initial progress value\.

<a name='PromptPlusLibrary.ProgressBarEvent.ProgressBarEvent(double,double,double,System.Collections.Generic.IDictionary_string,object_).min'></a>

`min` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Minimum allowed progress value\.

<a name='PromptPlusLibrary.ProgressBarEvent.ProgressBarEvent(double,double,double,System.Collections.Generic.IDictionary_string,object_).max'></a>

`max` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Maximum allowed progress value\.

<a name='PromptPlusLibrary.ProgressBarEvent.ProgressBarEvent(double,double,double,System.Collections.Generic.IDictionary_string,object_).paramcontext'></a>

`paramcontext` [System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')

Optional input context available to the callback\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when [min](ProgressBarEvent.md#PromptPlusLibrary.ProgressBarEvent.ProgressBarEvent(double,double,double,System.Collections.Generic.IDictionary_string,object_).min 'PromptPlusLibrary\.ProgressBarEvent\.ProgressBarEvent\(double, double, double, System\.Collections\.Generic\.IDictionary\<string,object\>\)\.min') is greater than or equal to [max](ProgressBarEvent.md#PromptPlusLibrary.ProgressBarEvent.ProgressBarEvent(double,double,double,System.Collections.Generic.IDictionary_string,object_).max 'PromptPlusLibrary\.ProgressBarEvent\.ProgressBarEvent\(double, double, double, System\.Collections\.Generic\.IDictionary\<string,object\>\)\.max')\.

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [value](ProgressBarEvent.md#PromptPlusLibrary.ProgressBarEvent.ProgressBarEvent(double,double,double,System.Collections.Generic.IDictionary_string,object_).value 'PromptPlusLibrary\.ProgressBarEvent\.ProgressBarEvent\(double, double, double, System\.Collections\.Generic\.IDictionary\<string,object\>\)\.value') is outside the [min](ProgressBarEvent.md#PromptPlusLibrary.ProgressBarEvent.ProgressBarEvent(double,double,double,System.Collections.Generic.IDictionary_string,object_).min 'PromptPlusLibrary\.ProgressBarEvent\.ProgressBarEvent\(double, double, double, System\.Collections\.Generic\.IDictionary\<string,object\>\)\.min')/[max](ProgressBarEvent.md#PromptPlusLibrary.ProgressBarEvent.ProgressBarEvent(double,double,double,System.Collections.Generic.IDictionary_string,object_).max 'PromptPlusLibrary\.ProgressBarEvent\.ProgressBarEvent\(double, double, double, System\.Collections\.Generic\.IDictionary\<string,object\>\)\.max') range\.
### Properties

<a name='PromptPlusLibrary.ProgressBarEvent.Error'></a>

## ProgressBarEvent\.Error Property

Gets the error that caused an abort, if any\.

```csharp
public System.Exception? Error { get; }
```

#### Property Value
[System\.Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception 'System\.Exception')

<a name='PromptPlusLibrary.ProgressBarEvent.Finish'></a>

## ProgressBarEvent\.Finish Property

Gets whether processing has completed \(aborted or reached max value\)\.

```csharp
public bool Finish { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='PromptPlusLibrary.ProgressBarEvent.Maxvalue'></a>

## ProgressBarEvent\.Maxvalue Property

Gets the maximum progress value\.

```csharp
public double Maxvalue { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='PromptPlusLibrary.ProgressBarEvent.Minvalue'></a>

## ProgressBarEvent\.Minvalue Property

Gets the minimum progress value\.

```csharp
public double Minvalue { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='PromptPlusLibrary.ProgressBarEvent.OutputContext'></a>

## ProgressBarEvent\.OutputContext Property

Gets the output context produced during handler execution\.

```csharp
public System.Collections.ObjectModel.ReadOnlyDictionary<string,object?> OutputContext { get; }
```

#### Property Value
[System\.Collections\.ObjectModel\.ReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.readonlydictionary-2 'System\.Collections\.ObjectModel\.ReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.readonlydictionary-2 'System\.Collections\.ObjectModel\.ReadOnlyDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.readonlydictionary-2 'System\.Collections\.ObjectModel\.ReadOnlyDictionary\`2')

<a name='PromptPlusLibrary.ProgressBarEvent.Value'></a>

## ProgressBarEvent\.Value Property

Gets the current progress value\.

```csharp
public double Value { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')
### Methods

<a name='PromptPlusLibrary.ProgressBarEvent.AddOutputContext_T_(string,T)'></a>

## ProgressBarEvent\.AddOutputContext\<T\>\(string, T\) Method

Adds or updates an output context entry\.

```csharp
public void AddOutputContext<T>(string key, T value);
```
#### Type parameters

<a name='PromptPlusLibrary.ProgressBarEvent.AddOutputContext_T_(string,T).T'></a>

`T`
#### Parameters

<a name='PromptPlusLibrary.ProgressBarEvent.AddOutputContext_T_(string,T).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Output context key\.

<a name='PromptPlusLibrary.ProgressBarEvent.AddOutputContext_T_(string,T).value'></a>

`value` [T](ProgressBarEvent.md#PromptPlusLibrary.ProgressBarEvent.AddOutputContext_T_(string,T).T 'PromptPlusLibrary\.ProgressBarEvent\.AddOutputContext\<T\>\(string, T\)\.T')

Value associated with [key](ProgressBarEvent.md#PromptPlusLibrary.ProgressBarEvent.AddOutputContext_T_(string,T).key 'PromptPlusLibrary\.ProgressBarEvent\.AddOutputContext\<T\>\(string, T\)\.key')\.

<a name='PromptPlusLibrary.ProgressBarEvent.ErrorAndAbort(System.Exception)'></a>

## ProgressBarEvent\.ErrorAndAbort\(Exception\) Method

Stores an error and aborts further processing\.

```csharp
public void ErrorAndAbort(System.Exception? error);
```
#### Parameters

<a name='PromptPlusLibrary.ProgressBarEvent.ErrorAndAbort(System.Exception).error'></a>

`error` [System\.Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception 'System\.Exception')

Associated error\. Can be `null`\.

<a name='PromptPlusLibrary.ProgressBarEvent.HasChange()'></a>

## ProgressBarEvent\.HasChange\(\) Method

Indicates whether state changed since the previous check\.

```csharp
public bool HasChange();
```

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` when aborted or when the value changed; otherwise, `false`\.

<a name='PromptPlusLibrary.ProgressBarEvent.InputParam_T_(string,bool)'></a>

## ProgressBarEvent\.InputParam\<T\>\(string, bool\) Method

Tries to read an input context value and cast it to [T](ProgressBarEvent.md#PromptPlusLibrary.ProgressBarEvent.InputParam_T_(string,bool).T 'PromptPlusLibrary\.ProgressBarEvent\.InputParam\<T\>\(string, bool\)\.T')\.

```csharp
public T InputParam<T>(string key, out bool found);
```
#### Type parameters

<a name='PromptPlusLibrary.ProgressBarEvent.InputParam_T_(string,bool).T'></a>

`T`

Expected value type\.
#### Parameters

<a name='PromptPlusLibrary.ProgressBarEvent.InputParam_T_(string,bool).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Input context key\.

<a name='PromptPlusLibrary.ProgressBarEvent.InputParam_T_(string,bool).found'></a>

`found` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` when the key exists and the value matches [T](ProgressBarEvent.md#PromptPlusLibrary.ProgressBarEvent.InputParam_T_(string,bool).T 'PromptPlusLibrary\.ProgressBarEvent\.InputParam\<T\>\(string, bool\)\.T'); otherwise, `false`\.

#### Returns
[T](ProgressBarEvent.md#PromptPlusLibrary.ProgressBarEvent.InputParam_T_(string,bool).T 'PromptPlusLibrary\.ProgressBarEvent\.InputParam\<T\>\(string, bool\)\.T')  
The typed value when found; otherwise, `default`\.

<a name='PromptPlusLibrary.ProgressBarEvent.RemoveOutputContext(string)'></a>

## ProgressBarEvent\.RemoveOutputContext\(string\) Method

Removes an output context entry by key\.

```csharp
public void RemoveOutputContext(string key);
```
#### Parameters

<a name='PromptPlusLibrary.ProgressBarEvent.RemoveOutputContext(string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Output context key\.

<a name='PromptPlusLibrary.ProgressBarEvent.Update(double)'></a>

## ProgressBarEvent\.Update\(double\) Method

Updates the current value, clamped to the configured range\.

```csharp
public void Update(double value);
```
#### Parameters

<a name='PromptPlusLibrary.ProgressBarEvent.Update(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

New progress value\.