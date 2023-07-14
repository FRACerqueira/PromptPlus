# <img align="left" width="100" height="100" src="./images/icon.png">PromptPlus HotKeys

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Main**](index.md#table-of-contents)  

The hotkeys for each control can be  globally or for each instance. Valid values ​​for shortcut keys range from F1 to F12 and can be combined with Modifiers Shift, Alt and Control.

**_Attention: conflict analysis is not carried out (same hotkey for different actions), so choose constomizations carefully to avoid unwanted behavior_**

## Table of Contents

- [Default HotKeys](#default-hotKeys)
- [Global HotKeys](#global-hotKeys)
- [Instance HotKeys](#instance-hotKeys)
- [HotKey API Reference](./APIS/pplus.controls.hotkey.md)

## Default HotKeys

- HotKey.TooltipDefault : F1
- HotKey.PasswordViewDefault : F2
- HotKey.SelectAllDefault : F2
- HotKey.EditItemDefault : F2
- HotKey.ChartBarSwitchType : F2
- HotKey.ToggleExpandNode : F3
- HotKey.InvertSelectedDefault : F3
- HotKey.RemoveItemDefault : F3
- HotKey.ChartBarSwitchLegend : F3
- HotKey.ToggleExpandAllNodeDefault : F4
- HotKey.ToggleExpandAllNodeDefault : F4

## Global HotKeys
[**Top**](#promptplus-hotkeys)

To set Global HotKeys use : PromptPlus.Config.[Property-Hotkey]. Example :

```csharp
PromptPlus.Config.PasswordViewPress = new HotKey(UserHotKey.F7)
```

### Properties Hotkeys
[**Top**](#promptplus-hotkeys)

- TooltipKeyPress
- PasswordViewPress
- SelectAllPress
- InvertSelectedPress
- EditItemPress
- RemoveItemPress
- FullPathPress
- ToggleExpandPress
- ToggleExpandAllPress
- ChartBarSwitchType
- ChartBarSwitchLegend


### Instance Hotkeys
[**Top**](#promptplus-hotkeys)

For each control has methods to change hotkey. Use : [control].\[Method\](HotKey(UserKey,[Shift],[Alt],[Ctrl])). Example :

```csharp
PromptPlus
    .Input("Input secret sample2")
    .IsSecret()
    .EnabledViewSecret(new HotKey(UserHotKey.F7))
    .Run();
```

### Input Controls

- EnabledViewSecret

### AddtoList

- HotKeyEditItem
- HotKeyRemoveItem

### AddtoMaskEditList Controls

- HotKeyEditItem
- HotKeyRemoveItem
 
### MultiSelect Controls

- HotKeySelectAll
- HotKeyInvertSelected

### BrowserMultiSelect Controls

- HotKeyFullPath
- HotKeyToggleExpand
- HotKeyToggleExpandAll

### Browser Controls

- HotKeyFullPath
- HotKeyToggleExpand
- HotKeyToggleExpandAll
 
### TreeViewMultiSelect Controls

- HotKeyFullPath
- HotKeyToggleExpand
- HotKeyToggleExpandAll

### TreeView Controls

- HotKeyFullPath
- HotKeyToggleExpand
- HotKeyToggleExpandAll
 
### ChartBar Controls

- HotKeySwitchType
- HotKeySwitchLegend
