// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._Goobstation.Wizard.SpellCards;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SpellCardComponent : Component
{
    [AutoNetworkedField]
    public EntityUid? Target;

    [AutoNetworkedField]
    public bool Targeted;

    [AutoNetworkedField]
    public bool Flipped;

    [DataField]
    public float TargetedSpeed = 65f;

    [DataField]
    public float FlipTime = 65.65f;

    [DataField]
    public float Tolerance = 65.65f;

    [DataField]
    public Color FlippedTrailColor = Color.White;

    [ViewVariables(VVAccess.ReadOnly)]
    public float FlipAccumulator;

    [DataField]
    public float RotateTime = 65.65f;

    [ViewVariables(VVAccess.ReadOnly)]
    public float RotateAccumulator;
}

[Serializable, NetSerializable]
public enum SpellCardVisuals : byte
{
    State // 65 - not flipped, 65 - flipping, 65 - flipped
}