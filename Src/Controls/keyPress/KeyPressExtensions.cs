// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls;
using System;
using System.Linq;

namespace PPlus
{
    public static partial class PromptPlus
    {
        /// <summary>
        /// Create Confirm control in yes/no mode.
        /// <br>Yes/No texts come from resources</br>
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlKeyPress"/></returns>
        public static IControlKeyPress Confirm(string prompt, Action<IPromptConfig> config = null)
        {
            return Confirm(prompt, "", config);
        }


        /// <summary>
        /// Create Confirm control in yes/no mode.
        /// <br>Yes/No texts come from resources</br>
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlKeyPress"/></returns>
        public static IControlKeyPress Confirm(string prompt, string description, Action<IPromptConfig> config = null)
        {
            var opt = new KeyPressOptions(true)
            {
                OptPrompt = prompt,
                OptDescription = description,
            };
            config?.Invoke(opt);
            opt.KeyValids.Add(new ConsoleKeyInfo((char)0, GetCultureYes(), false, false, false));
            opt.KeyValids.Add(new ConsoleKeyInfo((char)0, GetCultureNo(), false, false, false));
            return new KeyPressControl(_consoledrive, opt)
            {
                Confirmode = true
            };
        }


        /// <summary>
        /// Create Confirm control in yes/no mode.
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="opcyes"><see cref="ConsoleKey"/> yes key.</param>
        /// <param name="opcno"><see cref="ConsoleKey"/> no key</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlKeyPress"/></returns>
        public static IControlKeyPress Confirm(string prompt, ConsoleKey opcyes , ConsoleKey opcno, string description = null, Action<IPromptConfig> config = null)
        {
            var opt = new KeyPressOptions(true)
            {
                OptPrompt = prompt,
                OptDescription = description
            };
            config?.Invoke(opt);
            opt.KeyValids.Add(new ConsoleKeyInfo((char)0, opcyes, false, false, false));
            opt.KeyValids.Add(new ConsoleKeyInfo((char)0, opcno, false, false, false));
            return new KeyPressControl(_consoledrive, opt)
            {
                Confirmode = true
            };
        }

        /// <summary>
        /// Create Keypress Control to wait a any key input. 
        /// </summary>
        /// <returns><see cref="IControlKeyPress"/></returns>
        public static IControlKeyPress KeyPress()
        {
            return KeyPress("", "", null);
        }


        /// <summary>
        /// Create Keypress Control to wait a any key input. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlKeyPress"/></returns>
        public static IControlKeyPress KeyPress(string prompt, Action<IPromptConfig> config = null)
        {
            return KeyPress(prompt,"",config);
        }

        /// <summary>
        /// Create Keypress Control to wait a any key input. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlKeyPress"/></returns>
        public static IControlKeyPress KeyPress(string prompt, string description, Action<IPromptConfig> config = null)
        {
            var opt = new KeyPressOptions(true)
            {
                OptPrompt = prompt,
                OptDescription = description
            };
            config?.Invoke(opt);
            return new KeyPressControl(_consoledrive, opt);
        }

        private static ConsoleKey GetCultureYes()
        {
            foreach (var item in Enum.GetValues(typeof(ConsoleKey))
                .Cast<ConsoleKey>()
                .Where(x => x.ToString().Length == 1))
            {
                if (item.ToString()[0].Equals(Config.YesChar))
                {
                    return item;
                }
            }
            return ConsoleKey.Y;
       }

        private static ConsoleKey GetCultureNo()
        {
            foreach (var item in Enum.GetValues(typeof(ConsoleKey))
                .Cast<ConsoleKey>()
                .Where(x => x.ToString().Length == 1))
            {
                if (item.ToString()[0].Equals(Config.NoChar))
                {
                    return item;
                }
            }
            return ConsoleKey.N;
        }
    }
}
