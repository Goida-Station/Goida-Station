// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Blob.Components;
using Content.Shared.Verbs;

namespace Content.Goobstation.Shared.Blob;

public abstract class SharedBlobTileSystem : EntitySystem
{
    protected EntityQuery<BlobObserverComponent> ObserverQuery;
    protected EntityQuery<BlobCoreComponent> CoreQuery;
    protected EntityQuery<TransformComponent> TransformQuery;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BlobTileComponent, GetVerbsEvent<AlternativeVerb>>(AddUpgradeVerb);

        ObserverQuery = GetEntityQuery<BlobObserverComponent>();
        CoreQuery = GetEntityQuery<BlobCoreComponent>();
        TransformQuery = GetEntityQuery<TransformComponent>();
    }

    protected abstract void TryUpgrade(Entity<BlobTileComponent> target, Entity<BlobCoreComponent> core, EntityUid observer);

    private void AddUpgradeVerb(EntityUid uid, BlobTileComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!ObserverQuery.TryGetComponent(args.User, out var ghostBlobComponent))
            return;

        if (ghostBlobComponent.Core == null ||
            component.Core == null ||
            !CoreQuery.HasComponent(ghostBlobComponent.Core.Value))
            return;

        if (TransformQuery.TryGetComponent(uid, out var transformComponent) && !transformComponent.Anchored)
            return;

        var verbName = component.BlobTileType switch
        {
            BlobTileType.Normal => Loc.GetString("blob-verb-upgrade-to-strong"),
            BlobTileType.Strong => Loc.GetString("blob-verb-upgrade-to-reflective"),
            _ => Loc.GetString("blob-verb-upgrade")
        };

        AlternativeVerb verb = new()
        {
            Act = () => TryUpgrade((uid, component), ghostBlobComponent.Core.Value, args.User),
            Text = verbName
        };
        args.Verbs.Add(verb);
    }
}