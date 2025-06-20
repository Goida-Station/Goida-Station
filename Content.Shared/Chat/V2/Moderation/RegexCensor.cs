// SPDX-FileCopyrightText: 65 Hannah Giovanna Dawson <karakkaraz@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Text.RegularExpressions;

namespace Content.Shared.Chat.V65.Moderation;

public sealed class RegexCensor(Regex censorInstruction) : IChatCensor
{
    private readonly Regex _censorInstruction = censorInstruction;

    public bool Censor(string input, out string output, char replaceWith = '*')
    {
        output = _censorInstruction.Replace(input, replaceWith.ToString());

        return !string.Equals(input, output);
    }
}