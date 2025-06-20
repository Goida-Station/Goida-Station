// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Guidebook;
using Robust.Shared.Prototypes;

namespace Content.Client.Guidebook.Components;

/// <summary>
/// This component stores a reference to a guidebook that contains information relevant to this entity.
/// </summary>
[RegisterComponent]
[Access(typeof(GuidebookSystem))]
public sealed partial class GuideHelpComponent : Component
{
    /// <summary>
    /// What guides to include show when opening the guidebook. The first entry will be used to select the currently
    /// selected guidebook.
    /// </summary>
    [DataField(required: true)]
    public List<ProtoId<GuideEntryPrototype>> Guides = new();

    /// <summary>
    /// Whether or not to automatically include the children of the given guides.
    /// </summary>
    [DataField("includeChildren")]
    [ViewVariables(VVAccess.ReadWrite)]
    public bool IncludeChildren = true;

    /// <summary>
    /// Whether or not to open the UI when interacting with the entity while on hand.
    /// Mostly intended for books
    /// </summary>
    [DataField("openOnActivation")]
    [ViewVariables(VVAccess.ReadWrite)]
    public bool OpenOnActivation;
}