![PromptPlus Logo](https://raw.githubusercontent.com/FRACerqueira/PromptPlus/refs/heads/main/icon.png)

### Color structure
</br>


#### Represents an RGB color (optionally mapped to an indexed palette entry).

```csharp
public struct Color : IEquatable<Color>
```

| parameter | description |
| --- | --- |
| red | The red component (0-255). |
| green | The green component (0-255). |
| blue | The blue component (0-255). |

### Public Members

| name | description |
| --- | --- |
| [Color](Color/Color.md)(…) | Represents an RGB color (optionally mapped to an indexed palette entry). |
| static [Aqua](Color/Aqua.md) { get; } | Gets the Color "Aqua" (RGB 0,255,255). |
| static [Aquamarine1](Color/Aquamarine1.md) { get; } | Gets the Color "Aquamarine1" (RGB 95,255,215). |
| static [Aquamarine1_1](Color/Aquamarine1_1.md) { get; } | Gets the Color "Aquamarine1_1" (RGB 135,255,215). |
| static [Aquamarine3](Color/Aquamarine3.md) { get; } | Gets the Color "Aquamarine3" (RGB 95,215,175). |
| static [Black](Color/Black.md) { get; } | Gets the Color "Black" (RGB 0,0,0). |
| static [Blue](Color/Blue.md) { get; } | Gets the Color "Blue" (RGB 0,0,255). |
| static [Blue1](Color/Blue1.md) { get; } | Gets the Color "Blue1" (RGB 0,0,255). |
| static [Blue3](Color/Blue3.md) { get; } | Gets the Color "Blue3" (RGB 0,0,175). |
| static [Blue3_1](Color/Blue3_1.md) { get; } | Gets the Color "Blue3_1" (RGB 0,0,215). |
| static [BlueViolet](Color/BlueViolet.md) { get; } | Gets the Color "BlueViolet" (RGB 95,0,255). |
| static [CadetBlue](Color/CadetBlue.md) { get; } | Gets the Color "CadetBlue" (RGB 95,175,135). |
| static [CadetBlue_1](Color/CadetBlue_1.md) { get; } | Gets the Color "CadetBlue_1" (RGB 95,175,175). |
| static [Chartreuse1](Color/Chartreuse1.md) { get; } | Gets the Color "Chartreuse1" (RGB 135,255,0). |
| static [Chartreuse2](Color/Chartreuse2.md) { get; } | Gets the Color "Chartreuse2" (RGB 95,255,0). |
| static [Chartreuse2_1](Color/Chartreuse2_1.md) { get; } | Gets the Color "Chartreuse2_1" (RGB 135,215,0). |
| static [Chartreuse3](Color/Chartreuse3.md) { get; } | Gets the Color "Chartreuse3" (RGB 95,175,0). |
| static [Chartreuse3_1](Color/Chartreuse3_1.md) { get; } | Gets the Color "Chartreuse3_1" (RGB 95,215,0). |
| static [Chartreuse4](Color/Chartreuse4.md) { get; } | Gets the Color "Chartreuse4" (RGB 95,135,0). |
| static [CornflowerBlue](Color/CornflowerBlue.md) { get; } | Gets the Color "CornflowerBlue" (RGB 95,135,255). |
| static [Cornsilk1](Color/Cornsilk1.md) { get; } | Gets the Color "Cornsilk1" (RGB 255,255,215). |
| static [Cyan1](Color/Cyan1.md) { get; } | Gets the Color "Cyan1" (RGB 0,255,255). |
| static [Cyan2](Color/Cyan2.md) { get; } | Gets the Color "Cyan2" (RGB 0,255,215). |
| static [Cyan3](Color/Cyan3.md) { get; } | Gets the Color "Cyan3" (RGB 0,215,175). |
| static [DarkBlue](Color/DarkBlue.md) { get; } | Gets the Color "DarkBlue" (RGB 0,0,135). |
| static [DarkCyan](Color/DarkCyan.md) { get; } | Gets the Color "DarkCyan" (RGB 0,175,135). |
| static [DarkGoldenrod](Color/DarkGoldenrod.md) { get; } | Gets the Color "DarkGoldenrod" (RGB 175,135,0). |
| static [DarkGreen](Color/DarkGreen.md) { get; } | Gets the Color "DarkGreen" (RGB 0,95,0). |
| static [DarkKhaki](Color/DarkKhaki.md) { get; } | Gets the Color "DarkKhaki" (RGB 175,175,95). |
| static [DarkMagenta](Color/DarkMagenta.md) { get; } | Gets the Color "DarkMagenta" (RGB 135,0,135). |
| static [DarkMagenta_1](Color/DarkMagenta_1.md) { get; } | Gets the Color "DarkMagenta_1" (RGB 135,0,175). |
| static [DarkOliveGreen1](Color/DarkOliveGreen1.md) { get; } | Gets the Color "DarkOliveGreen1" (RGB 215,255,95). |
| static [DarkOliveGreen1_1](Color/DarkOliveGreen1_1.md) { get; } | Gets the Color "DarkOliveGreen1_1" (RGB 215,255,135). |
| static [DarkOliveGreen2](Color/DarkOliveGreen2.md) { get; } | Gets the Color "DarkOliveGreen2" (RGB 175,255,95). |
| static [DarkOliveGreen3](Color/DarkOliveGreen3.md) { get; } | Gets the Color "DarkOliveGreen3" (RGB 135,175,95). |
| static [DarkOliveGreen3_1](Color/DarkOliveGreen3_1.md) { get; } | Gets the Color "DarkOliveGreen3_1" (RGB 135,215,95). |
| static [DarkOliveGreen3_2](Color/DarkOliveGreen3_2.md) { get; } | Gets the Color "DarkOliveGreen3_2" (RGB 175,215,95). |
| static [DarkOrange](Color/DarkOrange.md) { get; } | Gets the Color "DarkOrange" (RGB 255,135,0). |
| static [DarkOrange3](Color/DarkOrange3.md) { get; } | Gets the Color "DarkOrange3" (RGB 175,95,0). |
| static [DarkOrange3_1](Color/DarkOrange3_1.md) { get; } | Gets the Color "DarkOrange3_1" (RGB 215,95,0). |
| static [DarkRed](Color/DarkRed.md) { get; } | Gets the Color "DarkRed" (RGB 95,0,0). |
| static [DarkRed_1](Color/DarkRed_1.md) { get; } | Gets the Color "DarkRed_1" (RGB 135,0,0). |
| static [DarkSeaGreen](Color/DarkSeaGreen.md) { get; } | Gets the Color "DarkSeaGreen" (RGB 135,175,135). |
| static [DarkSeaGreen1](Color/DarkSeaGreen1.md) { get; } | Gets the Color "DarkSeaGreen1" (RGB 175,255,215). |
| static [DarkSeaGreen1_1](Color/DarkSeaGreen1_1.md) { get; } | Gets the Color "DarkSeaGreen1_1" (RGB 215,255,175). |
| static [DarkSeaGreen2](Color/DarkSeaGreen2.md) { get; } | Gets the Color "DarkSeaGreen2" (RGB 175,215,175). |
| static [DarkSeaGreen2_1](Color/DarkSeaGreen2_1.md) { get; } | Gets the Color "DarkSeaGreen2_1" (RGB 175,255,175). |
| static [DarkSeaGreen3](Color/DarkSeaGreen3.md) { get; } | Gets the Color "DarkSeaGreen3" (RGB 135,215,175). |
| static [DarkSeaGreen3_1](Color/DarkSeaGreen3_1.md) { get; } | Gets the Color "DarkSeaGreen3_1" (RGB 175,215,135). |
| static [DarkSeaGreen4](Color/DarkSeaGreen4.md) { get; } | Gets the Color "DarkSeaGreen4" (RGB 95,135,95). |
| static [DarkSeaGreen4_1](Color/DarkSeaGreen4_1.md) { get; } | Gets the Color "DarkSeaGreen4_1" (RGB 95,175,95). |
| static [DarkSlateGray1](Color/DarkSlateGray1.md) { get; } | Gets the Color "DarkSlateGray1" (RGB 135,255,255). |
| static [DarkSlateGray2](Color/DarkSlateGray2.md) { get; } | Gets the Color "DarkSlateGray2" (RGB 95,255,255). |
| static [DarkSlateGray3](Color/DarkSlateGray3.md) { get; } | Gets the Color "DarkSlateGray3" (RGB 135,215,215). |
| static [DarkTurquoise](Color/DarkTurquoise.md) { get; } | Gets the Color "DarkTurquoise" (RGB 0,215,215). |
| static [DarkViolet](Color/DarkViolet.md) { get; } | Gets the Color "DarkViolet" (RGB 135,0,215). |
| static [DarkViolet_1](Color/DarkViolet_1.md) { get; } | Gets the Color "DarkViolet_1" (RGB 175,0,215). |
| static [DeepPink1](Color/DeepPink1.md) { get; } | Gets the Color "DeepPink1" (RGB 255,0,135). |
| static [DeepPink1_1](Color/DeepPink1_1.md) { get; } | Gets the Color "DeepPink1_1" (RGB 255,0,175). |
| static [DeepPink2](Color/DeepPink2.md) { get; } | Gets the Color "DeepPink2" (RGB 255,0,95). |
| static [DeepPink3](Color/DeepPink3.md) { get; } | Gets the Color "DeepPink3" (RGB 215,0,95). |
| static [DeepPink3_1](Color/DeepPink3_1.md) { get; } | Gets the Color "DeepPink3_1" (RGB 215,0,135). |
| static [DeepPink4](Color/DeepPink4.md) { get; } | Gets the Color "DeepPink4" (RGB 95,0,95). |
| static [DeepPink4_1](Color/DeepPink4_1.md) { get; } | Gets the Color "DeepPink4_1" (RGB 135,0,95). |
| static [DeepPink4_2](Color/DeepPink4_2.md) { get; } | Gets the Color "DeepPink4_2" (RGB 175,0,95). |
| static [DeepSkyBlue1](Color/DeepSkyBlue1.md) { get; } | Gets the Color "DeepSkyBlue1" (RGB 0,175,255). |
| static [DeepSkyBlue2](Color/DeepSkyBlue2.md) { get; } | Gets the Color "DeepSkyBlue2" (RGB 0,175,215). |
| static [DeepSkyBlue3](Color/DeepSkyBlue3.md) { get; } | Gets the Color "DeepSkyBlue3" (RGB 0,135,175). |
| static [DeepSkyBlue3_1](Color/DeepSkyBlue3_1.md) { get; } | Gets the Color "DeepSkyBlue3_1" (RGB 0,135,215). |
| static [DeepSkyBlue4](Color/DeepSkyBlue4.md) { get; } | Gets the Color "DeepSkyBlue4" (RGB 0,95,95). |
| static [DeepSkyBlue4_1](Color/DeepSkyBlue4_1.md) { get; } | Gets the Color "DeepSkyBlue4_1" (RGB 0,95,135). |
| static [DeepSkyBlue4_2](Color/DeepSkyBlue4_2.md) { get; } | Gets the Color "DeepSkyBlue4_2" (RGB 0,95,175). |
| static [DodgerBlue1](Color/DodgerBlue1.md) { get; } | Gets the Color "DodgerBlue1" (RGB 0,135,255). |
| static [DodgerBlue2](Color/DodgerBlue2.md) { get; } | Gets the Color "DodgerBlue2" (RGB 0,95,255). |
| static [DodgerBlue3](Color/DodgerBlue3.md) { get; } | Gets the Color "DodgerBlue3" (RGB 0,95,215). |
| static [Fuchsia](Color/Fuchsia.md) { get; } | Gets the Color "Fuchsia" (RGB 255,0,255). |
| static [Gold1](Color/Gold1.md) { get; } | Gets the Color "Gold1" (RGB 255,215,0). |
| static [Gold3](Color/Gold3.md) { get; } | Gets the Color "Gold3" (RGB 175,175,0). |
| static [Gold3_1](Color/Gold3_1.md) { get; } | Gets the Color "Gold3_1" (RGB 215,175,0). |
| static [Green](Color/Green.md) { get; } | Gets the Color "Green" (RGB 0,128,0). |
| static [Green1](Color/Green1.md) { get; } | Gets the Color "Green1" (RGB 0,255,0). |
| static [Green3](Color/Green3.md) { get; } | Gets the Color "Green3" (RGB 0,175,0). |
| static [Green3_1](Color/Green3_1.md) { get; } | Gets the Color "Green3_1" (RGB 0,215,0). |
| static [Green4](Color/Green4.md) { get; } | Gets the Color "Green4" (RGB 0,135,0). |
| static [GreenYellow](Color/GreenYellow.md) { get; } | Gets the Color "GreenYellow" (RGB 175,255,0). |
| static [Grey](Color/Grey.md) { get; } | Gets the Color "Grey" (RGB 128,128,128). |
| static [Grey0](Color/Grey0.md) { get; } | Gets the Color "Grey0" (RGB 0,0,0). |
| static [Grey100](Color/Grey100.md) { get; } | Gets the Color "Grey100" (RGB 255,255,255). |
| static [Grey11](Color/Grey11.md) { get; } | Gets the Color "Grey11" (RGB 28,28,28). |
| static [Grey15](Color/Grey15.md) { get; } | Gets the Color "Grey15" (RGB 38,38,38). |
| static [Grey19](Color/Grey19.md) { get; } | Gets the Color "Grey19" (RGB 48,48,48). |
| static [Grey23](Color/Grey23.md) { get; } | Gets the Color "Grey23" (RGB 58,58,58). |
| static [Grey27](Color/Grey27.md) { get; } | Gets the Color "Grey27" (RGB 68,68,68). |
| static [Grey3](Color/Grey3.md) { get; } | Gets the Color "Grey3" (RGB 8,8,8). |
| static [Grey30](Color/Grey30.md) { get; } | Gets the Color "Grey30" (RGB 78,78,78). |
| static [Grey35](Color/Grey35.md) { get; } | Gets the Color "Grey35" (RGB 88,88,88). |
| static [Grey37](Color/Grey37.md) { get; } | Gets the Color "Grey37" (RGB 95,95,95). |
| static [Grey39](Color/Grey39.md) { get; } | Gets the Color "Grey39" (RGB 98,98,98). |
| static [Grey42](Color/Grey42.md) { get; } | Gets the Color "Grey42" (RGB 108,108,108). |
| static [Grey46](Color/Grey46.md) { get; } | Gets the Color "Grey46" (RGB 118,118,118). |
| static [Grey50](Color/Grey50.md) { get; } | Gets the Color "Grey50" (RGB 128,128,128). |
| static [Grey53](Color/Grey53.md) { get; } | Gets the Color "Grey53" (RGB 135,135,135). |
| static [Grey54](Color/Grey54.md) { get; } | Gets the Color "Grey54" (RGB 138,138,138). |
| static [Grey58](Color/Grey58.md) { get; } | Gets the Color "Grey58" (RGB 148,148,148). |
| static [Grey62](Color/Grey62.md) { get; } | Gets the Color "Grey62" (RGB 158,158,158). |
| static [Grey63](Color/Grey63.md) { get; } | Gets the Color "Grey63" (RGB 175,135,175). |
| static [Grey66](Color/Grey66.md) { get; } | Gets the Color "Grey66" (RGB 168,168,168). |
| static [Grey69](Color/Grey69.md) { get; } | Gets the Color "Grey69" (RGB 175,175,175). |
| static [Grey7](Color/Grey7.md) { get; } | Gets the Color "Grey7" (RGB 18,18,18). |
| static [Grey70](Color/Grey70.md) { get; } | Gets the Color "Grey70" (RGB 178,178,178). |
| static [Grey74](Color/Grey74.md) { get; } | Gets the Color "Grey74" (RGB 188,188,188). |
| static [Grey78](Color/Grey78.md) { get; } | Gets the Color "Grey78" (RGB 198,198,198). |
| static [Grey82](Color/Grey82.md) { get; } | Gets the Color "Grey82" (RGB 208,208,208). |
| static [Grey84](Color/Grey84.md) { get; } | Gets the Color "Grey84" (RGB 215,215,215). |
| static [Grey85](Color/Grey85.md) { get; } | Gets the Color "Grey85" (RGB 218,218,218). |
| static [Grey89](Color/Grey89.md) { get; } | Gets the Color "Grey89" (RGB 228,228,228). |
| static [Grey93](Color/Grey93.md) { get; } | Gets the Color "Grey93" (RGB 238,238,238). |
| static [Honeydew2](Color/Honeydew2.md) { get; } | Gets the Color "Honeydew2" (RGB 215,255,215). |
| static [HotPink](Color/HotPink.md) { get; } | Gets the Color "HotPink" (RGB 255,95,175). |
| static [HotPink2](Color/HotPink2.md) { get; } | Gets the Color "HotPink2" (RGB 215,95,175). |
| static [HotPink3](Color/HotPink3.md) { get; } | Gets the Color "HotPink3" (RGB 175,95,135). |
| static [HotPink3_1](Color/HotPink3_1.md) { get; } | Gets the Color "HotPink3_1" (RGB 215,95,135). |
| static [HotPink_1](Color/HotPink_1.md) { get; } | Gets the Color "HotPink_1" (RGB 255,95,215). |
| static [IndianRed](Color/IndianRed.md) { get; } | Gets the Color "IndianRed" (RGB 175,95,95). |
| static [IndianRed1](Color/IndianRed1.md) { get; } | Gets the Color "IndianRed1" (RGB 255,95,95). |
| static [IndianRed1_1](Color/IndianRed1_1.md) { get; } | Gets the Color "IndianRed1_1" (RGB 255,95,135). |
| static [IndianRed_1](Color/IndianRed_1.md) { get; } | Gets the Color "IndianRed_1" (RGB 215,95,95). |
| static [Khaki1](Color/Khaki1.md) { get; } | Gets the Color "Khaki1" (RGB 255,255,135). |
| static [Khaki3](Color/Khaki3.md) { get; } | Gets the Color "Khaki3" (RGB 215,215,95). |
| static [LightCoral](Color/LightCoral.md) { get; } | Gets the Color "LightCoral" (RGB 255,135,135). |
| static [LightCyan1](Color/LightCyan1.md) { get; } | Gets the Color "LightCyan1" (RGB 215,255,255). |
| static [LightCyan3](Color/LightCyan3.md) { get; } | Gets the Color "LightCyan3" (RGB 175,215,215). |
| static [LightGoldenrod1](Color/LightGoldenrod1.md) { get; } | Gets the Color "LightGoldenrod1" (RGB 255,255,95). |
| static [LightGoldenrod2](Color/LightGoldenrod2.md) { get; } | Gets the Color "LightGoldenrod2" (RGB 215,215,135). |
| static [LightGoldenrod2_1](Color/LightGoldenrod2_1.md) { get; } | Gets the Color "LightGoldenrod2_1" (RGB 255,215,95). |
| static [LightGoldenrod2_2](Color/LightGoldenrod2_2.md) { get; } | Gets the Color "LightGoldenrod2_2" (RGB 255,215,135). |
| static [LightGoldenrod3](Color/LightGoldenrod3.md) { get; } | Gets the Color "LightGoldenrod3" (RGB 215,175,95). |
| static [LightGreen](Color/LightGreen.md) { get; } | Gets the Color "LightGreen" (RGB 135,255,95). |
| static [LightGreen_1](Color/LightGreen_1.md) { get; } | Gets the Color "LightGreen_1" (RGB 135,255,135). |
| static [LightPink1](Color/LightPink1.md) { get; } | Gets the Color "LightPink1" (RGB 255,175,175). |
| static [LightPink3](Color/LightPink3.md) { get; } | Gets the Color "LightPink3" (RGB 215,135,135). |
| static [LightPink4](Color/LightPink4.md) { get; } | Gets the Color "LightPink4" (RGB 135,95,95). |
| static [LightSalmon1](Color/LightSalmon1.md) { get; } | Gets the Color "LightSalmon1" (RGB 255,175,135). |
| static [LightSalmon3](Color/LightSalmon3.md) { get; } | Gets the Color "LightSalmon3" (RGB 175,135,95). |
| static [LightSalmon3_1](Color/LightSalmon3_1.md) { get; } | Gets the Color "LightSalmon3_1" (RGB 215,135,95). |
| static [LightSeaGreen](Color/LightSeaGreen.md) { get; } | Gets the Color "LightSeaGreen" (RGB 0,175,175). |
| static [LightSkyBlue1](Color/LightSkyBlue1.md) { get; } | Gets the Color "LightSkyBlue1" (RGB 175,215,255). |
| static [LightSkyBlue3](Color/LightSkyBlue3.md) { get; } | Gets the Color "LightSkyBlue3" (RGB 135,175,175). |
| static [LightSkyBlue3_1](Color/LightSkyBlue3_1.md) { get; } | Gets the Color "LightSkyBlue3_1" (RGB 135,175,215). |
| static [LightSlateBlue](Color/LightSlateBlue.md) { get; } | Gets the Color "LightSlateBlue" (RGB 135,135,255). |
| static [LightSlateGrey](Color/LightSlateGrey.md) { get; } | Gets the Color "LightSlateGrey" (RGB 135,135,175). |
| static [LightSteelBlue](Color/LightSteelBlue.md) { get; } | Gets the Color "LightSteelBlue" (RGB 175,175,255). |
| static [LightSteelBlue1](Color/LightSteelBlue1.md) { get; } | Gets the Color "LightSteelBlue1" (RGB 215,215,255). |
| static [LightSteelBlue3](Color/LightSteelBlue3.md) { get; } | Gets the Color "LightSteelBlue3" (RGB 175,175,215). |
| static [LightYellow3](Color/LightYellow3.md) { get; } | Gets the Color "LightYellow3" (RGB 215,215,175). |
| static [Lime](Color/Lime.md) { get; } | Gets the Color "Lime" (RGB 0,255,0). |
| static [Magenta1](Color/Magenta1.md) { get; } | Gets the Color "Magenta1" (RGB 255,0,255). |
| static [Magenta2](Color/Magenta2.md) { get; } | Gets the Color "Magenta2" (RGB 215,0,255). |
| static [Magenta2_1](Color/Magenta2_1.md) { get; } | Gets the Color "Magenta2_1" (RGB 255,0,215). |
| static [Magenta3](Color/Magenta3.md) { get; } | Gets the Color "Magenta3" (RGB 175,0,175). |
| static [Magenta3_1](Color/Magenta3_1.md) { get; } | Gets the Color "Magenta3_1" (RGB 215,0,175). |
| static [Magenta3_2](Color/Magenta3_2.md) { get; } | Gets the Color "Magenta3_2" (RGB 215,0,215). |
| static [Maroon](Color/Maroon.md) { get; } | Gets the Color "Maroon" (RGB 128,0,0). |
| static [MediumOrchid](Color/MediumOrchid.md) { get; } | Gets the Color "MediumOrchid" (RGB 175,95,215). |
| static [MediumOrchid1](Color/MediumOrchid1.md) { get; } | Gets the Color "MediumOrchid1" (RGB 215,95,255). |
| static [MediumOrchid1_1](Color/MediumOrchid1_1.md) { get; } | Gets the Color "MediumOrchid1_1" (RGB 255,95,255). |
| static [MediumOrchid3](Color/MediumOrchid3.md) { get; } | Gets the Color "MediumOrchid3" (RGB 175,95,175). |
| static [MediumPurple](Color/MediumPurple.md) { get; } | Gets the Color "MediumPurple" (RGB 135,135,215). |
| static [MediumPurple1](Color/MediumPurple1.md) { get; } | Gets the Color "MediumPurple1" (RGB 175,135,255). |
| static [MediumPurple2](Color/MediumPurple2.md) { get; } | Gets the Color "MediumPurple2" (RGB 175,95,255). |
| static [MediumPurple2_1](Color/MediumPurple2_1.md) { get; } | Gets the Color "MediumPurple2_1" (RGB 175,135,215). |
| static [MediumPurple3](Color/MediumPurple3.md) { get; } | Gets the Color "MediumPurple3" (RGB 135,95,175). |
| static [MediumPurple3_1](Color/MediumPurple3_1.md) { get; } | Gets the Color "MediumPurple3_1" (RGB 135,95,215). |
| static [MediumPurple4](Color/MediumPurple4.md) { get; } | Gets the Color "MediumPurple4" (RGB 95,95,135). |
| static [MediumSpringGreen](Color/MediumSpringGreen.md) { get; } | Gets the Color "MediumSpringGreen" (RGB 0,255,175). |
| static [MediumTurquoise](Color/MediumTurquoise.md) { get; } | Gets the Color "MediumTurquoise" (RGB 95,215,215). |
| static [MediumVioletRed](Color/MediumVioletRed.md) { get; } | Gets the Color "MediumVioletRed" (RGB 175,0,135). |
| static [MistyRose1](Color/MistyRose1.md) { get; } | Gets the Color "MistyRose1" (RGB 255,215,215). |
| static [MistyRose3](Color/MistyRose3.md) { get; } | Gets the Color "MistyRose3" (RGB 215,175,175). |
| static [NavajoWhite1](Color/NavajoWhite1.md) { get; } | Gets the Color "NavajoWhite1" (RGB 255,215,175). |
| static [NavajoWhite3](Color/NavajoWhite3.md) { get; } | Gets the Color "NavajoWhite3" (RGB 175,175,135). |
| static [Navy](Color/Navy.md) { get; } | Gets the Color "Navy" (RGB 0,0,128). |
| static [NavyBlue](Color/NavyBlue.md) { get; } | Gets the Color "NavyBlue" (RGB 0,0,95). |
| static [Olive](Color/Olive.md) { get; } | Gets the Color "Olive" (RGB 128,128,0). |
| static [Orange1](Color/Orange1.md) { get; } | Gets the Color "Orange1" (RGB 255,175,0). |
| static [Orange3](Color/Orange3.md) { get; } | Gets the Color "Orange3" (RGB 215,135,0). |
| static [Orange4](Color/Orange4.md) { get; } | Gets the Color "Orange4" (RGB 95,95,0). |
| static [Orange4_1](Color/Orange4_1.md) { get; } | Gets the Color "Orange4_1" (RGB 135,95,0). |
| static [OrangeRed1](Color/OrangeRed1.md) { get; } | Gets the Color "OrangeRed1" (RGB 255,95,0). |
| static [Orchid](Color/Orchid.md) { get; } | Gets the Color "Orchid" (RGB 215,95,215). |
| static [Orchid1](Color/Orchid1.md) { get; } | Gets the Color "Orchid1" (RGB 255,135,255). |
| static [Orchid2](Color/Orchid2.md) { get; } | Gets the Color "Orchid2" (RGB 255,135,215). |
| static [PaleGreen1](Color/PaleGreen1.md) { get; } | Gets the Color "PaleGreen1" (RGB 135,255,175). |
| static [PaleGreen1_1](Color/PaleGreen1_1.md) { get; } | Gets the Color "PaleGreen1_1" (RGB 175,255,135). |
| static [PaleGreen3](Color/PaleGreen3.md) { get; } | Gets the Color "PaleGreen3" (RGB 95,215,95). |
| static [PaleGreen3_1](Color/PaleGreen3_1.md) { get; } | Gets the Color "PaleGreen3_1" (RGB 135,215,135). |
| static [PaleTurquoise1](Color/PaleTurquoise1.md) { get; } | Gets the Color "PaleTurquoise1" (RGB 175,255,255). |
| static [PaleTurquoise4](Color/PaleTurquoise4.md) { get; } | Gets the Color "PaleTurquoise4" (RGB 95,135,135). |
| static [PaleVioletRed1](Color/PaleVioletRed1.md) { get; } | Gets the Color "PaleVioletRed1" (RGB 255,135,175). |
| static [Pink1](Color/Pink1.md) { get; } | Gets the Color "Pink1" (RGB 255,175,215). |
| static [Pink3](Color/Pink3.md) { get; } | Gets the Color "Pink3" (RGB 215,135,175). |
| static [Plum1](Color/Plum1.md) { get; } | Gets the Color "Plum1" (RGB 255,175,255). |
| static [Plum2](Color/Plum2.md) { get; } | Gets the Color "Plum2" (RGB 215,175,255). |
| static [Plum3](Color/Plum3.md) { get; } | Gets the Color "Plum3" (RGB 215,135,215). |
| static [Plum4](Color/Plum4.md) { get; } | Gets the Color "Plum4" (RGB 135,95,135). |
| static [Purple](Color/Purple.md) { get; } | Gets the Color "Purple" (RGB 128,0,128). |
| static [Purple3](Color/Purple3.md) { get; } | Gets the Color "Purple3" (RGB 95,0,215). |
| static [Purple4](Color/Purple4.md) { get; } | Gets the Color "Purple4" (RGB 95,0,135). |
| static [Purple4_1](Color/Purple4_1.md) { get; } | Gets the Color "Purple4_1" (RGB 95,0,175). |
| static [Purple_1](Color/Purple_1.md) { get; } | Gets the Color "Purple_1" (RGB 135,0,255). |
| static [Purple_2](Color/Purple_2.md) { get; } | Gets the Color "Purple_2" (RGB 175,0,255). |
| static [Red](Color/Red.md) { get; } | Gets the Color "Red" (RGB 255,0,0). |
| static [Red1](Color/Red1.md) { get; } | Gets the Color "Red1" (RGB 255,0,0). |
| static [Red3](Color/Red3.md) { get; } | Gets the Color "Red3" (RGB 175,0,0). |
| static [Red3_1](Color/Red3_1.md) { get; } | Gets the Color "Red3_1" (RGB 215,0,0). |
| static [RosyBrown](Color/RosyBrown.md) { get; } | Gets the Color "RosyBrown" (RGB 175,135,135). |
| static [RoyalBlue1](Color/RoyalBlue1.md) { get; } | Gets the Color "RoyalBlue1" (RGB 95,95,255). |
| static [Salmon1](Color/Salmon1.md) { get; } | Gets the Color "Salmon1" (RGB 255,135,95). |
| static [SandyBrown](Color/SandyBrown.md) { get; } | Gets the Color "SandyBrown" (RGB 255,175,95). |
| static [SeaGreen1](Color/SeaGreen1.md) { get; } | Gets the Color "SeaGreen1" (RGB 95,255,135). |
| static [SeaGreen1_1](Color/SeaGreen1_1.md) { get; } | Gets the Color "SeaGreen1_1" (RGB 95,255,175). |
| static [SeaGreen2](Color/SeaGreen2.md) { get; } | Gets the Color "SeaGreen2" (RGB 95,255,95). |
| static [SeaGreen3](Color/SeaGreen3.md) { get; } | Gets the Color "SeaGreen3" (RGB 95,215,135). |
| static [Silver](Color/Silver.md) { get; } | Gets the Color "Silver" (RGB 192,192,192). |
| static [SkyBlue1](Color/SkyBlue1.md) { get; } | Gets the Color "SkyBlue1" (RGB 135,215,255). |
| static [SkyBlue2](Color/SkyBlue2.md) { get; } | Gets the Color "SkyBlue2" (RGB 135,175,255). |
| static [SkyBlue3](Color/SkyBlue3.md) { get; } | Gets the Color "SkyBlue3" (RGB 95,175,215). |
| static [SlateBlue1](Color/SlateBlue1.md) { get; } | Gets the Color "SlateBlue1" (RGB 135,95,255). |
| static [SlateBlue3](Color/SlateBlue3.md) { get; } | Gets the Color "SlateBlue3" (RGB 95,95,175). |
| static [SlateBlue3_1](Color/SlateBlue3_1.md) { get; } | Gets the Color "SlateBlue3_1" (RGB 95,95,215). |
| static [SpringGreen1](Color/SpringGreen1.md) { get; } | Gets the Color "SpringGreen1" (RGB 0,255,135). |
| static [SpringGreen2](Color/SpringGreen2.md) { get; } | Gets the Color "SpringGreen2" (RGB 0,215,135). |
| static [SpringGreen2_1](Color/SpringGreen2_1.md) { get; } | Gets the Color "SpringGreen2_1" (RGB 0,255,95). |
| static [SpringGreen3](Color/SpringGreen3.md) { get; } | Gets the Color "SpringGreen3" (RGB 0,175,95). |
| static [SpringGreen3_1](Color/SpringGreen3_1.md) { get; } | Gets the Color "SpringGreen3_1" (RGB 0,215,95). |
| static [SpringGreen4](Color/SpringGreen4.md) { get; } | Gets the Color "SpringGreen4" (RGB 0,135,95). |
| static [SteelBlue](Color/SteelBlue.md) { get; } | Gets the Color "SteelBlue" (RGB 95,135,175). |
| static [SteelBlue1](Color/SteelBlue1.md) { get; } | Gets the Color "SteelBlue1" (RGB 95,175,255). |
| static [SteelBlue1_1](Color/SteelBlue1_1.md) { get; } | Gets the Color "SteelBlue1_1" (RGB 95,215,255). |
| static [SteelBlue3](Color/SteelBlue3.md) { get; } | Gets the Color "SteelBlue3" (RGB 95,135,215). |
| static [Tan](Color/Tan.md) { get; } | Gets the Color "Tan" (RGB 215,175,135). |
| static [Teal](Color/Teal.md) { get; } | Gets the Color "Teal" (RGB 0,128,128). |
| static [Thistle1](Color/Thistle1.md) { get; } | Gets the Color "Thistle1" (RGB 255,215,255). |
| static [Thistle3](Color/Thistle3.md) { get; } | Gets the Color "Thistle3" (RGB 215,175,215). |
| static [Turquoise2](Color/Turquoise2.md) { get; } | Gets the Color "Turquoise2" (RGB 0,215,255). |
| static [Turquoise4](Color/Turquoise4.md) { get; } | Gets the Color "Turquoise4" (RGB 0,135,135). |
| static [Violet](Color/Violet.md) { get; } | Gets the Color "Violet" (RGB 215,135,255). |
| static [Wheat1](Color/Wheat1.md) { get; } | Gets the Color "Wheat1" (RGB 255,255,175). |
| static [Wheat4](Color/Wheat4.md) { get; } | Gets the Color "Wheat4" (RGB 135,135,95). |
| static [White](Color/White.md) { get; } | Gets the Color "White" (RGB 255,255,255). |
| static [Yellow](Color/Yellow.md) { get; } | Gets the Color "Yellow" (RGB 255,255,0). |
| static [Yellow1](Color/Yellow1.md) { get; } | Gets the Color "Yellow1" (RGB 255,255,0). |
| static [Yellow2](Color/Yellow2.md) { get; } | Gets the Color "Yellow2" (RGB 215,255,0). |
| static [Yellow3](Color/Yellow3.md) { get; } | Gets the Color "Yellow3" (RGB 175,215,0). |
| static [Yellow3_1](Color/Yellow3_1.md) { get; } | Gets the Color "Yellow3_1" (RGB 215,215,0). |
| static [Yellow4](Color/Yellow4.md) { get; } | Gets the Color "Yellow4" (RGB 135,135,0). |
| static [Yellow4_1](Color/Yellow4_1.md) { get; } | Gets the Color "Yellow4_1" (RGB 135,175,0). |
| static [FromConsoleColor](Color/FromConsoleColor.md)(…) | Converts a ConsoleColor to a [`Color`](./Color.md). |
| static [FromHtml](Color/FromHtml.md)(…) | Parses a HTML hex color string (#RRGGBB) into a [`Color`](./Color.md). |
| static [FromInt32](Color/FromInt32.md)(…) | Creates a color from a palette index. |
| [B](Color/B.md) { get; } | Gets the blue component. |
| [G](Color/G.md) { get; } | Gets the green component. |
| [R](Color/R.md) { get; } | Gets the red component. |
| [Blend](Color/Blend.md)(…) | Blends (interpolates) this color with another color. |
| [Equals](Color/Equals.md)(…) | Determines whether this instance equals another [`Color`](./Color.md). |
| override [Equals](Color/Equals.md)(…) | Determines whether this instance equals another object. |
| override [GetHashCode](Color/GetHashCode.md)() |  |
| [GetInvertedColor](Color/GetInvertedColor.md)() | Gets a contrasting color (black or white) based on luminance for readability. |
| override [ToString](Color/ToString.md)() | Returns a textual representation, using the palette name if available, otherwise formatted as `#RRGGBB (RGB=R,G,B)`. |
| static [FromHex](Color/FromHex.md)(…) | Gets the hexadecimal (RRGGBB) representation of a color. |
| static [ToConsoleColor](Color/ToConsoleColor.md)(…) | Converts a [`Color`](./Color.md) to a ConsoleColor, approximating if necessary. |
| [operator ==](Color/op_Equality.md) | Determines whether two colors are equal. |
| [implicit operator](Color/op_Implicit.md) | Converts a Int32 to a [`Color`](./Color.md). (3 operators) |
| [operator !=](Color/op_Inequality.md) | Determines whether two colors are different. |

### Remarks

The optional internal `Number` corresponds to a color index in a known palette (e.g. standard/extended console colors). When present it enables efficient conversion to ConsoleColor or palette lookups. When absent the color is treated as a raw 24-bit RGB value.

### See Also

* namespace [PromptPlusLibrary](../PromptPlus.md)

<!-- DO NOT EDIT: generated by xmldocmd for PromptPlus.dll -->
