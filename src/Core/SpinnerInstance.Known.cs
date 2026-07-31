// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary.Core
{
    internal abstract partial class SpinnerBase
    {
        /// <summary>
        /// Contains well-known spinners.
        /// </summary>
        public static class Known
        {
            /// <summary>
            /// Gets the "Default" spinner.
            /// </summary>
            public static SpinnerBase Default { get; } = new DefaultSpinner();
            /// <summary>
            /// Gets the "Ascii" spinner.
            /// </summary>
            public static SpinnerBase Ascii { get; } = new AsciiSpinner();
            /// <summary>
            /// Gets the "dots" spinner.
            /// </summary>
            public static SpinnerBase Dots { get; } = new DotsSpinner();
            /// <summary>
            /// Gets the "dots2" spinner.
            /// </summary>
            public static SpinnerBase Dots2 { get; } = new Dots2Spinner();
            /// <summary>
            /// Gets the "dots3" spinner.
            /// </summary>
            public static SpinnerBase Dots3 { get; } = new Dots3Spinner();
            /// <summary>
            /// Gets the "dots4" spinner.
            /// </summary>
            public static SpinnerBase Dots4 { get; } = new Dots4Spinner();
            /// <summary>
            /// Gets the "dots5" spinner.
            /// </summary>
            public static SpinnerBase Dots5 { get; } = new Dots5Spinner();
            /// <summary>
            /// Gets the "dots6" spinner.
            /// </summary>
            public static SpinnerBase Dots6 { get; } = new Dots6Spinner();
            /// <summary>
            /// Gets the "dots7" spinner.
            /// </summary>
            public static SpinnerBase Dots7 { get; } = new Dots7Spinner();
            /// <summary>
            /// Gets the "dots8" spinner.
            /// </summary>
            public static SpinnerBase Dots8 { get; } = new Dots8Spinner();
            /// <summary>
            /// Gets the "dots9" spinner.
            /// </summary>
            public static SpinnerBase Dots9 { get; } = new Dots9Spinner();
            /// <summary>
            /// Gets the "dots10" spinner.
            /// </summary>
            public static SpinnerBase Dots10 { get; } = new Dots10Spinner();
            /// <summary>
            /// Gets the "dots11" spinner.
            /// </summary>
            public static SpinnerBase Dots11 { get; } = new Dots11Spinner();
            /// <summary>
            /// Gets the "dots12" spinner.
            /// </summary>
            public static SpinnerBase Dots12 { get; } = new Dots12Spinner();
            /// <summary>
            /// Gets the "dots13" spinner.
            /// </summary>
            public static SpinnerBase Dots13 { get; } = new Dots13Spinner();
            /// <summary>
            /// Gets the "dots14" spinner.
            /// </summary>
            public static SpinnerBase Dots14 { get; } = new Dots14Spinner();
            /// <summary>
            /// Gets the "dots8Bit" spinner.
            /// </summary>
            public static SpinnerBase Dots8Bit { get; } = new Dots8BitSpinner();
            /// <summary>
            /// Gets the "dotsCircle" spinner.
            /// </summary>
            public static SpinnerBase DotsCircle { get; } = new DotsCircleSpinner();
            /// <summary>
            /// Gets the "sand" spinner.
            /// </summary>
            public static SpinnerBase Sand { get; } = new SandSpinner();
            /// <summary>
            /// Gets the "line" spinner.
            /// </summary>
            public static SpinnerBase Line { get; } = new LineSpinner();
            /// <summary>
            /// Gets the "line2" spinner.
            /// </summary>
            public static SpinnerBase Line2 { get; } = new Line2Spinner();
            /// <summary>
            /// Gets the "pipe" spinner.
            /// </summary>
            public static SpinnerBase Pipe { get; } = new PipeSpinner();
            /// <summary>
            /// Gets the "simpleDots" spinner.
            /// </summary>
            public static SpinnerBase SimpleDots { get; } = new SimpleDotsSpinner();
            /// <summary>
            /// Gets the "simpleDotsScrolling" spinner.
            /// </summary>
            public static SpinnerBase SimpleDotsScrolling { get; } = new SimpleDotsScrollingSpinner();
            /// <summary>
            /// Gets the "star" spinner.
            /// </summary>
            public static SpinnerBase Star { get; } = new StarSpinner();
            /// <summary>
            /// Gets the "star2" spinner.
            /// </summary>
            public static SpinnerBase Star2 { get; } = new Star2Spinner();
            /// <summary>
            /// Gets the "flip" spinner.
            /// </summary>
            public static SpinnerBase Flip { get; } = new FlipSpinner();
            /// <summary>
            /// Gets the "hamburger" spinner.
            /// </summary>
            public static SpinnerBase Hamburger { get; } = new HamburgerSpinner();
            /// <summary>
            /// Gets the "growVertical" spinner.
            /// </summary>
            public static SpinnerBase GrowVertical { get; } = new GrowVerticalSpinner();
            /// <summary>
            /// Gets the "growHorizontal" spinner.
            /// </summary>
            public static SpinnerBase GrowHorizontal { get; } = new GrowHorizontalSpinner();
            /// <summary>
            /// Gets the "balloon" spinner.
            /// </summary>
            public static SpinnerBase Balloon { get; } = new BalloonSpinner();
            /// <summary>
            /// Gets the "balloon2" spinner.
            /// </summary>
            public static SpinnerBase Balloon2 { get; } = new Balloon2Spinner();
            /// <summary>
            /// Gets the "noise" spinner.
            /// </summary>
            public static SpinnerBase Noise { get; } = new NoiseSpinner();
            /// <summary>
            /// Gets the "bounce" spinner.
            /// </summary>
            public static SpinnerBase Bounce { get; } = new BounceSpinner();
            /// <summary>
            /// Gets the "boxBounce" spinner.
            /// </summary>
            public static SpinnerBase BoxBounce { get; } = new BoxBounceSpinner();
            /// <summary>
            /// Gets the "boxBounce2" spinner.
            /// </summary>
            public static SpinnerBase BoxBounce2 { get; } = new BoxBounce2Spinner();
            /// <summary>
            /// Gets the "triangle" spinner.
            /// </summary>
            public static SpinnerBase Triangle { get; } = new TriangleSpinner();
            /// <summary>
            /// Gets the "binary" spinner.
            /// </summary>
            public static SpinnerBase Binary { get; } = new BinarySpinner();
            /// <summary>
            /// Gets the "arc" spinner.
            /// </summary>
            public static SpinnerBase Arc { get; } = new ArcSpinner();
            /// <summary>
            /// Gets the "circle" spinner.
            /// </summary>
            public static SpinnerBase Circle { get; } = new CircleSpinner();
            /// <summary>
            /// Gets the "squareCorners" spinner.
            /// </summary>
            public static SpinnerBase SquareCorners { get; } = new SquareCornersSpinner();
            /// <summary>
            /// Gets the "circleQuarters" spinner.
            /// </summary>
            public static SpinnerBase CircleQuarters { get; } = new CircleQuartersSpinner();
            /// <summary>
            /// Gets the "circleHalves" spinner.
            /// </summary>
            public static SpinnerBase CircleHalves { get; } = new CircleHalvesSpinner();
            /// <summary>
            /// Gets the "squish" spinner.
            /// </summary>
            public static SpinnerBase Squish { get; } = new SquishSpinner();
            /// <summary>
            /// Gets the "toggle" spinner.
            /// </summary>
            public static SpinnerBase Toggle { get; } = new ToggleSpinner();
            /// <summary>
            /// Gets the "toggle2" spinner.
            /// </summary>
            public static SpinnerBase Toggle2 { get; } = new Toggle2Spinner();
            /// <summary>
            /// Gets the "toggle3" spinner.
            /// </summary>
            public static SpinnerBase Toggle3 { get; } = new Toggle3Spinner();
            /// <summary>
            /// Gets the "toggle4" spinner.
            /// </summary>
            public static SpinnerBase Toggle4 { get; } = new Toggle4Spinner();
            /// <summary>
            /// Gets the "toggle5" spinner.
            /// </summary>
            public static SpinnerBase Toggle5 { get; } = new Toggle5Spinner();
            /// <summary>
            /// Gets the "toggle6" spinner.
            /// </summary>
            public static SpinnerBase Toggle6 { get; } = new Toggle6Spinner();
            /// <summary>
            /// Gets the "toggle7" spinner.
            /// </summary>
            public static SpinnerBase Toggle7 { get; } = new Toggle7Spinner();
            /// <summary>
            /// Gets the "toggle8" spinner.
            /// </summary>
            public static SpinnerBase Toggle8 { get; } = new Toggle8Spinner();
            /// <summary>
            /// Gets the "toggle9" spinner.
            /// </summary>
            public static SpinnerBase Toggle9 { get; } = new Toggle9Spinner();
            /// <summary>
            /// Gets the "toggle10" spinner.
            /// </summary>
            public static SpinnerBase Toggle10 { get; } = new Toggle10Spinner();
            /// <summary>
            /// Gets the "toggle11" spinner.
            /// </summary>
            public static SpinnerBase Toggle11 { get; } = new Toggle11Spinner();
            /// <summary>
            /// Gets the "toggle12" spinner.
            /// </summary>
            public static SpinnerBase Toggle12 { get; } = new Toggle12Spinner();
            /// <summary>
            /// Gets the "toggle13" spinner.
            /// </summary>
            public static SpinnerBase Toggle13 { get; } = new Toggle13Spinner();
            /// <summary>
            /// Gets the "arrow" spinner.
            /// </summary>
            public static SpinnerBase Arrow { get; } = new ArrowSpinner();
            /// <summary>
            /// Gets the "arrow2" spinner.
            /// </summary>
            public static SpinnerBase Arrow2 { get; } = new Arrow2Spinner();
            /// <summary>
            /// Gets the "arrow3" spinner.
            /// </summary>
            public static SpinnerBase Arrow3 { get; } = new Arrow3Spinner();
            /// <summary>
            /// Gets the "bouncingBar" spinner.
            /// </summary>
            public static SpinnerBase BouncingBar { get; } = new BouncingBarSpinner();
            /// <summary>
            /// Gets the "bouncingBall" spinner.
            /// </summary>
            public static SpinnerBase BouncingBall { get; } = new BouncingBallSpinner();
            /// <summary>
            /// Gets the "smiley" spinner.
            /// </summary>
            public static SpinnerBase Smiley { get; } = new SmileySpinner();
            /// <summary>
            /// Gets the "monkey" spinner.
            /// </summary>
            public static SpinnerBase Monkey { get; } = new MonkeySpinner();
            /// <summary>
            /// Gets the "hearts" spinner.
            /// </summary>
            public static SpinnerBase Hearts { get; } = new HeartsSpinner();
            /// <summary>
            /// Gets the "clock" spinner.
            /// </summary>
            public static SpinnerBase Clock { get; } = new ClockSpinner();
            /// <summary>
            /// Gets the "earth" spinner.
            /// </summary>
            public static SpinnerBase Earth { get; } = new EarthSpinner();
            /// <summary>
            /// Gets the "material" spinner.
            /// </summary>
            public static SpinnerBase Material { get; } = new MaterialSpinner();
            /// <summary>
            /// Gets the "moon" spinner.
            /// </summary>
            public static SpinnerBase Moon { get; } = new MoonSpinner();
            /// <summary>
            /// Gets the "runner" spinner.
            /// </summary>
            public static SpinnerBase Runner { get; } = new RunnerSpinner();
            /// <summary>
            /// Gets the "pong" spinner.
            /// </summary>
            public static SpinnerBase Pong { get; } = new PongSpinner();
            /// <summary>
            /// Gets the "shark" spinner.
            /// </summary>
            public static SpinnerBase Shark { get; } = new SharkSpinner();
            /// <summary>
            /// Gets the "dqpb" spinner.
            /// </summary>
            public static SpinnerBase Dqpb { get; } = new DqpbSpinner();
            /// <summary>
            /// Gets the "weather" spinner.
            /// </summary>
            public static SpinnerBase Weather { get; } = new WeatherSpinner();
            /// <summary>
            /// Gets the "christmas" spinner.
            /// </summary>
            public static SpinnerBase Christmas { get; } = new ChristmasSpinner();
            /// <summary>
            /// Gets the "grenade" spinner.
            /// </summary>
            public static SpinnerBase Grenade { get; } = new GrenadeSpinner();
            /// <summary>
            /// Gets the "point" spinner.
            /// </summary>
            public static SpinnerBase Point { get; } = new PointSpinner();
            /// <summary>
            /// Gets the "layer" spinner.
            /// </summary>
            public static SpinnerBase Layer { get; } = new LayerSpinner();
            /// <summary>
            /// Gets the "betaWave" spinner.
            /// </summary>
            public static SpinnerBase BetaWave { get; } = new BetaWaveSpinner();
            /// <summary>
            /// Gets the "fingerDance" spinner.
            /// </summary>
            public static SpinnerBase FingerDance { get; } = new FingerDanceSpinner();
            /// <summary>
            /// Gets the "fistBump" spinner.
            /// </summary>
            public static SpinnerBase FistBump { get; } = new FistBumpSpinner();
            /// <summary>
            /// Gets the "soccerHeader" spinner.
            /// </summary>
            public static SpinnerBase SoccerHeader { get; } = new SoccerHeaderSpinner();
            /// <summary>
            /// Gets the "mindblown" spinner.
            /// </summary>
            public static SpinnerBase Mindblown { get; } = new MindblownSpinner();
            /// <summary>
            /// Gets the "speaker" spinner.
            /// </summary>
            public static SpinnerBase Speaker { get; } = new SpeakerSpinner();
            /// <summary>
            /// Gets the "orangePulse" spinner.
            /// </summary>
            public static SpinnerBase OrangePulse { get; } = new OrangePulseSpinner();
            /// <summary>
            /// Gets the "bluePulse" spinner.
            /// </summary>
            public static SpinnerBase BluePulse { get; } = new BluePulseSpinner();
            /// <summary>
            /// Gets the "orangeBluePulse" spinner.
            /// </summary>
            public static SpinnerBase OrangeBluePulse { get; } = new OrangeBluePulseSpinner();
            /// <summary>
            /// Gets the "timeTravel" spinner.
            /// </summary>
            public static SpinnerBase TimeTravel { get; } = new TimeTravelSpinner();
            /// <summary>
            /// Gets the "aesthetic" spinner.
            /// </summary>
            public static SpinnerBase Aesthetic { get; } = new AestheticSpinner();
            /// <summary>
            /// Gets the "dwarfFortress" spinner.
            /// </summary>
            public static SpinnerBase DwarfFortress { get; } = new DwarfFortressSpinner();

            /// <summary>
            /// Gets the known <see cref="SpinnerBase"/> that corresponds to the specified <see cref="SpinnersType"/>.
            /// </summary>
            /// <param name="type">The spinner identifier to resolve.</param>
            /// <returns>The <see cref="SpinnerBase"/> associated with <paramref name="type"/>.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="type"/> is not a defined value.</exception>
            public static SpinnerBase FromType(SpinnersType type) => type switch
            {
                SpinnersType.Default => Default,
                SpinnersType.Ascii => Ascii,
                SpinnersType.Dots => Dots,
                SpinnersType.Dots2 => Dots2,
                SpinnersType.Dots3 => Dots3,
                SpinnersType.Dots4 => Dots4,
                SpinnersType.Dots5 => Dots5,
                SpinnersType.Dots6 => Dots6,
                SpinnersType.Dots7 => Dots7,
                SpinnersType.Dots8 => Dots8,
                SpinnersType.Dots9 => Dots9,
                SpinnersType.Dots10 => Dots10,
                SpinnersType.Dots11 => Dots11,
                SpinnersType.Dots12 => Dots12,
                SpinnersType.Dots13 => Dots13,
                SpinnersType.Dots14 => Dots14,
                SpinnersType.Dots8Bit => Dots8Bit,
                SpinnersType.DotsCircle => DotsCircle,
                SpinnersType.Sand => Sand,
                SpinnersType.Line => Line,
                SpinnersType.Line2 => Line2,
                SpinnersType.Pipe => Pipe,
                SpinnersType.SimpleDots => SimpleDots,
                SpinnersType.SimpleDotsScrolling => SimpleDotsScrolling,
                SpinnersType.Star => Star,
                SpinnersType.Star2 => Star2,
                SpinnersType.Flip => Flip,
                SpinnersType.Hamburger => Hamburger,
                SpinnersType.GrowVertical => GrowVertical,
                SpinnersType.GrowHorizontal => GrowHorizontal,
                SpinnersType.Balloon => Balloon,
                SpinnersType.Balloon2 => Balloon2,
                SpinnersType.Noise => Noise,
                SpinnersType.Bounce => Bounce,
                SpinnersType.BoxBounce => BoxBounce,
                SpinnersType.BoxBounce2 => BoxBounce2,
                SpinnersType.Triangle => Triangle,
                SpinnersType.Binary => Binary,
                SpinnersType.Arc => Arc,
                SpinnersType.Circle => Circle,
                SpinnersType.SquareCorners => SquareCorners,
                SpinnersType.CircleQuarters => CircleQuarters,
                SpinnersType.CircleHalves => CircleHalves,
                SpinnersType.Squish => Squish,
                SpinnersType.Toggle => Toggle,
                SpinnersType.Toggle2 => Toggle2,
                SpinnersType.Toggle3 => Toggle3,
                SpinnersType.Toggle4 => Toggle4,
                SpinnersType.Toggle5 => Toggle5,
                SpinnersType.Toggle6 => Toggle6,
                SpinnersType.Toggle7 => Toggle7,
                SpinnersType.Toggle8 => Toggle8,
                SpinnersType.Toggle9 => Toggle9,
                SpinnersType.Toggle10 => Toggle10,
                SpinnersType.Toggle11 => Toggle11,
                SpinnersType.Toggle12 => Toggle12,
                SpinnersType.Toggle13 => Toggle13,
                SpinnersType.Arrow => Arrow,
                SpinnersType.Arrow2 => Arrow2,
                SpinnersType.Arrow3 => Arrow3,
                SpinnersType.BouncingBar => BouncingBar,
                SpinnersType.BouncingBall => BouncingBall,
                SpinnersType.Smiley => Smiley,
                SpinnersType.Monkey => Monkey,
                SpinnersType.Hearts => Hearts,
                SpinnersType.Clock => Clock,
                SpinnersType.Earth => Earth,
                SpinnersType.Material => Material,
                SpinnersType.Moon => Moon,
                SpinnersType.Runner => Runner,
                SpinnersType.Pong => Pong,
                SpinnersType.Shark => Shark,
                SpinnersType.Dqpb => Dqpb,
                SpinnersType.Weather => Weather,
                SpinnersType.Christmas => Christmas,
                SpinnersType.Grenade => Grenade,
                SpinnersType.Point => Point,
                SpinnersType.Layer => Layer,
                SpinnersType.BetaWave => BetaWave,
                SpinnersType.FingerDance => FingerDance,
                SpinnersType.FistBump => FistBump,
                SpinnersType.SoccerHeader => SoccerHeader,
                SpinnersType.Mindblown => Mindblown,
                SpinnersType.Speaker => Speaker,
                SpinnersType.OrangePulse => OrangePulse,
                SpinnersType.BluePulse => BluePulse,
                SpinnersType.OrangeBluePulse => OrangeBluePulse,
                SpinnersType.TimeTravel => TimeTravel,
                SpinnersType.Aesthetic => Aesthetic,
                SpinnersType.DwarfFortress => DwarfFortress,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };
        }
    }
}
