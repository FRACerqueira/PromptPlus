// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents the available spinner styles that can be selected for animated console output.
    /// </summary>
    public enum SpinnersType
    {
        #region Dots and braille spinners

        /// <summary>
        /// The "Default" spinner.
        /// </summary>
        Default,

        /// <summary>
        /// The "dots" spinner.
        /// </summary>
        Dots,

        /// <summary>
        /// The "dots2" spinner.
        /// </summary>
        Dots2,

        /// <summary>
        /// The "dots3" spinner.
        /// </summary>
        Dots3,

        /// <summary>
        /// The "dots4" spinner.
        /// </summary>
        Dots4,

        /// <summary>
        /// The "dots5" spinner.
        /// </summary>
        Dots5,

        /// <summary>
        /// The "dots6" spinner.
        /// </summary>
        Dots6,

        /// <summary>
        /// The "dots7" spinner.
        /// </summary>
        Dots7,

        /// <summary>
        /// The "dots8" spinner.
        /// </summary>
        Dots8,

        /// <summary>
        /// The "dots9" spinner.
        /// </summary>
        Dots9,

        /// <summary>
        /// The "dots10" spinner.
        /// </summary>
        Dots10,

        /// <summary>
        /// The "dots11" spinner.
        /// </summary>
        Dots11,

        /// <summary>
        /// The "dots12" spinner.
        /// </summary>
        Dots12,

        /// <summary>
        /// The "dots13" spinner.
        /// </summary>
        Dots13,

        /// <summary>
        /// The "dots14" spinner.
        /// </summary>
        Dots14,

        /// <summary>
        /// The "dots8Bit" spinner.
        /// </summary>
        Dots8Bit,

        /// <summary>
        /// The "dotsCircle" spinner.
        /// </summary>
        DotsCircle,

        /// <summary>
        /// The "sand" spinner.
        /// </summary>
        Sand,

        #endregion

        #region Lines and bars spinners

        /// <summary>
        /// The "Ascii" spinner.
        /// </summary>
        Ascii,

        /// <summary>
        /// The "line" spinner.
        /// </summary>
        Line,

        /// <summary>
        /// The "line2" spinner.
        /// </summary>
        Line2,

        /// <summary>
        /// The "pipe" spinner.
        /// </summary>
        Pipe,

        /// <summary>
        /// The "simpleDots" spinner.
        /// </summary>
        SimpleDots,

        /// <summary>
        /// The "simpleDotsScrolling" spinner.
        /// </summary>
        SimpleDotsScrolling,

        /// <summary>
        /// The "flip" spinner.
        /// </summary>
        Flip,

        /// <summary>
        /// The "binary" spinner.
        /// </summary>
        Binary,

        /// <summary>
        /// The "dqpb" spinner.
        /// </summary>
        Dqpb,

        /// <summary>
        /// The "bouncingBar" spinner.
        /// </summary>
        BouncingBar,

        /// <summary>
        /// The "material" spinner.
        /// </summary>
        Material,

        /// <summary>
        /// The "betaWave" spinner.
        /// </summary>
        BetaWave,

        /// <summary>
        /// The "aesthetic" spinner.
        /// </summary>
        Aesthetic,

        /// <summary>
        /// The "layer" spinner.
        /// </summary>
        Layer,

        /// <summary>
        /// The "toggle13" spinner.
        /// </summary>
        Toggle13,

        #endregion

        #region Shapes spinners

        /// <summary>
        /// The "star" spinner.
        /// </summary>
        Star,

        /// <summary>
        /// The "star2" spinner.
        /// </summary>
        Star2,

        /// <summary>
        /// The "hamburger" spinner.
        /// </summary>
        Hamburger,

        /// <summary>
        /// The "growVertical" spinner.
        /// </summary>
        GrowVertical,

        /// <summary>
        /// The "growHorizontal" spinner.
        /// </summary>
        GrowHorizontal,

        /// <summary>
        /// The "balloon" spinner.
        /// </summary>
        Balloon,

        /// <summary>
        /// The "balloon2" spinner.
        /// </summary>
        Balloon2,

        /// <summary>
        /// The "noise" spinner.
        /// </summary>
        Noise,

        /// <summary>
        /// The "bounce" spinner.
        /// </summary>
        Bounce,

        /// <summary>
        /// The "boxBounce" spinner.
        /// </summary>
        BoxBounce,

        /// <summary>
        /// The "boxBounce2" spinner.
        /// </summary>
        BoxBounce2,

        /// <summary>
        /// The "triangle" spinner.
        /// </summary>
        Triangle,

        /// <summary>
        /// The "arc" spinner.
        /// </summary>
        Arc,

        /// <summary>
        /// The "circle" spinner.
        /// </summary>
        Circle,

        /// <summary>
        /// The "squareCorners" spinner.
        /// </summary>
        SquareCorners,

        /// <summary>
        /// The "circleQuarters" spinner.
        /// </summary>
        CircleQuarters,

        /// <summary>
        /// The "circleHalves" spinner.
        /// </summary>
        CircleHalves,

        /// <summary>
        /// The "squish" spinner.
        /// </summary>
        Squish,

        /// <summary>
        /// The "point" spinner.
        /// </summary>
        Point,

        #endregion

        #region Toggle spinners

        /// <summary>
        /// The "toggle" spinner.
        /// </summary>
        Toggle,

        /// <summary>
        /// The "toggle2" spinner.
        /// </summary>
        Toggle2,

        /// <summary>
        /// The "toggle3" spinner.
        /// </summary>
        Toggle3,

        /// <summary>
        /// The "toggle4" spinner.
        /// </summary>
        Toggle4,

        /// <summary>
        /// The "toggle5" spinner.
        /// </summary>
        Toggle5,

        /// <summary>
        /// The "toggle6" spinner.
        /// </summary>
        Toggle6,

        /// <summary>
        /// The "toggle7" spinner.
        /// </summary>
        Toggle7,

        /// <summary>
        /// The "toggle8" spinner.
        /// </summary>
        Toggle8,

        /// <summary>
        /// The "toggle9" spinner.
        /// </summary>
        Toggle9,

        /// <summary>
        /// The "toggle10" spinner.
        /// </summary>
        Toggle10,

        /// <summary>
        /// The "toggle11" spinner.
        /// </summary>
        Toggle11,

        /// <summary>
        /// The "toggle12" spinner.
        /// </summary>
        Toggle12,

        #endregion

        #region Arrows and motion spinners

        /// <summary>
        /// The "arrow" spinner.
        /// </summary>
        Arrow,

        /// <summary>
        /// The "arrow2" spinner.
        /// </summary>
        Arrow2,

        /// <summary>
        /// The "arrow3" spinner.
        /// </summary>
        Arrow3,

        /// <summary>
        /// The "bouncingBall" spinner.
        /// </summary>
        BouncingBall,

        /// <summary>
        /// The "pong" spinner.
        /// </summary>
        Pong,

        /// <summary>
        /// The "shark" spinner.
        /// </summary>
        Shark,

        #endregion

        #region Emoji spinners

        /// <summary>
        /// The "smiley" spinner.
        /// </summary>
        Smiley,

        /// <summary>
        /// The "monkey" spinner.
        /// </summary>
        Monkey,

        /// <summary>
        /// The "hearts" spinner.
        /// </summary>
        Hearts,

        /// <summary>
        /// The "clock" spinner.
        /// </summary>
        Clock,

        /// <summary>
        /// The "earth" spinner.
        /// </summary>
        Earth,

        /// <summary>
        /// The "moon" spinner.
        /// </summary>
        Moon,

        /// <summary>
        /// The "runner" spinner.
        /// </summary>
        Runner,

        /// <summary>
        /// The "weather" spinner.
        /// </summary>
        Weather,

        /// <summary>
        /// The "christmas" spinner.
        /// </summary>
        Christmas,

        /// <summary>
        /// The "grenade" spinner.
        /// </summary>
        Grenade,

        /// <summary>
        /// The "fingerDance" spinner.
        /// </summary>
        FingerDance,

        /// <summary>
        /// The "fistBump" spinner.
        /// </summary>
        FistBump,

        /// <summary>
        /// The "soccerHeader" spinner.
        /// </summary>
        SoccerHeader,

        /// <summary>
        /// The "mindblown" spinner.
        /// </summary>
        Mindblown,

        /// <summary>
        /// The "speaker" spinner.
        /// </summary>
        Speaker,

        /// <summary>
        /// The "orangePulse" spinner.
        /// </summary>
        OrangePulse,

        /// <summary>
        /// The "bluePulse" spinner.
        /// </summary>
        BluePulse,

        /// <summary>
        /// The "orangeBluePulse" spinner.
        /// </summary>
        OrangeBluePulse,

        /// <summary>
        /// The "timeTravel" spinner.
        /// </summary>
        TimeTravel,

        /// <summary>
        /// The "dwarfFortress" spinner.
        /// </summary>
        DwarfFortress

        #endregion
    }
}
