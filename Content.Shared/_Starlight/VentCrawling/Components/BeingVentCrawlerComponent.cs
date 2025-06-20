// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Rinary <65Rinary65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 ss65-Starlight <ss65-Starlight@outlook.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared._Starlight.VentCrawling.Components;

/// <summary>
/// A component indicating that the entity is in the process of moving through the venting process
/// </summary>
[RegisterComponent]
public sealed partial class BeingVentCrawlerComponent : Component
{
    /// <summary>
    /// The entity that contains this object in the vent
    /// </summary>
    [DataField("holder")]
    private EntityUid _holder;

    /// <summary>
    /// Gets or sets up a holder entity
    /// </summary>
    public EntityUid Holder
    {
        get => _holder;
        set
        {
            if (_holder == value)
                return;

            if (value == default)
                throw new ArgumentException("Holder cannot be default EntityUid");

            _holder = value;
        }
    }
}