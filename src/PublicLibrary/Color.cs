// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core.Colors;
using System;
using System.Globalization;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents a Color RGB.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Color"/> struct.
    /// </remarks>
    /// <param name="red">The red component.</param>
    /// <param name="green">The green component.</param>
    /// <param name="blue">The blue component.</param>
    #pragma warning disable CA1707 // Identifiers should not contain underscores
    public readonly struct Color(byte red, byte green, byte blue) : IEquatable<Color>
    {
        /// <summary>
        /// Gets the default Color.
        /// </summary>
        public static Color DefaultBackColor => Console.BackgroundColor;

        /// <summary>
        /// Gets the default Color.
        /// </summary>
        public static Color DefaulForeColor => Console.ForegroundColor;

        /// <summary>
        /// Gets the red component.
        /// </summary>

        public byte R { get; } = red;

        /// <summary>
        /// Gets the green component.
        /// </summary>

        public byte G { get; } = green;

        /// <summary>
        /// Gets the blue component.
        /// </summary>

        public byte B { get; } = blue;

        /// <summary>
        /// Gets the number of the Color, if any.
        /// </summary>

        internal byte? Number { get; } = null;

        /// <summary>
        /// Blends two ColorRGBs.
        /// </summary>
        /// <param name="other">The other Color.</param>
        /// <param name="factor">The blend factor.</param>
        /// <returns>The resulting Color.</returns>
        public readonly Color Blend(Color other, float factor)
        {
            // https://github.com/willmcgugan/rich/blob/f092b1d04252e6f6812021c0f415dd1d7be6a16a/rich/Color.py#L494
            return new Color(
                (byte)(R + (other.R - R) * factor),
                (byte)(G + (other.G - G) * factor),
                (byte)(B + (other.B - B) * factor));
        }

        /// <summary>
        /// Get Inverted Color by Luminance for best contrast 
        /// </summary>
        /// <returns>
        /// <see cref="Color"/> White or Black
        /// </returns>
        public readonly Color GetInvertedColor()
        {
            return GetLuminance(this) < 140 ? White : Black;
        }

        private static float GetLuminance(Color ColorRGB)
        {
            return (float)(0.2126 * ColorRGB.R + 0.7152 * ColorRGB.G + 0.0722 * ColorRGB.B);
        }

        /// <summary>
        /// Gets the hexadecimal representation of the Color.
        /// </summary>
        /// <param name="value">The <see cref="Color"/></param>
        /// <returns>The hexadecimal representation of the Color.</returns>
        public static string FromHex(Color value)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}{1}{2}",
                value.R.ToString("X2", CultureInfo.InvariantCulture),
                value.G.ToString("X2", CultureInfo.InvariantCulture),
                value.B.ToString("X2", CultureInfo.InvariantCulture));
        }

        /// <inheritdoc/>
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(R, G, B);
        }

        /// <summary>
        /// Checks if <see cref="Color"/> are equal the instance.
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns><c>true</c> if the two ColorRGBs are equal, otherwise <c>false</c>.</returns>
        public override readonly bool Equals(object? obj)
        {
            return obj is Color ColorRGB && Equals(ColorRGB);
        }


        /// <summary>
        /// Checks if <see cref="Color"/> are equal the instance.
        /// </summary>
        /// <param name="other">The <see cref="Color"/></param>
        /// <returns><c>true</c> if the two ColorRGBs are equal, otherwise <c>false</c>.</returns>
        public readonly bool Equals(Color other)
        {
            return R == other.R && G == other.G && B == other.B;
        }

        /// <summary>
        /// Checks if two <see cref="Color"/> instances are equal.
        /// </summary>
        /// <param name="left">The first Color instance to compare.</param>
        /// <param name="right">The second Color instance to compare.</param>
        /// <returns><c>true</c> if the two ColorRGBs are equal, otherwise <c>false</c>.</returns>
        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks if two <see cref="Color"/> instances are different.
        /// </summary>
        /// <param name="left">The first Color instance to compare.</param>
        /// <param name="right">The second Color instance to compare.</param>
        /// <returns><c>true</c> if the two ColorRGBs are different, otherwise <c>false</c>.</returns>
        public static bool operator !=(Color left, Color right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Converts a <see cref="int"/> to a <see cref="Color"/>.
        /// </summary>
        /// <param name="number">The Color number to convert.</param>
        public static implicit operator Color(int number)
        {
            return FromInt32(number);
        }

        /// <summary>
        /// Converts a <see cref="ConsoleColor"/> to a <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="ConsoleColor"/> to convert.</param>

        public static implicit operator Color(ConsoleColor color)
        {
            return FromConsoleColor(color);
        }

        /// <summary>
        /// Converts a <see cref="Color"/> to a <see cref="ConsoleColor"/>.
        /// </summary>
        /// <param name="ColorRGB">The console Color to convert.</param>

        public static implicit operator ConsoleColor(Color ColorRGB)
        {
            return ToConsoleColor(ColorRGB);
        }

        /// <summary>
        /// Converts string Color Html format (#RRGGBB) into <see cref="Color"/>.
        /// </summary>
        /// <param name="value">The html Color to convert.</param>
        /// <returns>A <see cref="Color"/>.</returns>
        public static Color FromHtml(string value)
        {
            if (value == null || value.Length != 7 || !value.StartsWith('#'))
            {
                throw new ArgumentException("Invalid Html ColorRGB. Length must be equal 7 and start with #");
            }
            int RGBint = Convert.ToInt32(value.Substring(1, 6), 16);
            byte localRed = (byte)(RGBint >> 16 & 255);
            byte localGreen = (byte)(RGBint >> 8 & 255);
            byte localBlue = (byte)(RGBint & 255);
            return new Color(localRed, localGreen, localBlue);
        }

        /// <summary>
        /// Converts a <see cref="Color"/> to a <see cref="ConsoleColor"/>.
        /// </summary>
        /// <param name="ColorRGB">The Color to convert.</param>
        /// <returns>A <see cref="ConsoleColor"/> representing the <see cref="Color"/>.</returns>
        public static ConsoleColor ToConsoleColor(Color ColorRGB)
        {
            if (ColorRGB.Number == null || ColorRGB.Number.Value >= 16)
            {
                ColorRGB = ColorPalette.ExactOrClosest(ColorSystem.Standard, ColorRGB);
            }

            return ColorRGB.Number!.Value switch
            {
                0 => ConsoleColor.Black,
                1 => ConsoleColor.DarkRed,
                2 => ConsoleColor.DarkGreen,
                3 => ConsoleColor.DarkYellow,
                4 => ConsoleColor.DarkBlue,
                5 => ConsoleColor.DarkMagenta,
                6 => ConsoleColor.DarkCyan,
                7 => ConsoleColor.Gray,
                8 => ConsoleColor.DarkGray,
                9 => ConsoleColor.Red,
                10 => ConsoleColor.Green,
                11 => ConsoleColor.Yellow,
                12 => ConsoleColor.Blue,
                13 => ConsoleColor.Magenta,
                14 => ConsoleColor.Cyan,
                15 => ConsoleColor.White,
                _ => throw new ArgumentException($"Cannot convert ColorRGB to console ColorRGB. ColorRGB.Number: {ColorRGB.Number}"),
            };
        }

        /// <summary>
        /// Converts a Color number into a <see cref="Color"/>.
        /// </summary>
        /// <param name="number">The Color number.</param>
        /// <returns>The Color representing the specified Color number.</returns>
        public static Color FromInt32(int number)
        {
            return ColorTable.GetColorRGB(number);
        }

        /// <summary>
        /// Converts a <see cref="ConsoleColor"/> to a <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The Color to convert.</param>
        /// <returns>A <see cref="Color"/> representing the <see cref="ConsoleColor"/>.</returns>
        public static Color FromConsoleColor(ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Black => Black,
                ConsoleColor.Blue => Blue,
                ConsoleColor.Cyan => Aqua,
                ConsoleColor.DarkBlue => Navy,
                ConsoleColor.DarkCyan => Teal,
                ConsoleColor.DarkGray => Grey,
                ConsoleColor.DarkGreen => Green,
                ConsoleColor.DarkMagenta => Purple,
                ConsoleColor.DarkRed => Maroon,
                ConsoleColor.DarkYellow => Olive,
                ConsoleColor.Gray => Silver,
                ConsoleColor.Green => Lime,
                ConsoleColor.Magenta => Fuchsia,
                ConsoleColor.Red => Red,
                ConsoleColor.White => White,
                ConsoleColor.Yellow => Yellow,
                _ => throw new ArgumentException($"Cannot convert ConsoleColor to console ColorRGB. ConsoleColor: {color}"),
            };
        }

        /// <summary>
        /// Convert to string 
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override readonly string ToString()
        {

            if (Number != null)
            {
                string? name = ColorTable.GetName(Number.Value);
                if (!string.IsNullOrWhiteSpace(name))
                {
                    return name;
                }
            }
            return string.Format(CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2} (RGB={0},{1},{2})", R, G, B);
        }


        internal Color(byte number, byte red, byte green, byte blue)
            : this(red, green, blue)
        {
            Number = number;
        }

        /// <summary>
        /// Gets the Color "Black" (RGB 0,0,0).
        /// </summary>
        public static Color Black { get; } = new Color(0, 0, 0, 0);

        /// <summary>
        /// Gets the Color "Maroon" (RGB 128,0,0).
        /// </summary>
        public static Color Maroon { get; } = new Color(1, 128, 0, 0);

        /// <summary>
        /// Gets the Color "Green" (RGB 0,128,0).
        /// </summary>
        public static Color Green { get; } = new Color(2, 0, 128, 0);

        /// <summary>
        /// Gets the Color "Olive" (RGB 128,128,0).
        /// </summary>
        public static Color Olive { get; } = new Color(3, 128, 128, 0);

        /// <summary>
        /// Gets the Color "Navy" (RGB 0,0,128).
        /// </summary>
        public static Color Navy { get; } = new Color(4, 0, 0, 128);

        /// <summary>
        /// Gets the Color "Purple" (RGB 128,0,128).
        /// </summary>
        public static Color Purple { get; } = new Color(5, 128, 0, 128);

        /// <summary>
        /// Gets the Color "Teal" (RGB 0,128,128).
        /// </summary>
        public static Color Teal { get; } = new Color(6, 0, 128, 128);

        /// <summary>
        /// Gets the Color "Silver" (RGB 192,192,192).
        /// </summary>
        public static Color Silver { get; } = new Color(7, 192, 192, 192);

        /// <summary>
        /// Gets the Color "Grey" (RGB 128,128,128).
        /// </summary>
        public static Color Grey { get; } = new Color(8, 128, 128, 128);

        /// <summary>
        /// Gets the Color "Red" (RGB 255,0,0).
        /// </summary>
        public static Color Red { get; } = new Color(9, 255, 0, 0);

        /// <summary>
        /// Gets the Color "Lime" (RGB 0,255,0).
        /// </summary>
        public static Color Lime { get; } = new Color(10, 0, 255, 0);

        /// <summary>
        /// Gets the Color "Yellow" (RGB 255,255,0).
        /// </summary>
        public static Color Yellow { get; } = new Color(11, 255, 255, 0);

        /// <summary>
        /// Gets the Color "Blue" (RGB 0,0,255).
        /// </summary>
        public static Color Blue { get; } = new Color(12, 0, 0, 255);

        /// <summary>
        /// Gets the Color "Fuchsia" (RGB 255,0,255).
        /// </summary>
        public static Color Fuchsia { get; } = new Color(13, 255, 0, 255);

        /// <summary>
        /// Gets the Color "Aqua" (RGB 0,255,255).
        /// </summary>
        public static Color Aqua { get; } = new Color(14, 0, 255, 255);

        /// <summary>
        /// Gets the Color "White" (RGB 255,255,255).
        /// </summary>
        public static Color White { get; } = new Color(15, 255, 255, 255);

        /// <summary>
        /// Gets the Color "Grey0" (RGB 0,0,0).
        /// </summary>
        public static Color Grey0 { get; } = new Color(16, 0, 0, 0);

        /// <summary>
        /// Gets the Color "NavyBlue" (RGB 0,0,95).
        /// </summary>
        public static Color NavyBlue { get; } = new Color(17, 0, 0, 95);

        /// <summary>
        /// Gets the Color "DarkBlue" (RGB 0,0,135).
        /// </summary>
        public static Color DarkBlue { get; } = new Color(18, 0, 0, 135);

        /// <summary>
        /// Gets the Color "Blue3" (RGB 0,0,175).
        /// </summary>

        public static Color Blue3 { get; } = new Color(19, 0, 0, 175);

        /// <summary>
        /// Gets the Color "Blue3_1" (RGB 0,0,215).
        /// </summary>
        public static Color Blue3_1 { get; } = new Color(20, 0, 0, 215);

        /// <summary>
        /// Gets the Color "Blue1" (RGB 0,0,255).
        /// </summary>

        public static Color Blue1 { get; } = new Color(21, 0, 0, 255);

        /// <summary>
        /// Gets the Color "DarkGreen" (RGB 0,95,0).
        /// </summary>

        public static Color DarkGreen { get; } = new Color(22, 0, 95, 0);

        /// <summary>
        /// Gets the Color "DeepSkyBlue4" (RGB 0,95,95).
        /// </summary>

        public static Color DeepSkyBlue4 { get; } = new Color(23, 0, 95, 95);

        /// <summary>
        /// Gets the Color "DeepSkyBlue4_1" (RGB 0,95,135).
        /// </summary>

        public static Color DeepSkyBlue4_1 { get; } = new Color(24, 0, 95, 135);

        /// <summary>
        /// Gets the Color "DeepSkyBlue4_2" (RGB 0,95,175).
        /// </summary>

        public static Color DeepSkyBlue4_2 { get; } = new Color(25, 0, 95, 175);

        /// <summary>
        /// Gets the Color "DodgerBlue3" (RGB 0,95,215).
        /// </summary>

        public static Color DodgerBlue3 { get; } = new Color(26, 0, 95, 215);

        /// <summary>
        /// Gets the Color "DodgerBlue2" (RGB 0,95,255).
        /// </summary>

        public static Color DodgerBlue2 { get; } = new Color(27, 0, 95, 255);

        /// <summary>
        /// Gets the Color "Green4" (RGB 0,135,0).
        /// </summary>

        public static Color Green4 { get; } = new Color(28, 0, 135, 0);

        /// <summary>
        /// Gets the Color "SpringGreen4" (RGB 0,135,95).
        /// </summary>

        public static Color SpringGreen4 { get; } = new Color(29, 0, 135, 95);

        /// <summary>
        /// Gets the Color "Turquoise4" (RGB 0,135,135).
        /// </summary>

        public static Color Turquoise4 { get; } = new Color(30, 0, 135, 135);

        /// <summary>
        /// Gets the Color "DeepSkyBlue3" (RGB 0,135,175).
        /// </summary>

        public static Color DeepSkyBlue3 { get; } = new Color(31, 0, 135, 175);

        /// <summary>
        /// Gets the Color "DeepSkyBlue3_1" (RGB 0,135,215).
        /// </summary>
        public static Color DeepSkyBlue3_1 { get; } = new Color(32, 0, 135, 215);

        /// <summary>
        /// Gets the Color "DodgerBlue1" (RGB 0,135,255).
        /// </summary>

        public static Color DodgerBlue1 { get; } = new Color(33, 0, 135, 255);

        /// <summary>
        /// Gets the Color "Green3" (RGB 0,175,0).
        /// </summary>

        public static Color Green3 { get; } = new Color(34, 0, 175, 0);

        /// <summary>
        /// Gets the Color "SpringGreen3" (RGB 0,175,95).
        /// </summary>

        public static Color SpringGreen3 { get; } = new Color(35, 0, 175, 95);

        /// <summary>
        /// Gets the Color "DarkCyan" (RGB 0,175,135).
        /// </summary>

        public static Color DarkCyan { get; } = new Color(36, 0, 175, 135);

        /// <summary>
        /// Gets the Color "LightSeaGreen" (RGB 0,175,175).
        /// </summary>

        public static Color LightSeaGreen { get; } = new Color(37, 0, 175, 175);

        /// <summary>
        /// Gets the Color "DeepSkyBlue2" (RGB 0,175,215).
        /// </summary>

        public static Color DeepSkyBlue2 { get; } = new Color(38, 0, 175, 215);

        /// <summary>
        /// Gets the Color "DeepSkyBlue1" (RGB 0,175,255).
        /// </summary>

        public static Color DeepSkyBlue1 { get; } = new Color(39, 0, 175, 255);

        /// <summary>
        /// Gets the Color "Green3_1" (RGB 0,215,0).
        /// </summary>
        public static Color Green3_1 { get; } = new Color(40, 0, 215, 0);

        /// <summary>
        /// Gets the Color "SpringGreen3_1" (RGB 0,215,95).
        /// </summary>
        public static Color SpringGreen3_1 { get; } = new Color(41, 0, 215, 95);

        /// <summary>
        /// Gets the Color "SpringGreen2" (RGB 0,215,135).
        /// </summary>

        public static Color SpringGreen2 { get; } = new Color(42, 0, 215, 135);

        /// <summary>
        /// Gets the Color "Cyan3" (RGB 0,215,175).
        /// </summary>

        public static Color Cyan3 { get; } = new Color(43, 0, 215, 175);

        /// <summary>
        /// Gets the Color "DarkTurquoise" (RGB 0,215,215).
        /// </summary>

        public static Color DarkTurquoise { get; } = new Color(44, 0, 215, 215);

        /// <summary>
        /// Gets the Color "Turquoise2" (RGB 0,215,255).
        /// </summary>

        public static Color Turquoise2 { get; } = new Color(45, 0, 215, 255);

        /// <summary>
        /// Gets the Color "Green1" (RGB 0,255,0).
        /// </summary>

        public static Color Green1 { get; } = new Color(46, 0, 255, 0);

        /// <summary>
        /// Gets the Color "SpringGreen2_1" (RGB 0,255,95).
        /// </summary>
        public static Color SpringGreen2_1 { get; } = new Color(47, 0, 255, 95);

        /// <summary>
        /// Gets the Color "SpringGreen1" (RGB 0,255,135).
        /// </summary>

        public static Color SpringGreen1 { get; } = new Color(48, 0, 255, 135);

        /// <summary>
        /// Gets the Color "MediumSpringGreen" (RGB 0,255,175).
        /// </summary>

        public static Color MediumSpringGreen { get; } = new Color(49, 0, 255, 175);

        /// <summary>
        /// Gets the Color "Cyan2" (RGB 0,255,215).
        /// </summary>

        public static Color Cyan2 { get; } = new Color(50, 0, 255, 215);

        /// <summary>
        /// Gets the Color "Cyan1" (RGB 0,255,255).
        /// </summary>

        public static Color Cyan1 { get; } = new Color(51, 0, 255, 255);

        /// <summary>
        /// Gets the Color "DarkRed" (RGB 95,0,0).
        /// </summary>

        public static Color DarkRed { get; } = new Color(52, 95, 0, 0);

        /// <summary>
        /// Gets the Color "DeepPink4" (RGB 95,0,95).
        /// </summary>

        public static Color DeepPink4 { get; } = new Color(53, 95, 0, 95);

        /// <summary>
        /// Gets the Color "Purple4" (RGB 95,0,135).
        /// </summary>

        public static Color Purple4 { get; } = new Color(54, 95, 0, 135);

        /// <summary>
        /// Gets the Color "Purple4_1" (RGB 95,0,175).
        /// </summary>
        public static Color Purple4_1 { get; } = new Color(55, 95, 0, 175);

        /// <summary>
        /// Gets the Color "Purple3" (RGB 95,0,215).
        /// </summary>

        public static Color Purple3 { get; } = new Color(56, 95, 0, 215);

        /// <summary>
        /// Gets the Color "BlueViolet" (RGB 95,0,255).
        /// </summary>

        public static Color BlueViolet { get; } = new Color(57, 95, 0, 255);

        /// <summary>
        /// Gets the Color "Orange4" (RGB 95,95,0).
        /// </summary>

        public static Color Orange4 { get; } = new Color(58, 95, 95, 0);

        /// <summary>
        /// Gets the Color "Grey37" (RGB 95,95,95).
        /// </summary>

        public static Color Grey37 { get; } = new Color(59, 95, 95, 95);

        /// <summary>
        /// Gets the Color "MediumPurple4" (RGB 95,95,135).
        /// </summary>

        public static Color MediumPurple4 { get; } = new Color(60, 95, 95, 135);

        /// <summary>
        /// Gets the Color "SlateBlue3" (RGB 95,95,175).
        /// </summary>

        public static Color SlateBlue3 { get; } = new Color(61, 95, 95, 175);

        /// <summary>
        /// Gets the Color "SlateBlue3_1" (RGB 95,95,215).
        /// </summary>
        public static Color SlateBlue3_1 { get; } = new Color(62, 95, 95, 215);

        /// <summary>
        /// Gets the Color "RoyalBlue1" (RGB 95,95,255).
        /// </summary>
        public static Color RoyalBlue1 { get; } = new Color(63, 95, 95, 255);

        /// <summary>
        /// Gets the Color "Chartreuse4" (RGB 95,135,0).
        /// </summary>
        public static Color Chartreuse4 { get; } = new Color(64, 95, 135, 0);

        /// <summary>
        /// Gets the Color "DarkSeaGreen4" (RGB 95,135,95).
        /// </summary>
        public static Color DarkSeaGreen4 { get; } = new Color(65, 95, 135, 95);

        /// <summary>
        /// Gets the Color "PaleTurquoise4" (RGB 95,135,135).
        /// </summary>
        public static Color PaleTurquoise4 { get; } = new Color(66, 95, 135, 135);

        /// <summary>
        /// Gets the Color "SteelBlue" (RGB 95,135,175).
        /// </summary>
        public static Color SteelBlue { get; } = new Color(67, 95, 135, 175);

        /// <summary>
        /// Gets the Color "SteelBlue3" (RGB 95,135,215).
        /// </summary>
        public static Color SteelBlue3 { get; } = new Color(68, 95, 135, 215);

        /// <summary>
        /// Gets the Color "CornflowerBlue" (RGB 95,135,255).
        /// </summary>
        public static Color CornflowerBlue { get; } = new Color(69, 95, 135, 255);

        /// <summary>
        /// Gets the Color "Chartreuse3" (RGB 95,175,0).
        /// </summary>
        public static Color Chartreuse3 { get; } = new Color(70, 95, 175, 0);

        /// <summary>
        /// Gets the Color "DarkSeaGreen4_1" (RGB 95,175,95).
        /// </summary>
        public static Color DarkSeaGreen4_1 { get; } = new Color(71, 95, 175, 95);

        /// <summary>
        /// Gets the Color "CadetBlue" (RGB 95,175,135).
        /// </summary>
        public static Color CadetBlue { get; } = new Color(72, 95, 175, 135);

        /// <summary>
        /// Gets the Color "CadetBlue_1" (RGB 95,175,175).
        /// </summary>
        public static Color CadetBlue_1 { get; } = new Color(73, 95, 175, 175);

        /// <summary>
        /// Gets the Color "SkyBlue3" (RGB 95,175,215).
        /// </summary>
        public static Color SkyBlue3 { get; } = new Color(74, 95, 175, 215);

        /// <summary>
        /// Gets the Color "SteelBlue1" (RGB 95,175,255).
        /// </summary>
        public static Color SteelBlue1 { get; } = new Color(75, 95, 175, 255);

        /// <summary>
        /// Gets the Color "Chartreuse3_1" (RGB 95,215,0).
        /// </summary>
        public static Color Chartreuse3_1 { get; } = new Color(76, 95, 215, 0);

        /// <summary>
        /// Gets the Color "PaleGreen3" (RGB 95,215,95).
        /// </summary>
        public static Color PaleGreen3 { get; } = new Color(77, 95, 215, 95);

        /// <summary>
        /// Gets the Color "SeaGreen3" (RGB 95,215,135).
        /// </summary>
        public static Color SeaGreen3 { get; } = new Color(78, 95, 215, 135);

        /// <summary>
        /// Gets the Color "Aquamarine3" (RGB 95,215,175).
        /// </summary>
        public static Color Aquamarine3 { get; } = new Color(79, 95, 215, 175);

        /// <summary>
        /// Gets the Color "MediumTurquoise" (RGB 95,215,215).
        /// </summary>
        public static Color MediumTurquoise { get; } = new Color(80, 95, 215, 215);

        /// <summary>
        /// Gets the Color "SteelBlue1_1" (RGB 95,215,255).
        /// </summary>
        public static Color SteelBlue1_1 { get; } = new Color(81, 95, 215, 255);

        /// <summary>
        /// Gets the Color "Chartreuse2" (RGB 95,255,0).
        /// </summary>
        public static Color Chartreuse2 { get; } = new Color(82, 95, 255, 0);

        /// <summary>
        /// Gets the Color "SeaGreen2" (RGB 95,255,95).
        /// </summary>
        public static Color SeaGreen2 { get; } = new Color(83, 95, 255, 95);

        /// <summary>
        /// Gets the Color "SeaGreen1" (RGB 95,255,135).
        /// </summary>
        public static Color SeaGreen1 { get; } = new Color(84, 95, 255, 135);

        /// <summary>
        /// Gets the Color "SeaGreen1_1" (RGB 95,255,175).
        /// </summary>
        public static Color SeaGreen1_1 { get; } = new Color(85, 95, 255, 175);

        /// <summary>
        /// Gets the Color "Aquamarine1" (RGB 95,255,215).
        /// </summary>
        public static Color Aquamarine1 { get; } = new Color(86, 95, 255, 215);

        /// <summary>
        /// Gets the Color "DarkSlateGray2" (RGB 95,255,255).
        /// </summary>
        public static Color DarkSlateGray2 { get; } = new Color(87, 95, 255, 255);

        /// <summary>
        /// Gets the Color "DarkRed_1" (RGB 135,0,0).
        /// </summary>
        public static Color DarkRed_1 { get; } = new Color(88, 135, 0, 0);

        /// <summary>
        /// Gets the Color "DeepPink4_1" (RGB 135,0,95).
        /// </summary>
        public static Color DeepPink4_1 { get; } = new Color(89, 135, 0, 95);

        /// <summary>
        /// Gets the Color "DarkMagenta" (RGB 135,0,135).
        /// </summary>
        public static Color DarkMagenta { get; } = new Color(90, 135, 0, 135);

        /// <summary>
        /// Gets the Color "DarkMagenta_1" (RGB 135,0,175).
        /// </summary>
        public static Color DarkMagenta_1 { get; } = new Color(91, 135, 0, 175);

        /// <summary>
        /// Gets the Color "DarkViolet" (RGB 135,0,215).
        /// </summary>
        public static Color DarkViolet { get; } = new Color(92, 135, 0, 215);

        /// <summary>
        /// Gets the Color "Purple_1" (RGB 135,0,255).
        /// </summary>
        public static Color Purple_1 { get; } = new Color(93, 135, 0, 255);

        /// <summary>
        /// Gets the Color "Orange4_1" (RGB 135,95,0).
        /// </summary>
        public static Color Orange4_1 { get; } = new Color(94, 135, 95, 0);

        /// <summary>
        /// Gets the Color "LightPink4" (RGB 135,95,95).
        /// </summary>
        public static Color LightPink4 { get; } = new Color(95, 135, 95, 95);

        /// <summary>
        /// Gets the Color "Plum4" (RGB 135,95,135).
        /// </summary>
        public static Color Plum4 { get; } = new Color(96, 135, 95, 135);

        /// <summary>
        /// Gets the Color "MediumPurple3" (RGB 135,95,175).
        /// </summary>
        public static Color MediumPurple3 { get; } = new Color(97, 135, 95, 175);

        /// <summary>
        /// Gets the Color "MediumPurple3_1" (RGB 135,95,215).
        /// </summary>
        public static Color MediumPurple3_1 { get; } = new Color(98, 135, 95, 215);

        /// <summary>
        /// Gets the Color "SlateBlue1" (RGB 135,95,255).
        /// </summary>
        public static Color SlateBlue1 { get; } = new Color(99, 135, 95, 255);

        /// <summary>
        /// Gets the Color "Yellow4" (RGB 135,135,0).
        /// </summary>
        public static Color Yellow4 { get; } = new Color(100, 135, 135, 0);

        /// <summary>
        /// Gets the Color "Wheat4" (RGB 135,135,95).
        /// </summary>
        public static Color Wheat4 { get; } = new Color(101, 135, 135, 95);

        /// <summary>
        /// Gets the Color "Grey53" (RGB 135,135,135).
        /// </summary>
        public static Color Grey53 { get; } = new Color(102, 135, 135, 135);

        /// <summary>
        /// Gets the Color "LightSlateGrey" (RGB 135,135,175).
        /// </summary>
        public static Color LightSlateGrey { get; } = new Color(103, 135, 135, 175);

        /// <summary>
        /// Gets the Color "MediumPurple" (RGB 135,135,215).
        /// </summary>
        public static Color MediumPurple { get; } = new Color(104, 135, 135, 215);

        /// <summary>
        /// Gets the Color "LightSlateBlue" (RGB 135,135,255).
        /// </summary>
        public static Color LightSlateBlue { get; } = new Color(105, 135, 135, 255);

        /// <summary>
        /// Gets the Color "Yellow4_1" (RGB 135,175,0).
        /// </summary>
        public static Color Yellow4_1 { get; } = new Color(106, 135, 175, 0);

        /// <summary>
        /// Gets the Color "DarkOliveGreen3" (RGB 135,175,95).
        /// </summary>
        public static Color DarkOliveGreen3 { get; } = new Color(107, 135, 175, 95);

        /// <summary>
        /// Gets the Color "DarkSeaGreen" (RGB 135,175,135).
        /// </summary>
        public static Color DarkSeaGreen { get; } = new Color(108, 135, 175, 135);

        /// <summary>
        /// Gets the Color "LightSkyBlue3" (RGB 135,175,175).
        /// </summary>
        public static Color LightSkyBlue3 { get; } = new Color(109, 135, 175, 175);

        /// <summary>
        /// Gets the Color "LightSkyBlue3_1" (RGB 135,175,215).
        /// </summary>
        public static Color LightSkyBlue3_1 { get; } = new Color(110, 135, 175, 215);

        /// <summary>
        /// Gets the Color "SkyBlue2" (RGB 135,175,255).
        /// </summary>
        public static Color SkyBlue2 { get; } = new Color(111, 135, 175, 255);

        /// <summary>
        /// Gets the Color "Chartreuse2_1" (RGB 135,215,0).
        /// </summary>
        public static Color Chartreuse2_1 { get; } = new Color(112, 135, 215, 0);

        /// <summary>
        /// Gets the Color "DarkOliveGreen3_1" (RGB 135,215,95).
        /// </summary>
        public static Color DarkOliveGreen3_1 { get; } = new Color(113, 135, 215, 95);

        /// <summary>
        /// Gets the Color "PaleGreen3_1" (RGB 135,215,135).
        /// </summary>
        public static Color PaleGreen3_1 { get; } = new Color(114, 135, 215, 135);

        /// <summary>
        /// Gets the Color "DarkSeaGreen3" (RGB 135,215,175).
        /// </summary>
        public static Color DarkSeaGreen3 { get; } = new Color(115, 135, 215, 175);

        /// <summary>
        /// Gets the Color "DarkSlateGray3" (RGB 135,215,215).
        /// </summary>
        public static Color DarkSlateGray3 { get; } = new Color(116, 135, 215, 215);

        /// <summary>
        /// Gets the Color "SkyBlue1" (RGB 135,215,255).
        /// </summary>
        public static Color SkyBlue1 { get; } = new Color(117, 135, 215, 255);

        /// <summary>
        /// Gets the Color "Chartreuse1" (RGB 135,255,0).
        /// </summary>
        public static Color Chartreuse1 { get; } = new Color(118, 135, 255, 0);

        /// <summary>
        /// Gets the Color "LightGreen" (RGB 135,255,95).
        /// </summary>
        public static Color LightGreen { get; } = new Color(119, 135, 255, 95);

        /// <summary>
        /// Gets the Color "LightGreen_1" (RGB 135,255,135).
        /// </summary>
        public static Color LightGreen_1 { get; } = new Color(120, 135, 255, 135);

        /// <summary>
        /// Gets the Color "PaleGreen1" (RGB 135,255,175).
        /// </summary>
        public static Color PaleGreen1 { get; } = new Color(121, 135, 255, 175);

        /// <summary>
        /// Gets the Color "Aquamarine1_1" (RGB 135,255,215).
        /// </summary>
        public static Color Aquamarine1_1 { get; } = new Color(122, 135, 255, 215);

        /// <summary>
        /// Gets the Color "DarkSlateGray1" (RGB 135,255,255).
        /// </summary>
        public static Color DarkSlateGray1 { get; } = new Color(123, 135, 255, 255);

        /// <summary>
        /// Gets the Color "Red3" (RGB 175,0,0).
        /// </summary>
        public static Color Red3 { get; } = new Color(124, 175, 0, 0);

        /// <summary>
        /// Gets the Color "DeepPink4_2" (RGB 175,0,95).
        /// </summary>
        public static Color DeepPink4_2 { get; } = new Color(125, 175, 0, 95);

        /// <summary>
        /// Gets the Color "MediumVioletRed" (RGB 175,0,135).
        /// </summary>
        public static Color MediumVioletRed { get; } = new Color(126, 175, 0, 135);

        /// <summary>
        /// Gets the Color "Magenta3" (RGB 175,0,175).
        /// </summary>
        public static Color Magenta3 { get; } = new Color(127, 175, 0, 175);

        /// <summary>
        /// Gets the Color "DarkViolet_1" (RGB 175,0,215).
        /// </summary>
        public static Color DarkViolet_1 { get; } = new Color(128, 175, 0, 215);

        /// <summary>
        /// Gets the Color "Purple_2" (RGB 175,0,255).
        /// </summary>
        public static Color Purple_2 { get; } = new Color(129, 175, 0, 255);

        /// <summary>
        /// Gets the Color "DarkOrange3" (RGB 175,95,0).
        /// </summary>
        public static Color DarkOrange3 { get; } = new Color(130, 175, 95, 0);

        /// <summary>
        /// Gets the Color "IndianRed" (RGB 175,95,95).
        /// </summary>
        public static Color IndianRed { get; } = new Color(131, 175, 95, 95);

        /// <summary>
        /// Gets the Color "HotPink3" (RGB 175,95,135).
        /// </summary>
        public static Color HotPink3 { get; } = new Color(132, 175, 95, 135);

        /// <summary>
        /// Gets the Color "MediumOrchid3" (RGB 175,95,175).
        /// </summary>
        public static Color MediumOrchid3 { get; } = new Color(133, 175, 95, 175);

        /// <summary>
        /// Gets the Color "MediumOrchid" (RGB 175,95,215).
        /// </summary>
        public static Color MediumOrchid { get; } = new Color(134, 175, 95, 215);

        /// <summary>
        /// Gets the Color "MediumPurple2" (RGB 175,95,255).
        /// </summary>
        public static Color MediumPurple2 { get; } = new Color(135, 175, 95, 255);

        /// <summary>
        /// Gets the Color "DarkGoldenrod" (RGB 175,135,0).
        /// </summary>
        public static Color DarkGoldenrod { get; } = new Color(136, 175, 135, 0);

        /// <summary>
        /// Gets the Color "LightSalmon3" (RGB 175,135,95).
        /// </summary>
        public static Color LightSalmon3 { get; } = new Color(137, 175, 135, 95);

        /// <summary>
        /// Gets the Color "RosyBrown" (RGB 175,135,135).
        /// </summary>
        public static Color RosyBrown { get; } = new Color(138, 175, 135, 135);

        /// <summary>
        /// Gets the Color "Grey63" (RGB 175,135,175).
        /// </summary>
        public static Color Grey63 { get; } = new Color(139, 175, 135, 175);

        /// <summary>
        /// Gets the Color "MediumPurple2_1" (RGB 175,135,215).
        /// </summary>
        public static Color MediumPurple2_1 { get; } = new Color(140, 175, 135, 215);

        /// <summary>
        /// Gets the Color "MediumPurple1" (RGB 175,135,255).
        /// </summary>
        public static Color MediumPurple1 { get; } = new Color(141, 175, 135, 255);

        /// <summary>
        /// Gets the Color "Gold3" (RGB 175,175,0).
        /// </summary>
        public static Color Gold3 { get; } = new Color(142, 175, 175, 0);

        /// <summary>
        /// Gets the Color "DarkKhaki" (RGB 175,175,95).
        /// </summary>
        public static Color DarkKhaki { get; } = new Color(143, 175, 175, 95);

        /// <summary>
        /// Gets the Color "NavajoWhite3" (RGB 175,175,135).
        /// </summary>
        public static Color NavajoWhite3 { get; } = new Color(144, 175, 175, 135);

        /// <summary>
        /// Gets the Color "Grey69" (RGB 175,175,175).
        /// </summary>
        public static Color Grey69 { get; } = new Color(145, 175, 175, 175);

        /// <summary>
        /// Gets the Color "LightSteelBlue3" (RGB 175,175,215).
        /// </summary>
        public static Color LightSteelBlue3 { get; } = new Color(146, 175, 175, 215);

        /// <summary>
        /// Gets the Color "LightSteelBlue" (RGB 175,175,255).
        /// </summary>
        public static Color LightSteelBlue { get; } = new Color(147, 175, 175, 255);

        /// <summary>
        /// Gets the Color "Yellow3" (RGB 175,215,0).
        /// </summary>
        public static Color Yellow3 { get; } = new Color(148, 175, 215, 0);

        /// <summary>
        /// Gets the Color "DarkOliveGreen3_2" (RGB 175,215,95).
        /// </summary>
        public static Color DarkOliveGreen3_2 { get; } = new Color(149, 175, 215, 95);

        /// <summary>
        /// Gets the Color "DarkSeaGreen3_1" (RGB 175,215,135).
        /// </summary>
        public static Color DarkSeaGreen3_1 { get; } = new Color(150, 175, 215, 135);

        /// <summary>
        /// Gets the Color "DarkSeaGreen2" (RGB 175,215,175).
        /// </summary>
        public static Color DarkSeaGreen2 { get; } = new Color(151, 175, 215, 175);

        /// <summary>
        /// Gets the Color "LightCyan3" (RGB 175,215,215).
        /// </summary>
        public static Color LightCyan3 { get; } = new Color(152, 175, 215, 215);

        /// <summary>
        /// Gets the Color "LightSkyBlue1" (RGB 175,215,255).
        /// </summary>
        public static Color LightSkyBlue1 { get; } = new Color(153, 175, 215, 255);

        /// <summary>
        /// Gets the Color "GreenYellow" (RGB 175,255,0).
        /// </summary>
        public static Color GreenYellow { get; } = new Color(154, 175, 255, 0);

        /// <summary>
        /// Gets the Color "DarkOliveGreen2" (RGB 175,255,95).
        /// </summary>
        public static Color DarkOliveGreen2 { get; } = new Color(155, 175, 255, 95);

        /// <summary>
        /// Gets the Color "PaleGreen1_1" (RGB 175,255,135).
        /// </summary>
        public static Color PaleGreen1_1 { get; } = new Color(156, 175, 255, 135);

        /// <summary>
        /// Gets the Color "DarkSeaGreen2_1" (RGB 175,255,175).
        /// </summary>
        public static Color DarkSeaGreen2_1 { get; } = new Color(157, 175, 255, 175);

        /// <summary>
        /// Gets the Color "DarkSeaGreen1" (RGB 175,255,215).
        /// </summary>
        public static Color DarkSeaGreen1 { get; } = new Color(158, 175, 255, 215);

        /// <summary>
        /// Gets the Color "PaleTurquoise1" (RGB 175,255,255).
        /// </summary>
        public static Color PaleTurquoise1 { get; } = new Color(159, 175, 255, 255);

        /// <summary>
        /// Gets the Color "Red3_1" (RGB 215,0,0).
        /// </summary>
        public static Color Red3_1 { get; } = new Color(160, 215, 0, 0);

        /// <summary>
        /// Gets the Color "DeepPink3" (RGB 215,0,95).
        /// </summary>

        public static Color DeepPink3 { get; } = new Color(161, 215, 0, 95);

        /// <summary>
        /// Gets the Color "DeepPink3_1" (RGB 215,0,135).
        /// </summary>
        public static Color DeepPink3_1 { get; } = new Color(162, 215, 0, 135);

        /// <summary>
        /// Gets the Color "Magenta3_1" (RGB 215,0,175).
        /// </summary>
        public static Color Magenta3_1 { get; } = new Color(163, 215, 0, 175);

        /// <summary>
        /// Gets the Color "Magenta3_2" (RGB 215,0,215).
        /// </summary>
        public static Color Magenta3_2 { get; } = new Color(164, 215, 0, 215);

        /// <summary>
        /// Gets the Color "Magenta2" (RGB 215,0,255).
        /// </summary>

        public static Color Magenta2 { get; } = new Color(165, 215, 0, 255);

        /// <summary>
        /// Gets the Color "DarkOrange3_1" (RGB 215,95,0).
        /// </summary>
        public static Color DarkOrange3_1 { get; } = new Color(166, 215, 95, 0);

        /// <summary>
        /// Gets the Color "IndianRed_1" (RGB 215,95,95).
        /// </summary>
        public static Color IndianRed_1 { get; } = new Color(167, 215, 95, 95);

        /// <summary>
        /// Gets the Color "HotPink3_1" (RGB 215,95,135).
        /// </summary>
        public static Color HotPink3_1 { get; } = new Color(168, 215, 95, 135);

        /// <summary>
        /// Gets the Color "HotPink2" (RGB 215,95,175).
        /// </summary>

        public static Color HotPink2 { get; } = new Color(169, 215, 95, 175);

        /// <summary>
        /// Gets the Color "Orchid" (RGB 215,95,215).
        /// </summary>

        public static Color Orchid { get; } = new Color(170, 215, 95, 215);

        /// <summary>
        /// Gets the Color "MediumOrchid1" (RGB 215,95,255).
        /// </summary>

        public static Color MediumOrchid1 { get; } = new Color(171, 215, 95, 255);

        /// <summary>
        /// Gets the Color "Orange3" (RGB 215,135,0).
        /// </summary>

        public static Color Orange3 { get; } = new Color(172, 215, 135, 0);

        /// <summary>
        /// Gets the Color "LightSalmon3_1" (RGB 215,135,95).
        /// </summary>
        public static Color LightSalmon3_1 { get; } = new Color(173, 215, 135, 95);

        /// <summary>
        /// Gets the Color "LightPink3" (RGB 215,135,135).
        /// </summary>

        public static Color LightPink3 { get; } = new Color(174, 215, 135, 135);

        /// <summary>
        /// Gets the Color "Pink3" (RGB 215,135,175).
        /// </summary>

        public static Color Pink3 { get; } = new Color(175, 215, 135, 175);

        /// <summary>
        /// Gets the Color "Plum3" (RGB 215,135,215).
        /// </summary>

        public static Color Plum3 { get; } = new Color(176, 215, 135, 215);

        /// <summary>
        /// Gets the Color "Violet" (RGB 215,135,255).
        /// </summary>

        public static Color Violet { get; } = new Color(177, 215, 135, 255);

        /// <summary>
        /// Gets the Color "Gold3_1" (RGB 215,175,0).
        /// </summary>
        public static Color Gold3_1 { get; } = new Color(178, 215, 175, 0);

        /// <summary>
        /// Gets the Color "LightGoldenrod3" (RGB 215,175,95).
        /// </summary>

        public static Color LightGoldenrod3 { get; } = new Color(179, 215, 175, 95);

        /// <summary>
        /// Gets the Color "Tan" (RGB 215,175,135).
        /// </summary>

        public static Color Tan { get; } = new Color(180, 215, 175, 135);

        /// <summary>
        /// Gets the Color "MistyRose3" (RGB 215,175,175).
        /// </summary>

        public static Color MistyRose3 { get; } = new Color(181, 215, 175, 175);

        /// <summary>
        /// Gets the Color "Thistle3" (RGB 215,175,215).
        /// </summary>

        public static Color Thistle3 { get; } = new Color(182, 215, 175, 215);

        /// <summary>
        /// Gets the Color "Plum2" (RGB 215,175,255).
        /// </summary>

        public static Color Plum2 { get; } = new Color(183, 215, 175, 255);

        /// <summary>
        /// Gets the Color "Yellow3_1" (RGB 215,215,0).
        /// </summary>
        public static Color Yellow3_1 { get; } = new Color(184, 215, 215, 0);

        /// <summary>
        /// Gets the Color "Khaki3" (RGB 215,215,95).
        /// </summary>

        public static Color Khaki3 { get; } = new Color(185, 215, 215, 95);

        /// <summary>
        /// Gets the Color "LightGoldenrod2" (RGB 215,215,135).
        /// </summary>

        public static Color LightGoldenrod2 { get; } = new Color(186, 215, 215, 135);

        /// <summary>
        /// Gets the Color "LightYellow3" (RGB 215,215,175).
        /// </summary>

        public static Color LightYellow3 { get; } = new Color(187, 215, 215, 175);

        /// <summary>
        /// Gets the Color "Grey84" (RGB 215,215,215).
        /// </summary>

        public static Color Grey84 { get; } = new Color(188, 215, 215, 215);

        /// <summary>
        /// Gets the Color "LightSteelBlue1" (RGB 215,215,255).
        /// </summary>

        public static Color LightSteelBlue1 { get; } = new Color(189, 215, 215, 255);

        /// <summary>
        /// Gets the Color "Yellow2" (RGB 215,255,0).
        /// </summary>

        public static Color Yellow2 { get; } = new Color(190, 215, 255, 0);

        /// <summary>
        /// Gets the Color "DarkOliveGreen1" (RGB 215,255,95).
        /// </summary>

        public static Color DarkOliveGreen1 { get; } = new Color(191, 215, 255, 95);

        /// <summary>
        /// Gets the Color "DarkOliveGreen1_1" (RGB 215,255,135).
        /// </summary>
        public static Color DarkOliveGreen1_1 { get; } = new Color(192, 215, 255, 135);

        /// <summary>
        /// Gets the Color "DarkSeaGreen1_1" (RGB 215,255,175).
        /// </summary>
        public static Color DarkSeaGreen1_1 { get; } = new Color(193, 215, 255, 175);

        /// <summary>
        /// Gets the Color "Honeydew2" (RGB 215,255,215).
        /// </summary>

        public static Color Honeydew2 { get; } = new Color(194, 215, 255, 215);

        /// <summary>
        /// Gets the Color "LightCyan1" (RGB 215,255,255).
        /// </summary>

        public static Color LightCyan1 { get; } = new Color(195, 215, 255, 255);

        /// <summary>
        /// Gets the Color "Red1" (RGB 255,0,0).
        /// </summary>

        public static Color Red1 { get; } = new Color(196, 255, 0, 0);

        /// <summary>
        /// Gets the Color "DeepPink2" (RGB 255,0,95).
        /// </summary>

        public static Color DeepPink2 { get; } = new Color(197, 255, 0, 95);

        /// <summary>
        /// Gets the Color "DeepPink1" (RGB 255,0,135).
        /// </summary>

        public static Color DeepPink1 { get; } = new Color(198, 255, 0, 135);

        /// <summary>
        /// Gets the Color "DeepPink1_1" (RGB 255,0,175).
        /// </summary>
        public static Color DeepPink1_1 { get; } = new Color(199, 255, 0, 175);

        /// <summary>
        /// Gets the Color "Magenta2_1" (RGB 255,0,215).
        /// </summary>
        public static Color Magenta2_1 { get; } = new Color(200, 255, 0, 215);

        /// <summary>
        /// Gets the Color "Magenta1" (RGB 255,0,255).
        /// </summary>

        public static Color Magenta1 { get; } = new Color(201, 255, 0, 255);

        /// <summary>
        /// Gets the Color "OrangeRed1" (RGB 255,95,0).
        /// </summary>

        public static Color OrangeRed1 { get; } = new Color(202, 255, 95, 0);

        /// <summary>
        /// Gets the Color "IndianRed1" (RGB 255,95,95).
        /// </summary>

        public static Color IndianRed1 { get; } = new Color(203, 255, 95, 95);

        /// <summary>
        /// Gets the Color "IndianRed1_1" (RGB 255,95,135).
        /// </summary>
        public static Color IndianRed1_1 { get; } = new Color(204, 255, 95, 135);

        /// <summary>
        /// Gets the Color "HotPink" (RGB 255,95,175).
        /// </summary>

        public static Color HotPink { get; } = new Color(205, 255, 95, 175);

        /// <summary>
        /// Gets the Color "HotPink_1" (RGB 255,95,215).
        /// </summary>
        public static Color HotPink_1 { get; } = new Color(206, 255, 95, 215);

        /// <summary>
        /// Gets the Color "MediumOrchid1_1" (RGB 255,95,255).
        /// </summary>
        public static Color MediumOrchid1_1 { get; } = new Color(207, 255, 95, 255);

        /// <summary>
        /// Gets the Color "DarkOrange" (RGB 255,135,0).
        /// </summary>

        public static Color DarkOrange { get; } = new Color(208, 255, 135, 0);

        /// <summary>
        /// Gets the Color "Salmon1" (RGB 255,135,95).
        /// </summary>

        public static Color Salmon1 { get; } = new Color(209, 255, 135, 95);

        /// <summary>
        /// Gets the Color "LightCoral" (RGB 255,135,135).
        /// </summary>

        public static Color LightCoral { get; } = new Color(210, 255, 135, 135);

        /// <summary>
        /// Gets the Color "PaleVioletRed1" (RGB 255,135,175).
        /// </summary>

        public static Color PaleVioletRed1 { get; } = new Color(211, 255, 135, 175);

        /// <summary>
        /// Gets the Color "Orchid2" (RGB 255,135,215).
        /// </summary>

        public static Color Orchid2 { get; } = new Color(212, 255, 135, 215);

        /// <summary>
        /// Gets the Color "Orchid1" (RGB 255,135,255).
        /// </summary>

        public static Color Orchid1 { get; } = new Color(213, 255, 135, 255);

        /// <summary>
        /// Gets the Color "Orange1" (RGB 255,175,0).
        /// </summary>

        public static Color Orange1 { get; } = new Color(214, 255, 175, 0);

        /// <summary>
        /// Gets the Color "SandyBrown" (RGB 255,175,95).
        /// </summary>

        public static Color SandyBrown { get; } = new Color(215, 255, 175, 95);

        /// <summary>
        /// Gets the Color "LightSalmon1" (RGB 255,175,135).
        /// </summary>

        public static Color LightSalmon1 { get; } = new Color(216, 255, 175, 135);

        /// <summary>
        /// Gets the Color "LightPink1" (RGB 255,175,175).
        /// </summary>

        public static Color LightPink1 { get; } = new Color(217, 255, 175, 175);

        /// <summary>
        /// Gets the Color "Pink1" (RGB 255,175,215).
        /// </summary>

        public static Color Pink1 { get; } = new Color(218, 255, 175, 215);

        /// <summary>
        /// Gets the Color "Plum1" (RGB 255,175,255).
        /// </summary>

        public static Color Plum1 { get; } = new Color(219, 255, 175, 255);

        /// <summary>
        /// Gets the Color "Gold1" (RGB 255,215,0).
        /// </summary>

        public static Color Gold1 { get; } = new Color(220, 255, 215, 0);

        /// <summary>
        /// Gets the Color "LightGoldenrod2_1" (RGB 255,215,95).
        /// </summary>
        public static Color LightGoldenrod2_1 { get; } = new Color(221, 255, 215, 95);

        /// <summary>
        /// Gets the Color "LightGoldenrod2_2" (RGB 255,215,135).
        /// </summary>
        public static Color LightGoldenrod2_2 { get; } = new Color(222, 255, 215, 135);

        /// <summary>
        /// Gets the Color "NavajoWhite1" (RGB 255,215,175).
        /// </summary>

        public static Color NavajoWhite1 { get; } = new Color(223, 255, 215, 175);

        /// <summary>
        /// Gets the Color "MistyRose1" (RGB 255,215,215).
        /// </summary>

        public static Color MistyRose1 { get; } = new Color(224, 255, 215, 215);

        /// <summary>
        /// Gets the Color "Thistle1" (RGB 255,215,255).
        /// </summary>

        public static Color Thistle1 { get; } = new Color(225, 255, 215, 255);

        /// <summary>
        /// Gets the Color "Yellow1" (RGB 255,255,0).
        /// </summary>

        public static Color Yellow1 { get; } = new Color(226, 255, 255, 0);

        /// <summary>
        /// Gets the Color "LightGoldenrod1" (RGB 255,255,95).
        /// </summary>

        public static Color LightGoldenrod1 { get; } = new Color(227, 255, 255, 95);

        /// <summary>
        /// Gets the Color "Khaki1" (RGB 255,255,135).
        /// </summary>

        public static Color Khaki1 { get; } = new Color(228, 255, 255, 135);

        /// <summary>
        /// Gets the Color "Wheat1" (RGB 255,255,175).
        /// </summary>

        public static Color Wheat1 { get; } = new Color(229, 255, 255, 175);

        /// <summary>
        /// Gets the Color "Cornsilk1" (RGB 255,255,215).
        /// </summary>

        public static Color Cornsilk1 { get; } = new Color(230, 255, 255, 215);

        /// <summary>
        /// Gets the Color "Grey100" (RGB 255,255,255).
        /// </summary>

        public static Color Grey100 { get; } = new Color(231, 255, 255, 255);

        /// <summary>
        /// Gets the Color "Grey3" (RGB 8,8,8).
        /// </summary>

        public static Color Grey3 { get; } = new Color(232, 8, 8, 8);

        /// <summary>
        /// Gets the Color "Grey7" (RGB 18,18,18).
        /// </summary>

        public static Color Grey7 { get; } = new Color(233, 18, 18, 18);

        /// <summary>
        /// Gets the Color "Grey11" (RGB 28,28,28).
        /// </summary>

        public static Color Grey11 { get; } = new Color(234, 28, 28, 28);

        /// <summary>
        /// Gets the Color "Grey15" (RGB 38,38,38).
        /// </summary>

        public static Color Grey15 { get; } = new Color(235, 38, 38, 38);

        /// <summary>
        /// Gets the Color "Grey19" (RGB 48,48,48).
        /// </summary>

        public static Color Grey19 { get; } = new Color(236, 48, 48, 48);

        /// <summary>
        /// Gets the Color "Grey23" (RGB 58,58,58).
        /// </summary>

        public static Color Grey23 { get; } = new Color(237, 58, 58, 58);

        /// <summary>
        /// Gets the Color "Grey27" (RGB 68,68,68).
        /// </summary>

        public static Color Grey27 { get; } = new Color(238, 68, 68, 68);

        /// <summary>
        /// Gets the Color "Grey30" (RGB 78,78,78).
        /// </summary>

        public static Color Grey30 { get; } = new Color(239, 78, 78, 78);

        /// <summary>
        /// Gets the Color "Grey35" (RGB 88,88,88).
        /// </summary>

        public static Color Grey35 { get; } = new Color(240, 88, 88, 88);

        /// <summary>
        /// Gets the Color "Grey39" (RGB 98,98,98).
        /// </summary>

        public static Color Grey39 { get; } = new Color(241, 98, 98, 98);

        /// <summary>
        /// Gets the Color "Grey42" (RGB 108,108,108).
        /// </summary>

        public static Color Grey42 { get; } = new Color(242, 108, 108, 108);

        /// <summary>
        /// Gets the Color "Grey46" (RGB 118,118,118).
        /// </summary>

        public static Color Grey46 { get; } = new Color(243, 118, 118, 118);

        /// <summary>
        /// Gets the Color "Grey50" (RGB 128,128,128).
        /// </summary>

        public static Color Grey50 { get; } = new Color(244, 128, 128, 128);

        /// <summary>
        /// Gets the Color "Grey54" (RGB 138,138,138).
        /// </summary>

        public static Color Grey54 { get; } = new Color(245, 138, 138, 138);

        /// <summary>
        /// Gets the Color "Grey58" (RGB 148,148,148).
        /// </summary>

        public static Color Grey58 { get; } = new Color(246, 148, 148, 148);

        /// <summary>
        /// Gets the Color "Grey62" (RGB 158,158,158).
        /// </summary>

        public static Color Grey62 { get; } = new Color(247, 158, 158, 158);

        /// <summary>
        /// Gets the Color "Grey66" (RGB 168,168,168).
        /// </summary>

        public static Color Grey66 { get; } = new Color(248, 168, 168, 168);

        /// <summary>
        /// Gets the Color "Grey70" (RGB 178,178,178).
        /// </summary>

        public static Color Grey70 { get; } = new Color(249, 178, 178, 178);

        /// <summary>
        /// Gets the Color "Grey74" (RGB 188,188,188).
        /// </summary>

        public static Color Grey74 { get; } = new Color(250, 188, 188, 188);

        /// <summary>
        /// Gets the Color "Grey78" (RGB 198,198,198).
        /// </summary>

        public static Color Grey78 { get; } = new Color(251, 198, 198, 198);

        /// <summary>
        /// Gets the Color "Grey82" (RGB 208,208,208).
        /// </summary>

        public static Color Grey82 { get; } = new Color(252, 208, 208, 208);

        /// <summary>
        /// Gets the Color "Grey85" (RGB 218,218,218).
        /// </summary>

        public static Color Grey85 { get; } = new Color(253, 218, 218, 218);

        /// <summary>
        /// Gets the Color "Grey89" (RGB 228,228,228).
        /// </summary>

        public static Color Grey89 { get; } = new Color(254, 228, 228, 228);

        /// <summary>
        /// Gets the Color "Grey93" (RGB 238,238,238).
        /// </summary>

        public static Color Grey93 { get; } = new Color(255, 238, 238, 238);

    }
    #pragma warning restore CA1707 // Identifiers should not contain underscores
}
