<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## PromptPlus Class

Provides the global entry point for all Prompt services\.

```csharp
public static class PromptPlus
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → PromptPlus
### Properties

<a name='PromptPlusLibrary.PromptPlus.Config'></a>

## PromptPlus\.Config Property

Gets the global configuration for PromptPlus, allowing for customization of behavior and appearance across all PromptPlus components\.

```csharp
public static PromptPlusLibrary.IPromptPlusConfig Config { get; }
```

#### Property Value
[IPromptPlusConfig](IPromptPlusConfig.md 'PromptPlusLibrary\.IPromptPlusConfig')

<a name='PromptPlusLibrary.PromptPlus.Console'></a>

## PromptPlus\.Console Property

Gets the console interface used by PromptPlus, providing access to console input/output operations and properties\.

```csharp
public static ConsolePlusLibrary.IConsole Console { get; }
```

#### Property Value
[ConsolePlusLibrary\.IConsole](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.iconsole 'ConsolePlusLibrary\.IConsole')

<a name='PromptPlusLibrary.PromptPlus.Controls'></a>

## PromptPlus\.Controls Property

Gets a factory for interactive controls \(input, select, file select, progress, masking, etc\.\)\.
Each method returns a fluent configuration object\.

```csharp
public static PromptPlusLibrary.IControls Controls { get; }
```

#### Property Value
[IControls](IControls.md 'PromptPlusLibrary\.IControls')

<a name='PromptPlusLibrary.PromptPlus.Widgets'></a>

## PromptPlus\.Widgets Property

Gets a factory for creating and emitting visual widgets \(banner, dash lines, chart bar, slider, etc\.\)\.

```csharp
public static PromptPlusLibrary.IWidgets Widgets { get; }
```

#### Property Value
[IWidgets](IWidgets.md 'PromptPlusLibrary\.IWidgets')