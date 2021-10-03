// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;

using PromptPlus.ValueObjects;

namespace PromptPlus
{
    public interface IFormPPlusBase : IDisposable
    {
        string PipeId { get; set; }

        string PipeTitle { get; set; }

        object ContextState { get; set; }

        Func<ResultPipe[], object, bool> PipeCondition { get; set; }

    }
}
