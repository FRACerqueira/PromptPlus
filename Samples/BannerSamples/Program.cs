// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************


using PPlus;
using PPlus.Controls;

//ValueResult Banner
PromptPlus
    .Banner("PromptPlus v4.0")
    .Run();

//with Forecolor
PromptPlus
    .Banner("PromptPlus v4.0")
    .Run(Color.Green);

//with border
PromptPlus
    .Banner("PromptPlus v4.0")
    .Run(Color.Yellow,BannerDashOptions.DoubleBorderUpDown);

//Load external font
PromptPlus
    .Banner("StarWars")
    .LoadFont("starwars.flf")
    .Run(Color.Blue);

PromptPlus.WriteLines(2);
PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
    .Run();

