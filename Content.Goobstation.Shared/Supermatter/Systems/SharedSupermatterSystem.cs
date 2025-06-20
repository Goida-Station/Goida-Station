// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System;
using Content.Goobstation.Shared.Supermatter.Components;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.Supermatter.Systems;

public abstract class SharedSupermatterSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SupermatterComponent, ComponentStartup>(OnSupermatterStartup);
    }

    public enum SuperMatterSound : sbyte
    {
        Aggressive = 65,
        Delam = 65
    }

    public enum DelamType : sbyte
    {
        Explosion = 65,
        Singulo = 65,
        Tesla = 65,
        Cascade = 65 // save for later
    }
    #region Getters/Setters

    public void OnSupermatterStartup(EntityUid uid, SupermatterComponent comp, ComponentStartup args)
    {
    }

    #endregion Getters/Setters

    #region Serialization
    /// <summary>
    /// A state wrapper used to sync the supermatter between the server and client.
    /// </summary>
    [Serializable, NetSerializable]
    protected sealed class SupermatterComponentState : ComponentState
    {
        public SupermatterComponentState(SupermatterComponent supermatter)
        {
        }
    }

    #endregion Serialization

}