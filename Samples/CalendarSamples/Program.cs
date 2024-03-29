﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

Console.WriteLine("Hello, World!");

Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
var cult = Thread.CurrentThread.CurrentCulture;
PromptPlus.Config.DefaultCulture = cult;

PromptPlus.Clear();

PromptPlus.DoubleDash($"Your default Culture is {cult.Name}", DashOptions.HeavyBorder, style: Color.Yellow.ToStyle());

PromptPlus.DoubleDash($"Control:Calendar ({cult.Name}) - minimal usage");

var cld =  PromptPlus
    .Calendar("Date", "Select date")
    .Run();

if (!cld.IsAborted)
{
    PromptPlus.WriteLine($"You input is {cld.Value}");
}

var typelayout = Enum.GetValues(typeof(CalendarLayout));
foreach (var type in typelayout)
{
    PromptPlus.DoubleDash($"Control:Calendar ({cult.Name}) - layout {type}");
    PromptPlus
    .Calendar("Date", "Select date")
    .Layout((CalendarLayout)Enum.Parse(typeof(CalendarLayout), type.ToString()!))
    .Run();
}


PromptPlus.DoubleDash($"Control:Calendar DateTime - with overwrite culture:pt-br.");

cld = PromptPlus
    .Calendar("Date", "Select date")
    .Layout(CalendarLayout.DoubleGrid)
    .Culture("pt-BR")
    .Run();
if (!cld.IsAborted)
{
    PromptPlus.WriteLine($"You input is {cld.Value}");
}

PromptPlus.DoubleDash($"Control:Calendar DateTime - with Disabled Weekends");

PromptPlus
    .Calendar("Date", "Select date")
    .Layout(CalendarLayout.SingleGrid)
    .DisabledWeekends()
    .Run();

PromptPlus.DoubleDash($"Control:Calendar DateTime - with Disabled dates");

PromptPlus
    .Calendar("Date", "Select date")
    .Layout(CalendarLayout.AsciiSingleGrid)
    .AddItems(CalendarScope.Disabled,
        new ItemCalendar(DateTime.Now.AddDays(1)),
        new ItemCalendar(DateTime.Now.AddDays(2)))
    .Run();

PromptPlus.DoubleDash($"Control:Calendar DateTime - with Notes");

PromptPlus
    .Calendar("Date", "Select date")
    .Layout(CalendarLayout.AsciiDoubleGrid)
    .AddItems(CalendarScope.Note,
        new ItemCalendar(DateTime.Now.AddDays(1), "Note1"),
        new ItemCalendar(DateTime.Now.AddDays(1), "Note2"),
        new ItemCalendar(DateTime.Now.AddDays(1), "Note3"),
        new ItemCalendar(DateTime.Now.AddDays(1), "Note4"),
        new ItemCalendar(DateTime.Now.AddDays(1), "Note5"),
        new ItemCalendar(DateTime.Now.AddDays(1), "Note6"),
        new ItemCalendar(DateTime.Now.AddDays(1), "Note7"),
        new ItemCalendar(DateTime.Now.AddDays(1), "Note8"))
    .Run();

PromptPlus.DoubleDash($"Control:Calendar DateTime - with Highlight");

PromptPlus
    .Calendar("Date", "Select date")
    .AddItems(CalendarScope.Highlight,
        new ItemCalendar(DateTime.Now.AddDays(1), "Note1"),
        new ItemCalendar(DateTime.Now.AddDays(1), "Note2"),
        new ItemCalendar(DateTime.Now.AddDays(1), "Note3"),
        new ItemCalendar(DateTime.Now.AddDays(1), "Note4"),
        new ItemCalendar(DateTime.Now.AddDays(1), "Note5"),
        new ItemCalendar(DateTime.Now.AddDays(1), "Note6"),
        new ItemCalendar(DateTime.Now.AddDays(1), "Note7"),
        new ItemCalendar(DateTime.Now.AddDays(1), "Note8"),
       new ItemCalendar(DateTime.Now.AddDays(2)))
    .Run();

PromptPlus.DoubleDash($"Control:Calendar DateTime - with Range (1 month ago to current date)");

PromptPlus
    .Calendar("Date", "Select date")
    .Default(DateTime.Now.AddDays(-7))
    .Range(DateTime.Now.AddMonths(-1),DateTime.Now)
    .Run();

PromptPlus.DoubleDash($"Control:Calendar DateTime - with ChangeDescription");

PromptPlus
    .Calendar("Date", "Select date")
    .ChangeDescription((date) => date.Date == DateTime.Now.Date?"Today":string.Empty)
    .Run();

PromptPlus.DoubleDash($"Control:Calendar DateTime - with Styles");

PromptPlus
    .Calendar("Date", "Select date")
    .Styles(CalendarStyles.Lines, Color.Blue.ToStyle())
    .Styles(CalendarStyles.Selected, Color.Green.ToStyle())
    .Styles(CalendarStyles.CalendarDay, Color.Yellow.ToStyle())
    .Styles(CalendarStyles.CalendarHighlight, Color.Blue.ToStyle())
    .Styles(CalendarStyles.CalendarMonth, Color.Green.ToStyle())
    .Styles(CalendarStyles.CalendarWeekDay, Color.Aqua.ToStyle())
    .Styles(CalendarStyles.CalendarYear, Color.Violet.ToStyle())
    .AddItems(CalendarScope.Highlight,
        new ItemCalendar(DateTime.Now.AddDays(1), "Note1"),
       new ItemCalendar(DateTime.Now.AddDays(2)))
    .Run();

PromptPlus.DoubleDash("For other features below see - input samples (same behavior)");
PromptPlus.WriteLine(". [yellow]ChangeDescription[/] - InputBasicSamples");
PromptPlus.WriteLine(". [yellow]AddValidators[/] - InputWithValidatorSamples");
PromptPlus.WriteLine(". [yellow]OverwriteDefaultFrom[/] - InputOverwriteDefaultFromSamples");
PromptPlus.WriteLines(2);
PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
    .Run();