# PromptPlus # Any Key
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus)

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
ResultPPlus<bool>
```
### Sample
[**Top**](#promptplus--any-key)

```csharp
PPlus.AnyKey(cancellationToken:_stopApp)
if (key.IsAborted)
{
    return;
}
Console.WriteLine($"Hello, key pressed");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus)
