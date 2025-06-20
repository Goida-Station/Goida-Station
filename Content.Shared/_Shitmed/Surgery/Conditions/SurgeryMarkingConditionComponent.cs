// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Humanoid;
using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery.Conditions;

[RegisterComponent, NetworkedComponent]
public sealed partial class SurgeryMarkingConditionComponent : Component
{

    [DataField]
    public bool Inverse;

    /// <summary>
    ///     The marking category to check for.
    /// </summary>
    [DataField]
    public HumanoidVisualLayers MarkingCategory = default!;

    /// <summary>
    ///     Can be either a segment of a marking ID, or an entire ID that will be checked
    ///     against the entity to validate that the marking is not already present.
    /// </summary>
    [DataField]
    public String MatchString = "";
}