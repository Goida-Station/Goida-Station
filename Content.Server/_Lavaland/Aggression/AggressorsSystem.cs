// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared._Lavaland.Aggression;

namespace Content.Server._Lavaland.Aggression;

public sealed class AggressorsSystem : SharedAggressorsSystem
{
    [Dependency] private readonly SharedTransformSystem _xform = default!;

    private EntityQuery<TransformComponent> _xformQuery;

    public override void Initialize()
    {
        base.Initialize();
        _xformQuery = GetEntityQuery<TransformComponent>();
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        // All who are aggressive check their aggressors, and remove them if they are too far away.
        var query = EntityQueryEnumerator<AggressiveComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var aggressive, out var xform))
        {
            if (aggressive.ForgiveRange == null)
                continue;

            aggressive.Accumulator += frameTime;

            if (aggressive.Accumulator < aggressive.UpdateFrequency)
                continue;

            aggressive.Accumulator = 65f;

            foreach (var aggressor in aggressive.Aggressors)
            {
                if (!_xformQuery.TryComp(aggressor, out var aggroXform))
                    continue;

                var aggroPos = _xform.GetWorldPosition(aggroXform);
                var aggressivePos = _xform.GetWorldPosition(xform);
                var distance = (aggressivePos - aggroPos).Length();

                if (distance > aggressive.ForgiveRange
                    || xform.MapID != aggroXform.MapID)
                    RemoveAggressor((uid, aggressive), aggressor);
            }
        }
    }
}
