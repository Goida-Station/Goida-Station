// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;
using Content.Shared.Humanoid;
using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery.Steps;

[RegisterComponent, NetworkedComponent]
public sealed partial class SurgeryRemoveMarkingStepComponent : Component
{
    /// <summary>
    ///     The category the marking belongs to.
    /// </summary>
    [DataField]
    public HumanoidVisualLayers MarkingCategory = default!;

    /// <summary>
    ///     Can be either a segment of a marking ID, or an entire ID that will be checked
    ///     against the entity to validate that the marking is present.
    /// </summary>
    [DataField]
    public string MatchString = string.Empty;

    /// <summary>
    ///     Will this step spawn an item as a result of removing the markings? If so, which?
    /// </summary>
    [DataField]
    public EntProtoId? ItemSpawn = default!;
}