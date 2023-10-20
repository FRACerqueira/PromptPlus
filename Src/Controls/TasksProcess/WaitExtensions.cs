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
        /// <returns><see cref="IControlWait{T}"/></returns>
        public static IControlWait<T> WaitProcess<T>(string prompt, string description = null)
        {
            return WaitProcess<T>(prompt, description, null);
        }

        /// <summary>
        /// Create Wait Control
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <returns><see cref="IControlWait{T}"/></returns>
        public static IControlWait<object> WaitProcess(string prompt, string description = null)
        {
            return WaitProcess<object>(prompt, description, null);
        }

        /// <summary>
        /// Create Wait Control
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlWait{T}"/></returns>
        public static IControlWait<T> WaitProcess<T>(string prompt, string description, Action<IPromptConfig> config = null)
        {
            var opt = new WaitOptions<T>(_styleschema,_configcontrols,_consoledrive, false)
            {
                WaitTime = false,
                OptPrompt = prompt,
                OptDescription = description,
            };
            config?.Invoke(opt);
            return new WaitControl<T>(_consoledrive, opt);
        }


        /// <summary>
        /// Create Wait Control
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlWait{T}"/></returns>
        public static IControlWait<object> WaitProcess(string prompt, string description, Action<IPromptConfig> config = null)
        {
            var opt = new WaitOptions<object>(_styleschema, _configcontrols, _consoledrive, false)
            {
                WaitTime = false,
                OptPrompt = prompt,
                OptDescription = description,
            };
            config?.Invoke(opt);
            return new WaitControl<object>(_consoledrive, opt);
        }

        /// <summary>
        /// Create Wait Control with step <see cref="TimeSpan"/> delay and run
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="delay">The delay process</param>
        /// <param name="spinnersType">The <see cref="SpinnersType"/></param>
        /// <param name="showCountdown">True show Countdown, otherwise 'no'</param>
        /// <param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <param name="spinnerStyle"><see cref="Style"/> Spinner</param>
        /// <param name="elapsedTimeStyle"><see cref="Style"/> Countdown</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> for control</param>
        public static void WaitTimer(string prompt, TimeSpan delay, SpinnersType spinnersType = SpinnersType.Ascii, bool showCountdown = false, Action<IPromptConfig> config = null, Style? spinnerStyle = null, Style? elapsedTimeStyle = null, CancellationToken? cancellationToken = null)
        {
            var cts = cancellationToken ?? CancellationToken.None;
            var opt = new WaitOptions<object>(_styleschema, _configcontrols, _consoledrive, false)
            {
                WaitTime = true,
                TimeDelay = delay,
                ShowCountdown = showCountdown,
                OptPrompt = prompt
            };
            if (spinnerStyle != null)
            {
                opt.SpinnerStyle = spinnerStyle.Value;
            }
            if (spinnerStyle != null)
            {
                opt.ElapsedTimeStyle = elapsedTimeStyle.Value;
            }
            opt.HideAfterFinish(true);
            opt.EnabledAbortKey(false);
            config?.Invoke(opt);
            var aux = new WaitControl<object>(_consoledrive, opt);
            aux.Context(null);
            aux.Spinner(spinnersType);
            aux.AddStep(StepMode.Sequential, (eventw, cts) =>
            {
                cts.WaitHandle.WaitOne(delay);
            });
            aux.Run(cts);
        }
     }
}

