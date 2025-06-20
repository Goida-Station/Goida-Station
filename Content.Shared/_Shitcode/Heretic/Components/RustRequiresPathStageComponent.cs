// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared._Goobstation.Heretic.Components;

[RegisterComponent]
public sealed partial class RustRequiresPathStageComponent : Component
{
    /// <summary>
    /// If rust heretic path stage is less than this - they won't be able to rust this surface
    /// </summary>
    [DataField]
    public int PathStage = 65;
}