<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ISwitchWidget Interface

Provides a fluent API for configuring and rendering a read\-only Switch widget that
displays a boolean on/off state as a visual toggle, without waiting for user interaction\.

```csharp
public interface ISwitchWidget
```

### Remarks
A widget is for display only: unlike [ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl'), it does not read input\.
Call [Show\(\)](ISwitchWidget.md#PromptPlusLibrary.ISwitchWidget.Show() 'PromptPlusLibrary\.ISwitchWidget\.Show\(\)') last to render the switch on the console\.
### Methods

<a name='PromptPlusLibrary.ISwitchWidget.OffValue(ConsolePlusLibrary.EmojiName,string)'></a>

## ISwitchWidget\.OffValue\(EmojiName, string\) Method

Sets the label for the `off` \(false\) state using an emoji, with a plain\-text fallback
for terminals that do not support emoji rendering\.

```csharp
PromptPlusLibrary.ISwitchWidget OffValue(ConsolePlusLibrary.EmojiName emojiName, string fallbacktext);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchWidget.OffValue(ConsolePlusLibrary.EmojiName,string).emojiName'></a>

`emojiName` [ConsolePlusLibrary\.EmojiName](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.emojiname 'ConsolePlusLibrary\.EmojiName')

The emoji to display for the off state\.

<a name='PromptPlusLibrary.ISwitchWidget.OffValue(ConsolePlusLibrary.EmojiName,string).fallbacktext'></a>

`fallbacktext` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The plain\-text label used when the emoji cannot be rendered\.

#### Returns
[ISwitchWidget](ISwitchWidget.md 'PromptPlusLibrary\.ISwitchWidget')  
The current [ISwitchWidget](ISwitchWidget.md 'PromptPlusLibrary\.ISwitchWidget') instance for chaining\.

<a name='PromptPlusLibrary.ISwitchWidget.OffValue(string)'></a>

## ISwitchWidget\.OffValue\(string\) Method

Sets the label displayed for the `off` \(false\) state, replacing the default localized text\.

```csharp
PromptPlusLibrary.ISwitchWidget OffValue(string value);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchWidget.OffValue(string).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text to show when the switch is off\.

#### Returns
[ISwitchWidget](ISwitchWidget.md 'PromptPlusLibrary\.ISwitchWidget')  
The current [ISwitchWidget](ISwitchWidget.md 'PromptPlusLibrary\.ISwitchWidget') instance for chaining\.

<a name='PromptPlusLibrary.ISwitchWidget.OnValue(ConsolePlusLibrary.EmojiName,string)'></a>

## ISwitchWidget\.OnValue\(EmojiName, string\) Method

Sets the label for the `on` \(true\) state using an emoji, with a plain\-text fallback
for terminals that do not support emoji rendering\.

```csharp
PromptPlusLibrary.ISwitchWidget OnValue(ConsolePlusLibrary.EmojiName emojiName, string fallbacktext);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchWidget.OnValue(ConsolePlusLibrary.EmojiName,string).emojiName'></a>

`emojiName` [ConsolePlusLibrary\.EmojiName](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.emojiname 'ConsolePlusLibrary\.EmojiName')

The emoji to display for the on state\.

<a name='PromptPlusLibrary.ISwitchWidget.OnValue(ConsolePlusLibrary.EmojiName,string).fallbacktext'></a>

`fallbacktext` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The plain\-text label used when the emoji cannot be rendered\.

#### Returns
[ISwitchWidget](ISwitchWidget.md 'PromptPlusLibrary\.ISwitchWidget')  
The current [ISwitchWidget](ISwitchWidget.md 'PromptPlusLibrary\.ISwitchWidget') instance for chaining\.

<a name='PromptPlusLibrary.ISwitchWidget.OnValue(string)'></a>

## ISwitchWidget\.OnValue\(string\) Method

Sets the label displayed for the `on` \(true\) state, replacing the default localized text\.

```csharp
PromptPlusLibrary.ISwitchWidget OnValue(string value);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchWidget.OnValue(string).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text to show when the switch is on\.

#### Returns
[ISwitchWidget](ISwitchWidget.md 'PromptPlusLibrary\.ISwitchWidget')  
The current [ISwitchWidget](ISwitchWidget.md 'PromptPlusLibrary\.ISwitchWidget') instance for chaining\.

<a name='PromptPlusLibrary.ISwitchWidget.Show()'></a>

## ISwitchWidget\.Show\(\) Method

Renders the Switch widget on the console using the current configuration\. Call this method last\.

```csharp
void Show();
```

<a name='PromptPlusLibrary.ISwitchWidget.Styles(PromptPlusLibrary.SwitchStyles,ConsolePlusLibrary.Style)'></a>

## ISwitchWidget\.Styles\(SwitchStyles, Style\) Method

Overrides the visual style applied to a specific region of the Switch widget\.

```csharp
PromptPlusLibrary.ISwitchWidget Styles(PromptPlusLibrary.SwitchStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchWidget.Styles(PromptPlusLibrary.SwitchStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [SwitchStyles](SwitchStyles.md 'PromptPlusLibrary\.SwitchStyles')

The [SwitchStyles](SwitchStyles.md 'PromptPlusLibrary\.SwitchStyles') region whose style is overridden\.

<a name='PromptPlusLibrary.ISwitchWidget.Styles(PromptPlusLibrary.SwitchStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\.

#### Returns
[ISwitchWidget](ISwitchWidget.md 'PromptPlusLibrary\.ISwitchWidget')  
The current [ISwitchWidget](ISwitchWidget.md 'PromptPlusLibrary\.ISwitchWidget') instance for chaining\.