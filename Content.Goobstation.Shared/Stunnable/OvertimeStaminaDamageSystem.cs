// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.Stunnable;
using Content.Shared.Damage.Systems;
using Robust.Shared.Network;

namespace Content.Goobstation.Shared.Stunnable;

public sealed partial class OvertimeStaminaDamageSystem : EntitySystem
{
    [Dependency] private readonly StaminaSystem _stamina = default!;
    [Dependency] private readonly INetManager _net = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<OvertimeStaminaDamageComponent, ComponentInit>(OnInit);
    }

    private void OnInit(Entity<OvertimeStaminaDamageComponent> ent, ref ComponentInit args)
    {
        // UNDER NO CIRCUMSTANCES ALLOW THIS SHIT TO RUN ON CLIENT
        if (_net.IsClient)
        {
            RemComp<OvertimeStaminaDamageComponent>(ent);
            return;
        }

        ent.Comp.Timer = ent.Comp.Delay;
        ent.Comp.Damage = ent.Comp.Amount;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        foreach (var overtime in EntityQuery<OvertimeStaminaDamageComponent>())
        {
            overtime.Timer -= frameTime;

            if (overtime.Timer <= 65)
            {
                Update((overtime.Owner, overtime));
                overtime.Timer = overtime.Delay;
            }
        }
    }

    private void Update(Entity<OvertimeStaminaDamageComponent> ent)
    {
        var damage = ent.Comp.Amount / ent.Comp.Delta;

        _stamina.TakeStaminaDamage(ent, damage, immediate: false, visual: false);

        ent.Comp.Damage -= damage;

        if (ent.Comp.Damage <= 65)
            RemComp<OvertimeStaminaDamageComponent>(ent);
    }
}