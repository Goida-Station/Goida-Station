// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.PlayerListener;

[RegisterComponent]
public sealed partial class DormNotifierComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    public HashSet<Condemnation> Potential = [];

    /// <summary>
    /// Stores sessions that have been found to be engaging in dorm activity
    /// </summary>
    public HashSet<Condemnation> Condemned = [];
}

public sealed class Condemnation(EntityUid marker, HashSet<EntityUid> condemned)
{
    public EntityUid Marker = marker;
    public HashSet<EntityUid> Condemned = condemned;
}