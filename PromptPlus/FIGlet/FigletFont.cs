// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace PPlus.FIGlet
{

    internal class FIGletFont
    {
        public string Signature { get; }

        public char HardBlank { get; }

        public int Height { get; }

        public int BaseLine { get; }

        public int MaxLength { get; }

        public int OldLayout { get; }

        public int CommentLines { get; }

        public int PrintDirection { get; }

        public int FullLayout { get; }

        public int CodeTagCount { get; }

        public string[][] Lines { get; }

        public string Commit { get; }

        public static FIGletFont Default
        {
            get
            {
                lock (s_lockFont)
                {
                    FIGletFont defaultFont = null;
                    s_lockFont?.TryGetTarget(out defaultFont);
                    if (defaultFont == null)
                    {
                        Stream stream = null;
                        try
                        {
                            stream = typeof(FIGletFont).GetTypeInfo().
                                  Assembly.GetManifestResourceStream(typeof(FIGletFont).Namespace
                                    + @".Fonts.standard.flf");
                            defaultFont = new FIGletFont(stream);
                            s_lockFont.SetTarget(defaultFont);
                        }
                        finally
                        {
                            stream?.Dispose();
                        }
                    }
                    return defaultFont;
                }
            }
        }

        public FIGletFont(Stream fontStream)
        {
            using (var reader = new StreamReader(fontStream))
            {
                var configs = reader.ReadLine()?.Split(' ');
                if (configs == null || (!configs[0].StartsWith(@"flf2a")))
                {
                    throw new ArgumentException($"{nameof(fontStream)} missing signature", nameof(fontStream));
                }

                Signature = @"flf2a";
                HardBlank = configs[0].Last();
                try
                {
                    Height = Convert.ToInt32(TryGetMember(configs, 1));
                    BaseLine = Convert.ToInt32(TryGetMember(configs, 2));
                    MaxLength = Convert.ToInt32(TryGetMember(configs, 3));
                    OldLayout = Convert.ToInt32(TryGetMember(configs, 4));
                    CommentLines = Convert.ToInt32(TryGetMember(configs, 5));
                    PrintDirection = Convert.ToInt32(TryGetMember(configs, 6));
                    FullLayout = Convert.ToInt32(TryGetMember(configs, 7));
                    CodeTagCount = Convert.ToInt32(TryGetMember(configs, 8));
                }
                catch (IndexOutOfRangeException)
                {
                }

                var commentBuilder = new StringBuilder();
                for (var lineCount = 0; lineCount < CommentLines; lineCount++)
                {
                    commentBuilder.AppendLine(reader.ReadLine());
                }

                Commit = commentBuilder.ToString();
                Lines = new string[256][];
                var currentChar = 32;
                while (!reader.EndOfStream)
                {
                    var currentLine = reader.ReadLine() ?? string.Empty;
                    if (int.TryParse(currentLine, out var charIndex))
                    {
                        currentChar = charIndex;
                    }
                    Lines[currentChar] = new string[Height];
                    var currentLineIndex = 0;
                    while (currentLineIndex < Height)
                    {
                        Lines[currentChar][currentLineIndex] = currentLine.TrimEnd('@').Replace(HardBlank, ' ');
                        if (currentLine.EndsWith(@"@@"))
                        {
                            break;
                        }
                        currentLine = reader.ReadLine() ?? string.Empty;
                        currentLineIndex++;
                    }
                    currentChar++;
                }
            }
        }

        private static readonly WeakReference<FIGletFont> s_lockFont = new(null);

        private static T TryGetMember<T>(T[] array, int index)
        {
            if (index < array.Length)
            {
                return array[index];
            }
            return default;
        }

        public string GetCharacter(char sourceChar, int line)
        {
            if (line < 0 || line >= Height)
            {
                throw new ArgumentOutOfRangeException(nameof(line));
            }
            if (Lines[Convert.ToInt16(sourceChar)] == null)
            {
                return string.Empty;
            }
            return Lines[Convert.ToInt16(sourceChar)][line];
        }
    }

}
