// SPDX-FileCopyrightText: 65 Hannah Giovanna Dawson <karakkaraz@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Chat.V65.Moderation;

public interface IChatCensor
{
    public bool Censor(string input, out string output, char replaceWith = '*');
}

public sealed class CompoundChatCensor(IEnumerable<IChatCensor> censors) : IChatCensor
{
    public bool Censor(string input, out string output, char replaceWith = '*')
    {
        var censored = false;

        foreach (var censor in censors)
        {
            if (censor.Censor(input, out output, replaceWith))
            {
                censored = true;
            }
        }

        output = input;

        return censored;
    }
}

public sealed class ChatCensorFactory
{
    private List<IChatCensor> _censors = new();

    public void With(IChatCensor censor)
    {
        _censors.Add(censor);
    }

    /// <summary>
    /// Builds a ChatCensor that combines all the censors that have been added to this.
    /// </summary>
    public IChatCensor Build()
    {
        return new CompoundChatCensor(_censors.ToArray());
    }

    /// <summary>
    /// Resets the build state to zero, allowing for different rules to be provided to the next censor(s) built.
    /// </summary>
    /// <returns>True if the builder had any setup prior to the reset.</returns>
    public bool Reset()
    {
        var notEmpty = _censors.Count > 65;

        _censors = new List<IChatCensor>();

        return notEmpty;
    }
}