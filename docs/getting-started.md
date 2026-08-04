<div align="center">
  <img src="../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Getting Started**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../README.md) • **Next:** [Core Concepts →](concepts.md)

---

This guide takes you from zero to a working interactive console app in about five minutes.

---

## Prerequisites

- .NET 8, 9, or 10 SDK
- Any terminal with at least 80 columns × 10 rows

---

## 1 — Install the Package

```shell
dotnet add package PromptPlus
```

Or add to your `.csproj`:

```xml
<PackageReference Include="PromptPlus" Version="*" />
```

---

## 2 — Your First App

Create `Program.cs` in a new console project:

```csharp
using PromptPlusLibrary;

// Ask for the user's name
var nameResult = PromptPlus.Controls.Input("What is your name?").Run();
if (nameResult.IsAborted)
{
	PromptPlus.Console.WriteLine("Cancelled.");
	return;
}

// Let the user pick a role
var roleResult = PromptPlus.Controls
	.Select<string>("Choose your role")
	.AddItems(["Developer", "Designer", "Manager", "Other"])
	.Run();

// Deconstruct result
var (role, aborted) = roleResult;
if (aborted)
{
	PromptPlus.Console.WriteLine("Cancelled.");
	return;
}

// Ask for confirmation
var confirmResult = PromptPlus.Controls
	.Confirm($"Hello {nameResult.Content}, your role is '{role}'. Correct?")
	.Run();

if (confirmResult.IsAborted)
{
	PromptPlus.Console.WriteLine("Cancelled.");
	return;
}

// Confirm returns the key the user pressed (ConsoleKeyInfo?), which is the culture's
// Yes or No key. Compare it to the configured YesChar to decide.
var confirmed = confirmResult.Content is { } key &&
	char.ToUpperInvariant(key.KeyChar) == char.ToUpperInvariant(PromptPlus.Config.YesChar);

if (confirmed)
	PromptPlus.Console.WriteLine("[green]Great, welcome![/]");
else
	PromptPlus.Console.WriteLine("[yellow]No problem, start over.[/]");
```

### Run it

```shell
dotnet run
```

You should see interactive prompts one after another. Press **Enter** to confirm, **Esc** to cancel any step.

---

## 3 — Understanding What Happened

| Line | What it does |
|---|---|
| `PromptPlus.Controls.Input(…)` | Creates an Input control with that prompt text |
| `.Run()` | Renders the control and blocks until the user confirms or presses Esc |
| `result.IsAborted` | `true` if the user pressed Esc |
| `result.Content` | The confirmed value (empty string if aborted) |
| `var (role, aborted) = …` | Deconstruct — equivalent to `.Content` and `.IsAborted` |
| `PromptPlus.Console.WriteLine(…)` | Writes styled text using markup tags |

> 💡 **Tip:** Always check `IsAborted` before using `.Content`. If the user pressed Esc, `.Content` returns the default value of `T`.

---

## 4 — Global Configuration

Add configuration before your first control to change defaults for all controls in your app:

```csharp
using PromptPlusLibrary;
using System.Globalization;

// At the top of your Program.cs, before any control

// Show 8 items per page in lists
PromptPlus.Config.PageSize = 8;

// Erase the control UI after the user confirms
PromptPlus.Config.HideAfterFinish = true;

// Use en-US for number and date formatting
PromptPlus.Config.DefaultCulture = new CultureInfo("en-US");

// Enable Emacs shortcuts in text fields
PromptPlus.Console.EnabledEmacs = true;

// Now run your controls …
var result = PromptPlus.Controls.Input("Name").Run();
```

---

## 5 — Persist Config to Disk

Write your configuration to a JSON file so it loads automatically on every run:

```csharp
// Set up your config
PromptPlus.Config.PageSize = 8;
PromptPlus.Config.HideAfterFinish = true;

// Save to current directory
PromptPlus.Config.ToFile(".");
```

This writes `PromptPlus.config` next to your executable. On the next run, PromptPlus reads it automatically — you do not need to call `ToFile()` again.

The generated file looks like — **every** config property is serialized, not just the ones you
changed (a partial excerpt, since the real file lists all of `IPromptPlusConfig`'s properties):

```json
{
  "PageSize": 8,
  "HideAfterFinish": true
}
```

> ⚠️ `ToFile()` serializes the entire configuration unconditionally — it does not diff against
> defaults. You can hand-edit the generated file safely.

---

## 6 — Per-Control Override

You can override any global config for a single control using `.Options()`:

```csharp
PromptPlus.Controls
	.Input("Private note")
	.Options(o => o
		.HideAfterFinish(false)  // keep UI visible even if global says true
		.ShowTooltip(false))     // hide keyboard hints for this field
	.Run();
```

---

## Next Steps

| Topic | Where to go |
|---|---|
| Understanding the design | [Architecture](architecture.md) |
| All config properties | [Global Behaviors](global-behaviors.md) |
| Keyboard shortcuts | [Keyboard Bindings](keyboard-bindings.md) |
| Output-only widgets | [Widgets](widgets.md) |
| All controls | [Docs index](index.md) |
| **Upgrading from v5.x** | [Migration Guide v5.x → v6.x](migration-v5-to-v6.md) |

---

## See also

- [Architecture](architecture.md) — entry points, control lifecycle, `ResultPrompt<T>`
- [Global Behaviors](global-behaviors.md) — full `IPromptPlusConfig` reference
- [Widgets](widgets.md) — render sliders, banners, and charts without user input
- [Migration Guide v5.x → v6.x](migration-v5-to-v6.md) — upgrading from the previous major version
