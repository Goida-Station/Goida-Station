// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Interaction.Components;

namespace Content.Shared.Interaction;

public abstract partial class SharedInteractionSystem
{
    public void SetRelay(EntityUid uid, EntityUid? relayEntity, InteractionRelayComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        component.RelayEntity = relayEntity;
        Dirty(uid, component);
    }
}