# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IColumnStatusbar 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IColumnStatusbar

Namespace: PPlus.Controls



```csharp
public interface IColumnStatusbar
```

## Methods

### <a id="methods-addcolumn"/>**AddColumn(String, String, Byte, AutoSize, Alignment)**



```csharp
IColumnStatusbar AddColumn(string id, string value, byte minlength, AutoSize autosize, Alignment alignment)
```

#### Parameters

`id` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`minlength` [Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>

`autosize` [AutoSize](./pplus.controls.autosize.md)<br>

`alignment` [Alignment](./pplus.controls.alignment.md)<br>

#### Returns

[IColumnStatusbar](./pplus.controls.icolumnstatusbar.md)

### <a id="methods-addseparator"/>**AddSeparator()**



```csharp
IColumnStatusbar AddSeparator()
```

#### Returns

[IColumnStatusbar](./pplus.controls.icolumnstatusbar.md)

### <a id="methods-backcolorbar"/>**BackColorBar(Color)**



```csharp
IColumnStatusbar BackColorBar(Color value)
```

#### Parameters

`value` [Color](./pplus.color.md)<br>

#### Returns

[IColumnStatusbar](./pplus.controls.icolumnstatusbar.md)

### <a id="methods-build"/>**Build(TargetScreen)**



```csharp
void Build(TargetScreen target)
```

#### Parameters

`target` [TargetScreen](./pplus.controls.targetscreen.md)<br>

### <a id="methods-forecolorbar"/>**ForeColorBar(Color)**



```csharp
IColumnStatusbar ForeColorBar(Color value)
```

#### Parameters

`value` [Color](./pplus.color.md)<br>

#### Returns

[IColumnStatusbar](./pplus.controls.icolumnstatusbar.md)


- - -
[**Back to List Api**](./apis.md)
