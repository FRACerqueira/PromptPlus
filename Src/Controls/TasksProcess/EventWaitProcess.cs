using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents the event to task process with with conex value
    /// </summary>
    /// <typeparam name="T">Typeof Input</typeparam>
    public class EventWaitProcess<T>
    {
        private object _lock = new object();
        private T _value = default;
        private bool _cancelnext;

        private EventWaitProcess()
        {
            throw new PromptPlusException("EventWaitProcess CTOR NotImplemented");
        }

        internal EventWaitProcess(ref T value, bool cancelnextalltasks)
        {
            _value = value;
            _cancelnext = cancelnextalltasks;
        }


        /// <summary>
        /// Get/set Context value
        /// </summary>
        public T Context 
        { 
            get 
            {
                lock (_lock)
                { 
                    return _value;
                }
            }
            set
            {
                lock (_lock)
                {
                    _value = value;
                }
            }
        }

        /// <summary>
        /// Get/Set Cancel all next tasks.
        /// </summary>
        public bool CancelAllNextTasks
        {
            get
            {
                lock (_lock)
                {
                    return _cancelnext;
                }
            }
            set
            {
                lock (_lock)
                {
                    _cancelnext = value;
                }
            }
        }
    }
}

