using System.Globalization;
using PPlus.Controls;
using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls.AddToList
{
    public class AddToListMaskedTests : BaseTest
    {
        SuggestionOutput SuggestionInputSample(SuggestionInput arg)
        {
            var result = new SuggestionOutput();
            result.Add("suggestion 1");
            result.Add("suggestion 2");
            result.Add("suggestion 3");
            return result;
        }

        [Fact]
        public void Should_ValidInitControlPromptEmptyAddtoList1()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", (cfg) => { })
                .Mask("A{10}");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Fact]
        public void Should_ValidInitControlPromptEmptyAddtoList2()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P","D")
                .Mask("A{10}");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Theory]
        [InlineData(MaskedType.DateOnly)]
        [InlineData(MaskedType.DateTime)]
        [InlineData(MaskedType.TimeOnly)]
        public void Should_ValidInitControlPromptEmptyAddtoList3(MaskedType maskedType)
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask(maskedType);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Theory]
        [InlineData(MaskedType.DateOnly, "00/00/0000")]
        [InlineData(MaskedType.DateTime, "00/00/0000 00:00:00 AM")]
        [InlineData(MaskedType.TimeOnly, "00:00:00 AM")]
        public void Should_ValidInitControlPromptMaskedTypeDateTimeFillZerosEN(MaskedType maskedType, string expected)
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .FillZeros()
                .Culture("en-US")
                .Mask(maskedType);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expected, init);
        }

        [Theory]
        [InlineData(MaskedType.DateOnly, "00/00/0000")]
        [InlineData(MaskedType.DateTime, "00/00/0000 00:00:00")]
        [InlineData(MaskedType.TimeOnly, "00:00:00")]
        public void Should_ValidInitControlPromptMaskedTypeDateTimeFillZerosPT(MaskedType maskedType, string expected)
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
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
        public void Should_ValidInitControlPromptMaskedTypeDateTimeFormatYearPT1(MaskedType maskedType, FormatYear fmty, string expected)
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
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
        public void Should_ValidInitControlPromptMaskedTypeDateTimeFormatTimePT1(MaskedType maskedType, FormatTime fmtt, string defval, string expected)
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
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
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
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
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask(MaskedType.Number)
                .Culture(new CultureInfo("en-US"))
                .AmmoutPositions(4, 2, signal);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("0,000.00", init);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_ValidInitControlPromptMaskedNumberPT(bool signal)
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask(MaskedType.Number)
                .Culture(new CultureInfo("pt-BR"))
                .AmmoutPositions(4, 2, signal);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("0.000,00", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValueMaskedGeneric()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
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
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Culture(new CultureInfo("en-US"))
                .Mask(maskedType)
                .Default(defvalue);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expeted, init);
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
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
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
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
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
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
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
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
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
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
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
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
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
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
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
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
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
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
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
            var ctrl = (MaskEditListControl)PromptPlus
                 .AddtoMaskEditList("P", "D")
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
            var ctrl = (MaskEditListControl)PromptPlus
                 .AddtoMaskEditList("P", "D")
                .Culture(new CultureInfo("pt-BR"))
                .Mask(maskedType)
                .AmmoutPositions(4, 2, true)
                .Default(defvalue);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(expeted, init);
        }

        [Fact]
        public void Should_AcceptInputTemplateMaskedChar1()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                 .AddtoMaskEditList("P", "D")
                .Mask("9{3}-AAA", '*');
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
            var ctrl = (MaskEditListControl)PromptPlus
                 .AddtoMaskEditList("P", "D")
                .Mask(maskedType, '*')
                .AmmoutPositions(4, 4, true);
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
            var ctrl = (MaskEditListControl)PromptPlus
                 .AddtoMaskEditList("P", "D")
                 .Mask(maskedType)
                 .AcceptEmptyValue();
            ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("", result.Value.First().Input);
                Assert.Equal("", result.Value.First().Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlAcceptEmptyValueFillZerosDateOnly()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                 .AddtoMaskEditList("P", "D")
                 .Mask(MaskedType.DateOnly)
                 .FillZeros()
                 .AcceptEmptyValue();
            ctrl.InitControl(CancellationToken.None);
            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("", result.Value.First().Input);
                Assert.Equal("00/00/0000", result.Value.First().Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlAcceptEmptyValueFillZerosTimeOnly()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                 .AddtoMaskEditList("P", "D")
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
                Assert.True(result.IsRunning);
                Assert.Equal("000000A", result.Value.First().Input);
                Assert.Equal("00:00:00 AM", result.Value.First().Masked);
            });
        }

        [Fact]
        public void Should_ValidInitControlAcceptEmptyValueFillZerosDateTime()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                 .AddtoMaskEditList("P", "D")
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
                Assert.True(result.IsRunning);
                Assert.Equal("", result.Value.First().Input);
                Assert.Equal("00/00/0000 00:00:00 AM", result.Value.First().Masked);
            });
        }

        [Fact]
        public void Should_ChangeDescription()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                 .AddtoMaskEditList("P", "D")
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
        public void Should_ValidInitControlPromptNotEmptySelect()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}")
                .AddItem("item1")
                .AddItem("item2");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("", init);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item1"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item2"));
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValue()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}")
                .AddItem("item1")
                .AddItem("item2")
                .Default("item3");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("item3", init);
        }


        [Fact]
        public void Should_FinalizeControl()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}");
           ctrl.FinalizeControl(CancellationToken.None);
        }

        [Fact]
        public void Should_AcceptInputTemplateWithTooltip()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}");
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
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}")
                .Config((cfg) =>
                {
                    cfg.ShowTooltip(false);
                });
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
            var ctrl = (MaskEditListControl)PromptPlus.AddtoMaskEditList("P", "D", (cfg) =>
            {
                cfg.Tooltips("CustomTooltip");
            })
            .Mask("A{10}");
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "CustomTooltip");
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateNoAbort()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}")
                .AddItems(new string[] {"item1","item2"});

            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, new ResultMasked[] { new ResultMasked("item1","item1"), new ResultMasked("item2", "item2") }, false);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item1"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item2"));
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateAbort()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                   .AddtoMaskEditList("P", "D")
                   .Mask("A{10}")
                   .AddItems(new string[] { "item1", "item2" });

            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, new ResultMasked[] { new ResultMasked("item1", "item1"), new ResultMasked("item2", "item2") }, true);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == Messages.CanceledKey);
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "item1");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "item2");
        }


        [Fact]
        public void Should_ValidInitControlPromptInteraction()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}")
                .Interaction(new string[] { "item1", "item2" }, (ctrl, item) =>
                {
                    ctrl.AddItem(item);
                });
            var init = ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item1"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item2"));
        }


        [Fact]
        public void Should_ValidInitControlPromptPageSize()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}")
                .AddItem("item1")
                .AddItem("item2")
                .PageSize(2);
            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item1"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item2"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item3"));
        }

        [Fact]
        public void Should_TryResulAcceptEsc()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}");
            var init = ctrl.InitControl(CancellationToken.None);

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
        public void Should_TryResultAbort()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}");

            var init = ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                using var cts = new CancellationTokenSource();
                cts.Cancel();
                var result = ctrl.TryResult(cts.Token);
                Assert.True(result.IsAborted);
            });
        }


        [Fact]
        public void Should_TryResulNotAcceptEsc()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}")
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
        public void Should_CtrlEnterEnd()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}");

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                var sb = new ScreenBuffer();
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter,false,false,true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsRunning);
                Assert.False(result.IsAborted);
            });
        }

        [Fact]
        public void Should_TryResultValidator1()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}")
                .AddValidators(PromptValidators.MinLength(2));
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.NotNull(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 0);
            });
        }

        [Fact]
        public void Should_TryResultValidator2()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}")
                .AddValidators(PromptValidators.MinLength(2));
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("AA");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 1);
            });
        }

        [Fact]
        public void Should_TryResultUppercase()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}")
                .InputToCase(CaseOptions.Uppercase);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("a");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("A", result.Value.First().Masked);
            });
        }

        [Fact]
        public void Should_TryResultLowercase()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{10}")
                .InputToCase(CaseOptions.Lowercase);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("a", result.Value.First().Masked);
            });
        }

        [Fact]
        public void Should_TryResulSuggestion1()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .SuggestionHandler(SuggestionInputSample);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)9, ConsoleKey.Tab, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("suggestion 1", result.Value.First().Masked);
            });
        }

        [Fact]
        public void Should_TryResulSuggestion2()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .SuggestionHandler(SuggestionInputSample);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)9, ConsoleKey.Tab, true, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("suggestion 3", result.Value.First().Masked);
            });
        }

        [Fact]
        public void Should_TryResulCancelSuggestion()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
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
                Assert.True(result.Value.Count() == 0);
            });
        }


        [Fact]
        public void Should_TryResulNotAllowDuplicate()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("A");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.NotNull(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 1);
            });
        }

        [Fact]
        public void Should_TryResulAllowDuplicate()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .AllowDuplicate();
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("A");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == 2);
            });
        }

        [Fact]
        public void Should_AllowDuplicate1()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .AllowDuplicate()
                .AddItem("item1")
                .AddItem("item1");
            ctrl.InitControl(CancellationToken.None);
            PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
            var result = ctrl.TryResult(CancellationToken.None);
            Assert.False(result.IsAborted);
            Assert.False(result.IsRunning);
            Assert.True(result.Value.Count() == 2);
        }

        [Fact]
        public void Should_NotAllowDuplicate()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .AddItem("item1")
                .AddItem("item1");
            ctrl.InitControl(CancellationToken.None);
            PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
            var result = ctrl.TryResult(CancellationToken.None);
            Assert.False(result.IsAborted);
            Assert.False(result.IsRunning);
            Assert.True(result.Value.Count() == 1);
        }

        [Fact]
        public void Should_Range0()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .AddItem("item1")
                .AddItem("item2")
                .AddItem("item3")
                .Range(0,2);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 2);
            });
        }

        [Fact]
        public void Should_Range1()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .Range(2);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.NotNull(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 1);
            });
        }


        [Fact]
        public void Should_Range2()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .Range(2);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("b");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 2);
            });
        }


        [Fact]
        public void Should_Range3()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .Range(2, 3);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("b");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("c");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("d");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.NotNull(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 3);
            });
        }

        [Fact]
        public void Should_Range4()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .Range(2,3);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("b");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("c");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 3);
            });
        }



        [Fact]
        public void Should_DeleteItem()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .AddItem("item1")
                .AddItem("item2");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F3, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 1);
                Assert.True(result.Value.First().Masked == "item2");
            });
        }

        [Fact]
        public void Should_DeleteItem_With_ChangeHotKeyEditItem()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .HotKeyRemoveItem(new HotKey(ConsoleKey.F7))
                .AddItem("item1")
                .AddItem("item2");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F7, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 1);
                Assert.True(result.Value.First().Masked == "item2");
            });
        }

        [Fact]
        public void Should_Edit()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .AddItem("item1")
                .AddItem("item2");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F2, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("test");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 2);
                Assert.True(result.Value.First().Masked == "item1test");
            });
        }

        [Fact]
        public void Should_Edit_With_ChangeHotKeyEditItem()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .HotKeyEditItem(new HotKey(ConsoleKey.F7))
                .AddItem("item1")
                .AddItem("item2");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F7, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("test");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 2);
                Assert.True(result.Value.First().Masked == "item1test");
            });
        }

        [Fact]
        public void Should_AbortEdit()
        {
            var ctrl = (MaskEditListControl)PromptPlus
                .AddtoMaskEditList("P", "D")
                .Mask("A{20}")
                .AddItem("item1")
                .AddItem("item2");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F2, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("test");
                 PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("test"));
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == 2);
                Assert.True(result.Value.First().Masked == "item1");

            });
        }
    }
}
