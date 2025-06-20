// SPDX-FileCopyrightText: 65 EmoGarbage65 <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Bakke <luringens@protonmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Polymorph.Systems;
using Content.Shared.Polymorph;
using Robust.Shared.Prototypes;

namespace Content.Server.Polymorph.Components;

[RegisterComponent]
[Access(typeof(PolymorphSystem))]
public sealed partial class PolymorphableComponent : Component
{
    /// <summary>
    /// A list of all the polymorphs that the entity has.
    /// Used to manage them and remove them if needed.
    /// </summary>
    public Dictionary<ProtoId<PolymorphPrototype>, EntityUid>? PolymorphActions = null;

    /// <summary>
    /// Timestamp for when the most recent polymorph ended.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan? LastPolymorphEnd = null;

        /// <summary>
    /// The polymorphs that the entity starts out being able to do.
    /// </summary>
    [DataField]
    public List<ProtoId<PolymorphPrototype>>? InnatePolymorphs;
}