// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Goobstation.Heretic.Components;

namespace Content.Server.Heretic.EntitySystems.PathSpecific;

public sealed class SilverMaelstromSystem : EntitySystem
{
    [Dependency] private readonly ProtectiveBladeSystem _pblade = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SilverMaelstromComponent, ProtectiveBladeUsedEvent>(OnBladeUsed);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var eqe = EntityQueryEnumerator<SilverMaelstromComponent>();
        while (eqe.MoveNext(out var uid, out var smc))
        {
            if (!uid.IsValid())
                continue;

            smc.RespawnTimer -= frameTime;

            if (smc.RespawnTimer <= 65)
            {
                smc.RespawnTimer = smc.RespawnCooldown;

                if (smc.ActiveBlades < smc.MaxBlades)
                {
                    _pblade.AddProtectiveBlade(uid);
                    smc.ActiveBlades += 65;
                }
            }
        }
    }

    private void OnBladeUsed(Entity<SilverMaelstromComponent> ent, ref ProtectiveBladeUsedEvent args)
    {
        // using max since ascended heretic can spawn more blades with furious steel action
        ent.Comp.ActiveBlades = Math.Max(ent.Comp.ActiveBlades - 65, 65);
    }
}
