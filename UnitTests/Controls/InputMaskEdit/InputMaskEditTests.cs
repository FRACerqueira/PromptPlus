// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus.Controls;
using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls.InputMaskEdit
{
    public class InputMaskEditTests : BaseTest
    {

        SuggestionOutput SuggestionInputSample(SuggestionInput arg)
        {
            var result = new SuggestionOutput();
            result.Add("123-AAA");
            result.Add("234-BBB");
            result.Add("567-CCC");
            return result;
        }

        [Fact]
        public void Should_ValidInitControlPromptMaskedGeneric1()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P")
                .Mask("9{3}-AAA");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Fact]
        public void Should_ValidInitControlPromptMaskedGeneric2()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("9{3}-AAA");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Theory]
        [InlineData(MaskedType.DateOnly)]
        [InlineData(MaskedType.DateTime)]
        [InlineData(MaskedType.TimeOnly)]
        public void Should_ValidInitControlPromptMaskedTypeDateTime(MaskedType maskedType)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask(maskedType);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Theory]
        [InlineData(MaskedType.DateOnly, "00/00/0000")]
        [InlineData(MaskedType.DateTime,"00/00/0000 00:00:00 AM")]
        [InlineData(MaskedType.TimeOnly,"00:00:00 AM")]
        public void Should_ValidInitControlPromptMaskedTypeDateTimeFillZerosEN(MaskedType maskedType, string expected)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .FillZeros()
                .Culture("en-US")
                .Mask(maskedType);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expected,init);
        }

        [Theory]
        [InlineData(MaskedType.DateOnly, "00/00/0000")]
        [InlineData(MaskedType.DateTime, "00/00/0000 00:00:00")]
        [InlineData(MaskedType.TimeOnly, "00:00:00")]
        public void Should_ValidInitControlPromptMaskedTypeDateTimeFillZerosPT(MaskedType maskedType, string expected)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .FillZeros()
                .Culture(new CultureInfo("pt-BR"))
                .Mask(maskedType);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expected, init);
        }


        [Theory]
        [InlineData(MaskedType.DateOnly, FormatYear.Long, "00/00/0000")]
        [InlineData(MaskedType.DateOnly, FormatYear.Short, "00/00/00")]
        [InlineData(MaskedType.DateTime, FormatYear.Long, "00/00/0000 00:00:00")]
        [InlineData(MaskedType.DateTime, FormatYear.Short, "00/00/00 00:00:00")]
        public void Should_ValidInitControlPromptMaskedTypeDateTimeFormatYearPT1(MaskedType maskedType,FormatYear fmty, string expected)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .FillZeros()
                .FormatYear(fmty)
                .Culture(new CultureInfo("pt-BR"))
                .Mask(maskedType);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expected, init);
        }

        [Theory]
        [InlineData(MaskedType.TimeOnly, FormatTime.HMS, "12:34:56", "12:34:56")]
        [InlineData(MaskedType.TimeOnly, FormatTime.HMS, "5:4:3", "05:04:03")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyHM, "12:34:56", "12:34:00")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyHM, "12:34", "12:34:00")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyH, "12:34:56", "12:00:00")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyH, "12", "12:00:00")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyH, "5", "05:00:00")]
        public void Should_ValidInitControlPromptMaskedTypeDateTimeFormatTimePT1(MaskedType maskedType, FormatTime fmtt,string defval, string expected)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .FillZeros()
                .FormatTime(fmtt)
                .Default(defval)
                .Culture(new CultureInfo("pt-BR"))
                .Mask(maskedType);
            var init = ctrl.InitControl(CancellationToken.None);

            Assert.Equal(expected, init);
        }

        [Theory]
        [InlineData(MaskedType.TimeOnly, FormatTime.HMS, "11:34:56", "11:34:56 AM")]
        [InlineData(MaskedType.TimeOnly, FormatTime.HMS, "11:34:56 PM", "11:34:56 PM")]
        [InlineData(MaskedType.TimeOnly, FormatTime.HMS, "5:4:3", "05:04:03 AM")]
        [InlineData(MaskedType.TimeOnly, FormatTime.HMS, "5:4:3 PM", "05:04:03 PM")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyHM, "11:34:56", "11:34:00 AM")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyHM, "11:34:56 PM", "11:34:00 PM")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyHM, "11:34", "11:34:00 AM")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyHM, "11:34 PM", "11:34:00 PM")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyH, "11:34:56", "11:00:00 AM")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyH, "11:34:56 PM", "11:00:00 PM")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyH, "11", "11:00:00 AM")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyH, "11 PM", "11:00:00 PM")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyH, "5", "05:00:00 AM")]
        [InlineData(MaskedType.TimeOnly, FormatTime.OnlyH, "5 PM", "05:00:00 PM")]
        public void Should_ValidInitControlPromptMaskedTypeDateTimeFormatTimeEN1(MaskedType maskedType, FormatTime fmtt, string defval, string expected)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .FillZeros()
                .FormatTime(fmtt)
                .Default(defval)
                .Culture(new CultureInfo("en-US"))
                .Mask(maskedType);
            var init = ctrl.InitControl(CancellationToken.None);

            Assert.Equal(expected, init);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_ValidInitControlPromptMaskedNumberEN(bool signal)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask(MaskedType.Number)
                .Culture(new CultureInfo("en-US"))
                .AmmoutPositions(4,2, signal);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("0,000.00",init);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_ValidInitControlPromptMaskedNumberPT(bool signal)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask(MaskedType.Number)
                .Culture(new CultureInfo("pt-BR"))
                .AmmoutPositions(4, 2, signal);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("0.000,00", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValueMaskedGeneric()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("9{3}-AAA")
                .Default("123-ABC");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("123-ABC", init);
        }

        [Theory]
        [InlineData(MaskedType.DateOnly, "01/01/2001", "01/01/2001")]
        [InlineData(MaskedType.DateOnly, "01/01", "01/01")]
        [InlineData(MaskedType.DateOnly, "12/31", "12/31")]
        public void Should_ValidInitControlPromptDefaultValueMaskedTypeDateOnlyEN(MaskedType maskedType, string defvalue, string expeted)
        {
            if (expeted.Length == 5)
            {
                expeted += "/" + DateTime.Now.Year;
            }
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Culture(new CultureInfo("en-US"))
                .Mask(maskedType)
                .Default(defvalue);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expeted,init);
        }

        [Theory]
        [InlineData(MaskedType.DateOnly, "01/01/2001", "01/01/2001")]
        [InlineData(MaskedType.DateOnly, "01/01", "01/01")]
        [InlineData(MaskedType.DateOnly, "31/12", "31/12")]
        public void Should_ValidInitControlPromptDefaultValueMaskedTypeDateOnlyPT(MaskedType maskedType, string defvalue, string expeted)
        {
            if (expeted.Length == 5)
            {
                expeted += "/" + DateTime.Now.Year;
            }
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Culture(new CultureInfo("pt-BR"))
                .Mask(maskedType)
                .Default(defvalue);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expeted, init);
        }

        [Theory]
        [InlineData(MaskedType.TimeOnly, "10:23:45", "10:23:45 AM")]
        [InlineData(MaskedType.TimeOnly, "10:23:45 PM", "10:23:45 PM")]
        [InlineData(MaskedType.TimeOnly, "10:23:45 AM", "10:23:45 AM")]
        [InlineData(MaskedType.TimeOnly, "22:23:45", "10:23:45 PM")]
        public void Should_ValidInitControlPromptDefaultValueMaskedTypeTimeOnlyEN(MaskedType maskedType, string defvalue, string expeted)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Culture(new CultureInfo("en-US"))
                .Mask(maskedType)
                .Default(defvalue);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expeted, init);
        }

        [Theory]
        [InlineData(MaskedType.TimeOnly, "10:23:45", "10:23:45")]
        [InlineData(MaskedType.TimeOnly, "22:23:45", "22:23:45")]
        [InlineData(MaskedType.TimeOnly, "10:23:45 PM", "22:23:45")]
        [InlineData(MaskedType.TimeOnly, "10:23:45 AM", "10:23:45")]
        public void Should_ValidInitControlPromptDefaultValueMaskedTypeTimeOnlyPT(MaskedType maskedType, string defvalue, string expeted)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Culture(new CultureInfo("pt-BR"))
                .Mask(maskedType)
                .Default(defvalue);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expeted, init);
        }

        [Theory]
        [InlineData(MaskedType.DateTime, "31/12/2001 11:23:45", "31/12/2001 11:23:45")]
        [InlineData(MaskedType.DateTime, "31/12/2001 23:23:45", "31/12/2001 23:23:45")]
        [InlineData(MaskedType.DateTime, "31/12/2001 11:23:45 PM", "31/12/2001 23:23:45")]
        [InlineData(MaskedType.DateTime, "31/12/2001 11:23:45 AM", "31/12/2001 11:23:45")]
        [InlineData(MaskedType.DateTime, "31/12/2001", "31/12/2001 00:00:00")]
        public void Should_ValidInitControlPromptDefaultValueMaskedTypeDateTimePT(MaskedType maskedType, string defvalue, string expeted)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Culture(new CultureInfo("pt-BR"))
                .Mask(maskedType)
                .Default(defvalue);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expeted, init);
        }

        [Theory]
        [InlineData(MaskedType.DateTime, "31/12/2001", "31/12/2001 00:00:00")]
        [InlineData(MaskedType.DateTime, "31/12/2001 11:23:45", "31/12/2001 11:23:00")]
        [InlineData(MaskedType.DateTime, "31/12/2001 23:23:45", "31/12/2001 23:23:00")]
        [InlineData(MaskedType.DateTime, "31/12/2001 11:23:45 PM", "31/12/2001 23:23:00")]
        [InlineData(MaskedType.DateTime, "31/12/2001 11:23:45 AM", "31/12/2001 11:23:00")]
        public void Should_ValidInitControlPromptDefaultValueMaskedTypeDateTimePT1(MaskedType maskedType, string defvalue, string expeted)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Culture(new CultureInfo("pt-BR"))
                .FormatTime(FormatTime.OnlyHM)
                .Mask(maskedType)
                .Default(defvalue);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expeted, init);
        }

        [Theory]
        [InlineData(MaskedType.DateTime, "31/12/2001 11:23:45", "31/12/2001 11:00:00")]
        [InlineData(MaskedType.DateTime, "31/12/2001 23:59:59", "31/12/2001 23:00:00")]
        [InlineData(MaskedType.DateTime, "31/12/2001 11:59:59 PM", "31/12/2001 23:00:00")]
        [InlineData(MaskedType.DateTime, "31/12/2001 11:59:59 AM", "31/12/2001 11:00:00")]
        [InlineData(MaskedType.DateTime, "31/12/2001", "31/12/2001 00:00:00")]
        public void Should_ValidInitControlPromptDefaultValueMaskedTypeDateTimePT2(MaskedType maskedType, string defvalue, string expeted)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Culture(new CultureInfo("pt-BR"))
                .FormatTime(FormatTime.OnlyH)
                .Mask(maskedType)
                .Default(defvalue);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expeted, init);
        }

        [Theory]
        [InlineData(MaskedType.DateTime, "12/31/2001 11:59:59", "12/31/2001 11:59:59 AM")]
        [InlineData(MaskedType.DateTime, "12/31/2001 23:59:59", "12/31/2001 11:59:59 PM")]
        [InlineData(MaskedType.DateTime, "12/31/2001 11:59:59 PM", "12/31/2001 11:59:59 PM")]
        [InlineData(MaskedType.DateTime, "12/31/2001 11:59:59 AM", "12/31/2001 11:59:59 AM")]
        [InlineData(MaskedType.DateTime, "12/31/2001", "12/31/2001 00:00:00 AM")]
        public void Should_ValidInitControlPromptDefaultValueMaskedTypeDateTimeEN(MaskedType maskedType, string defvalue, string expeted)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Culture(new CultureInfo("en-US"))
                .Mask(maskedType)
                .Default(defvalue);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expeted, init);
        }

        [Theory]
        [InlineData(MaskedType.Number, "123", "0,123.00")]
        [InlineData(MaskedType.Number, "0.12", "0,000.12")]
        [InlineData(MaskedType.Number, "1234.567", "1,234.57")]
        [InlineData(MaskedType.Number, "-1234.567", "-1,234.57")]
        [InlineData(MaskedType.Number, "+1234.567", "1,234.57")]
        public void Should_ValidInitControlPromptDefaultValueMaskedTypeNumberEN(MaskedType maskedType, string defvalue, string expeted)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Culture(new CultureInfo("en-US"))
                .Mask(maskedType)
                .AmmoutPositions(4, 2, true)
                .Default(defvalue);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expeted, init);
        }

        [Theory]
        [InlineData(MaskedType.Number, "123", "0.123,00")]
        [InlineData(MaskedType.Number, "0,12", "0.000,12")]
        [InlineData(MaskedType.Number, "1234,567", "1.234,57")]
        [InlineData(MaskedType.Number, "-1234,567", "-1.234,57")]
        [InlineData(MaskedType.Number, "+1234,567", "1.234,57")]
        public void Should_ValidInitControlPromptDefaultValueMaskedTypeNumberPT(MaskedType maskedType, string defvalue, string expeted)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Culture(new CultureInfo("pt-BR"))
                .Mask(maskedType)
                .AmmoutPositions(4, 2, true)
                .Default(defvalue);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expeted, init);
        }

        [Theory]
        [InlineData(MaskedType.Currency, "123", "$ 0,123.00")]
        [InlineData(MaskedType.Currency, "0.12", "$ 0,000.12")]
        [InlineData(MaskedType.Currency, "1234.567", "$ 1,234.57")]
        [InlineData(MaskedType.Currency, "-1234.567", "-$ 1,234.57")]
        [InlineData(MaskedType.Currency, "+1234.567", "$ 1,234.57")]
        [InlineData(MaskedType.Currency, "-$1234.567", "-$ 1,234.57")]
        [InlineData(MaskedType.Currency, "+$1234.567", "$ 1,234.57")]

        public void Should_ValidInitControlPromptDefaultValueMaskedTypeCurrencyEN(MaskedType maskedType, string defvalue, string expeted)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Culture(new CultureInfo("en-US"))
                .Mask(maskedType)
                .AmmoutPositions(4, 2, true)
                .Default(defvalue);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expeted, init);
        }


        [Theory]
        [InlineData(MaskedType.Currency, "123", "R$ 0.123,00")]
        [InlineData(MaskedType.Currency, "0,12", "R$ 0.000,12")]
        [InlineData(MaskedType.Currency, "1234,567", "R$ 1.234,57")]
        [InlineData(MaskedType.Currency, "-1234,567", "-R$ 1.234,57")]
        [InlineData(MaskedType.Currency, "+1234,567", "R$ 1.234,57")]
        [InlineData(MaskedType.Currency, "-R$1234,567", "-R$ 1.234,57")]
        [InlineData(MaskedType.Currency, "+R$1234,567", "R$ 1.234,57")]

        public void Should_ValidInitControlPromptDefaultValueMaskedTypeCurrencyPT(MaskedType maskedType, string defvalue, string expeted)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Culture(new CultureInfo("pt-BR"))
                .Mask(maskedType)
                .AmmoutPositions(4, 2, true)
                .Default(defvalue);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expeted, init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory1()
        {
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory.AddHistory("234-xyz", TimeSpan.FromSeconds(30), null);
            FileHistory.SaveHistory(namehist, hist);

            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("9{3}-AAA")
                 .Default("123-ABC")
                 .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);
            FileHistory.ClearHistory(namehist);

            Assert.Equal("234-xyz", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory2()
        {
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);

            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("9{3}-AAA")
                 .OverwriteDefaultFrom(namehist);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("234xyz");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.FinishTemplate(sb, result.Value, false);
            });

            ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("9{3}-AAA")
                 .Default("123-ABC")
                 .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);
            
            FileHistory.ClearHistory(namehist);

            Assert.Equal("234-xyz", init);
        }

        [Fact]
        public void Should_FinalizeControl()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("9{3}-AAA");
            ctrl.InitControl(CancellationToken.None);

            ctrl.FinalizeControl(CancellationToken.None);
        }

        [Fact]
        public void Should_AcceptInputTemplateWithTooltip()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("9{3}-AAA");
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F1"));
        }

        [Fact]
        public void Should_AcceptInputTemplateWithoutTooltip()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Config( (cfg) => 
                {
                    cfg.ShowTooltip(false);
                })
                .Mask("9{3}-AAA");
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F1"));
        }

        [Fact]
        public void Should_AcceptInputTemplateCustomTooltip()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Config((cfg) =>
                {
                    cfg.Tooltips("CustomTooltip");
                })
                .Mask("9{3}-AAA");
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "CustomTooltip");
        }

        [Fact]
        public void Should_AcceptInputTemplateMaskedChar1()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("9{3}-AAA",'*');
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains('*'));
        }

        [Theory]
        [InlineData(MaskedType.DateOnly)]
        [InlineData(MaskedType.DateTime)]
        [InlineData(MaskedType.TimeOnly)]
        public void Should_AcceptInputTemplateMaskedChar2(MaskedType maskedType)
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask(maskedType, '*')
                .AmmoutPositions(4,4,true);
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains('*'));
        }


        [Theory]
        [InlineData(MaskedType.DateOnly)]
        [InlineData(MaskedType.DateTime)]
        [InlineData(MaskedType.TimeOnly)]
        public void Should_ValidInitControlAcceptEmptyValue(MaskedType maskedType)
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask(maskedType)
                 .AcceptEmptyValue();
            ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("", result.Value.Input);
                Assert.Equal("", result.Value.Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlAcceptEmptyValueFillZerosDateOnly()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask(MaskedType.DateOnly)
                 .FillZeros()
                 .AcceptEmptyValue();
            ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("", result.Value.Input);
                Assert.Equal("00/00/0000", result.Value.Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlAcceptEmptyValueFillZerosTimeOnly()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask(MaskedType.TimeOnly)
                 .FillZeros()
                 .Culture(new CultureInfo("en-US"))
                 .AcceptEmptyValue();
            ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("000000A", result.Value.Input);
                Assert.Equal("00:00:00 AM", result.Value.Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlAcceptEmptyValueFillZerosDateTime()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask(MaskedType.DateTime)
                 .FillZeros()
                 .Culture(new CultureInfo("en-US"))
                 .AcceptEmptyValue();
            ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("", result.Value.Input);
                Assert.Equal("00/00/0000 00:00:00 AM", result.Value.Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValueMaskedGenericDefaultIfEmpty()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("9{3}-AAA")
                .DefaultIfEmpty("123-ABC");
            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("123-ABC", result.Value.Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValueMaskedDateOnlyDefaultIfEmpty1()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask(MaskedType.DateOnly)
                .DefaultIfEmpty("01/10/2021");
            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("01/10/2021", result.Value.Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValueMaskedDateOnlyDefaultIfEmpty2()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask(MaskedType.DateOnly)
                .FillZeros()
                .DefaultIfEmpty("01/10/2021");
            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("01/10/2021", result.Value.Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValueMaskedTimeOnlyDefaultIfEmpty1()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask(MaskedType.TimeOnly)
                .Culture(new CultureInfo("pt-BR"))
                .DefaultIfEmpty("01:23:45");
            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("01:23:45", result.Value.Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValueMaskedTimeOnlyDefaultIfEmpty2()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask(MaskedType.TimeOnly)
                .Culture(new CultureInfo("pt-BR"))
                .FillZeros()
                .DefaultIfEmpty("01:23:45");
            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("01:23:45", result.Value.Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValueMaskedDateTimeDefaultIfEmpty1()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask(MaskedType.DateTime)
                .Culture(new CultureInfo("pt-BR"))
                .DefaultIfEmpty("01/02/2021 01:23:45");
            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("01/02/2021 01:23:45", result.Value.Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValueMaskedDateTimeDefaultIfEmpty2()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask(MaskedType.DateTime)
                .Culture(new CultureInfo("pt-BR"))
                .FillZeros()
                .DefaultIfEmpty("01/02/2021 01:23:45");
            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("01/02/2021 01:23:45", result.Value.Masked);
            });
        }


        [Fact]
        public void Should_ValidInitControlPromptDefaultValueMaskedNumberDefaultIfEmpty()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask(MaskedType.Number)
                .Culture(new CultureInfo("pt-BR"))
                .AmmoutPositions(2,0,false)
                .DefaultIfEmpty("10",true);
            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("10", result.Value.Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValueMaskedCurrencyDefaultIfEmpty1()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask(MaskedType.Currency)
                .Culture(new CultureInfo("pt-BR"))
                .AmmoutPositions(2, 0, false)
                .DefaultIfEmpty("10", true);
            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("R$ 10", result.Value.Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValueMaskedCurrencyDefaultIfEmpty2()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask(MaskedType.Currency)
                .Culture(new CultureInfo("pt-BR"))
                .AmmoutPositions(2, 0, false)
                .DefaultIfEmpty("R$10", true);
            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("R$ 10", result.Value.Masked);
            });
        }


        [Fact]
        public void Should_TryResultUppercase()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("AAA")
                .InputToCase(CaseOptions.Uppercase);
            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("a");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("A", result.Value.Masked);
                Assert.Equal("A", result.Value.Input);
            });
        }

        [Fact]
        public void Should_TryResultLowercase()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("AAA")
                .InputToCase(CaseOptions.Lowercase);
            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("a", result.Value.Masked);
                Assert.Equal("a", result.Value.Input);
            });
        }


        [Fact]
        public void Should_TryResultValidator1()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("AAA")
                .AddValidators(PromptValidators.MinLength(3));
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("AA");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("AA", result.Value.Input);
            });
        }

        [Fact]
        public void Should_TryResultValidator2()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("AAA")
                .AddValidators(PromptValidators.MinLength(2));
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("AA");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("AA", result.Value.Input);
            });
        }

        [Fact]
        public void Should_ValidateOnDemand1()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("AAA")
                 .ValidateOnDemand(true)
                 .AddValidators(PromptValidators.MinLength(3));

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.NotNull(ctrl.ValidateError);
            });
        }

        [Fact]
        public void Should_ValidateOnDemand2()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("AAA")
                 .ValidateOnDemand(false)
                 .AddValidators(PromptValidators.MinLength(3));

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Null(ctrl.ValidateError);
            });
        }


        [Fact]
        public void Should_ChangeDescription()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("AAA")
                .ChangeDescription((input) =>
                {
                    if (input.Length == 1)
                    {
                        return "ChangeDescription=1";
                    }
                    return "";
                });

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("ChangeDescription=1"));
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("ChangeDescription=1"));

            });
        }


        [Fact]
        public void Should_TryResulSuggestion1()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("9{3}-AAA")
                .SuggestionHandler(SuggestionInputSample);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)9, ConsoleKey.Tab, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("123-AAA", result.Value.Masked);
            });
        }

        [Fact]
        public void Should_TryResulSuggestion2()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("9{3}-AAA")
                .SuggestionHandler(SuggestionInputSample);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)9, ConsoleKey.Tab, true, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("567-CCC", result.Value.Masked);
            });
        }


        [Fact]
        public void Should_TryResulCancelSuggestion()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("9{3}-AAA")
                .SuggestionHandler(SuggestionInputSample);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)9, ConsoleKey.Tab, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("123-AAA", result.Value.Masked);
            });
        }

        [Fact]
        public void Should_SaveHistory()
        {
            var ctrl = (MaskEditControl)PromptPlus
                   .MaskEdit("P", "D")
                   .Mask("A{20}")
                   .HistoryMinimumPrefixLength(2)
                   .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);

            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("NEWHISTORY");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                ctrl.FinishTemplate(sb, result.Value, result.IsAborted);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal(1, FileHistory.LoadHistory(namehist).Count);
                Assert.Equal("NEWHISTORY", FileHistory.LoadHistory(namehist)[0].History);

            });

            FileHistory.ClearHistory(namehist);

        }


        [Fact]
        public void Should_ValidTryResultClearHistory()
        {
            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);


            var ctrl = (MaskEditControl)PromptPlus
               .MaskEdit("P", "D")
               .Mask("A{20}")
               .HistoryMinimumPrefixLength(2)
               .HistoryEnabled("HistoryEnabled");

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("HI");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Delete, false, false, true));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.UpArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal(0, FileHistory.LoadHistory(namehist).Count);

            });

            FileHistory.ClearHistory(namehist);

        }

        [Fact]
        public void Should_ValidTryResultAbortHistory()
        {
            var ctrl = (MaskEditControl)PromptPlus
                   .MaskEdit("P", "D")
                   .Mask("A{20}")
                   .HistoryMinimumPrefixLength(2)
                   .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("HI");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("HIST3", result.Value.Masked);
            });

            FileHistory.ClearHistory(namehist);

        }

        [Fact]
        public void Should_ValidTryResultHistoryShowHistory5()
        {
            var ctrl = (MaskEditControl)PromptPlus
                   .MaskEdit("P", "D")
                   .Mask("A{20}")
                   .HistoryMinimumPrefixLength(2)
                   .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("HI");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.FinishTemplate(sb, result.Value, result.IsAborted);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);


                Assert.Equal("HI", result.Value.Masked);
            });

            ctrl = (MaskEditControl)PromptPlus
                   .MaskEdit("P", "D")
                   .Mask("A{20}")
                   .HistoryMinimumPrefixLength(2)
                   .HistoryEnabled("HistoryEnabled");

            ctrl.InitControl(CancellationToken.None);


            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("H");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("H", result.Value.Masked);
            });

            FileHistory.ClearHistory(namehist);
        }

        [Fact]
        public void Should_ValidTryResultHistoryShowHistory4()
        {
            var ctrl = (MaskEditControl)PromptPlus
                   .MaskEdit("P", "D")
                   .Mask("A{20}")
                   .HistoryMinimumPrefixLength(2)
                   .HistoryPageSize(1)
                   .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("HI");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.PageDown, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("HIST2", result.Value.Masked);
            });

            FileHistory.ClearHistory(namehist);
        }

        [Fact]
        public void Should_ValidTryResultHistoryShowHistory3()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("A{20}")
                .HistoryMaxItems(1)
                .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("HI");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.UpArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("HIST3", result.Value.Masked);
            });

            FileHistory.ClearHistory(namehist);
        }


        [Fact]
        public void Should_ValidTryResultHistoryShowHistory2()
        {
            var ctrl = (MaskEditControl)PromptPlus
                .MaskEdit("P", "D")
                .Mask("A{20}")
                .HistoryMinimumPrefixLength(2)
                .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("HI");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.UpArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("HIST2", result.Value.Masked);
            });

            FileHistory.ClearHistory(namehist);
        }

        [Fact]
        public void Should_ValidTryResultHistoryShowHistory1()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("A{20}")
                 .HistoryMinimumPrefixLength(2)
                 .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("HI");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("HIST3", result.Value.Masked);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("HIST3"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("HIST2"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("HIST1"));
            });


            FileHistory.ClearHistory(namehist);

        }

        [Fact]
        public void Should_ValidTryResultHistoryNotShowHistory()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("A{20}")
                 .HistoryMinimumPrefixLength(2)
                 .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("12");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("12", result.Value.Masked);
            });

            FileHistory.ClearHistory(namehist);

        }

        [Fact]
        public void Should_ValidTryResultHistoryMinimumPrefixLength()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("A{20}")
                 .HistoryMinimumPrefixLength(2)
                 .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(10), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(10), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("1");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("1", result.Value.Masked);
            });

            FileHistory.ClearHistory(namehist);

        }

        [Fact]
        public void Should_ValidInitControlHistoryEnabled()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("A{20}")
                 .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(10), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(10), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

        }

        [Fact]
        public void Should_TryResulNotAcceptEsc()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("A{20}")
                 .Config((cfg) => cfg.EnabledAbortKey(false));
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
            });
        }

        [Fact]
        public void Should_TryResultAbort()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("A{20}");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                using var cts = new CancellationTokenSource();
                cts.Cancel();
                var result = ctrl.TryResult(cts.Token);
                Assert.True(result.IsAborted);
            });
        }

        [Fact]
        public void Should_TryResulAcceptEsc()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("A{20}");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.True(result.IsAborted);
                Assert.False(result.IsRunning);
            });
        }

        [Fact]
        public void Should_TryResultHideTooltips()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("A{20}");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F1, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F1"));
            });
        }


        [Fact]
        public void Should_AcceptInputFinishTemplateAbort1()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("A{20}");
            
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb,new ResultMasked("Result", "Result"), true);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Result"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("######"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == Messages.CanceledKey);
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateNoAbort()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("A{20}");

            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, new ResultMasked("Result", "Result"), false);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Result"));
        }

        [Fact]
        public void Should_AcceptInputTemplateCustomTooltipWithConfigMethod()
        {
            var ctrl = (MaskEditControl)PromptPlus
                 .MaskEdit("P", "D")
                 .Mask("A{20}")
                .Config((cfg) =>
                {
                    cfg.Tooltips("CustomTooltip");
                });
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "CustomTooltip");
        }
    }
}
