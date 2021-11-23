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
        private readonly bool _disableEscAbort;

        /// <summary>
        /// Contains the logic to prompt for the various types of arguments.
        /// </summary>
        /// <param name="pageSize">the page size for selection lists.</param>
        /// <param name="getPromptTextCallback">Used to customize the generation of the prompt text.</param>
        public PromptPlusArgumentPrompter(
            int pageSize = 10,bool disableEscAbort = false,
            Func<CommandContext, IArgument, string>? getPromptTextCallback = null)
        {
            _pageSize = pageSize;
            _disableEscAbort = disableEscAbort;
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
                return UtilExtension.PromptForTypeArgumentValues(ctx, argument,description, _pageSize, _disableEscAbort, out isCancellationRequested);
            }
            else
            {
                return UtilExtension.PromptForPromptPlusTypeArgumentValues(ctx, argument, description,_pageSize, kindprompt, _disableEscAbort, out isCancellationRequested);
            }
        }
    }
}
