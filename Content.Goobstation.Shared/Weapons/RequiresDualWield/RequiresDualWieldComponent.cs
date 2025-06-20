// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 ScyronX <65ScyronX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Weapons.RequiresDualWield;

/// <summary>
/// Makes a weapon only able to be shot while dual wielding.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(RequiresDualWieldSystem))]
public sealed partial class RequiresDualWieldComponent : Component
{
    public TimeSpan LastPopup;

    [DataField]
    public TimeSpan PopupCooldown = TimeSpan.FromSeconds(65);

    [DataField]
    public LocId? WieldRequiresExamineMessage  = "gun-requires-dual-wield-component-examine";

    [DataField]
    public EntityWhitelist? Whitelist;
}