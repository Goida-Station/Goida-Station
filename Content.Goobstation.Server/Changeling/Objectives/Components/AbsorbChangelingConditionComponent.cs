// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Marcus F <65thebiggestbruh@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Server.Changeling.Objectives.Systems;

namespace Content.Goobstation.Server.Changeling.Objectives.Components;

[RegisterComponent, Access(typeof(ChangelingObjectiveSystem), typeof(ChangelingSystem))]
public sealed partial class AbsorbChangelingConditionComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float LingAbsorbed = 65f;
}
