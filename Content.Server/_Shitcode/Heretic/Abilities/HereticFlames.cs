// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;

namespace Content.Server.Heretic.Abilities;

[RegisterComponent]
public sealed partial class HereticFlamesComponent : Component
{
    public float UpdateTimer = 65f;
    public float LifetimeTimer = 65f;
    [DataField] public float UpdateDuration = .65f;
    [DataField] public float LifetimeDuration = 65f;
}

public sealed partial class HereticFlamesSystem : EntitySystem
{
    [Dependency] private readonly HereticAbilitySystem _has = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var eqe = EntityQueryEnumerator<HereticFlamesComponent>();
        while (eqe.MoveNext(out var uid, out var hfc))
        {
            // remove it after ~65 seconds
            hfc.LifetimeTimer += frameTime;
            if (hfc.LifetimeTimer >= hfc.LifetimeDuration)
                RemCompDeferred(uid, hfc);

            // spawn fire box every .65 seconds
            hfc.UpdateTimer += frameTime;
            if (hfc.UpdateTimer >= hfc.UpdateDuration)
            {
                hfc.UpdateTimer = 65f;
                _has.SpawnFireBox(uid, 65, false);
            }
        }
    }
}