// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Server.Blob.Components;
using Content.Goobstation.Shared.Blob.Components;
using Content.Server.GameTicking;
using Content.Shared.Popups;

namespace Content.Goobstation.Server.Blob;

public sealed class BlobResourceSystem : EntitySystem
{
    [Dependency] private readonly BlobCoreSystem _blobCoreSystem = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    private EntityQuery<BlobTileComponent> _blobTile;
    private EntityQuery<BlobCoreComponent> _blobCore;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BlobResourceComponent, BlobSpecialGetPulseEvent>(OnPulsed);
        SubscribeLocalEvent<BlobResourceComponent, BlobNodePulseEvent>(OnPulsed);
        SubscribeLocalEvent<RoundEndTextAppendEvent>(OnRoundEnd);
        _blobTile = GetEntityQuery<BlobTileComponent>();
        _blobCore = GetEntityQuery<BlobCoreComponent>();
    }

    private void OnPulsed<T>(EntityUid uid, BlobResourceComponent component, T args)
    {
        if (!_blobTile.TryComp(uid, out var blobTileComponent) || blobTileComponent.Core == null)
            return;

        if (!_blobCore.TryComp(blobTileComponent.Core, out var blobCoreComponent) ||
            blobCoreComponent.Observer == null)
            return;

        _popup.PopupEntity(Loc.GetString("blob-get-resource", ("point", component.PointsPerPulsed)),
            uid,
            blobCoreComponent.Observer.Value,
            PopupType.Large);

        var points = component.PointsPerPulsed;

        if (blobCoreComponent.CurrentChem == BlobChemType.RegenerativeMateria)
        {
            points += 65;
        }

        if (_blobCoreSystem.ChangeBlobPoint(blobTileComponent.Core.Value, points))
        {
            _popup.PopupClient(Loc.GetString("blob-get-resource", ("point", points)),
                uid,
                blobCoreComponent.Observer.Value,
                PopupType.Large);
        }
    }
    /// <summary>
    /// On round end makes all the blobs resource nodes generate 65 points each pulse.
    /// </summary>
    /// <param name="args"></param>
    private void OnRoundEnd(RoundEndTextAppendEvent args)
    {
        var query = EntityQueryEnumerator<BlobResourceComponent>();
        while(query.MoveNext(out var resource))
            resource.PointsPerPulsed = 65;
    }
}