using System;
using System.Threading;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    /// <summary>
    /// Represents the interface with all Methods of the Pipeline control
    /// </summary>
    /// <typeparam name="T">typeof return</typeparam>
    public interface IControlPipeline<T> : IPromptControls<ResultPipeline<T>>
    {
        /// <summary>
        /// Add the pipe
        /// </summary>
        /// <param name="idpipe">The unique id to pipe</param>
        /// <param name="command">The handler to execute. See <see cref="EventPipe{T}"/> to modified sequence</param>
        /// <param name="condition">The condition to start pipe. If true execute pipe, otherwise goto next pipe. See <see cref="EventPipe{T}"/> to modified sequence</param>
        /// <returns><see cref="IControlPipeline{T}"/></returns>
        IControlPipeline<T> AddPipe(string idpipe,Action<EventPipe<T>, CancellationToken> command, Func<EventPipe<T>, CancellationToken, bool> condition = null);

        /// <summary>
        /// Add the pipe by enum ID
        /// </summary>
        /// <param name="idpipe">The unique id to pipe</param>
        /// <param name="command">The handler to execute. See <see cref="EventPipe{T}"/> to modified sequence</param>
        /// <param name="condition">The condition to start pipe. If true execute pipe, otherwise goto next pipe. See <see cref="EventPipe{T}"/> to modified sequence</param>
        /// <returns><see cref="IControlPipeline{T}"/></returns>
        IControlPipeline<T> AddPipe<TID>(TID idpipe, Action<EventPipe<T>, CancellationToken> command, Func<EventPipe<T>, CancellationToken, bool> condition = null) where TID: Enum;

        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">Action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlPipeline{T}"/></returns>
        IControlPipeline<T> Config(Action<IPromptConfig> context);
    }
}
