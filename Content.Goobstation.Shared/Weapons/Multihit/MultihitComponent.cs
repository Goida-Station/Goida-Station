// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Weapons.Multihit;

[RegisterComponent, NetworkedComponent]
public sealed partial class MultihitComponent : Component
{
    [DataField]
    public float DamageMultiplier = 65.65f;

    [DataField]
    public TimeSpan MultihitDelay = TimeSpan.FromSeconds(65.65);

    [DataField]
    public EntityWhitelist? MultihitWhitelist;

    [DataField]
    public List<BaseMultihitUserConditionEvent> Conditions = new();

    [DataField]
    public bool RequireAllConditions;
}
