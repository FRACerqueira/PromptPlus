# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlStatusbar 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlStatusbar

Namespace: PPlus.Controls

Represents the interface with all Methods of the Statusbar control

```csharp
public interface IControlStatusbar
```

## Methods

### <a id="methods-create"/>**Create()**



```csharp
IColumnStatusbar Create()
```

#### Returns

[IColumnStatusbar](./pplus.controls.icolumnstatusbar.md)

### <a id="methods-getgetcolumn"/>**GetGetColumn(String)**



```csharp
string GetGetColumn(string idcolumn)
```

#### Parameters

`idcolumn` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)

### <a id="methods-hide"/>**Hide()**



```csharp
void Hide()
```

### <a id="methods-show"/>**Show()**



```csharp
bool Show()
```

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)

### <a id="methods-update"/>**Update(String, String)**



```csharp
bool Update(string idcolumn, string value)
```

#### Parameters

`idcolumn` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)


- - -
[**Back to List Api**](./apis.md)
