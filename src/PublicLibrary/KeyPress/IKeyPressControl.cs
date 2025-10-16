// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a keypress control.
    /// </summary>
    public interface IKeyPressControl
    {
        /// <summary>
        /// Adds a valid key and optional modifiers for keypress.
        /// </summary>
        /// <param name="key">The key, <see cref="ConsoleKey"/>.</param>
        /// <param name="modifiers">The optional modifiers, <see cref="ConsoleModifiers"/>.</param>
        /// <param name="showtext">The text to overwrite the default <see cref="ConsoleKey"/> string.</param>
        /// <returns>The current <see cref="IKeyPressControl"/> instance for chaining.</returns>
        IKeyPressControl AddKeyValid(ConsoleKey key, ConsoleModifiers? modifiers = null, string? showtext = null);

        /// <summary>
        /// Configures whether invalid keypresses should be displayed to the user.
        /// </summary>
        /// <param name="value">If <c>true</c>, invalid keypresses will be shown; otherwise, they will be hidden.</param>
        /// <returns>The current <see cref="IKeyPressControl"/> instance for chaining.</returns>
        IKeyPressControl ShowInvalidKey(bool value = true);

        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// </summary>
        /// <typeparam name="T">The type of the collection.</typeparam>
        /// <param name="items">The collection of items.</param>
        /// <param name="interactionaction">The interaction action to perform.</param>
        /// <returns>The current <see cref="IKeyPressControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="items"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionaction"/> is <c>null</c>.</exception>
        IKeyPressControl Interaction<T>(IEnumerable<T> items, Action<T, IKeyPressControl> interactionaction);

        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IKeyPressControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IKeyPressControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites styles for the keypress.
        /// </summary>
        /// <param name="styleType">The <see cref="KeyPressStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IKeyPressControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IKeyPressControl Styles(KeyPressStyles styleType, Style style);

        /// <summary>
        /// Shows a <see cref="SpinnersType"/> animation at the end of the prompt.
        /// </summary>
        /// <param name="spinnersType">The <see cref="SpinnersType"/> to display.</param>
        /// <returns>The current <see cref="IKeyPressControl"/> instance for chaining.</returns>
        IKeyPressControl Spinner(SpinnersType spinnersType);

        /// <summary>
        /// Runs the keypress control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result with type <see cref="ResultPrompt{T}"/> of the keypress control execution.</returns>
        ResultPrompt<ConsoleKeyInfo?> Run(CancellationToken token = default);
    }
}
