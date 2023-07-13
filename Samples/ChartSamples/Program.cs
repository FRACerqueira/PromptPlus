using System.Globalization;
using PPlus;
using PPlus.Controls;

PromptPlus.Clear();

//Ensure default Culture for all controls
PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

PromptPlus
    .ChartBar()
    .Title("Control: ChartBar - basic usage")
    .AddItem("Label1", 10, Color.Blue)
    .AddItem("Label2", 20, Color.Green)
    .AddItem("Label3", 30, Color.Red)
    .Run(dashOptions: BannerDashOptions.AsciiDoubleBorderUpDown);

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
    .ChartBar()
    .Title("Control:ChartBar - With legends", TitleAligment.Center, Style.Plain.Foreground(Color.Gold1))
    .AddItem("Label1", 10, Color.Blue)
    .AddItem("Label2", 20, Color.Green)
    .AddItem("Label3", 30, Color.Red)
    .ShowLegends()
    .Run(dashOptions: BannerDashOptions.AsciiDoubleBorderUpDown);

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
    .ChartStackBar()
    .Title("Control:ChartStackBar", TitleAligment.Center, Style.Plain.Foreground(Color.Gold1))
    .AddItem("Label1", 10, Color.Blue)
    .AddItem("Label2", 20, Color.Green)
    .AddItem("Label3", 30, Color.Red)
    .Run(dashOptions: BannerDashOptions.AsciiDoubleBorderUpDown);

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
    .ChartBar()
    .Title("Control:ChartBar - Auto-Color, Custom Style", TitleAligment.Center, Style.Plain.Foreground(Color.Gold1))
    .AddItem("Label1", 10)
    .AddItem("Label2", 20)
    .AddItem("Label3", 30)
    .ChartPadLeft(2)
    .HideValue()
    .ShowLegends(true,false)
    .Styles(StyleChart.Label, Style.Plain.Foreground(Color.Yellow))
    .Styles(StyleChart.Percent, Style.Plain.Foreground(Color.IndianRed))
    .Styles(StyleChart.Value, Style.Plain.Foreground(Color.Aqua))
    .Run(dashOptions: BannerDashOptions.AsciiDoubleBorderUpDown);

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
    .ChartBar()
    .Title("Control: ChartBar - with Pagesize and many itens")
    .Interaction(items, (ctrl, value) =>
    {
        ctrl.AddItem(value, index * 10);
        index++;
    })
    .ShowLegends()
    .PageSize(3)
    .Run(dashOptions: BannerDashOptions.AsciiDoubleBorderUpDown);

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

index = 1;
PromptPlus
    .ChartStackBar()
    .Title("Control: ChartStackBar - with Pagesize and many itens")
    .Interaction(items, (ctrl, value) =>
    {
        ctrl.AddItem(value, index * 10);
        index++;
    })
    .ShowLegends()
    .PageSize(3)
    .Run(dashOptions: BannerDashOptions.AsciiDoubleBorderUpDown);

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
    PromptPlus.ChartBar()
    .Title($"Control:ProgChartBarressBar - ProgressBarType {bt}", TitleAligment.Center, Style.Plain.Foreground(Color.Gold1))
    .AddItem("Label1", 10)
    .AddItem("Label2", 20)
    .AddItem("Label3", 30)
    .ChartPadLeft(2)
    .HideValue()
    .ShowLegends(true, false)
    .Styles(StyleChart.Label, Style.Plain.Foreground(Color.Yellow))
    .Styles(StyleChart.Percent, Style.Plain.Foreground(Color.IndianRed))
    .Styles(StyleChart.Value, Style.Plain.Foreground(Color.Aqua))
    .Run(bt);
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


