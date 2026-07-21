<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IControlOptions Interface

Provides a fluent API for configuring control behavior and presentation\.

```csharp
public interface IControlOptions
```
### Methods

<a name='PromptPlusLibrary.IControlOptions.Description(string)'></a>

## IControlOptions\.Description\(string\) Method

Sets a descriptive text displayed with the control\.

```csharp
PromptPlusLibrary.IControlOptions Description(string description);
```
#### Parameters

<a name='PromptPlusLibrary.IControlOptions.Description(string).description'></a>

`description` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description text\. If `null` or empty, any existing description may be cleared\.

#### Returns
[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')  
The current [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') instance for chaining\.

<a name='PromptPlusLibrary.IControlOptions.EnabledAbortKey(bool)'></a>

## IControlOptions\.EnabledAbortKey\(bool\) Method

Enables or disables the abort \(Esc\) hotkey for the control\.

```csharp
PromptPlusLibrary.IControlOptions EnabledAbortKey(bool isEnabled=true);
```
#### Parameters

<a name='PromptPlusLibrary.IControlOptions.EnabledAbortKey(bool).isEnabled'></a>

`isEnabled` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the abort key is enabled; otherwise, it is disabled\. Default is `true`\.

#### Returns
[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')  
The current [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') instance for chaining\.

<a name='PromptPlusLibrary.IControlOptions.HideAfterFinish(bool)'></a>

## IControlOptions\.HideAfterFinish\(bool\) Method

Clears the control's render area after successful completion\.

```csharp
PromptPlusLibrary.IControlOptions HideAfterFinish(bool shouldHide=true);
```
#### Parameters

<a name='PromptPlusLibrary.IControlOptions.HideAfterFinish(bool).shouldHide'></a>

`shouldHide` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the render area is cleared; otherwise, it remains\. Default is `true`\.

#### Returns
[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')  
The current [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') instance for chaining\.

<a name='PromptPlusLibrary.IControlOptions.HideOnAbort(bool)'></a>

## IControlOptions\.HideOnAbort\(bool\) Method

Clears the control's render area after an abort \(escape\) action\.

```csharp
PromptPlusLibrary.IControlOptions HideOnAbort(bool shouldHide=true);
```
#### Parameters

<a name='PromptPlusLibrary.IControlOptions.HideOnAbort(bool).shouldHide'></a>

`shouldHide` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the render area is cleared on abort; otherwise, it remains\. Default is `true`\.

#### Returns
[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')  
The current [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') instance for chaining\.

<a name='PromptPlusLibrary.IControlOptions.PrefixExtraInfo(string)'></a>

## IControlOptions\.PrefixExtraInfo\(string\) Method

Sets the prefix string displayed before extra info text\.

```csharp
PromptPlusLibrary.IControlOptions PrefixExtraInfo(string prefix="(");
```
#### Parameters

<a name='PromptPlusLibrary.IControlOptions.PrefixExtraInfo(string).prefix'></a>

`prefix` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The prefix string\. Defaults to `"("`\.

#### Returns
[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')  
The current [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') instance for chaining\.

<a name='PromptPlusLibrary.IControlOptions.Prompt(string)'></a>

## IControlOptions\.Prompt\(string\) Method

Sets the prompt text displayed to the user\.

```csharp
PromptPlusLibrary.IControlOptions Prompt(string prompt);
```
#### Parameters

<a name='PromptPlusLibrary.IControlOptions.Prompt(string).prompt'></a>

`prompt` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The prompt text\. Should be concise and user‑facing\.

#### Returns
[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')  
The current [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') instance for chaining\.

<a name='PromptPlusLibrary.IControlOptions.ShowMessageAbortKey(bool)'></a>

## IControlOptions\.ShowMessageAbortKey\(bool\) Method

Shows or hides the abort key help message \(localized when available\)\.

```csharp
PromptPlusLibrary.IControlOptions ShowMessageAbortKey(bool isshow=true);
```
#### Parameters

<a name='PromptPlusLibrary.IControlOptions.ShowMessageAbortKey(bool).isshow'></a>

`isshow` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the abort key message is displayed; otherwise, it is hidden\. Default is `true`\.

#### Returns
[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')  
The current [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') instance for chaining\.

<a name='PromptPlusLibrary.IControlOptions.ShowTooltip(bool)'></a>

## IControlOptions\.ShowTooltip\(bool\) Method

Shows or hides the tooltip associated with the control\.

```csharp
PromptPlusLibrary.IControlOptions ShowTooltip(bool isVisible=true);
```
#### Parameters

<a name='PromptPlusLibrary.IControlOptions.ShowTooltip(bool).isVisible'></a>

`isVisible` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the tooltip is shown; otherwise, it is hidden\. Default is `true`\.

#### Returns
[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')  
The current [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') instance for chaining\.

<a name='PromptPlusLibrary.IControlOptions.SuffixExtraInfo(string)'></a>

## IControlOptions\.SuffixExtraInfo\(string\) Method

Sets the suffix string displayed after extra info text\.

```csharp
PromptPlusLibrary.IControlOptions SuffixExtraInfo(string suffix=")");
```
#### Parameters

<a name='PromptPlusLibrary.IControlOptions.SuffixExtraInfo(string).suffix'></a>

`suffix` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The suffix string\. Defaults to `")"`\.

#### Returns
[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')  
The current [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') instance for chaining\.

<a name='PromptPlusLibrary.IControlOptions.SufixAfterPrompt(string)'></a>

## IControlOptions\.SufixAfterPrompt\(string\) Method

Sets a suffix string to be displayed after the prompt text\.

```csharp
PromptPlusLibrary.IControlOptions SufixAfterPrompt(string sufix=": ");
```
#### Parameters

<a name='PromptPlusLibrary.IControlOptions.SufixAfterPrompt(string).sufix'></a>

`sufix` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The suffix string to display after the prompt text\.

#### Returns
[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')  
The current [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') instance for chaining\.