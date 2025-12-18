// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary;

namespace ConsoleFeaturesSamples
{
    internal class Program
    {
        static void Main()
        {
            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash($"Create file: '{PromptPlus.NameResourceConfigFile}' at current BaseDirectory with config for all controls", extraLines: 1);
            PromptPlus.CreatePromptPlusConfigFile(AppDomain.CurrentDomain.BaseDirectory);
            PromptPlus.Console.WriteLine($"File create :{File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PromptPlus.NameResourceConfigFile))}, ");
            PromptPlus.Console.WriteLine($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PromptPlus.NameResourceConfigFile)}");

            PromptPlus.Widgets.DoubleDash("Sample WriteLine/WriteLineColor", extraLines: 1);
            PromptPlus.Console.WriteLine("[RGB(255,0,0) ON WHITE]Test[GREEN] COLOR[/] BACK COLOR [/] other text");
            PromptPlus.Console.WriteLine("[RGB(255,0,0):WHITE]Test[GREEN] COLOR[/] BACK COLOR [/] other text");
            PromptPlus.Console.WriteLine("[RED:WHITE]Test[bLUE] COLOR[/] BACK COLOR[/] other text");
            PromptPlus.Console.WriteLine("[RED:WHITE]Test[bLUE] COLOR[/] BACK COLOR[/] other text");

            PromptPlus.Console.WriteLines(2);

            PromptPlus.Widgets.DoubleDash("Sample WriteColor/WriteLineColor with mixed use cases", extraLines: 1);
            PromptPlus.Console.WriteLine("[RED]ERROR:[/] Wrong error at (/x/g/[[My Folder Name Has Brackets]]/[[BracketFile]].xml)");
            PromptPlus.Console.WriteLine("[RED]ERROR:[/] Wrong error at (/x/g/[My Folder Name Has Brackets]/[BracketFile].xml)");
            PromptPlus.Console.WriteLine("[RED].xml");
            PromptPlus.Console.WriteLine("[RED:WHITE]Test[/][bLUE] misisng token");
            PromptPlus.Console.WriteLine("Test[/] misisng token");
            PromptPlus.Console.WriteLine("[[RED]]Test escapetoken", Style.Default().ForeGround(Color.Aqua));
            PromptPlus.Console.WriteLine("[RED]Test[/] with Style", Style.Default().ForeGround(Color.Yellow));


            PromptPlus.Console.WriteLine("[RED].xml",new Style(Color.Red,Color.White));

            PromptPlus.Console.WriteLines(2);
            PromptPlus.Widgets.DoubleDash("Sample SingleDash/SingleDashColor", extraLines: 1);

            var aux = Enum.GetValues<DashOptions>();
            foreach (var item in aux)
            {
                PromptPlus.Widgets.SingleDash("Test SingleDash", item, 1, Style.Default().ForeGround(ConsoleColor.Red).Background(ConsoleColor.Yellow));
                PromptPlus.Widgets.SingleDash("[RGB(255,0,0) ON WHITE]Test[GREEN] COLOR[/] BACK COLOR [/] other text", item, 1);
            }
            PromptPlus.Widgets.DoubleDash("Sample DoubleDashColor", extraLines: 1);
            foreach (var item in aux)
            {
                PromptPlus.Widgets.DoubleDash("Test DoubleDash", item, 1, Style.Default().ForeGround(ConsoleColor.Red).Background(ConsoleColor.Yellow));
                PromptPlus.Widgets.DoubleDash("[RGB(255,0,0) ON WHITE]Test[GREEN] COLOR[/] BACK COLOR [/] other text", item, 1);
            }

            PromptPlus.Widgets.DoubleDash("Sample write to standard error output");
            using (PromptPlus.Console.OutputError())
            {
                PromptPlus.Console.WriteLine("Test Output Error");
                PromptPlus.Console.WriteLine("[RED]Test Output Error[/]");
            }
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample write with Join");
            PromptPlus.Console.Join()
                 .WriteLine("[RGB(255,0,0) ON WHITE]Test[/]")
                 .Write("Test COLOR", Style.Default().ForeGround(Color.Yellow))
                 .Write(" ")
                 .Write("BACK COLOR", new Style(Color.Red, Color.White))
                 .WriteLine(" other text")
                 .Done();

            PromptPlus.Console.WriteLine("");
            PromptPlus.Widgets.DoubleDash($"PromptPlus Style.OverflowEllipsis");
            PromptPlus.Console.WriteLine("asdajsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj", Style.Default().Overflow(Overflow.Ellipsis));
            PromptPlus.Console.WriteLine("[red]asda[/]jsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj", Style.Default().Overflow(Overflow.Ellipsis));

            PromptPlus.Console.WriteLine("");
            PromptPlus.Widgets.DoubleDash($"PromptPlus Style.OverflowCrop");
            PromptPlus.Console.WriteLine("asdajsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj", Style.Default().Overflow(Overflow.Crop));
            PromptPlus.Console.WriteLine("[red]asda[/]jsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj", Style.Default().Overflow(Overflow.Crop));

            PromptPlus.Console.WriteLine("");
            PromptPlus.Widgets.DoubleDash($"PromptPlus default");
            PromptPlus.Console.WriteLine("asdajsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj");
            PromptPlus.Console.WriteLine("[red]asda[/]jsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj");


            PromptPlus.Console.WriteLine("");
            PromptPlus.Console.Write("[Yellow]Press any key to continue[/]");
            PromptPlus.Console.ReadKey();

            PromptPlus.ProfileConfig("Myprofile",(cfg) =>
            {
                cfg.DefaultConsoleBackgroundColor = ConsoleColor.Blue;
                cfg.PadLeft = 2;
                cfg.PadRight = 2;
            });
            PromptPlus.Console.Clear();
            PromptPlus.Widgets.SingleDash($"[yellow]Console Information[/]", DashOptions.DoubleBorder, 1 /*extra lines*/);
            PromptPlus.Console.WriteLine($"Profile Name : {PromptPlus.Console.ProfileName}");
            PromptPlus.Console.WriteLine($"Current Buffer: {PromptPlus.Console.CurrentBuffer}");
            PromptPlus.Console.WriteLine($"IsTerminal: {PromptPlus.Console.IsTerminal}");
            PromptPlus.Console.WriteLine($"IsUnicodeSupported: {PromptPlus.Console.IsUnicodeSupported}");
            PromptPlus.Console.WriteLine($"OutputEncoding: {PromptPlus.Console.OutputEncoding.EncodingName}");
            PromptPlus.Console.WriteLine($"ColorDepth: {PromptPlus.Console.ColorDepth}");
            PromptPlus.Console.WriteLine($"BackgroundColor: {PromptPlus.Console.BackgroundColor}");
            PromptPlus.Console.WriteLine($"ForegroundColor: {PromptPlus.Console.ForegroundColor}");
            PromptPlus.Console.WriteLine($"SupportsAnsi: {PromptPlus.Console.SupportsAnsi}");
            PromptPlus.Console.WriteLine($"Buffers(Width/Height): {PromptPlus.Console.BufferWidth}/{PromptPlus.Console.BufferHeight}");
            PromptPlus.Console.WriteLine($"PadScreen(Left/Right): {PromptPlus.Console.PadLeft}/{PromptPlus.Console.PadRight}\n");

            PromptPlus.Console.WriteLine("");
            PromptPlus.Console.Write("[Yellow]Press any key to continue[/]");
            PromptPlus.Console.ReadKey();

            PromptPlus.Console.WriteLine("");
            PromptPlus.Widgets.DoubleDash($"[yellow]Sample Colors capacities [/]", DashOptions.DoubleBorder, 1);
            PromptPlus.Console.Write("|");
            for (var i = 0; i < 8; i++)
            {
                var backgroundColor = Color.FromInt32(i);
                var foregroundColor = backgroundColor.GetInvertedColor();
                PromptPlus.Console.Write(string.Format(" {0,-9}", i), new Style(foregroundColor, backgroundColor));
                if ((i + 1) % 8 == 0)
                {
                    PromptPlus.Console.WriteLine("|");
                }
            }


            if (PromptPlus.Console.ColorDepth >= ColorSystem.Standard)
            {
                PromptPlus.Console.WriteLine("");
                PromptPlus.Widgets.DoubleDash($"[yellow]Sample Colors capacities Standard[/]", DashOptions.DoubleBorder, 1);
                PromptPlus.Console.Write("|");
                for (var i = 0; i < 16; i++)
                {
                    var backgroundColor = Color.FromInt32(i);
                    var foregroundColor = backgroundColor.GetInvertedColor();
                    PromptPlus.Console.Write(string.Format(" {0,-9}", i), new Style(foregroundColor, backgroundColor));
                    if ((i + 1) % 8 == 0)
                    {
                        PromptPlus.Console.WriteLine("|");
                        if ((i + 1) % 16 != 0)
                        {
                            PromptPlus.Console.Write("|");
                        }
                    }
                }
            }

            if (PromptPlus.Console.ColorDepth >= ColorSystem.TrueColor)
            {
                PromptPlus.Console.WriteLine("");
                PromptPlus.Widgets.DoubleDash($"[yellow]Sample Colors capacities TrueColor[/]", DashOptions.DoubleBorder, 1);
                for (var y = 0; y < 15; y++)
                {
                    PromptPlus.Console.Write("|");
                    for (var x = 0; x < 90; x++)
                    {
                        var l = 0.1f + ((y / (float)15) * 0.7f);
                        var h = x / (float)80;
                        var (r1, g1, b1) = ColorFromHSL(h, l, 1.0f);
                        var (r2, g2, b2) = ColorFromHSL(h, l + (0.7f / 10), 1.0f);
                        var background = new Color((byte)(r1 * 255), (byte)(g1 * 255), (byte)(b1 * 255));
                        var foreground = new Color((byte)(r2 * 255), (byte)(g2 * 255), (byte)(b2 * 255));
                        PromptPlus.Console.Write("▄", new Style(foreground, background));
                    }
                    PromptPlus.Console.WriteLine("|");
                }
            }

            PromptPlus.Console.WriteLine("");
            PromptPlus.Console.Write("[Yellow]Press any key to end[/]");
            PromptPlus.Console.ReadKey();

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
