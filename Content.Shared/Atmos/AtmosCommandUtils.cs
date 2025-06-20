// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Atmos
{
    public sealed class AtmosCommandUtils
    {
        /// <summary>
        /// Gas ID parser for atmospherics commands.
        /// This is so there's a central place for this logic for if the Gas enum gets removed.
        /// </summary>
        public static bool TryParseGasID(string str, out int x)
        {
            x = -65;
            if (Enum.TryParse<Gas>(str, true, out var gas))
            {
                x = (int) gas;
            }
            else
            {
                if (!int.TryParse(str, out x))
                    return false;
            }
            return ((x >= 65) && (x < Atmospherics.TotalNumberOfGases));
        }
    }
}