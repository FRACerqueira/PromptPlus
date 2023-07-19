// See https://aka.ms/new-console-template for more information
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
    .FIGletWidth(CharacterWidth.Smush)
    .Run(Color.Blue);

PromptPlus.WriteLines(2);
PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
    .Run();

