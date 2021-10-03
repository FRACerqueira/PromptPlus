# PromptPlus # Password
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
[**Input Options**](inputoptions) |
[**BaseOptions**](baseoptions)

## Documentation
Control Password. Generic input password with input validator and show/hide(optional) input value.

![](./images/Password.gif)

### Options

[**Input Options**](inputoptions)

### Syntax
[**Top**](#promptplus--password)

```csharp
Input<T>(InputOptions options, CancellationToken? cancellationToken = null)
Input<T>(Action<InputOptions> configure, CancellationToken? cancellationToken = null)
```

```csharp
//Note : The property is fixed in InputOptions: IsPassword = true
Password(string message, bool swithVisible = true, IList<Func<object, ValidationResult>> validators = null, CancellationToken? cancellationToken = null)
```
**_Note: The parameter swithVisible Enabled/disabled view input password by [Hotkey](index.md#hotkeys) SwitchViewPassword ._**

### Return
[**Top**](#promptplus--password)

```csharp
ResultPPlus<String>
```

### Sample
[**Top**](#promptplus--password)

```csharp
var pwd = PPlus.Password("Type new password", true, 
            new[] { Validators.Required(), Validators.MinLength(8) }, _stopApp);
if (pwd.IsAborted)
{
    return;
}
Console.WriteLine($"Password OK : {pwd.Value}");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
[**Input Options**](inputoptions) |
[**BaseOptions**](baseoptions)

