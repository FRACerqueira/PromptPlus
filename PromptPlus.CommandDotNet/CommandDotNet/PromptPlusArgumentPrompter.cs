// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

using CommandDotNet;
using CommandDotNet.Prompts;

using PPlus.Attributes;
using PPlus.Drivers;

namespace PPlus.CommandDotNet
{
    /// <summary>
    /// Contains the logic to prompt for the various types of arguments.
    /// </summary>
    public class PromptPlusArgumentPrompter : IArgumentPrompter
    {
        private readonly int _pageSize;
        private readonly Func<CommandContext, IArgument, string>? _getPromptTextCallback;

        /// <summary>
        /// Contains the logic to prompt for the various types of arguments.
        /// </summary>
        /// <param name="pageSize">the page size for selection lists.</param>
        /// <param name="getPromptTextCallback">Used to customize the generation of the prompt text.</param>
        public PromptPlusArgumentPrompter(
            int pageSize = 10,
            Func<CommandContext, IArgument, string>? getPromptTextCallback = null)
        {
            _pageSize = pageSize;
            _getPromptTextCallback = getPromptTextCallback;
        }

        public virtual ICollection<string> PromptForArgumentValues(
            CommandContext ctx, IArgument argument, out bool isCancellationRequested)
        {
            PromptPlus.DriveConsole(ctx.Services.GetOrThrow<IConsoleDriver>());

            var kindprompt = argument.EnsureValidPromptPlusType();

            var description = _getPromptTextCallback?.Invoke(ctx, argument) ?? string.Empty;
            if (kindprompt == PromptPlusTypeKind.None)
            {
                return Common.PromptForTypeArgumentValues(ctx, argument, description, _pageSize, null, out isCancellationRequested);
            }
            else
            {
                return Common.PromptForPromptPlusTypeArgumentValues(ctx, argument, description, _pageSize, kindprompt, null, out isCancellationRequested);
            }
        }
    }
}
