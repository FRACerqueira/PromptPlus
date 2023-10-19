// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

PromptPlus.Clear();

//Ensure default Culture for all controls
PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

PromptPlus
    .ChartBar("Control: ChartBar", "Basic usage")
    .AddItem("Label1", 10, Color.Blue)
    .AddItem("Label2", 20, Color.Green)
    .AddItem("Label3", 30, Color.Red)
    .Run();

PromptPlus
    .KeyPress()
    .Config(cfg =>
    {
        cfg.HideAfterFinish(true);
        cfg.ShowTooltip(false);
    }) 
    .Spinner(SpinnersType.DotsScrolling)
    .Run();
PromptPlus.Clear();


PromptPlus
    .ChartBar("Control: ChartBar", "layout usage")
    .AddItem("Label1", 10, Color.Blue)
    .AddItem("Label2", 20, Color.Green)
    .AddItem("Label3", 30, Color.Red)
    .Layout(LayoutChart.Stacked)
    .Run();

PromptPlus
    .KeyPress()
    .Config(cfg =>
    {
        cfg.HideAfterFinish(true);
        cfg.ShowTooltip(false);
    })
    .Spinner(SpinnersType.DotsScrolling)
    .Run();
PromptPlus.Clear();

PromptPlus
    .ChartBar("Control: ChartBar", "With legends")
    .TitleAlignment(Alignment.Center)
    .AddItem("Label1", 10, Color.Blue)
    .AddItem("Label2", 20, Color.Green)
    .AddItem("Label3", 30, Color.Red)
    .ShowLegends()
    .Run();

PromptPlus
    .KeyPress()
    .Config(cfg =>
    {
        cfg.HideAfterFinish(true);
        cfg.ShowTooltip(false);
    })
    .Spinner(SpinnersType.DotsScrolling)
    .Run();
PromptPlus.Clear();

PromptPlus
    .ChartBar("Control: ChartBar", "Auto-Color, Custom Style")
    .AddItem("Label1", 10)
    .AddItem("Label2", 20)
    .AddItem("Label3", 30)
    .HideValue()
    .ShowLegends(true,false)
    .Styles(StyleChart.Title, Style.Default.Foreground(Color.Turquoise4))
    .Styles(StyleChart.Label, Style.Default.Foreground(Color.Yellow))
    .Styles(StyleChart.Percent, Style.Default.Foreground(Color.IndianRed))
    .Styles(StyleChart.Value, Style.Default.Foreground(Color.Aqua))
    .Run();

PromptPlus
    .KeyPress()
    .Config(cfg =>
    {
        cfg.HideAfterFinish(true);
        cfg.ShowTooltip(false);
    })
    .Spinner(SpinnersType.DotsScrolling)
    .Run();
PromptPlus.Clear();

var items = new List<string>();
for (int i = 1; i <= 10; i++)
{
    items.Add($"Label {i}");
}
var index = 1;
PromptPlus
    .ChartBar("Control: ChartBar", "with Pagesize and many itens")
    .Interaction(items, (ctrl, value) =>
    {
        ctrl.AddItem(value, index * 10);
        index++;
    })
    .ShowLegends()
    .EnabledInteractionUser(true, true, true)
    .PageSize(3)
    .Run();

PromptPlus
    .KeyPress()
    .Config(cfg =>
    {
        cfg.HideAfterFinish(true);
        cfg.ShowTooltip(false);
    })
    .Spinner(SpinnersType.DotsScrolling)
    .Run();
PromptPlus.Clear();

var auxc = Enum.GetValues(typeof(ChartBarType));
foreach (var item in auxc)
{
    PromptPlus.Clear();
    var bt = (ChartBarType)Enum.Parse(typeof(ChartBarType), item.ToString()!);
    PromptPlus
        .ChartBar("Control: ChartBar", $"with ChartBarType:{bt}")
        .BarType(bt)
        .AddItem("Label1", 10)
        .AddItem("Label2", 20)
        .AddItem("Label3", 30)
        .HideOrdination()
        .HideValue()
        .ShowLegends(true, false)
        .Styles(StyleChart.Label, Style.Default.Foreground(Color.Yellow))
        .Styles(StyleChart.Percent, Style.Default.Foreground(Color.IndianRed))
        .Styles(StyleChart.Value, Style.Default.Foreground(Color.Aqua))
        .Run();

    PromptPlus
        .KeyPress()
        .Config(cfg =>
        {
            cfg.HideAfterFinish(true);
            cfg.ShowTooltip(false);
        })
        .Spinner(SpinnersType.DotsScrolling)
        .Run();
}
PromptPlus.WriteLines(2);
PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
    .Run();


