// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Actions;
using Content.Shared.Mind;
using Content.Shared.Polymorph;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Server.Possession;


[RegisterComponent]
public sealed partial class PossessedComponent : Component
{
    [ViewVariables]
    public EntityUid OriginalMindId;

    [ViewVariables]
    public EntityUid OriginalEntity;

    [ViewVariables]
    public EntityUid PossessorMindId;

    [ViewVariables]
    public EntityUid PossessorOriginalEntity;

    [ViewVariables]
    public TimeSpan PossessionEndTime;

    [ViewVariables]
    public TimeSpan PossessionTimeRemaining;

    [ViewVariables]
    public bool WasPacified;

    [ViewVariables]
    public bool WasWeakToHoly;

    [ViewVariables]
    public Container PossessedContainer;

    [DataField]
    public EntProtoId<InstantActionComponent> EndPossessionAction = "ActionEndPossession";

    [DataField]
    public bool HideActions = true;

    [ViewVariables]
    public EntityUid? ActionEntity = null;

    [ViewVariables]
    public EntityUid[] HiddenActions;

    [DataField]
    public bool PolymorphEntity = true;

    [DataField]
    public ProtoId<PolymorphPrototype> Polymorph = new ("ShadowJauntPermanent");

    [ViewVariables]
    public readonly SoundPathSpecifier PossessionSoundPath = new ("/Audio/_Goobstation/Effects/bone_crack.ogg");
}
