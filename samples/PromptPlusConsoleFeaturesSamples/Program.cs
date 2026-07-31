// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the ConsolePlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;

namespace PromptPlusConsoleFeaturesSamples
{
    internal class Program
    {
        static void Main()
        {
            PromptPlus.Console.EnabledEmacs = true;

            static void Pause(string message = "[Yellow]Press any key to continue[/]")
            {
                PromptPlus.Console.WriteLine("");
                PromptPlus.Console.WriteLine(message);
                PromptPlus.Console.ReadKey();
                PromptPlus.Console.WriteLine();
            }

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.Banner("PromptPlus", Color.Bisque);

            PromptPlus.Widgets.Dash($"Create file: '{IPromptPlusConfig.NameResourcePromptPlusConfigFile}' at current BaseDirectory with config for all controls",Color.Yellow,DashOptions.DoubleBorderUpDown,1);
            PromptPlus.Config.ToFile(AppDomain.CurrentDomain.BaseDirectory);
            PromptPlus.Console.WriteLine($"File create :{File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, IPromptPlusConfig.NameResourcePromptPlusConfigFile))}, ");
            PromptPlus.Console.WriteLine($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, IPromptPlusConfig.NameResourcePromptPlusConfigFile)}");

            PromptPlus.Widgets.Dash("01 - Basic Markup (foreground/background)", Color.Yellow, DashOptions.DoubleBorderUpDown, 1);
            PromptPlus.Console.WriteLine("[RGB(255,0,0) ON WHITE]Test[GREEN] COLOR[/] BACK COLOR [/] other text");
            PromptPlus.Console.WriteLine("[RGB(255,0,0):WHITE]Test[GREEN] COLOR[/] BACK COLOR [/] other text");
            PromptPlus.Console.WriteLine("[RED:WHITE]Test[bLUE] COLOR[/] BACK COLOR[/] other text");
            PromptPlus.Console.WriteLine("[TEAL]Named CSS color[/] and [#FF8C00]HEX color[/]");
            PromptPlus.Console.WriteLine("[Darkseagreen810]Named CSS color Darkseagreen with weight 810[/]");

            Pause();

            PromptPlus.Console.WriteLines(2);

            PromptPlus.Widgets.Dash("02 - Common cases and markup escape", Color.Yellow, DashOptions.DoubleBorderUpDown, 1);
            PromptPlus.Console.WriteLine("[RED]ERROR:[/] Wrong format! (/x/g/[My Folder Name Has Brackets]/[[BracketFile]].xml)");
            PromptPlus.Console.WriteLine("[RED]ERROR:[/] Wrong format! (/x/g/[My Folder Name Has Brackets]/[BracketFile].xml)");
            PromptPlus.Console.WriteLine("[RED].xml");
            PromptPlus.Console.WriteLine("[RED].xml".EscapeMarkup());
            PromptPlus.Console.WriteLine("[RED:WHITE]Test[/][bLUE] missing token but ok!");
            PromptPlus.Console.WriteLine("Test[/] missing token but ok!");
            PromptPlus.Console.WriteLine("[[RED]]Test escape token", PromptPlus.Console.CurrentStyle.ForeGround(Color.Aqua));
            PromptPlus.Console.WriteLine("[RED]Test escape token".EscapeMarkup(), PromptPlus.Console.CurrentStyle.ForeGround(Color.Aqua));
            PromptPlus.Console.WriteLine("[RED]Test[/] with Style", PromptPlus.Console.CurrentStyle.ForeGround(Color.Yellow));
            PromptPlus.Console.WriteLine("[RED]Test.xml {1}", PromptPlus.Console.CurrentStyle.Background(Color.White));
            PromptPlus.Console.WriteLine("Array[1][2]", PromptPlus.Console.CurrentStyle.Background(Color.White));
            PromptPlus.Console.WriteLine("Array[[RED]1][[BLUE]2]");
            Pause();

            PromptPlus.Console.WriteLine("Emoji Icons - 2 equivalent approaches:");
            PromptPlus.Console.WriteLine();
            PromptPlus.Console.WriteLine(":red_heart:  Red Heart");
            PromptPlus.Console.WriteLine($"{(EmojiValue)EmojiName.RedHeart}  Red Heart");

            PromptPlus.Console.WriteLine();
            PromptPlus.Console.WriteLine(":thumbs_up: Thumbs Up");
            PromptPlus.Console.WriteLine($"{(EmojiValue)EmojiName.ThumbsUp} Thumbs Up");

            PromptPlus.Console.WriteLine();
            PromptPlus.Console.WriteLine(":fire: Fire");
            PromptPlus.Console.WriteLine($"{(EmojiValue)EmojiName.Fire} Fire");

            PromptPlus.Console.WriteLine();
            PromptPlus.Console.WriteLine($"Emoji Icons with markup:");
            PromptPlus.Console.WriteLine();
            PromptPlus.Console.WriteLine($"[BLUE]:RabbitFace: Rabbit");
            PromptPlus.Console.WriteLine($"[BLUE]:Tangerine: Tangerine");
            PromptPlus.Console.WriteLine($"[BLUE]:FaxMachine: Fax Machine");
            PromptPlus.Console.WriteLine($"[RED]:xxxx: Invalid Emoji");

            PromptPlus.Console.WriteLine();
            PromptPlus.Console.WriteLine($"Emoji Icons by group (EmojiGroup.Name):");
            PromptPlus.Console.WriteLine();
            PromptPlus.Console.WriteLine($"{EmojiActivities.Balloon} Balloon (Activities)");
            PromptPlus.Console.WriteLine($"{EmojiSymbols.CheckMarkButton} Check Mark (Symbols)");
            PromptPlus.Console.WriteLine($"{EmojiTravelAndPlaces.Rocket} Rocket (Travel & Places)");

            PromptPlus.Console.WriteLine();
            PromptPlus.Console.WriteLine("Emoji Icons by typed name (EmojiName + EmojiValue):");
            PromptPlus.Console.WriteLine();
            PromptPlus.Console.WriteLine($"{(EmojiValue)EmojiName.Balloon} Balloon (Activities)");
            PromptPlus.Console.WriteLine($"{(EmojiValue)EmojiName.CheckMarkButton} Check Mark (Symbols)");
            PromptPlus.Console.WriteLine($"{(EmojiValue)EmojiName.Rocket} Rocket (Travel & Places)");

            PromptPlus.Console.WriteLines(2);

            Pause();

            PromptPlus.Widgets.Dash("03 - Dash styles", Color.Yellow, DashOptions.DoubleBorderUpDown, 1);

            var aux = Enum.GetValues<DashOptions>();
            foreach (var item in aux)
            {
                PromptPlus.Widgets.Dash("Test Dash",Color.Yellow, item, 1);
                PromptPlus.Widgets.Dash("[RGB(255,0,0) ON WHITE]Test[GREEN] COLOR[/] BACK COLOR [/] other text",null, item, 1);
            }

            PromptPlus.Widgets.Dash("04 - Writing to standard error", Color.Yellow, DashOptions.DoubleBorderUpDown, 1);
            using (PromptPlus.Console.OutputError())
            {
                PromptPlus.Console.WriteLine("Test Output Error");
                PromptPlus.Console.WriteLine("[RED]Test Output Error[/]");
            }
            PromptPlus.Console.WriteLine("");

            PromptPlus.Console.WriteLine("");
            PromptPlus.Widgets.Dash($"05 - Overflow: Ellipsis", Color.Yellow, DashOptions.DoubleBorderUpDown, 1);
            PromptPlus.Console.WriteLine("asdajsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj", PromptPlus.Console.CurrentStyle.Overflow(Overflow.Ellipsis));
            PromptPlus.Console.WriteLine("[red]asda[/]jsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj", PromptPlus.Console.CurrentStyle.Overflow(Overflow.Ellipsis));

            PromptPlus.Console.WriteLine("");
            PromptPlus.Widgets.Dash($"06 - Overflow: Crop", Color.Yellow, DashOptions.DoubleBorderUpDown, 1);
            PromptPlus.Console.WriteLine("asdajsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj", PromptPlus.Console.CurrentStyle.Overflow(Overflow.Crop));
            PromptPlus.Console.WriteLine("[red]asda[/]jsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj", PromptPlus.Console.CurrentStyle.Overflow(Overflow.Crop));

            PromptPlus.Console.WriteLine("");
            PromptPlus.Widgets.Dash($"07 - Overflow: Default", Color.Yellow, DashOptions.DoubleBorderUpDown, 1);
            PromptPlus.Console.WriteLine("asdajsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj");
            PromptPlus.Console.WriteLine("[red]asda[/]jsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj");


            Pause();
            PromptPlus.Console.Clear(ConsoleColor.Blue);
            PromptPlus.Widgets.Dash($"08 - Console Information", Color.Yellow, DashOptions.SingleBorder, 1 /*extra lines*/);
            PromptPlus.Console.WriteLine($"Profile Name : {PromptPlus.Console.Profile.ProfileName}");
            PromptPlus.Console.WriteLine($"Current Buffer: {PromptPlus.Console.CurrentBuffer}");
            PromptPlus.Console.WriteLine($"IsTerminal: {PromptPlus.Console.Profile.IsTerminal}");
            PromptPlus.Console.WriteLine($"IsUnicodeSupported: {PromptPlus.Console.Profile.SupportUnicode}");
            PromptPlus.Console.WriteLine($"OutputEncoding: {PromptPlus.Console.OutputEncoding.EncodingName}");
            PromptPlus.Console.WriteLine($"ColorDepth: {PromptPlus.Console.Profile.ColorDepth}");
            PromptPlus.Console.WriteLine($"BackgroundColor: {PromptPlus.Console.BackgroundColor}");
            PromptPlus.Console.WriteLine($"ForegroundColor: {PromptPlus.Console.ForegroundColor}");
            PromptPlus.Console.WriteLine($"SupportsAnsi: {PromptPlus.Console.Profile.SupportsAnsi}");
            PromptPlus.Console.WriteLine($"Buffers(Width/Height): {PromptPlus.Console.Width}/{PromptPlus.Console.Height}");

            Pause();

            PromptPlus.Console.WriteLine("");
            PromptPlus.Widgets.Dash($"09 - Palette: Legacy (0..7)", Color.Yellow, DashOptions.DoubleBorderUpDown, 1);
            PromptPlus.Console.Write('|');
            for (var i = 0; i < 8; i++)
            {
                var backgroundColor = Color.FromInt32(i);
                var foregroundColor = backgroundColor.GetInvertedColor();
                PromptPlus.Console.Write(string.Format(" {0,-9}", i), new Style(foregroundColor, backgroundColor));
                if ((i + 1) % 8 == 0)
                {
                    PromptPlus.Console.WriteLine('|');
                }
            }

            PromptPlus.Config.ContrastRatio = 0;

            if (PromptPlus.Console.ColorDepth >= ColorSystem.FourBit)
            {
                PromptPlus.Console.WriteLine("");
                PromptPlus.Widgets.Dash($"10 - Palette: Standard (0..15)", Color.Yellow, DashOptions.DoubleBorderUpDown, 1);
                PromptPlus.Console.Write("|");
                for (var i = 0; i < 16; i++)
                {
                    var backgroundColor = Color.FromInt32(i);
                    var foregroundColor = backgroundColor.GetInvertedColor();
                    PromptPlus.Console.Write(string.Format(" {0,-9}", i), new Style(foregroundColor, backgroundColor));
                    if ((i + 1) % 8 == 0)
                    {
                        PromptPlus.Console.WriteLine('|');
                        if ((i + 1) % 16 != 0)
                        {
                            PromptPlus.Console.Write('|');
                        }
                    }
                }
                Pause();
            }

            if (PromptPlus.Console.ColorDepth >= ColorSystem.FourBit)
            {
                PromptPlus.Console.WriteLine("");
                PromptPlus.Widgets.Dash($"11 - Weighted CSS Colors", Color.Yellow, DashOptions.DoubleBorderUpDown, 1);
                PromptPlus.Console.WriteLine("[silver]How to use:[/] [aqua]Color.Red.Weighted(500)[/]");
                PromptPlus.Console.WriteLine("");

                int[] sampleWeights = [100, 200, 300, 400, 500, 600, 700, 800, 900];
                Color[] baseColors = [Color.Red, Color.Green, Color.Blue, Color.Teal, Color.Purple, Color.Orange];

                foreach (Color baseColor in baseColors)
                {
                    PromptPlus.Console.Write("| ");
                    foreach (int weight in sampleWeights)
                    {
                        Color weighted = baseColor.Weighted(weight);
                        Color foreground = weighted.GetInvertedColor();
                        PromptPlus.Console.Write($" {weight,3} ", new Style(foreground, weighted));
                    }
                    PromptPlus.Console.WriteLine(" |");
                }

                PromptPlus.Console.WriteLine("");
                PromptPlus.Console.WriteLine($"Example Weighted(844) -> {Color.Blue.Weighted(844)}", new Style(Color.Blue.Weighted(844).GetInvertedColor(), Color.Blue.Weighted(844)));
                Pause();

            }

            PromptPlus.Console.WriteLine("");
            PromptPlus.Widgets.Dash($"11.1 - Color Utility Methods (extras)", Color.Yellow, DashOptions.DoubleBorderUpDown, 1);

            Color fromHex = Color.FromHex("#1E90FF");
            PromptPlus.Console.WriteLine($"FromHex('#1E90FF') -> {fromHex} / ToHex()={fromHex.ToHex()}");

            if (Color.TryFromHex("#FF69B4", out var hotPink))
            {
                PromptPlus.Console.WriteLine($"TryFromHex('#FF69B4') -> {hotPink}", new Style(hotPink.GetInvertedColor(), hotPink));
            }

            Color? fromName = Color.FromName("rebeccapurple");
            if (fromName != null)
            {
                PromptPlus.Console.WriteLine($"FromName('rebeccapurple') -> {fromName.Value} / ToMarkup()={fromName.Value.ToMarkup()}");
            }

            Color blended = Color.Red.Blend(Color.Blue, 0.5f);
            PromptPlus.Console.WriteLine($"Blend(Red, Blue, 0.5) -> {blended}", new Style(blended.GetInvertedColor(), blended));

            double contrast = Color.White.GetContrast(Color.Navy);
            PromptPlus.Console.WriteLine($"GetContrast(White, Navy) -> {contrast:F2}");

            Color fgByLuminance = Color.GetContrastForegroundColor(Color.Gold);
            PromptPlus.Console.WriteLine($"GetContrastForegroundColor(Gold) -> {fgByLuminance}");

            Color adjusted = Color.Yellow.AdjustForegroundColorForContrast(Color.White, 4.5);
            PromptPlus.Console.WriteLine($"AdjustForegroundColorForContrast(Yellow on White, 4.5) -> {adjusted}", new Style(adjusted, Color.White));

            Color closestStandard = fromHex.ExactOrClosest(ColorSystem.FourBit);
            PromptPlus.Console.WriteLine($"ExactOrClosest(Standard) for #1E90FF -> {closestStandard}", new Style(closestStandard.GetInvertedColor(), closestStandard));

            Pause();


            if (PromptPlus.Console.ColorDepth >= ColorSystem.TrueColor)
            {
                PromptPlus.Console.WriteLine("");
                PromptPlus.Widgets.Dash($"12 - TrueColor gradient", Color.Yellow, DashOptions.DoubleBorderUpDown, 1);
                for (var y = 0; y < 15; y++)
                {
                    PromptPlus.Console.Write('|');
                    for (var x = 0; x < 90; x++)
                    {
                        var l = 0.1f + ((y / (float)15) * 0.7f);
                        var h = x / (float)80;
                        var (r1, g1, b1) = ColorFromHSL(h, l, 1.0f);
                        var (r2, g2, b2) = ColorFromHSL(h, l + (0.7f / 10), 1.0f);
                        var background = new Color((byte)(r1 * 255), (byte)(g1 * 255), (byte)(b1 * 255));
                        var foreground = new Color((byte)(r2 * 255), (byte)(g2 * 255), (byte)(b2 * 255));
                        PromptPlus.Console.Write('▄', new Style(foreground, background));
                    }
                    PromptPlus.Console.WriteLine('|');
                }
            }

            Pause("[Yellow]Press any key to end[/]");

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();
        }

        private  static (float, float, float) ColorFromHSL(double h, double l, double s)
        {
            double r = 0, g = 0, b = 0;
            if (l != 0)
            {
                if (s == 0)
                {
                    r = g = b = l;
                }
                else
                {
                    double temp2;
                    if (l < 0.5)
                    {
                        temp2 = l * (1.0 + s);
                    }
                    else
                    {
                        temp2 = l + s - (l * s);
                    }

                    var temp1 = 2.0 * l - temp2;

                    r = GetColorComponent(temp1, temp2, h + 1.0 / 3.0);
                    g = GetColorComponent(temp1, temp2, h);
                    b = GetColorComponent(temp1, temp2, h - 1.0 / 3.0);
                }
            }

            return ((float)r, (float)g, (float)b);

        }

        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            if (temp3 < 0.0)
            {
                temp3 += 1.0;
            }
            else if (temp3 > 1.0)
            {
                temp3 -= 1.0;
            }

            if (temp3 < 1.0 / 6.0)
            {
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            }
            else if (temp3 < 0.5)
            {
                return temp2;
            }
            else if (temp3 < 2.0 / 3.0)
            {
                return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
            }
            else
            {
                return temp1;
            }
        }
    }
}
