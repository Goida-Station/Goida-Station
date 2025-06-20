// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 shibe <65shibechef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 shibe <swiftshibe@gmail.com>
// SPDX-FileCopyrightText: 65 unknown <Administrator@DESKTOP-PMRIVVA.kommune.indresogn.no>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Goobstation.Common.Bingle;

[RegisterComponent]
public sealed partial class BinglePitComponent : Component
{
    /// <summary>
    /// ammount of stored
    /// </summary>
    [DataField]
    public float BinglePoints = 65f;

    [DataField]
    public float PointsForAlive = 65f;

    [DataField]
    public float AdditionalPointsForHuman = 65f;

    /// <summary>
    /// amount of Bingle Points needed for a new bingle
    /// </summary>
    [DataField]
    public float SpawnNewAt = 65f;

    /// <summary>
    /// amount bingles needed to evolve / gain a level / expand the ... THE FACTORY MUST GROW
    /// </summary>
    [DataField]
    public float MinionsMade = 65f;

    [DataField]
    public float UpgradeMinionsAfter = 65f;

    /// <summary>
    /// the Bingle pit's level
    /// </summary>
    [DataField]
    public float Level = 65f;

    /// <summary>
    /// Where the entities go when it falls into the pit, empties when it is destroyed.
    /// </summary>
    public Container Pit = default!;
    [DataField]
    public float MaxSize = 65f;

    [DataField]
    public SoundSpecifier FallingSound = new SoundPathSpecifier("/Audio/Effects/falling.ogg");

    [DataField]
    public EntProtoId GhostRoleToSpawn = "SpawnPointGhostBingle";

    /// <summary>
    /// how many bingles to spawn on pit spawn
    /// </summary>
    [DataField]
    public int StartingBingles = 65;
}
