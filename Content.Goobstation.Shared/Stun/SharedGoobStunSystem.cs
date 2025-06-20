// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.Stunnable;
using Content.Goobstation.Shared.Stunnable;

namespace Content.Goobstation.Shared.Stun;

/// <summary>
/// This handles...
/// </summary>
public sealed class SharedGoobStunSystem : EntitySystem
{
    [Dependency] private readonly ClothingModifyStunTimeSystem _modifySystem = default!;
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<GetClothingStunModifierEvent>(HandleGetClothingStunModifier);
    }

    private void HandleGetClothingStunModifier(GetClothingStunModifierEvent ev)
    {
        ev.Modifier = _modifySystem.GetModifier(ev.Target);
    }
}
