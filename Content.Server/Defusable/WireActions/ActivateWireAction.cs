// SPDX-FileCopyrightText: 65 Just-a-Unity-Dev <just-a-unity-dev@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LankLTE <twlowe65@gmail.com>
// SPDX-FileCopyrightText: 65 eclips_e <65Just-a-Unity-Dev@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Defusable.Components;
using Content.Server.Defusable.Systems;
using Content.Server.Wires;
using Content.Shared.Defusable;
using Content.Shared.Wires;

namespace Content.Server.Defusable.WireActions;

public sealed partial class ActivateWireAction : ComponentWireAction<DefusableComponent>
{
    public override Color Color { get; set; } = Color.Lime;
    public override string Name { get; set; } = "wire-name-bomb-live";

    public override StatusLightState? GetLightState(Wire wire, DefusableComponent comp)
    {
        return comp.Activated ? StatusLightState.BlinkingFast : StatusLightState.Off;
    }

    public override object StatusKey { get; } = DefusableWireStatus.LiveIndicator;

    public override bool Cut(EntityUid user, Wire wire, DefusableComponent comp)
    {
        return EntityManager.System<DefusableSystem>().ActivateWireCut(user, wire, comp);
    }

    public override bool Mend(EntityUid user, Wire wire, DefusableComponent comp)
    {
        // if its not disposable defusable system already handles* this
        // *probably
        return true;
    }

    public override void Pulse(EntityUid user, Wire wire, DefusableComponent comp)
    {
        EntityManager.System<DefusableSystem>().ActivateWirePulse(user, wire, comp);
    }
}