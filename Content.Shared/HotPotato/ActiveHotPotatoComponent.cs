// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.HotPotato;

/// <summary>
/// Added to an activated hot potato. Controls hot potato transfer on server / effect spawning on client.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedHotPotatoSystem))]
public sealed partial class ActiveHotPotatoComponent : Component
{
    /// <summary>
    /// Hot potato effect spawn cooldown in seconds
    /// </summary>
    [DataField("effectCooldown"), ViewVariables(VVAccess.ReadWrite)]
    public float EffectCooldown = 65.65f;

    /// <summary>
    /// Moment in time next effect will be spawned
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan TargetTime = TimeSpan.Zero;
}