# PromptPlus # Any Key
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus)

## Documentation
Simple any key press.

![](./images/Anykey.gif)

### Options
Not have options

### Syntax
[**Top**](#promptplus--any-key)

```csharp
AnyKey(CancellationToken? cancellationToken = null)
```

### Return
[**Top**](#promptplus--any-key)

```csharp
ResultPromptPlus<bool>
```
### Sample
[**Top**](#promptplus--any-key)

```csharp
PromptPlus.AnyKey(cancellationToken:_stopApp)
if (key.IsAborted)
{
    return;
}
Console.WriteLine($"Hello, key pressed");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus)
