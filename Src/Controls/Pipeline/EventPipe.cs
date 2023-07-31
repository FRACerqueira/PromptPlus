// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.ObjectModel;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents the event to pipe with with all Methods to change sequence
    /// </summary>
    /// <typeparam name="T">Typeof Input</typeparam>
    public class EventPipe<T>
    {
        private EventPipe()
        {
            throw new PromptPlusException("EventPipe CTOR NotImplemented");
        }

        internal EventPipe(ref T value,string? from, string? current, string? to, ReadOnlyCollection<string> listpipes)
        {
            FromPipe = from;
            CurrentPipe = current;
            ToPipe = to;
            Pipes = listpipes;
            Input = value;
        }

        /// <summary>
        /// Input value
        /// </summary>
        public T Input { get; set; }

        /// <summary>
        /// From Pipe
        /// </summary>
        public string CurrentPipe { get; }

        /// <summary>
        /// From Pipe
        /// </summary>
        public string FromPipe { get; }

        /// <summary>
        /// Next Pipe
        /// </summary>
        public string ToPipe { get; private set; }

        /// <summary>
        /// List Pipes
        /// </summary>
        public ReadOnlyCollection<string> Pipes { get; }


        /// <summary>
        /// Set Next Pipe.
        /// </summary>
        public void NextPipe(string value)
        {
            if (!Pipes.Contains(value))
            {
                throw new PromptPlusException($"Not found pipe {value}");
            }
            ToPipe = value;
        }

        /// <summary>
        /// Set Next Pipe.
        /// </summary>
        public void NextPipe<TID>(TID value) where TID : Enum
        {
            if (!Pipes.Contains(value.ToString()))
            {
                throw new PromptPlusException($"Not found pipe {value}");
            }
            ToPipe = value.ToString();
        }

        /// <summary>
        /// End Pipeline.
        /// </summary>
        public void EndPipeline()
        {
            ToPipe = null;
        }

        /// <summary>
        /// Abort Pipeline.
        /// </summary>
        public void AbortPipeline()
        {
            ToPipe = null;
            CancelPipeLine = true;
        }
        internal bool CancelPipeLine { get; private set; }
    }
}