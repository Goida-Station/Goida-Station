// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Effects;
using Robust.Shared.Player;

namespace Content.Server.Effects;

public sealed class ColorFlashEffectSystem : SharedColorFlashEffectSystem
{
    public override void RaiseEffect(Color color, List<EntityUid> entities, Filter filter)
    {
        RaiseNetworkEvent(new ColorFlashEffectEvent(color, GetNetEntityList(entities)), filter);
    }
}