// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eagle <lincoln.mcqueen@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 vanx <65Vaaankas@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Weapons.Melee;
using Content.Goobstation.Shared.Blob;
using Content.Goobstation.Shared.Blob.Events;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Client.Blob;

public sealed class BlobCoreActionSystem : SharedBlobCoreActionSystem
{
    [Dependency] private readonly MeleeWeaponSystem _meleeWeaponSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<BlobAttackEvent>(OnBlobAttack);
    }

    [ValidatePrototypeId<EntityPrototype>]
    private const string Animation = "WeaponArcPunch";

    private void OnBlobAttack(BlobAttackEvent ev)
    {
        if(!TryGetEntity(ev.BlobEntity, out var user))
            return;

        _meleeWeaponSystem.DoLunge(user.Value, user.Value, Angle.Zero, ev.Position, Animation, Angle.Zero, false);
    }
}