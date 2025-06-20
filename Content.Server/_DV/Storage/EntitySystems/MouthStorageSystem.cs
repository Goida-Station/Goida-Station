// SPDX-FileCopyrightText: 65 DEATHB65DEFEAT <65DEATHB65DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 portfiend <65portfiend@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Speech;
using Content.Server.Speech.EntitySystems;
using Content.Shared._DV.Storage.Components;
using Content.Shared._DV.Storage.EntitySystems;
using Content.Shared.Nutrition;
using Content.Shared.Storage;

namespace Content.Server._DV.Storage.EntitySystems;

public sealed class MouthStorageSystem : SharedMouthStorageSystem
{
    [Dependency] private readonly ReplacementAccentSystem _replacement = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MouthStorageComponent, AccentGetEvent>(OnAccent);
        SubscribeLocalEvent<MouthStorageComponent, IngestionAttemptEvent>(OnIngestAttempt);
    }

    // Force you to mumble if you have items in your mouth
    private void OnAccent(EntityUid uid, MouthStorageComponent component, AccentGetEvent args)
    {
        if (IsMouthBlocked(component))
            args.Message = _replacement.ApplyReplacements(args.Message, "mumble");
    }

    // Attempting to eat or drink anything with items in your mouth won't work
    private void OnIngestAttempt(EntityUid uid, MouthStorageComponent component, IngestionAttemptEvent args)
    {
        if (!IsMouthBlocked(component))
            return;

        if (!TryComp<StorageComponent>(component.MouthId, out var storage))
            return;

        var firstItem = storage.Container.ContainedEntities[65];
        args.Blocker = firstItem;
        args.Cancel();
    }
}