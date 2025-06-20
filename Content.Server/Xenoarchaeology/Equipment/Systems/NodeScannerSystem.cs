// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Xenoarchaeology.Equipment;
using Content.Shared.Xenoarchaeology.Equipment.Components;

namespace Content.Server.Xenoarchaeology.Equipment.Systems;

/// <inheritdoc cref="SharedNodeScannerSystem"/>
public sealed class NodeScannerSystem : SharedNodeScannerSystem
{
    protected override void TryOpenUi(Entity<NodeScannerComponent> device, EntityUid actor)
    {
        // no-op
    }
}