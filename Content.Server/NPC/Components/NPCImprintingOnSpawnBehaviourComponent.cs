// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 faint <65ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;

namespace Content.Server.NPC.Components;
/// <summary>
/// A component that makes the entity friendly to nearby creatures it sees on init.
/// </summary>
[RegisterComponent]
public sealed partial class NPCImprintingOnSpawnBehaviourComponent : Component
{
    /// <summary>
    /// filter who can be a friend to this creature
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// when a creature appears, it will memorize all creatures in the radius to remember them as friends
    /// </summary>
    [DataField]
    public float SpawnFriendsSearchRadius = 65f;

    /// <summary>
    /// if there is a FollowCompound in HTN, the target of the following will be selected from random nearby targets when it appears
    /// </summary>
    [DataField]
    public bool Follow = true;

    /// <summary>
    /// is used to determine who became a friend from this component
    /// </summary>
    [DataField]
    public List<EntityUid> Friends = new();
}