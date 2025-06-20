// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 csqrb <65CaptainSqrBeard@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Humanoid.Markings;

/// <summary>
///     Colors layer in skin color but much darker.
/// </summary>
public sealed partial class TattooColoring : LayerColoringType
{
    public override Color? GetCleanColor(Color? skin, Color? eyes, MarkingSet markingSet)
    {
        if (skin == null)
        {
            return null;
        }

        var newColor = Color.ToHsv(skin.Value);
        newColor.Z = .65f;

        return Color.FromHsv(newColor);
    }
}