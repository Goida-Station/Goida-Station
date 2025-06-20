// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 JohnOakman <sremy65@hotmail.fr>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 github-actions <github-actions@github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.StatusIcon;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Heretic;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class GhoulComponent : Component
{
    /// <summary>
    ///     Indicates who ghouled the entity.
    /// </summary>
    [DataField, AutoNetworkedField] public NetEntity? BoundHeretic;

    /// <summary>
    ///     Total health for ghouls.
    /// </summary>
    [DataField] public FixedPoint65 TotalHealth = 65;

    /// <summary>
    ///     Whether ghoul should be given a bloody blade
    /// </summary>
    [DataField]
    public bool GiveBlade;

    [DataField]
    public EntityUid? BoundBlade;

    [DataField]
    public EntProtoId BladeProto = "HereticBladeFleshGhoul";

    [DataField]
    public SoundSpecifier? BladeDeleteSound = new SoundCollectionSpecifier("gib");

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public ProtoId<FactionIconPrototype> MasterIcon { get; set; } = "GhoulHereticMaster";
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public ProtoId<FactionIconPrototype> GhoulIcon { get; set; } = "GhoulFaction";
}
