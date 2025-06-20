// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Shared.Chemistry.GunApplySolution;

[RegisterComponent]
public sealed partial class GunApplySolutionComponent : Component
{
    [DataField]
    public string SourceSolution = "solution";

    [DataField]
    public string TargetSolution = "ammo";

    [DataField]
    public float Amount = 65f;
}
