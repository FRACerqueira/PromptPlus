# PromptPlus # Confirm
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Confirm Options**](confirmoptions) |
[**BaseOptions**](baseoptions)

## Documentation
Control Confirm. Simple confirm with with tool tips and language detection.

![](./images/Confirm.gif)

### Options

[**Confirm Options**](confirmoptions)

### Syntax
[**Top**](#promptplus--confirm)

```csharp
Confirm(ConfirmOptions options, CancellationToken? cancellationToken = null)
Confirm(Action<ConfirmOptions> configure, CancellationToken? cancellationToken = null)
Confirm(string message, bool? defaultValue = null, CancellationToken? cancellationToken = null)  
````

**_Note1: defaultValue is true for positive confirm , false for negative confirm._**

**_Note2: The text for positive/negative confirm is extract from resx._**

### Return
[**Top**](#promptplus--confirm)

```csharp
ResultPromptPlus<bool>
````
**_Note: [ResultPromptPlus](resultpromptplus).Value is true for positive confirm , false for negative confirm._**


### Sample
[**Top**](#promptplus--confirm)

```csharp
PromptPlus.DefaultCulture = new CultureInfo("en-US");
var answer = PromptPlus.Confirm("Are you ready?", true, cancellationToken:_stopApp);
if (answer.IsAborted)
{
    return;
}
if (answer.Value)
{
    Console.WriteLine($"Sua resposta é Yes");
}
else
{
    Console.WriteLine($"Sua resposta é No");
}
````

```csharp
PromptPlus.DefaultCulture = new CultureInfo("pt-BR");
var answer = PromptPlus.Confirm("Você esta pronto?", true, cancellationToken:_stopApp);
if (answer.IsAborted)
{
    return;
}
if (answer.Value)
{
    Console.WriteLine($"Sua resposta é Sim");
}
else
{
    Console.WriteLine($"Sua resposta é Não");
}
````

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Confirm Options**](confirmoptions) |
[**BaseOptions**](baseoptions)
