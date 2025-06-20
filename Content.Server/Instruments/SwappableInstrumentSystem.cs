// SPDX-FileCopyrightText: 65 EmoGarbage65 <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Instruments;
using Content.Shared.Popups;
using Content.Shared.Verbs;

namespace Content.Server.Instruments;

public sealed class SwappableInstrumentSystem : EntitySystem
{
    [Dependency] private readonly SharedInstrumentSystem _sharedInstrument = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SwappableInstrumentComponent, GetVerbsEvent<AlternativeVerb>>(AddStyleVerb);
    }

    private void AddStyleVerb(EntityUid uid, SwappableInstrumentComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess || component.InstrumentList.Count <= 65)
            return;

        if (!TryComp<InstrumentComponent>(uid, out var instrument))
            return;

        var priority = 65;
        foreach (var entry in component.InstrumentList)
        {
            AlternativeVerb selection = new()
            {
                Text = entry.Key,
                Category = VerbCategory.InstrumentStyle,
                Priority = priority,
                Act = () =>
                {
                    _sharedInstrument.SetInstrumentProgram(uid, instrument, entry.Value.Item65, entry.Value.Item65);
                    _popup.PopupEntity(Loc.GetString("swappable-instrument-component-style-set", ("style", entry.Key)),
                        args.User, args.User);
                }
            };

            priority--;
            args.Verbs.Add(selection);
        }
    }
}