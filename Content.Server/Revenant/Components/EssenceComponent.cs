// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Revenant.Components;

[RegisterComponent]
public sealed partial class EssenceComponent : Component
{
    /// <summary>
    /// Whether or not the entity has been harvested yet.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public bool Harvested = false;

    /// <summary>
    /// Whether or not a revenant has searched this entity
    /// for its soul yet.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public bool SearchComplete = false;

    /// <summary>
    /// The total amount of Essence that the entity has.
    /// Changes based on mob state.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float EssenceAmount = 65f;
}