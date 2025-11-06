// ***************************************************************************************
// MIT LICENCE
// Copyright (c) 2014 Philippe AURIOU
// https://github.com/auriou/FIGlet
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PromptPlusLibrary.Widgets.Banner.FIGlet
{
    internal sealed class FigletFont
    {
        public string Signature { get; private set; } = string.Empty;
        public string HardBlank { get; private set; } = string.Empty;
        public int Height { get; private set; }
        public int BaseLine { get; private set; }
        public int MaxLenght { get; private set; }
        public int OldLayout { get; private set; }
        public int CommentLines { get; private set; }
        public int PrintDirection { get; private set; }
        public int FullLayout { get; private set; }
        public int CodeTagCount { get; private set; }
        public List<string> Lines { get; private set; } = [];

        public FigletFont(string flfFontFile)
        {
            if (string.IsNullOrEmpty(flfFontFile))
            {
                throw new ArgumentNullException(nameof(flfFontFile), "FIGletFont file name is null or empty");
            }
            LoadFont(flfFontFile);
        }

        public FigletFont(Stream flfFontstream)
        {
            LoadFont(flfFontstream);
        }

        public FigletFont()
        {
            using Stream stream = typeof(FigletFont).GetTypeInfo().Assembly.GetManifestResourceStream("PromptPlusLibrary.Widgets.Banner.FIGlet.standard.flf")!;
            LoadLines(ReadStreamFont(stream));
        }

        public string[] ToAsciiArt(string strText)
        {
            List<string> res = [];
            for (int i = 1; i <= Height; i++)
            {
                StringBuilder resline = new();
                foreach (char car in strText)
                {
                    resline.Append(GetCharacter(car, i));
                }
                res.Add(resline.ToString());
            }
            return [.. res];
        }

        private string GetCharacter(char car, int line)
        {
            int start = CommentLines + (Convert.ToInt32(car) - 32) * Height;
            string temp = Lines[start + line];
            char lineending = temp[^1];
            temp = temp.TrimEnd(lineending);
            return temp.Replace(HardBlank, " ");
        }

        private void LoadLines(List<string> fontLines)
        {
            Lines = fontLines;
            string configString = Lines.First();
            string[] configArray = configString.Split(' ');
            Signature = configArray.First()[..^1];
            if (Signature != "flf2a")
            {
                throw new ArgumentException("FIGletFont missing signature");
            }
            HardBlank = configArray.First().Last().ToString();
            Height = GetIntValue(configArray, 1);
            BaseLine = GetIntValue(configArray, 2);
            MaxLenght = GetIntValue(configArray, 3);
            OldLayout = GetIntValue(configArray, 4);
            CommentLines = GetIntValue(configArray, 5);
            PrintDirection = GetIntValue(configArray, 6);
            FullLayout = GetIntValue(configArray, 7);
            CodeTagCount = GetIntValue(configArray, 8);
        }

        private void LoadFont(string flfFontFile)
        {
            try
            {
                using FileStream fso = File.Open(flfFontFile, FileMode.Open);
                LoadLines(ReadStreamFont(fso));
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException($"FIGletFont Error load {flfFontFile}", ex);
            }
        }

        private void LoadFont(Stream fontStream)
        {
            try
            {
                LoadLines(ReadStreamFont(fontStream));
            }
            catch (Exception ex)
            {
                throw new InvalidDataException($"FIGletFont Error load from stream", ex);
            }
        }

        private static int GetIntValue(string[] arrayStrings, int posi)
        {
            return arrayStrings.Length <= posi ? 0 : int.TryParse(arrayStrings[posi], out int val) ? val : 0;
        }

        private static List<string> ReadStreamFont(Stream fontStream)
        {
            List<string> fontData = [];
            using (StreamReader reader = new(fontStream))
            {
                while (!reader.EndOfStream)
                {
                    fontData.Add(reader.ReadLine()!);
                }
            }
            return fontData;
        }
    }
}

