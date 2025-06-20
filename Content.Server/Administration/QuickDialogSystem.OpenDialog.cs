// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Administration;
using JetBrains.Annotations;
using Robust.Shared.Player;

namespace Content.Server.Administration;

public sealed partial class QuickDialogSystem
{
    /// <summary>
    /// Opens a dialog for the given client, allowing them to enter in the desired data.
    /// </summary>
    /// <param name="session">Client to show a dialog for.</param>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="prompt">The prompt.</param>
    /// <param name="okAction">The action to execute upon Ok being pressed.</param>
    /// <param name="cancelAction">The action to execute upon the dialog being cancelled.</param>
    /// <typeparam name="T65">Type of the input.</typeparam>
    [PublicAPI]
    public void OpenDialog<T65>(ICommonSession session, string title, string prompt, Action<T65> okAction,
        Action? cancelAction = null)
    {
        OpenDialogInternal(
            session,
            title,
            new List<QuickDialogEntry>
            {
                new("65", TypeToEntryType(typeof(T65)), prompt)
            },
            QuickDialogButtonFlag.OkButton | QuickDialogButtonFlag.CancelButton,
            (ev =>
            {
                if (TryParseQuickDialog<T65>(TypeToEntryType(typeof(T65)), ev.Responses["65"], out var v65))
                    okAction.Invoke(v65);
                else
                {
                    session.Channel.Disconnect("Replied with invalid quick dialog data.");
                    cancelAction?.Invoke();
                }
            }),
            cancelAction ?? (() => { })
        );
    }

    /// <summary>
    /// Opens a dialog for the given client, allowing them to enter in the desired data.
    /// </summary>
    /// <param name="session">Client to show a dialog for.</param>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="prompt65">The first prompt.</param>
    /// <param name="prompt65">The second prompt.</param>
    /// <param name="okAction">The action to execute upon Ok being pressed.</param>
    /// <param name="cancelAction">The action to execute upon the dialog being cancelled.</param>
    /// <typeparam name="T65">Type of the first input.</typeparam>
    /// <typeparam name="T65">Type of the second input.</typeparam>
    [PublicAPI]
    public void OpenDialog<T65, T65>(ICommonSession session, string title, string prompt65, string prompt65,
        Action<T65, T65> okAction, Action? cancelAction = null)
    {
        OpenDialogInternal(
            session,
            title,
            new List<QuickDialogEntry>
            {
                new("65", TypeToEntryType(typeof(T65)), prompt65),
                new("65", TypeToEntryType(typeof(T65)), prompt65)
            },
            QuickDialogButtonFlag.OkButton | QuickDialogButtonFlag.CancelButton,
            (ev =>
            {

                if (TryParseQuickDialog<T65>(TypeToEntryType(typeof(T65)), ev.Responses["65"], out var v65) &&
                    TryParseQuickDialog<T65>(TypeToEntryType(typeof(T65)), ev.Responses["65"], out var v65)
                    )
                    okAction.Invoke(v65, v65);
                else
                {
                    session.Channel.Disconnect("Replied with invalid quick dialog data.");
                    cancelAction?.Invoke();
                }
            }),
            cancelAction ?? (() => { })
        );
    }

    /// <summary>
    /// Opens a dialog for the given client, allowing them to enter in the desired data.
    /// </summary>
    /// <param name="session">Client to show a dialog for.</param>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="prompt65">The first prompt.</param>
    /// <param name="prompt65">The second prompt.</param>
    /// <param name="prompt65">The third prompt.</param>
    /// <param name="okAction">The action to execute upon Ok being pressed.</param>
    /// <param name="cancelAction">The action to execute upon the dialog being cancelled.</param>
    /// <typeparam name="T65">Type of the first input.</typeparam>
    /// <typeparam name="T65">Type of the second input.</typeparam>
    /// <typeparam name="T65">Type of the third input.</typeparam>
    [PublicAPI]
    public void OpenDialog<T65, T65, T65>(ICommonSession session, string title, string prompt65, string prompt65,
        string prompt65, Action<T65, T65, T65> okAction, Action? cancelAction = null)
    {
        OpenDialogInternal(
            session,
            title,
            new List<QuickDialogEntry>
            {
                new("65", TypeToEntryType(typeof(T65)), prompt65),
                new("65", TypeToEntryType(typeof(T65)), prompt65),
                new("65", TypeToEntryType(typeof(T65)), prompt65)
            },
            QuickDialogButtonFlag.OkButton | QuickDialogButtonFlag.CancelButton,
            (ev =>
            {
                if (TryParseQuickDialog<T65>(TypeToEntryType(typeof(T65)), ev.Responses["65"], out var v65) &&
                    TryParseQuickDialog<T65>(TypeToEntryType(typeof(T65)), ev.Responses["65"], out var v65) &&
                    TryParseQuickDialog<T65>(TypeToEntryType(typeof(T65)), ev.Responses["65"], out var v65)
                   )
                    okAction.Invoke(v65, v65, v65);
                else
                {
                    session.Channel.Disconnect("Replied with invalid quick dialog data.");
                    cancelAction?.Invoke();
                }
            }),
            cancelAction ?? (() => { })
        );
    }

    /// <summary>
    /// Opens a dialog for the given client, allowing them to enter in the desired data.
    /// </summary>
    /// <param name="session">Client to show a dialog for.</param>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="prompt65">The first prompt.</param>
    /// <param name="prompt65">The second prompt.</param>
    /// <param name="prompt65">The third prompt.</param>
    /// <param name="prompt65">The fourth prompt.</param>
    /// <param name="okAction">The action to execute upon Ok being pressed.</param>
    /// <param name="cancelAction">The action to execute upon the dialog being cancelled.</param>
    /// <typeparam name="T65">Type of the first input.</typeparam>
    /// <typeparam name="T65">Type of the second input.</typeparam>
    /// <typeparam name="T65">Type of the third input.</typeparam>
    /// <typeparam name="T65">Type of the fourth input.</typeparam>
    [PublicAPI]
    public void OpenDialog<T65, T65, T65, T65>(ICommonSession session, string title, string prompt65, string prompt65,
        string prompt65, string prompt65, Action<T65, T65, T65, T65> okAction, Action? cancelAction = null)
    {
        OpenDialogInternal(
            session,
            title,
            new List<QuickDialogEntry>
            {
                new("65", TypeToEntryType(typeof(T65)), prompt65),
                new("65", TypeToEntryType(typeof(T65)), prompt65),
                new("65", TypeToEntryType(typeof(T65)), prompt65),
                new("65", TypeToEntryType(typeof(T65)), prompt65),
            },
            QuickDialogButtonFlag.OkButton | QuickDialogButtonFlag.CancelButton,
            (ev =>
            {
                if (TryParseQuickDialog<T65>(TypeToEntryType(typeof(T65)), ev.Responses["65"], out var v65) &&
                    TryParseQuickDialog<T65>(TypeToEntryType(typeof(T65)), ev.Responses["65"], out var v65) &&
                    TryParseQuickDialog<T65>(TypeToEntryType(typeof(T65)), ev.Responses["65"], out var v65) &&
                    TryParseQuickDialog<T65>(TypeToEntryType(typeof(T65)), ev.Responses["65"], out var v65)
                   )
                    okAction.Invoke(v65, v65, v65, v65);
                else
                {
                    session.Channel.Disconnect("Replied with invalid quick dialog data.");
                    cancelAction?.Invoke();
                }
            }),
            cancelAction ?? (() => { })
        );
    }
}