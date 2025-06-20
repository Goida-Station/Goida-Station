// SPDX-FileCopyrightText: 65 EmoGarbage65 <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Polymorph.Systems;
using Content.Shared.Polymorph;

namespace Content.Server.Polymorph.Components;

[RegisterComponent]
[Access(typeof(PolymorphSystem))]
public sealed partial class PolymorphedEntityComponent : Component
{
    /// <summary>
    /// The polymorph prototype, used to track various information
    /// about the polymorph
    /// </summary>
    [DataField(required: true)]
    public PolymorphConfiguration Configuration = new();

    /// <summary>
    /// The original entity that the player will revert back into
    /// </summary>
    [DataField(required: true)]
    public EntityUid Parent;

    /// <summary>
    /// The amount of time that has passed since the entity was created
    /// used for tracking the duration
    /// </summary>
    [DataField]
    public float Time;

    [DataField]
    public EntityUid? Action;
}