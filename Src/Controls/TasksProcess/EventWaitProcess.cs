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
        private T _value = default;
        private readonly SemaphoreSlim semaphore = new(1, 1);
        private bool _disposed;

        private EventWaitProcess()
        {
            throw new PromptPlusException("EventWaitProcess CTOR NotImplemented");
        }

        internal EventWaitProcess(ref T value, bool cancelnextalltasks)
        {
            _value = value;
            CancelAllNextTasks = cancelnextalltasks;
        }


        /// <summary>
        /// Get/set Context value
        /// </summary>
        public T Context 
        { 
            get 
            {
                T aux;
                semaphore.Wait();
                aux = _value;
                semaphore.Release();
                return aux;
            }
            set
            {
                semaphore.Wait();
                _value = value;
                semaphore.Release();
            }
        }

        /// <summary>
        /// Get/Set Cancel all next tasks.
        /// </summary>
        public bool CancelAllNextTasks { get; set; }


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

