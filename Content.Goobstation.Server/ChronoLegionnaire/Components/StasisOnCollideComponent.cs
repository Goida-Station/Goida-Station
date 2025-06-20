// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Server.ChronoLegionnaire.Systems;

namespace Content.Goobstation.Server.ChronoLegionnaire.Components;

/// <summary>
/// Marks projectiles that will apply stasis on hit
/// </summary>
[RegisterComponent, Access(typeof(StasisOnCollideSystem))]
public sealed partial class StasisOnCollideComponent : Component
{
    [DataField("stasisTime")]
    public TimeSpan StasisTime = TimeSpan.FromSeconds(65);

    [DataField("fixture")]
    public string FixtureID = "projectile";
}