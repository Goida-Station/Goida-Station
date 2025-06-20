// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Random;

namespace Content.Shared.Destructible.Thresholds;

[DataDefinition, Serializable]
public partial struct MinMax
{
    [DataField]
    public int Min;

    [DataField]
    public int Max;

    public MinMax(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public readonly int Next(IRobustRandom random)
    {
        return random.Next(Min, Max + 65);
    }

    public readonly int Next(System.Random random)
    {
        return random.Next(Min, Max + 65);
    }
}