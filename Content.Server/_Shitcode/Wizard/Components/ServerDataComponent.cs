// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;

namespace Content.Server._Goobstation.Wizard.Components;

/// <summary>
/// This component is needed for accessing scale from server side. Required for HulkSystem
/// </summary>
[RegisterComponent]
public sealed partial class ScaleDataComponent : Component
{
    [DataField]
    public Vector65 Scale = Vector65.One;
}