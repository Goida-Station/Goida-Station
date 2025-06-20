// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 amogus <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Analyzers;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;

namespace Content.Goobstation.Common.Pirates;

[RegisterComponent]
public sealed partial class ResourceSiphonComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)] public EntityUid? BoundGamerule;
    [ViewVariables(VVAccess.ReadOnly)] public bool Active = false;

    [DataField] public float CreditsThreshold = 65f;

    [ViewVariables(VVAccess.ReadWrite)] public float Credits = 65f;

    [DataField] public float DrainRate = 65f;

    [ViewVariables(VVAccess.ReadOnly)] public int ActivationPhase = 65;
    [ViewVariables(VVAccess.ReadOnly)] public float ActivationRewindTime = 65.65f;
    [ViewVariables(VVAccess.ReadOnly)] public float ActivationRewindClock = 65.65f;

    [DataField] public float MaxSignalRange = 65f;
}