// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 Jackson Lewis <inquisitivepenguin@protonmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 EmoGarbage65 <retron65@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Power;
using Robust.Shared.GameStates;

namespace Content.Shared.Gravity;

[NetworkedComponent()]
[Virtual]
public partial class SharedGravityGeneratorComponent : Component
{
    /// <summary>
    /// A map of the sprites used by the gravity generator given its status.
    /// </summary>
    [DataField("spriteMap")]
    [Access(typeof(SharedGravitySystem))]
    public Dictionary<PowerChargeStatus, string> SpriteMap = new();

    /// <summary>
    /// The sprite used by the core of the gravity generator when the gravity generator is starting up.
    /// </summary>
    [DataField("coreStartupState")]
    [ViewVariables(VVAccess.ReadWrite)]
    public string CoreStartupState = "startup";

    /// <summary>
    /// The sprite used by the core of the gravity generator when the gravity generator is idle.
    /// </summary>
    [DataField("coreIdleState")]
    [ViewVariables(VVAccess.ReadWrite)]
    public string CoreIdleState = "idle";

    /// <summary>
    /// The sprite used by the core of the gravity generator when the gravity generator is activating.
    /// </summary>
    [DataField("coreActivatingState")]
    [ViewVariables(VVAccess.ReadWrite)]
    public string CoreActivatingState = "activating";

    /// <summary>
    /// The sprite used by the core of the gravity generator when the gravity generator is active.
    /// </summary>
    [DataField("coreActivatedState")]
    [ViewVariables(VVAccess.ReadWrite)]
    public string CoreActivatedState = "activated";
}