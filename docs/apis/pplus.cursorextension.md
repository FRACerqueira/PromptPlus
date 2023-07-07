# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus CursorExtension 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# CursorExtension

Namespace: PPlus

Represents the Extension for cursor console.

```csharp
public static class CursorExtension
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [CursorExtension](./pplus.cursorextension.md)

## Methods

### **MoveCursor(IConsoleBase, CursorDirection, Int32)**

Moves the cursor relative to the current position.

```csharp
public static void MoveCursor(IConsoleBase consoleBase, CursorDirection direction, int steps)
```

#### Parameters

`consoleBase` [IConsoleBase](./pplus.iconsolebase.md)<br>

`direction` [CursorDirection](./pplus.cursordirection.md)<br>
The direction to move the cursor.

`steps` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The number of steps to move the cursor.


- - -
[**Back to List Api**](./apis.md)
