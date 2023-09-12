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
using System.Text.RegularExpressions;

namespace PPlus.FIGletCore
{
    internal class FigletFont
    {
        public string Signature { get; private set; }
        public string HardBlank { get; private set; }
        public int Height { get; private set; }
        public int BaseLine { get; private set; }
        public int MaxLenght { get; private set; }
        public int OldLayout { get; private set; }
        public int CommentLines { get; private set; }
        public int PrintDirection { get; private set; }
        public int FullLayout { get; private set; }
        public int CodeTagCount { get; private set; }
        public List<string> Lines { get; private set; }

        public FigletFont(string flfFontFile)
        {
            LoadFont(flfFontFile);
        }

        public FigletFont(Stream flfFontstream)
        {
            LoadFont(flfFontstream);
        }

        public FigletFont()
        {
            using Stream stream = typeof(FigletFont).GetTypeInfo().Assembly.GetManifestResourceStream("PPlus.Controls.Banner.FIGletCore.standard.flf")!;
            LoadLines(ReadStreamFont(stream));
        }

        public string[] ToAsciiArt(string strText)
        {
            var res = new List<string>();
            for (int i = 1; i <= Height; i++)
            {
                var resline = "";
                foreach (var car in strText)
                {
                    resline += GetCharacter(car, i);
                }
                res.Add(resline);
            }
            return res.ToArray();
        }

        private string GetCharacter(char car, int line)
        {
            var start = CommentLines + ((Convert.ToInt32(car) - 32) * Height);
            var temp = Lines[start + line];
            var lineending = temp[^1];
            var rx = new Regex(@"\" + lineending + "{1,2}$");
            temp = rx.Replace(temp, "");
            return temp.Replace(HardBlank, " ");
        }

        private void LoadLines(List<string> fontLines)
        {
            Lines = fontLines;
            var configString = Lines.First();
            var configArray = configString.Split(' ');
            Signature = configArray.First().Remove(configArray.First().Length - 1);
            if (Signature != "flf2a")
            {
                throw new PromptPlusException($"FIGletFont missing signature");
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
                using var fso = File.Open(flfFontFile, FileMode.Open);
                LoadLines(ReadStreamFont(fso));
            }
            catch (Exception ex)
            {
                throw new PromptPlusException($"FIGletFont Error load {flfFontFile}",ex);
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
                throw new PromptPlusException($"FIGletFont Error load from stream", ex);
            }
        }

        private static int GetIntValue(string[] arrayStrings, int posi)
        {
            if (arrayStrings.Length <= posi)
            {
                return 0;
            }
            if (int.TryParse(arrayStrings[posi], out var val))
            {
                return val;
            }
            return 0;
        }

        private static List<string> ReadStreamFont(Stream fontStream)
        {
            var _fontData = new List<string>();
            using (var reader = new StreamReader(fontStream))
            {
                while (!reader.EndOfStream)
                {
                    _fontData.Add(reader.ReadLine());
                }
            }
            return _fontData;
        }
    }
}

