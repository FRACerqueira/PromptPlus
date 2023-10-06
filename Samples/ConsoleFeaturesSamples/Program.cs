// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace ConsoleFeaturesSamples
{
    internal class Program
    {
        static void Main()
        {
            PromptPlus.Reset();
            PromptPlus.Clear();

            //global
            PromptPlus.Config.EnabledAbortKey = false;
            PromptPlus.StyleSchema.ApplyStyle(StyleControls.Prompt, new Style(Color.Red));
            //by instance
            PromptPlus.Input("").Config(cfg =>
            {
                cfg
                 .ApplyStyle(StyleControls.Answer, new Style(Color.Yellow))
                 .EnabledAbortKey(true);
            });

            PromptPlus.WriteLine("[RED ON WHITE]Hello[/] [YELLOW]Word[/]");

            PromptPlus.WriteLine("[RGB(255,0,0) ON WHITE]Test[YELLOW] COLOR [/] BACK COLOR [/] other text");

            PromptPlus.AppendText("[RGB(255,0,0) ON WHITE]Test[/]")
                .And(" COLOR ",Color.Yellow)
                .And(" BACK COLOR ",new Style(Color.Red,Color.White))
                .And("other text")
                .WriteLine();

            //standard 
            PromptPlus.DoubleDash($"PromptPlus IgnoreColorTokens = false - Default value and usage" );
            PromptPlus.WriteLine("Valid[[]]formedColor_TokenAny[[RED ON WHITE]]Text[[/]]_[[YELLOW]]Othertext[[/]]");
            PromptPlus.WriteLine("ValidformedColor_TokenAny[RED ON WHITE]Text[/]_[YELLOW]Othertext[/]");

            PromptPlus.DoubleDash($"PromptPlus IgnoreColorTokens = true");
            //global change IgnoreColorTokens
            PromptPlus.IgnoreColorTokens = true;
            //show text with no color!
            PromptPlus.WriteLine("Valid[]formedColor_TokenAny[RED ON WHITE]Text[/]_[YELLOW]Othertext[/]");
            //create context to IgnoreColorTokens  = false
            using (PromptPlus.AcceptColorTokens())
            {
                PromptPlus.DoubleDash($"PromptPlus with context IgnoreColorTokens = false");
                //show text with color!
                PromptPlus.WriteLine("ValidformedColor_TokenAny[RED ON WHITE]Text[/]_[YELLOW]Othertext[/]");
            }

            PromptPlus.IgnoreColorTokens = false;
            //create context to IgnoreColorTokens  = true
            using (PromptPlus.EscapeColorTokens())
            {
                PromptPlus.DoubleDash($"PromptPlus with context IgnoreColorTokens = true");
                //show text with color
                PromptPlus.WriteLine("ValidformedColor_TokenAny[RED ON WHITE]Text[/]_[YELLOW]Othertext[/]");
            }
            //restore original value IgnoreColorTokens 

            //write to standard error output stream for any output included within 'using'
            using (PromptPlus.OutputError())
            {
                PromptPlus.WriteLine("Test Output Error");
            }

            PromptPlus.WriteLine("[RGB(255,0,0) ON WHITE]Test[YELLOW] COLOR [/] BACK COLOR [/] other text");
            PromptPlus.WriteLine("[#ff0000 ON WHITE]Test [YELLOW] COLOR [/] BACK COLOR [/] other text");
            PromptPlus.WriteLine("[RED ON WHITE]Test[YELLOW] COLOR [/] BACK COLOR [/] other text");

            PromptPlus.WriteLine("Test", new Style(ConsoleColor.Red, ConsoleColor.White, Overflow.None));
            PromptPlus.WriteLine("Test", new Style(Color.White, Color.Red, Overflow.None));
            PromptPlus.WriteLine("Test", new Style(new Color(255, 255, 255), Color.Red, Overflow.None));
            PromptPlus.WriteLine("Test", new Style(Color.FromConsoleColor(ConsoleColor.White), Color.Red, Overflow.None));
            PromptPlus.WriteLine("Test", new Style(Color.FromInt32(255), Color.Red, Overflow.None));
            PromptPlus.WriteLine("Test", new Style(Color.FromHtml("#ffffff"), Color.Red, Overflow.None));

            PromptPlus.DoubleDash($"PromptPlus Style.OverflowEllipsis");
            PromptPlus.WriteLine("[RED ON WHITE]TESTE[YELLOW] COLOR [/] BACK COLOR [/]" + "asdajsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj", style: Style.OverflowEllipsis);

            PromptPlus.DoubleDash($"PromptPlus Style.OverflowCrop");
            PromptPlus.WriteLine("[RED ON WHITE]TESTE[YELLOW] COLOR [/] BACK COLOR [/]" + "asdajsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj", style: Style.OverflowCrop);

            PromptPlus.DoubleDash($"PromptPlus default");
            PromptPlus.WriteLine("[RED ON WHITE]TESTE[YELLOW] COLOR [/] BACK COLOR [/]" + "asdajsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj");

            PromptPlus
                .KeyPress()
                .Config(cfg => cfg.HideAfterFinish(true))
                .Run();

            PromptPlus.Setup((cfg) =>
            {
                cfg.PadLeft = 2;
                cfg.PadRight = 2;
            });

            PromptPlus.BackgroundColor = ConsoleColor.Blue;
            PromptPlus.Clear();

            PromptPlus.SingleDash($"[yellow]Console Information[/]", DashOptions.DoubleBorder, 1 /*extra lines*/);
            PromptPlus.WriteLine($"Current Buffer: {PromptPlus.CurrentTargetBuffer}");
            PromptPlus.WriteLine($"IsTerminal: {PromptPlus.IsTerminal}");
            PromptPlus.WriteLine($"CodePage : {PromptPlus.CodePage}");
            PromptPlus.WriteLine($"IsUnicodeSupported: {PromptPlus.IsUnicodeSupported}");
            PromptPlus.WriteLine($"OutputEncoding: {PromptPlus.OutputEncoding.EncodingName}");
            PromptPlus.WriteLine($"ColorDepth: {PromptPlus.ColorDepth}");
            PromptPlus.WriteLine($"BackgroundColor: {PromptPlus.BackgroundColor}");
            PromptPlus.WriteLine($"ForegroundColor: {PromptPlus.ForegroundColor}");
            PromptPlus.WriteLine($"SupportsAnsi: {PromptPlus.SupportsAnsi}");
            PromptPlus.WriteLine($"Buffers(Width/Height): {PromptPlus.BufferWidth}/{PromptPlus.BufferHeight}");
            PromptPlus.WriteLine($"PadScreen(Left/Right): {PromptPlus.PadLeft}/{PromptPlus.PadRight}\n");

            PromptPlus
                .KeyPress()
                .Config(cfg =>
                {
                    cfg.HideAfterFinish(true)
                      .ShowTooltip(false)
                      .ApplyStyle(StyleControls.Tooltips, Style.Default.Foreground(Color.Grey100));
                })
                .Spinner(SpinnersType.Balloon)
                .Run();

            PromptPlus.Clear();


            PromptPlus.DoubleDash($"[yellow]Sample Colors capacities [/]", DashOptions.HeavyBorder, 1);
            PromptPlus.Write("|");
            for (var i = 0; i < 8; i++)
            {
                var backgroundColor = Color.FromInt32(i);
                var foregroundColor = backgroundColor.GetInvertedColor();
                PromptPlus.Write(string.Format(" {0,-9}", i), new Style(foregroundColor, backgroundColor));
                if ((i + 1) % 8 == 0)
                {
                    PromptPlus.WriteLine("|");
                }
            }

            if (PromptPlus.ColorDepth >= ColorSystem.Standard)
            {
                PromptPlus.WriteLine();
                PromptPlus.DoubleDash($"[yellow]Sample Colors capacities Standard[/]", DashOptions.HeavyBorder, 1);
                PromptPlus.Write("|");
                for (var i = 0; i < 16; i++)
                {
                    var backgroundColor = Color.FromInt32(i);
                    var foregroundColor = backgroundColor.GetInvertedColor();
                    PromptPlus.Write(string.Format(" {0,-9}", i), new Style(foregroundColor, backgroundColor));
                    if ((i + 1) % 8 == 0)
                    {
                        PromptPlus.WriteLine("|");
                        if ((i + 1) % 16 != 0)
                        {
                            PromptPlus.Write("|");
                        }
                    }
                }
            }

            if (PromptPlus.ColorDepth >= ColorSystem.EightBit)
            {
                PromptPlus.WriteLine();
                PromptPlus.DoubleDash($"[yellow]Sample Colors capacities EightBit[/]", DashOptions.HeavyBorder, 1);
                for (var i = 0; i < 16; i++)
                {
                    PromptPlus.Write("|");
                    for (var j = 0; j < 16; j++)
                    {
                        var number = i * 16 + j;
                        var backgroundColor = Color.FromInt32(number);
                        var foregroundColor = backgroundColor.GetInvertedColor();
                        PromptPlus.Write(string.Format(" {0,-4}", number), new Style(foregroundColor, backgroundColor));
                    }
                    PromptPlus.WriteLine("|");
                }
            }

            if (PromptPlus.ColorDepth >= ColorSystem.TrueColor)
            {
                PromptPlus.WriteLine();
                PromptPlus.DoubleDash($"[yellow]Sample Colors capacities TrueColor[/]", DashOptions.HeavyBorder, 1);
                for (var y = 0; y < 15; y++)
                {
                    PromptPlus.Write("|");
                    for (var x = 0; x < 90; x++)
                    {
                        var l = 0.1f + ((y / (float)15) * 0.7f);
                        var h = x / (float)80;
                        var (r1, g1, b1) = ColorFromHSL(h, l, 1.0f);
                        var (r2, g2, b2) = ColorFromHSL(h, l + (0.7f / 10), 1.0f);
                        var background = new Color((byte)(r1 * 255), (byte)(g1 * 255), (byte)(b1 * 255));
                        var foreground = new Color((byte)(r2 * 255), (byte)(g2 * 255), (byte)(b2 * 255));
                        PromptPlus.Write("▄", new Style(foreground, background));
                    }
                    PromptPlus.WriteLine("|");
                }
            }
            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg =>
            {
                cfg.ShowTooltip(false);
                cfg.ApplyStyle(StyleControls.Tooltips, Style.Default.Foreground(Style.Default.Background.GetInvertedColor()));
            })
                .Run();


            static (float, float, float) ColorFromHSL(double h, double l, double s)
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
            static double GetColorComponent(double temp1, double temp2, double temp3)
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
}
