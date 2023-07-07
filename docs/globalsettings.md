# <img align="left" width="100" height="100" src="./images/icon.png">PromptPlus Settings

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Main**](index.md#table-of-contents)  

The global setting modified  properties for all controls.  To set use : PromptPlus.Config.[Property]. Example :

```csharp
PromptPlus.Config.PageSize = 10;
```



## Table of Contents

- [HotKeys](hotkeys.md)
- [Settings](#settings)
- [Symbols](#symbols)

## Settings
[**Top**](#promptplus-settings)

- DefaultCulture
	- Get/Set default Culture for all controls.
- PageSize
	- Get/Set Page Size from colletions. Default value : 10. If value < 1 internal sette to 1.
- CompletionMinimumPrefixLength
	- Get/Set Minimum Prefix Length for feature AutoComplete. Default value : 3.If value < 0 internal sette to 0.
- CompletionWaitToStart
	- Get/Set Interval in mileseconds to wait start Completion feature. Default value : 1000. If value less than 10 internal sette to 10.
- CompletionMaxCount
	- Get/Set Completion Max Items to return for feature AutoComplete. Default value : 1000. If value  less than 1 internal sette to 1.
- HistoryTimeout
	- Get/Set History feature Timeout. Default value : 365 days
- ShowTooltip
	- Get/Set enabled start show Tooltip for all controls. Default value : true
- EnabledAbortKey
	- Get/Set enabled abortKey(ESC) for all controls. Default value : true
- HideAfterFinish
	- Get/Set hide controls after finish for all controls. Default value : false 
- HideOnAbort
	- Get/Set hide controls On Abort for all controls. Default value : false 
- SecretChar
	- Get/Set value char for secret input. Default value : '#'.  Fall-back when null : '#' 
- YesChar
	- Get/Set value for YES answer. efault value : YesChar in built-in resources.  Fall-back when null : Y
- NoChar
	- Get/Set value for NO answer. Default value : NoChar in built-in resources.  Fall-back when null : N

## Symbols
[**Top**](#promptplus-settings)

Global symbols modify the graphical representation of some characters for all controls. Warning: Changes to these symbols may cause an unwanted layout.

To set use : PromptPlus.Config. Symbols(SymbolType, value, unicode). Example :

```csharp
//SET
PromptPlus.Config.Symbols(SymbolType.Done, "V", "√");
//GET
var values = PromptPlus.Config.Symbols(SymbolType.Done);
```


### SymbolType: (NO-UNICODE,UNICODE)

- MaskEmpty: ("_", "■")
- Done: ("V", "√")
- Selector: (">", "›")
- Selected: ("*", "♦")
- NotSelect: ("o", "○")
- Expanded: ("[-]", "[-]")
- Collapsed: ("[+]", "[+]")
- IndentGroup: ("|-", "├─")
- IndentEndGroup: ("|_", "└─")
- TreeLinecross: (" |-", " ├─")
- TreeLinecorner: (" |_", " └─")
- TreeLinevertical: (" | ", " │ ")
- TreeLinespace: ("   ", "   ")
- DoubleBorder: ("=", "═")
- SingleBorder: ("-", "─")
- HeavyBorder: ("*", "━")

