<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ResultPrompt\<T\> Struct

Represents The Result [T](ResultPrompt_T_.md#PromptPlusLibrary.ResultPrompt_T_.T 'PromptPlusLibrary\.ResultPrompt\<T\>\.T') to Controls

```csharp
public readonly struct ResultPrompt<T>
```
#### Type parameters

<a name='PromptPlusLibrary.ResultPrompt_T_.T'></a>

`T`

Type of return
### Constructors

<a name='PromptPlusLibrary.ResultPrompt_T_.ResultPrompt(T,bool)'></a>

## ResultPrompt\(T, bool\) Constructor

Represents The Result [T](ResultPrompt_T_.md#PromptPlusLibrary.ResultPrompt_T_.T 'PromptPlusLibrary\.ResultPrompt\<T\>\.T') to Controls

```csharp
public ResultPrompt(T value, bool aborted);
```
#### Parameters

<a name='PromptPlusLibrary.ResultPrompt_T_.ResultPrompt(T,bool).value'></a>

`value` [T](ResultPrompt_T_.md#PromptPlusLibrary.ResultPrompt_T_.T 'PromptPlusLibrary\.ResultPrompt\<T\>\.T')

The content value\.

<a name='PromptPlusLibrary.ResultPrompt_T_.ResultPrompt(T,bool).aborted'></a>

`aborted` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If aborted\.
### Properties

<a name='PromptPlusLibrary.ResultPrompt_T_.Content'></a>

## ResultPrompt\<T\>\.Content Property

[T](ResultPrompt_T_.md#PromptPlusLibrary.ResultPrompt_T_.T 'PromptPlusLibrary\.ResultPrompt\<T\>\.T') Content result

```csharp
public T Content { get; }
```

#### Property Value
[T](ResultPrompt_T_.md#PromptPlusLibrary.ResultPrompt_T_.T 'PromptPlusLibrary\.ResultPrompt\<T\>\.T')

<a name='PromptPlusLibrary.ResultPrompt_T_.IsAborted'></a>

## ResultPrompt\<T\>\.IsAborted Property

Control is Aborted\. True to aborted; otherwise, false\.

```csharp
public bool IsAborted { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')
### Methods

<a name='PromptPlusLibrary.ResultPrompt_T_.Deconstruct(T,bool)'></a>

## ResultPrompt\<T\>\.Deconstruct\(T, bool\) Method

Deconstructs the [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') into its components\.

```csharp
public void Deconstruct(out T ContentValue, out bool Aborted);
```
#### Parameters

<a name='PromptPlusLibrary.ResultPrompt_T_.Deconstruct(T,bool).ContentValue'></a>

`ContentValue` [T](ResultPrompt_T_.md#PromptPlusLibrary.ResultPrompt_T_.T 'PromptPlusLibrary\.ResultPrompt\<T\>\.T')

The value\.

<a name='PromptPlusLibrary.ResultPrompt_T_.Deconstruct(T,bool).Aborted'></a>

`Aborted` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If aborted\.