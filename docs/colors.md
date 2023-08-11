# <img align="left" width="100" height="100" src="./images/icon.png">PromptPlus Colors

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Main**](index.md#table-of-contents)  
The color class has implicit conversion to ConsoleColor. There are conversion methods to facilitate compatibility with other common color representations:

- Html
	-	Converts string color Html format (#RRGGBB).
- Int32
	-	Converts a standard color number (0 ~ 255).
- Console Color
	-	Converts a System.Console.ConsoleColor
- RGB
	-	Converts a RGB format (R,G,B)
- Name
	-	Converts a name standard color

### Samples
```csharp
PromptPlus.WriteLine("Test", new Style(Color.White, Color.Red, Overflow.None));
PromptPlus.WriteLine("Test", new Style(new Color(255, 255, 255), Color.Red, Overflow.None));
PromptPlus.WriteLine("Test", new Style(Color.FromConsoleColor(ConsoleColor.White), Color.Red, Overflow.None));
PromptPlus.WriteLine("Test", new Style(Color.FromInt32(255), Color.Red, Overflow.None));
```

## Table of Contents

- [Standard colors](#standard-colors)
- [Color API Reference](./apis/pplus.color.md)

## Standard Colors
[**Top**](#promptplus-colors)

Color | # | Name | RGB | HEX | ConsoleColor
| :---: | :---: | :--- | :--- | :---: | :--- |
![](https://placehold.co/15x15/000000/000000.png) | 0  | black | RGB(0,0,0) |#00000  |Black
![](https://placehold.co/15x15/800000/800000.png) | 1  | maroon | RGB(128,0,0) |#80000  |DarkRed
![](https://placehold.co/15x15/008000/008000.png) | 2  | green  | RGB(0,128,0) |#80000  |DarkGreen
![](https://placehold.co/15x15/808000/808000.png) | 3  | olive  | RGB(128,128,0) |#808000  |DarkYellow
![](https://placehold.co/15x15/000080/000080.png) | 4  | navy | RGB(0,0,128) |#000080  |DarkBlue
![](https://placehold.co/15x15/800080/800080.png) | 5  | purple | RGB(128,0,128) |#800080  |DarkMagenta
![](https://placehold.co/15x15/008080/008080.png) | 6  | teal | RGB(0,128,128) | #008080 |DarkCyan
![](https://placehold.co/15x15/c0c0c0/c0c0c0.png) | 7  | silver | RGB(192,192,192) |#c0c0c0 |Gray
![](https://placehold.co/15x15/808080/808080.png) | 8  | grey | RGB(128,128,128) |#c0c0c0 |DarkGray
![](https://placehold.co/15x15/ff0000/ff0000.png) | 9  | red | RGB(255,0,0) | #ff0000 |Red
![](https://placehold.co/15x15/00ff00/00ff00.png) | 10 | lime   | RGB(0,255,0) | #00ff00 |Green
![](https://placehold.co/15x15/ffff00/ffff00.png) | 11 | yellow | RGB(255,255,0) | #ffff00  |Yellow
![](https://placehold.co/15x15/0000ff/0000ff.png) | 12 | blue | RGB(0,0,255) | #0000ff  |Blue
![](https://placehold.co/15x15/ff00ff/ff00ff.png) | 13 | fuchsia | RGB(255,0,255) | #ff00ff  |Magenta
![](https://placehold.co/15x15/00ffff/00ffff.png) | 14 | aqua | RGB(0,255,255) | #00ffff  |Cyan
![](https://placehold.co/15x15/ffffff/ffffff.png) | 15 | white | RGB(255,255,255) | #ffffff  |White
![](https://placehold.co/15x15/000000/000000.png) | 16 | grey0 | RGB(0,0,0) | #000000 | - - - |
![](https://placehold.co/15x15/00005f/00005f.png) | 17 | navyblue | RGB(0,0,95) | #00005f  | - - - |
![](https://placehold.co/15x15/000087/000087.png) | 18 | darkblue | RGB(0,0,135) | #000087  | - - - |
![](https://placehold.co/15x15/0000af/0000af.png) | 19 | blue3 | RGB(0,0,175) | #0000af  | - - - |
![](https://placehold.co/15x15/0000d7/0000d7.png) | 20 | blue3_1 | RGB(0,0,215) | #0000d7  | - - - |
![](https://placehold.co/15x15/0000ff/0000ff.png) | 21 | blue1 | RGB(0,0,255) | #0000ff  | - - - |
![](https://placehold.co/15x15/005f00/005f00.png) | 22 | darkgreen | RGB(0,95,0) | #005f00  | - - - |
![](https://placehold.co/15x15/005f5f/005f5f.png) | 23 | deepskyblue4 | RGB(0,95,95) | #005f5f  | - - - |
![](https://placehold.co/15x15/005f87/005f87.png) | 24 | deepskyblue4_1 | RGB(0,95,135) | #005f87  | - - - |
![](https://placehold.co/15x15/005faf/005faf.png) | 25 | deepskyblue4_2 | RGB(0,95,175) | #005faf  | - - - |
![](https://placehold.co/15x15/005fd7/005fd7.png) | 26 | dodgerblue3 | RGB(0,95,215) | #005fd7  | - - - |
![](https://placehold.co/15x15/005fff/005fff.png) | 27 | dodgerblue2 | RGB(0,95,255) | #005fff  | - - - |
![](https://placehold.co/15x15/008700/008700.png) | 28 | green4 | RGB(0,135,0) | #008700  | - - - |
![](https://placehold.co/15x15/00875f/00875f.png) | 29 | springgreen4 | RGB(0,135,95) | #00875f  | - - - |
![](https://placehold.co/15x15/008787/008787.png) | 30 | turquoise4 | RGB(0,135,135) | #008787  | - - - |
![](https://placehold.co/15x15/0087af/0087af.png) | 31 | deepskyblue3 | RGB(0,135,175) | #0087af  | - - - |
![](https://placehold.co/15x15/0087d7/0087d7.png) | 32 | deepskyblue3_1 | RGB(0,135,215) | #0087d7  | - - - |
![](https://placehold.co/15x15/0087ff/0087ff.png) | 33 | dodgerblue1 | RGB(0,135,255) | #0087ff  | - - - |
![](https://placehold.co/15x15/00af00/00af00.png) | 34 | green3 | RGB(0,175,0) | #00af00  | - - - |
![](https://placehold.co/15x15/00af5f/00af5f.png) | 35 | springgreen3 | RGB(0,175,95) | #00af5f  | - - - |
![](https://placehold.co/15x15/00af87/00af87.png) | 36 | darkcyan | RGB(0,175,135) | #00af87  | - - - |
![](https://placehold.co/15x15/00afaf/00afaf.png) | 37 | lightseagreen | RGB(0,175,175) | #00afaf  | - - - |
![](https://placehold.co/15x15/00afd7/00afd7.png) | 38 | deepskyblue2 | RGB(0,175,215) | #00afd7  | - - - |
![](https://placehold.co/15x15/00afff/00afff.png) | 39 | deepskyblue1 | RGB(0,175,255) | #00afff  | - - - |
![](https://placehold.co/15x15/00d700/00d700.png) | 40 | green3_1 | RGB(0,215,0) | #00d700  | - - - |
![](https://placehold.co/15x15/00d75f/00d75f.png) | 41 | springgreen3_1 | RGB(0,215,95) | #00d75f  | - - - |
![](https://placehold.co/15x15/00d787/00d787.png) | 42 | springgreen2 | RGB(0,215,135) | #00d787  | - - - |
![](https://placehold.co/15x15/00d7af/00d7af.png) | 43 | cyan3 | RGB(0,215,175) | #00d7af  | - - - |
![](https://placehold.co/15x15/00d7d7/00d7d7.png) | 44 | darkturquoise | RGB(0,215,215) | #00d7d7  | - - - |
![](https://placehold.co/15x15/00d7ff/00d7ff.png) | 45 | turquoise2 | RGB(0,215,255) | #00d7ff  | - - - |
![](https://placehold.co/15x15/00ff00/00ff00.png) | 46 | green1 | RGB(0,255,0) | #00ff00  | - - - |
![](https://placehold.co/15x15/00ff5f/00ff5f.png) | 47 | springgreen2_1 | RGB(0,255,95) | #00ff5f  | - - - |
![](https://placehold.co/15x15/00ff87/00ff87.png) | 48 | springgreen1 | RGB(0,255,135) | #00ff87  | - - - |
![](https://placehold.co/15x15/00ffaf/00ffaf.png) | 49 | mediumspringgreen | RGB(0,255,175) | #00ffaf  | - - - |
![](https://placehold.co/15x15/00ffd7/00ffd7.png) | 50 | cyan2 | RGB(0,255,215) | #00ffd7  | - - - |
![](https://placehold.co/15x15/00ffff/00ffff.png) | 51 | cyan1 | RGB(0,255,255) | #00ffff  | - - - |
![](https://placehold.co/15x15/5f0000/5f0000.png) | 52 | darkred | RGB(95,0,0) | #5f0000  | - - - |
![](https://placehold.co/15x15/5f005f/5f005f.png) | 53 | deeppink4 | RGB(95,0,95) | #5f005f  | - - - |
![](https://placehold.co/15x15/5f0087/5f0087.png) | 54 | purple4 | RGB(95,0,135) | #5f0087  | - - - |
![](https://placehold.co/15x15/5f00af/5f00af.png) | 55 | purple4_1 | RGB(95,0,175) | #5f00af  | - - - |
![](https://placehold.co/15x15/5f00d7/5f00d7.png) | 56 | purple3 | RGB(95,0,215) | #5f00d7  | - - - |
![](https://placehold.co/15x15/5f00ff/5f00ff.png) | 57 | blueviolet | RGB(95,0,255) | #5f00ff  | - - - |
![](https://placehold.co/15x15/5f5f00/5f5f00.png) | 58 | orange4 | RGB(95,95,0) | #5f5f00  | - - - |
![](https://placehold.co/15x15/5f5f5f/5f5f5f.png) | 59 | grey37 | RGB(95,95,95) | #5f5f5f  | - - - |
![](https://placehold.co/15x15/5f5f87/5f5f87.png) | 60 | mediumpurple4 | RGB(95,95,135) | #5f5f87  | - - - |
![](https://placehold.co/15x15/5f5faf/5f5faf.png) | 61 | slateblue3 | RGB(95,95,175) | #5f5faf  | - - - |
![](https://placehold.co/15x15/5f5fd7/5f5fd7.png) | 62 | slateblue3_1 | RGB(95,95,215) | #5f5fd7  | - - - |
![](https://placehold.co/15x15/5f5fff/5f5fff.png) | 63 | royalblue1 | RGB(95,95,255) | #5f5fff  | - - - |
![](https://placehold.co/15x15/5f8700/5f8700.png) | 64 | chartreuse4 | RGB(95,135,0) | #5f8700  | - - - |
![](https://placehold.co/15x15/5f875f/5f875f.png) | 65 | darkseagreen4 | RGB(95,135,95) | #5f875f  | - - - |
![](https://placehold.co/15x15/5f8787/5f8787.png) | 66 | paleturquoise4 | RGB(95,135,135) | #5f8787  | - - - |
![](https://placehold.co/15x15/5f87af/5f87af.png) | 67 | steelblue | RGB(95,135,175) | #5f87af  | - - - |
![](https://placehold.co/15x15/5f87d7/5f87d7.png) | 68 | steelblue3 | RGB(95,135,215) | #5f87d7  | - - - |
![](https://placehold.co/15x15/5f87ff/5f87ff.png) | 69 | cornflowerblue | RGB(95,135,255) | #5f87ff  | - - - |
![](https://placehold.co/15x15/5faf00/5faf00.png) | 70 | chartreuse3 | RGB(95,175,0) | #5faf00  | - - - |
![](https://placehold.co/15x15/5faf5f/5faf5f.png) | 71 | darkseagreen4_1 | RGB(95,175,95) | #5faf5f  | - - - |
![](https://placehold.co/15x15/5faf87/5faf87.png) | 72 | cadetblue | RGB(95,175,135) | #5faf87  | - - - |
![](https://placehold.co/15x15/5fafaf/5fafaf.png) | 73 | cadetblue_1 | RGB(95,175,175) | #5fafaf  | - - - |
![](https://placehold.co/15x15/5fafd7/5fafd7.png) | 74 | skyblue3 | RGB(95,175,215) | #5fafd7  | - - - |
![](https://placehold.co/15x15/5fafff/5fafff.png) | 75 | steelblue1 | RGB(95,175,255) | #5fafff  | - - - |
![](https://placehold.co/15x15/5fd700/5fd700.png) | 76 | chartreuse3_1 | RGB(95,215,0) | #5fd700  | - - - |
![](https://placehold.co/15x15/5fd75f/5fd75f.png) | 77 | palegreen3 | RGB(95,215,95) | #5fd75f  | - - - |
![](https://placehold.co/15x15/5fd787/5fd787.png) | 78 | seagreen3 | RGB(95,215,135) | #5fd787  | - - - |
![](https://placehold.co/15x15/5fd7af/5fd7af.png) | 79 | aquamarine3 | RGB(95,215,175) | #5fd7af  | - - - |
![](https://placehold.co/15x15/5fd7d7/5fd7d7.png) | 80 | mediumturquoise | RGB(95,215,215) | #5fd7d7  | - - - |
![](https://placehold.co/15x15/5fd7ff/5fd7ff.png) | 81 | steelblue1_1 | RGB(95,215,255) | #5fd7ff  | - - - |
![](https://placehold.co/15x15/5fff00/5fff00.png) | 82 | chartreuse2 | RGB(95,255,0) | #5fff00  | - - - |
![](https://placehold.co/15x15/5fff5f/5fff5f.png) | 83 | seagreen2 | RGB(95,255,95) | #5fff5f  | - - - |
![](https://placehold.co/15x15/5fff87/5fff87.png) | 84 | seagreen1 | RGB(95,255,135) | #5fff87  | - - - |
![](https://placehold.co/15x15/5fffaf/5fffaf.png) | 85 | seagreen1_1 | RGB(95,255,175) | #5fffaf  | - - - |
![](https://placehold.co/15x15/5fffd7/5fffd7.png) | 86 | aquamarine1 | RGB(95,255,215) | #5fffd7  | - - - |
![](https://placehold.co/15x15/5fffff/5fffff.png) | 87 | darkslategray2 | RGB(95,255,255) | #5fffff  | - - - |
![](https://placehold.co/15x15/870000/870000.png) | 88 | darkred_1 | RGB(135,0,0) | #870000  | - - - |
![](https://placehold.co/15x15/87005f/87005f.png) | 89 | deeppink4_1 | RGB(135,0,95) | #87005f  | - - - |
![](https://placehold.co/15x15/870087/870087.png) | 90 | darkmagenta | RGB(135,0,135) | #870087  | - - - |
![](https://placehold.co/15x15/8700af/8700af.png) | 91 | darkmagenta_1 | RGB(135,0,175) | #8700af  | - - - |
![](https://placehold.co/15x15/8700d7/8700d7.png) | 92 | darkviolet | RGB(135,0,215) | #8700d7  | - - - |
![](https://placehold.co/15x15/8700ff/8700ff.png) | 93 | purple_1 | RGB(135,0,255) | #8700ff  | - - - |
![](https://placehold.co/15x15/875f00/875f00.png) | 94 | orange4_1 | RGB(135,95,0) | #875f00  | - - - |
![](https://placehold.co/15x15/875f5f/875f5f.png) | 95 | lightpink4 | RGB(135,95,95) | #875f5f  | - - - |
![](https://placehold.co/15x15/875f87/875f87.png) | 96 | plum4 | RGB(135,95,135) | #875f87  | - - - |
![](https://placehold.co/15x15/875faf/875faf.png) | 97 | mediumpurple3 | RGB(135,95,175) | #875faf  | - - - |
![](https://placehold.co/15x15/875fd7/875fd7.png) | 98 | mediumpurple3_1 | RGB(135,95,215) | #875fd7  | - - - |
![](https://placehold.co/15x15/875fff/875fff.png) | 99 | slateblue1 | RGB(135,95,255) | #875fff  | - - - |
![](https://placehold.co/15x15/878700/878700.png) | 100 | yellow4 | RGB(135,135,0) | #878700  | - - - |
![](https://placehold.co/15x15/87875f/87875f.png) | 101 | wheat4 | RGB(135,135,95) | #87875f  | - - - |
![](https://placehold.co/15x15/878787/878787.png) | 102 | grey53 | RGB(135,135,135) | #878787  | - - - |
![](https://placehold.co/15x15/8787af/8787af.png) | 103 | lightslategrey | RGB(135,135,175) | #8787af  | - - - |
![](https://placehold.co/15x15/8787d7/8787d7.png) | 104 | mediumpurple | RGB(135,135,215) | #8787d7  | - - - |
![](https://placehold.co/15x15/8787ff/8787ff.png) | 105 | lightslateblue | RGB(135,135,255) | #8787ff  | - - - |
![](https://placehold.co/15x15/87af00/87af00.png) | 106 | yellow4_1 | RGB(135,175,0) | #87af00  | - - - |
![](https://placehold.co/15x15/87af5f/87af5f.png) | 107 | darkolivegreen3 | RGB(135,175,95) | #87af5f  | - - - |
![](https://placehold.co/15x15/87af87/87af87.png) | 108 | darkseagreen | RGB(135,175,135) | #87af87  | - - - |
![](https://placehold.co/15x15/87afaf/87afaf.png) | 109 | lightskyblue3 | RGB(135,175,175) | #87afaf  | - - - |
![](https://placehold.co/15x15/87afd7/87afd7.png) | 110 | lightskyblue3_1 | RGB(135,175,215) | #87afd7  | - - - |
![](https://placehold.co/15x15/87afff/87afff.png) | 111 | skyblue2 | RGB(135,175,255) | #87afff  | - - - |
![](https://placehold.co/15x15/87d700/87d700.png) | 112 | chartreuse2_1 | RGB(135,215,0) | #87d700  | - - - |
![](https://placehold.co/15x15/87d75f/87d75f.png) | 113 | darkolivegreen3_1 | RGB(135,215,95) | #87d75f  | - - - |
![](https://placehold.co/15x15/87d787/87d787.png) | 114 | palegreen3_1 | RGB(135,215,135) | #87d787  | - - - |
![](https://placehold.co/15x15/87d7af/87d7af.png) | 115 | darkseagreen3 | RGB(135,215,175) | #87d7af  | - - - |
![](https://placehold.co/15x15/87d7d7/87d7d7.png) | 116 | darkslategray3 | RGB(135,215,215) | #87d7d7  | - - - |
![](https://placehold.co/15x15/87d7ff/87d7ff.png) | 117 | skyblue1 | RGB(135,215,255) | #87d7ff  | - - - |
![](https://placehold.co/15x15/87ff00/87ff00.png) | 118 | chartreuse1 | RGB(135,255,0) | #87ff00  | - - - |
![](https://placehold.co/15x15/87ff5f/87ff5f.png) | 119 | lightgreen | RGB(135,255,95) | #87ff5f  | - - - |
![](https://placehold.co/15x15/87ff87/87ff87.png) | 120 | lightgreen_1 | RGB(135,255,135) | #87ff87  | - - - |
![](https://placehold.co/15x15/87ffaf/87ffaf.png) | 121 | palegreen1 | RGB(135,255,175) | #87ffaf  | - - - |
![](https://placehold.co/15x15/87ffd7/87ffd7.png) | 122 | aquamarine1_1 | RGB(135,255,215) | #87ffd7  | - - - |
![](https://placehold.co/15x15/87ffff/87ffff.png) | 123 | darkslategray1 | RGB(135,255,255) | #87ffff  | - - - |
![](https://placehold.co/15x15/af0000/af0000.png) | 124 | red3 | RGB(175,0,0) | #af0000  | - - - |
![](https://placehold.co/15x15/af005f/af005f.png) | 125 | deeppink4_2 | RGB(175,0,95) | #af005f  | - - - |
![](https://placehold.co/15x15/af0087/af0087.png) | 126 | mediumvioletred | RGB(175,0,135) | #af0087  | - - - |
![](https://placehold.co/15x15/af00af/af00af.png) | 127 | magenta3 | RGB(175,0,175) | #af00af  | - - - |
![](https://placehold.co/15x15/af00d7/af00d7.png) | 128 | darkviolet_1 | RGB(175,0,215) | #af00d7  | - - - |
![](https://placehold.co/15x15/af00ff/af00ff.png) | 129 | purple_2 | RGB(175,0,255) | #af00ff  | - - - |
![](https://placehold.co/15x15/af5f00/af5f00.png) | 130 | darkorange3 | RGB(175,95,0) | #af5f00  | - - - |
![](https://placehold.co/15x15/af5f5f/af5f5f.png) | 131 | indianred | RGB(175,95,95) | #af5f5f  | - - - |
![](https://placehold.co/15x15/af5f87/af5f87.png) | 132 | hotpink3 | RGB(175,95,135) | #af5f87  | - - - |
![](https://placehold.co/15x15/af5faf/af5faf.png) | 133 | mediumorchid3 | RGB(175,95,175) | #af5faf  | - - - |
![](https://placehold.co/15x15/af5fd7/af5fd7.png) | 134 | mediumorchid | RGB(175,95,215) | #af5fd7  | - - - |
![](https://placehold.co/15x15/af5fff/af5fff.png) | 135 | mediumpurple2 | RGB(175,95,255) | #af5fff  | - - - |
![](https://placehold.co/15x15/af8700/af8700.png) | 136 | darkgoldenrod | RGB(175,135,0) | #af8700  | - - - |
![](https://placehold.co/15x15/af875f/af875f.png) | 137 | lightsalmon3 | RGB(175,135,95) | #af875f  | - - - |
![](https://placehold.co/15x15/af8787/af8787.png) | 138 | rosybrown | RGB(175,135,135) | #af8787  | - - - |
![](https://placehold.co/15x15/af87af/af87af.png) | 139 | grey63 | RGB(175,135,175) | #af87af  | - - - |
![](https://placehold.co/15x15/af87d7/af87d7.png) | 140 | mediumpurple2_1 | RGB(175,135,215) | #af87d7  | - - - |
![](https://placehold.co/15x15/af87ff/af87ff.png) | 141 | mediumpurple1 | RGB(175,135,255) | #af87ff  | - - - |
![](https://placehold.co/15x15/afaf00/afaf00.png) | 142 | gold3 | RGB(175,175,0) | #afaf00  | - - - |
![](https://placehold.co/15x15/afaf5f/afaf5f.png) | 143 | darkkhaki | RGB(175,175,95) | #afaf5f  | - - - |
![](https://placehold.co/15x15/afaf87/afaf87.png) | 144 | navajowhite3 | RGB(175,175,135) | #afaf87  | - - - |
![](https://placehold.co/15x15/afafaf/afafaf.png) | 145 | grey69 | RGB(175,175,175) | #afafaf  | - - - |
![](https://placehold.co/15x15/afafd7/afafd7.png) | 146 | lightsteelblue3 | RGB(175,175,215) | #afafd7  | - - - |
![](https://placehold.co/15x15/afafff/afafff.png) | 147 | lightsteelblue | RGB(175,175,255) | #afafff  | - - - |
![](https://placehold.co/15x15/afd700/afd700.png) | 148 | yellow3 | RGB(175,215,0) | #afd700  | - - - |
![](https://placehold.co/15x15/afd75f/afd75f.png) | 149 | darkolivegreen3_2 | RGB(175,215,95) | #afd75f  | - - - |
![](https://placehold.co/15x15/afd787/afd787.png) | 150 | darkseagreen3_1 | RGB(175,215,135) | #afd787  | - - - |
![](https://placehold.co/15x15/afd7af/afd7af.png) | 151 | darkseagreen2 | RGB(175,215,175) | #afd7af  | - - - |
![](https://placehold.co/15x15/afd7d7/afd7d7.png) | 152 | lightcyan3 | RGB(175,215,215) | #afd7d7  | - - - |
![](https://placehold.co/15x15/afd7ff/afd7ff.png) | 153 | lightskyblue1 | RGB(175,215,255) | #afd7ff  | - - - |
![](https://placehold.co/15x15/afff00/afff00.png) | 154 | greenyellow | RGB(175,255,0) | #afff00  | - - - |
![](https://placehold.co/15x15/afff5f/afff5f.png) | 155 | darkolivegreen2 | RGB(175,255,95) | #afff5f  | - - - |
![](https://placehold.co/15x15/afff87/afff87.png) | 156 | palegreen1_1 | RGB(175,255,135) | #afff87  | - - - |
![](https://placehold.co/15x15/afffaf/afffaf.png) | 157 | darkseagreen2_1 | RGB(175,255,175) | #afffaf  | - - - |
![](https://placehold.co/15x15/afffd7/afffd7.png) | 158 | darkseagreen1 | RGB(175,255,215) | #afffd7  | - - - |
![](https://placehold.co/15x15/afffff/afffff.png) | 159 | paleturquoise1 | RGB(175,255,255) | #afffff  | - - - |
![](https://placehold.co/15x15/d70000/d70000.png) | 160 | red3_1 | RGB(215,0,0) | #d70000  | - - - |
![](https://placehold.co/15x15/d7005f/d7005f.png) | 161 | deeppink3 | RGB(215,0,95) | #d7005f  | - - - |
![](https://placehold.co/15x15/d70087/d70087.png) | 162 | deeppink3_1 | RGB(215,0,135) | #d70087  | - - - |
![](https://placehold.co/15x15/d700af/d700af.png) | 163 | magenta3_1 | RGB(215,0,175) | #d700af  | - - - |
![](https://placehold.co/15x15/d700d7/d700d7.png) | 164 | magenta3_2 | RGB(215,0,215) | #d700d7  | - - - |
![](https://placehold.co/15x15/d700ff/d700ff.png) | 165 | magenta2 | RGB(215,0,255) | #d700ff  | - - - |
![](https://placehold.co/15x15/d75f00/d75f00.png) | 166 | darkorange3_1 | RGB(215,95,0) | #d75f00  | - - - |
![](https://placehold.co/15x15/d75f5f/d75f5f.png) | 167 | indianred_1 | RGB(215,95,95) | #d75f5f  | - - - |
![](https://placehold.co/15x15/d75f87/d75f87.png) | 168 | hotpink3_1 | RGB(215,95,135) | #d75f87  | - - - |
![](https://placehold.co/15x15/d75faf/d75faf.png) | 169 | hotpink2 | RGB(215,95,175) | #d75faf  | - - - |
![](https://placehold.co/15x15/d75fd7/d75fd7.png) | 170 | orchid | RGB(215,95,215) | #d75fd7  | - - - |
![](https://placehold.co/15x15/d75fff/d75fff.png) | 171 | mediumorchid1 | RGB(215,95,255) | #d75fff  | - - - |
![](https://placehold.co/15x15/d78700/d78700.png) | 172 | orange3 | RGB(215,135,0) | #d78700  | - - - |
![](https://placehold.co/15x15/d7875f/d7875f.png) | 173 | lightsalmon3_1 | RGB(215,135,95) | #d7875f  | - - - |
![](https://placehold.co/15x15/d78787/d78787.png) | 174 | lightpink3 | RGB(215,135,135) | #d78787  | - - - |
![](https://placehold.co/15x15/d787af/d787af.png) | 175 | pink3 | RGB(215,135,175) | #d787af  | - - - |
![](https://placehold.co/15x15/d787d7/d787d7.png) | 176 | plum3 | RGB(215,135,215) | #d787d7  | - - - |
![](https://placehold.co/15x15/d787ff/d787ff.png) | 177 | violet | RGB(215,135,255) | #d787ff  | - - - |
![](https://placehold.co/15x15/d7af00/d7af00.png) | 178 | gold3_1 | RGB(215,175,0) | #d7af00  | - - - |
![](https://placehold.co/15x15/d7af5f/d7af5f.png) | 179 | lightgoldenrod3 | RGB(215,175,95) | #d7af5f  | - - - |
![](https://placehold.co/15x15/d7af87/d7af87.png) | 180 | tan | RGB(215,175,135) | #d7af87  | - - - |
![](https://placehold.co/15x15/d7afaf/d7afaf.png) | 181 | mistyrose3 | RGB(215,175,175) | #d7afaf  | - - - |
![](https://placehold.co/15x15/d7afd7/d7afd7.png) | 182 | thistle3 | RGB(215,175,215) | #d7afd7  | - - - |
![](https://placehold.co/15x15/d7afff/d7afff.png) | 183 | plum2 | RGB(215,175,255) | #d7afff  | - - - |
![](https://placehold.co/15x15/d7d700/d7d700.png) | 184 | yellow3_1 | RGB(215,215,0) | #d7d700  | - - - |
![](https://placehold.co/15x15/d7d75f/d7d75f.png) | 185 | khaki3 | RGB(215,215,95) | #d7d75f  | - - - |
![](https://placehold.co/15x15/d7d787/d7d787.png) | 186 | lightgoldenrod2 | RGB(215,215,135) | #d7d787  | - - - |
![](https://placehold.co/15x15/d7d7af/d7d7af.png) | 187 | lightyellow3 | RGB(215,215,175) | #d7d7af  | - - - |
![](https://placehold.co/15x15/d7d7d7/d7d7d7.png) | 188 | grey84 | RGB(215,215,215) | #d7d7d7  | - - - |
![](https://placehold.co/15x15/d7d7ff/d7d7ff.png) | 189 | lightsteelblue1 | RGB(215,215,255) | #d7d7ff  | - - - |
![](https://placehold.co/15x15/d7ff00/d7ff00.png) | 190 | yellow2 | RGB(215,255,0) | #d7ff00  | - - - |
![](https://placehold.co/15x15/d7ff5f/d7ff5f.png) | 191 | darkolivegreen1 | RGB(215,255,95) | #d7ff5f  | - - - |
![](https://placehold.co/15x15/d7ff87/d7ff87.png) | 192 | darkolivegreen1_1 | RGB(215,255,135) | #d7ff87  | - - - |
![](https://placehold.co/15x15/d7ffaf/d7ffaf.png) | 193 | darkseagreen1_1 | RGB(215,255,175) | #d7ffaf  | - - - |
![](https://placehold.co/15x15/d7ffd7/d7ffd7.png) | 194 | honeydew2 | RGB(215,255,215) | #d7ffd7  | - - - |
![](https://placehold.co/15x15/d7ffff/d7ffff.png) | 195 | lightcyan1 | RGB(215,255,255) | #d7ffff  | - - - |
![](https://placehold.co/15x15/ff0000/ff0000.png) | 196 | red1 | RGB(255,0,0) | #ff0000  | - - - |
![](https://placehold.co/15x15/ff005f/ff005f.png) | 197 | deeppink2 | RGB(255,0,95) | #ff005f  | - - - |
![](https://placehold.co/15x15/ff0087/ff0087.png) | 198 | deeppink1 | RGB(255,0,135) | #ff0087  | - - - |
![](https://placehold.co/15x15/ff00af/ff00af.png) | 199 | deeppink1_1 | RGB(255,0,175) | #ff00af  | - - - |
![](https://placehold.co/15x15/ff00d7/ff00d7.png) | 200 | magenta2_1 | RGB(255,0,215) | #ff00d7  | - - - |
![](https://placehold.co/15x15/ff00ff/ff00ff.png) | 201 | magenta1 | RGB(255,0,255) | #ff00ff  | - - - |
![](https://placehold.co/15x15/ff5f00/ff5f00.png) | 202 | orangered1 | RGB(255,95,0) | #ff5f00  | - - - |
![](https://placehold.co/15x15/ff5f5f/ff5f5f.png) | 203 | indianred1 | RGB(255,95,95) | #ff5f5f  | - - - |
![](https://placehold.co/15x15/ff5f87/ff5f87.png) | 204 | indianred1_1 | RGB(255,95,135) | #ff5f87  | - - - |
![](https://placehold.co/15x15/ff5faf/ff5faf.png) | 205 | hotpink | RGB(255,95,175) | #ff5faf  | - - - |
![](https://placehold.co/15x15/ff5fd7/ff5fd7.png) | 206 | hotpink_1 | RGB(255,95,215) | #ff5fd7  | - - - |
![](https://placehold.co/15x15/ff5fff/ff5fff.png) | 207 | mediumorchid1_1 | RGB(255,95,255) | #ff5fff  | - - - |
![](https://placehold.co/15x15/ff8700/ff8700.png) | 208 | darkorange | RGB(255,135,0) | #ff8700  | - - - |
![](https://placehold.co/15x15/ff875f/ff875f.png) | 209 | salmon1 | RGB(255,135,95) | #ff875f  | - - - |
![](https://placehold.co/15x15/ff8787/ff8787.png) | 210 | lightcoral | RGB(255,135,135) | #ff8787  | - - - |
![](https://placehold.co/15x15/ff87af/ff87af.png) | 211 | palevioletred1 | RGB(255,135,175) | #ff87af  | - - - |
![](https://placehold.co/15x15/ff87d7/ff87d7.png) | 212 | orchid2 | RGB(255,135,215) | #ff87d7  | - - - |
![](https://placehold.co/15x15/ff87ff/ff87ff.png) | 213 | orchid1 | RGB(255,135,255) | #ff87ff  | - - - |
![](https://placehold.co/15x15/ffaf00/ffaf00.png) | 214 | orange1 | RGB(255,175,0) | #ffaf00  | - - - |
![](https://placehold.co/15x15/ffaf5f/ffaf5f.png) | 215 | sandybrown | RGB(255,175,95) | #ffaf5f  | - - - |
![](https://placehold.co/15x15/ffaf87/ffaf87.png) | 216 | lightsalmon1 | RGB(255,175,135) | #ffaf87  | - - - |
![](https://placehold.co/15x15/ffafaf/ffafaf.png) | 217 | lightpink1 | RGB(255,175,175) | #ffafaf  | - - - |
![](https://placehold.co/15x15/ffafd7/ffafd7.png) | 218 | pink1 | RGB(255,175,215) | #ffafd7  | - - - |
![](https://placehold.co/15x15/ffafff/ffafff.png) | 219 | plum1 | RGB(255,175,255) | #ffafff  | - - - |
![](https://placehold.co/15x15/ffd700/ffd700.png) | 220 | gold1 | RGB(255,215,0) | #ffd700  | - - - |
![](https://placehold.co/15x15/ffd75f/ffd75f.png) | 221 | lightgoldenrod2_1 | RGB(255,215,95) | #ffd75f  | - - - |
![](https://placehold.co/15x15/ffd787/ffd787.png) | 222 | lightgoldenrod2_2 | RGB(255,215,135) | #ffd787  | - - - |
![](https://placehold.co/15x15/ffd7af/ffd7af.png) | 223 | navajowhite1 | RGB(255,215,175) | #ffd7af  | - - - |
![](https://placehold.co/15x15/ffd7d7/ffd7d7.png) | 224 | mistyrose1 | RGB(255,215,215) | #ffd7d7  | - - - |
![](https://placehold.co/15x15/ffd7ff/ffd7ff.png) | 225 | thistle1 | RGB(255,215,255) | #ffd7ff  | - - - |
![](https://placehold.co/15x15/ffff00/ffff00.png) | 226 | yellow1 | RGB(255,255,0) | #ffff00  | - - - |
![](https://placehold.co/15x15/ffff5f/ffff5f.png) | 227 | lightgoldenrod1 | RGB(255,255,95) | #ffff5f  | - - - |
![](https://placehold.co/15x15/ffff87/ffff87.png) | 228 | khaki1 | RGB(255,255,135) | #ffff87  | - - - |
![](https://placehold.co/15x15/ffffaf/ffffaf.png) | 229 | wheat1 | RGB(255,255,175) | #ffffaf  | - - - |
![](https://placehold.co/15x15/ffffd7/ffffd7.png) | 230 | cornsilk1 | RGB(255,255,215) | #ffffd7  | - - - |
![](https://placehold.co/15x15/ffffff/ffffff.png) | 231 | grey100 | RGB(255,255,255) | #ffffff  | - - - |
![](https://placehold.co/15x15/080808/080808.png) | 232 | grey3 | RGB(8,8,8) | #080808  | - - - |
![](https://placehold.co/15x15/121212/121212.png) | 233 | grey7 | RGB(18,18,18) | #121212  | - - - |
![](https://placehold.co/15x15/1c1c1c/1c1c1c.png) | 234 | grey11 | RGB(28,28,28) | #1c1c1c  | - - - |
![](https://placehold.co/15x15/262626/262626.png) | 235 | grey15 | RGB(38,38,38) | #262626  | - - - |
![](https://placehold.co/15x15/303030/303030.png) | 236 | grey19 | RGB(48,48,48) | #303030  | - - - |
![](https://placehold.co/15x15/3a3a3a/3a3a3a.png) | 237 | grey23 | RGB(58,58,58) | #3a3a3a  | - - - |
![](https://placehold.co/15x15/444444/444444.png) | 238 | grey27 | RGB(68,68,68) | #444444  | - - - |
![](https://placehold.co/15x15/4e4e4e/4e4e4e.png) | 239 | grey30 | RGB(78,78,78) | #4e4e4e  | - - - |
![](https://placehold.co/15x15/585858/585858.png) | 240 | grey35 | RGB(88,88,88) | #585858  | - - - |
![](https://placehold.co/15x15/626262/626262.png) | 241 | grey39 | RGB(98,98,98) | #626262  | - - - |
![](https://placehold.co/15x15/6c6c6c/6c6c6c.png) | 242 | grey42 | RGB(108,108,108) | #6c6c6c  | - - - |
![](https://placehold.co/15x15/767676/767676.png) | 243 | grey46 | RGB(118,118,118) | #767676  | - - - |
![](https://placehold.co/15x15/808080/808080.png) | 244 | grey50 | RGB(128,128,128) | #808080  | - - - |
![](https://placehold.co/15x15/8a8a8a/8a8a8a.png) | 245 | grey54 | RGB(138,138,138) | #8a8a8a  | - - - |
![](https://placehold.co/15x15/949494/949494.png) | 246 | grey58 | RGB(148,148,148) | #949494  | - - - |
![](https://placehold.co/15x15/9e9e9e/9e9e9e.png) | 247 | grey62 | RGB(158,158,158) | #9e9e9e  | - - - |
![](https://placehold.co/15x15/a8a8a8/a8a8a8.png) | 248 | grey66 | RGB(168,168,168) | #a8a8a8  | - - - |
![](https://placehold.co/15x15/b2b2b2/b2b2b2.png) | 249 | grey70 | RGB(178,178,178) | #b2b2b2  | - - - |
![](https://placehold.co/15x15/bcbcbc/bcbcbc.png) | 250 | grey74 | RGB(188,188,188) | #bcbcbc  | - - - |
![](https://placehold.co/15x15/c6c6c6/c6c6c6.png) | 251 | grey78 | RGB(198,198,198) | #c6c6c6  | - - - |
![](https://placehold.co/15x15/d0d0d0/d0d0d0.png) | 252 | grey82 | RGB(208,208,208) | #d0d0d0  | - - - |
![](https://placehold.co/15x15/dadada/dadada.png) | 253 | grey85 | RGB(218,218,218) | #dadada  | - - - |
![](https://placehold.co/15x15/e4e4e4/e4e4e4.png) | 254 | grey89 | RGB(228,228,228) | #e4e4e4  | - - - |
![](https://placehold.co/15x15/eeeeee/eeeeee.png) | 255 | grey93 | RGB(238,238,238) | #eeeeee  | - - - |

