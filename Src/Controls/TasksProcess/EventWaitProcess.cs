// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents the event to task process with with conex value
    /// </summary>
    /// <typeparam name="T">Typeof Input</typeparam>
    public class EventWaitProcess<T> : IDisposable
    {
        private readonly Action<Action<T?>> _value;
        private readonly SemaphoreSlim semaphore = new(1, 1);
        private bool _disposed;

        private EventWaitProcess()
        {
            throw new PromptPlusException("EventWaitProcess CTOR NotImplemented");
        }

        internal EventWaitProcess(Action<Action<T?>> changecontext, bool cancelalltasks)
        {
            _value = changecontext;
            CancelAllTasks = cancelalltasks;
        }


        /// <summary>
        /// Change value Context.
        /// <br>The change will only be executed if the Context exists(not null).</br>
        /// </summary>
        /// <param name="action">
        /// The action to change value.
        /// <br>The action will only be executed if the Context exists(not null).</br>
        /// </param>
        public void ChangeContext(Action<T> action)
        {
            _value(action!);
        }

        /// <summary>
        /// Get/Set Cancel all ran tasks.
        /// </summary>
        public bool CancelAllTasks { get; set; }


        #region IDisposable

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">if disposing</param> 
        internal protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    semaphore.Dispose();
                }
                _disposed = true;
            }
        }

        #endregion
    }
}

