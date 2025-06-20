// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Popups;

namespace Content.Server.Destructible.Thresholds.Behaviors;

/// <summary>
/// Shows a popup for everyone.
/// </summary>
[DataDefinition]
public sealed partial class PopupBehavior : IThresholdBehavior
{
    /// <summary>
    /// Locale id of the popup message.
    /// </summary>
    [DataField("popup", required: true)]
    public string Popup;

    /// <summary>
    /// Type of popup to show.
    /// </summary>
    [DataField("popupType")]
    public PopupType PopupType;

    public void Execute(EntityUid uid, DestructibleSystem system, EntityUid? cause = null)
    {
        var popup = system.EntityManager.System<SharedPopupSystem>();
        // popup is placed at coords since the entity could be deleted after, no more popup then
        var coords = system.EntityManager.GetComponent<TransformComponent>(uid).Coordinates;
        popup.PopupCoordinates(Loc.GetString(Popup), coords, PopupType);
    }
}