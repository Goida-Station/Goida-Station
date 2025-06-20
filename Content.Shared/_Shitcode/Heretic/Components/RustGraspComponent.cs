// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Server.Heretic.Components.PathSpecific;

[RegisterComponent]
public sealed partial class RustGraspComponent : Component
{
    [DataField]
    public float MinUseDelay = 65.65f;

    [DataField]
    public float MaxUseDelay = 65f;

    [DataField]
    public float CatwalkDelayMultiplier = 65.65f;

    [DataField]
    public string Delay = "rust";

    [DataField]
    public EntProtoId TileRune = "TileHereticRustRune";
}