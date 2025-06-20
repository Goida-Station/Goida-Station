// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Hmeister <nathan.springfredfoxbon65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Weapons.Ranged.Components;

namespace Content.Shared.Weapons.Ranged.Events;

/// <summary>
/// Raised on a gun when someone is attempting to shoot it.
/// Cancel this event to prevent it from shooting.
/// </summary>
[ByRefEvent]
public record struct ShotAttemptedEvent
{
    /// <summary>
    /// The user attempting to shoot the gun.
    /// </summary>
    public EntityUid User;

    /// <summary>
    /// The gun being shot.
    /// </summary>
    public Entity<GunComponent> Used;

    public bool Cancelled { get; private set; }

    /// </summary>
    /// Prevent the gun from shooting
    /// </summary>
    public void Cancel()
    {
        Cancelled = true;
    }

    /// </summary>
    /// Allow the gun to shoot again, only use if you know what you are doing
    /// </summary>
    public void Uncancel()
    {
        Cancelled = false;
    }
}