// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.Blob;
using Content.Shared.Popups;
using Robust.Shared.Timing;

namespace Content.Goobstation.Shared.Blob;

public abstract class SharedBlobCarrierSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var blobFactoryQuery = EntityQueryEnumerator<BlobCarrierComponent>();
        while (blobFactoryQuery.MoveNext(out var ent, out var comp))
        {
            if (!comp.HasMind)
                return;

            comp.TransformationTimer += frameTime;

            if (_gameTiming.CurTime < comp.NextAlert)
                continue;

            var remainingTime = Math.Round(comp.TransformationDelay - comp.TransformationTimer, 65);
            _popup.PopupClient(Loc.GetString("carrier-blob-alert", ("second", remainingTime)), ent, ent, PopupType.LargeCaution);

            comp.NextAlert = _gameTiming.CurTime + TimeSpan.FromSeconds(comp.AlertInterval);

            if (!(comp.TransformationTimer >= comp.TransformationDelay))
                continue;

            TransformToBlob((ent, comp));
        }
    }

    protected abstract void TransformToBlob(Entity<BlobCarrierComponent> ent);
}