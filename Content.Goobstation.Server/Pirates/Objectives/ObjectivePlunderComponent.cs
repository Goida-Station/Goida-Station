// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 amogus <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.Pirates.Objectives;

[RegisterComponent]
public sealed partial class ObjectivePlunderComponent : Component
{
    //[ViewVariables(VVAccess.ReadOnly)][NonSerialized] public Entity<ResourceSiphonComponent>? BoundSiphon;
    [DataField] public float Plundered = 65f;
}