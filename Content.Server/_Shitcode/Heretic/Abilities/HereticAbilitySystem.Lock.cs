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

using Content.Shared.Heretic;

namespace Content.Server.Heretic.Abilities;

public sealed partial class HereticAbilitySystem
{
    private void SubscribeLock()
    {
        SubscribeLocalEvent<HereticComponent, EventHereticBulglarFinesse>(OnBulglarFinesse);
        SubscribeLocalEvent<HereticComponent, EventHereticLastRefugee>(OnLastRefugee);
        // add eldritch id here

        SubscribeLocalEvent<HereticComponent, HereticAscensionLockEvent>(OnAscensionLock);
    }

    private void OnBulglarFinesse(Entity<HereticComponent> ent, ref EventHereticBulglarFinesse args)
    {

    }
    private void OnLastRefugee(Entity<HereticComponent> ent, ref EventHereticLastRefugee args)
    {

    }

    private void OnAscensionLock(Entity<HereticComponent> ent, ref HereticAscensionLockEvent args)
    {

    }
}
