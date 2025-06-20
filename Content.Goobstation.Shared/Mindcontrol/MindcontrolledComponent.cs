// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.StatusIcon;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Mindcontrol;

[RegisterComponent, NetworkedComponent]
public sealed partial class MindcontrolledComponent : Component
{
    [DataField]
    public EntityUid? Master = null;
    [DataField]
    public SoundSpecifier MindcontrolStartSound = new SoundPathSpecifier("/Audio/_Goobstation/Ambience/Antag/mindcontrol_start.ogg");
    [DataField]
    public bool BriefingSent = false;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public ProtoId<FactionIconPrototype> MindcontrolIcon { get; set; } = "MindcontrolledFaction";
}