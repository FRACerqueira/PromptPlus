// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls;
using System;
using System.Threading;

namespace PPlus
{
    public static partial class PromptPlus
    {
        /// <summary>
        /// Create Wait Control
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <returns>IEnumerable <see cref="StateProcess"/> after Run method. <see cref="IControlWait"/></returns>
        public static IControlWait WaitProcess(string prompt, string description=null)
        {
            return WaitProcess(prompt,description, null);
        }

        /// <summary>
        /// Create Wait Control
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns>IEnumerable <see cref="StateProcess"/> after Run method. <see cref="IControlWait"/></returns>
        public static IControlWait WaitProcess(string prompt, string description, Action<IPromptConfig> config = null)
        {
            var opt = new WaitOptions(false)
            {
                WaitTime = false,
                OptPrompt = prompt,
                OptDescription = description,
            };
            config?.Invoke(opt);
            return new WaitControl(_consoledrive, opt);
        }

        /// <summary>
        /// Create Wait Control with step <see cref="TimeSpan"/> delay and run
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="delay">The delay process</param>
        /// <param name="spinnersType">spinners Type <see cref="SpinnersType"/></param>
        /// <param name="showCountdown">True show Countdown, otherwise 'no'</param>
        /// <param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> for control</param>

        public static void WaitTimer(string prompt, TimeSpan delay, SpinnersType spinnersType = SpinnersType.Ascii, bool showCountdown = false, Action<IPromptConfig> config = null, CancellationToken? cancellationToken = null)
        {
            var cts = cancellationToken ?? CancellationToken.None;
            var opt = new WaitOptions(false)
            {
                WaitTime = true,
                TimeDelay = delay,
                ShowCountdown = showCountdown,
                OptPrompt = prompt
            };
            opt.HideAfterFinish(true);
            opt.EnabledAbortKey(false);
            config?.Invoke(opt);
            var aux = new WaitControl(_consoledrive, opt);
            aux.Spinner(spinnersType);
            aux.AddStep(StepMode.Sequential, (cts) =>
            {
                cts.WaitHandle.WaitOne(delay);
            });
            aux.Run(cts);
        }
     }
}

