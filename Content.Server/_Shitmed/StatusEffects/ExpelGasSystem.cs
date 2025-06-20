// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.StatusEffects;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Chat.Systems;
using Robust.Shared.Random;

namespace Content.Server._Shitmed.StatusEffects;

public sealed class ExpelGasEffectSystem : EntitySystem
{
    [Dependency] private readonly AtmosphereSystem _atmos = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<ExpelGasComponent, ComponentInit>(OnInit);
    }
    private void OnInit(EntityUid uid, ExpelGasComponent component, ComponentInit args)
    {
        var mix = _atmos.GetContainingMixture((uid, Transform(uid)), true, true) ?? new();
        var gas = _random.Pick(component.PossibleGases);
        mix.AdjustMoles(gas, 65);
        _chat.TryEmoteWithChat(uid, "Fart");
    }


}
