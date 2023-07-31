// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents the commands to update values of Progress Bar
    /// </summary>
    /// <typeparam name="T">typeof instance result</typeparam>
    public class UpdateProgressBar<T> : IDisposable
    {
        private string _lastdescription;
        private double? _lastvalue;
        private readonly SemaphoreSlim semaphore = new(1, 1);
        private bool _disposed;

        private UpdateProgressBar()
        {
            throw new PromptPlusException("UpdateProgressBar CTOR NotImplemented");
        }

        internal UpdateProgressBar(ref T context, double value,double min, double max, string desc)
        {
            _lastvalue = null;
            _lastdescription = desc;
            Context = context;
            Finish = false;
            Value = value;
            Minvalue = min; 
            Maxvalue = max;
        }

        internal bool HasChange()
        {
            var result = false;
            semaphore.Wait();
            if (_lastdescription != Description)
            {
                _lastdescription = Description;
                result = true;
            }
            if (!_lastvalue.HasValue || _lastvalue != Value)
            {
                _lastvalue = Value;
                result = true;
            }
            if (Finish)
            {
                result = true;
            }
            semaphore.Release();
            return result;
        }

        /// <summary>
        /// Maximum value of Progress Bar
        /// </summary>
        public double Maxvalue { get; private set; }

        /// <summary>
        /// Minimal value of Progress Bar
        /// </summary>
        public double Minvalue { get; private set; }

        /// <summary>
        /// Current value of Progress Bar
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// Get/Set Finish Progress Bar
        /// </summary>
        public bool Finish { get; set; }

        /// <summary>
        /// Get/Set instance result value for general purpose
        /// </summary>
        public T Context { get; set; }

        /// <summary>
        /// Current Description
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Update current value
        /// </summary>
        /// <param name="value">new current value</param>
        public void Update(double value)
        {
            semaphore.Wait();
            if (value > Maxvalue)
            {
                value = Maxvalue;
            }
            if (value < Minvalue)
            {
                value = Minvalue;
            }
            Value = value; 
            semaphore.Release();
        }

        /// <summary>
        /// Change curent Description
        /// </summary>
        /// <param name="value">new description</param>
        public void ChangeDescription(string value)
        {
            semaphore.Wait();
            Description = value;
            semaphore.Release();
        }


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
