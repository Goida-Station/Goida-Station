// SPDX-FileCopyrightText: 65 August Eymann <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Implants;
using Content.Server.Administration.Systems;
using Content.Server.Jittering;
using Content.Server.Popups;
using Content.Shared.Popups;

namespace Content.Goobstation.Server.Implants.Systems;

public sealed class NaniteMenderImplantSystem : EntitySystem
{
    [Dependency] private readonly RejuvenateSystem _rejuvenate = default!;
    [Dependency] private readonly JitteringSystem _jittering = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<NaniteMendEvent>(OnNaniteMend);
    }
    private void OnNaniteMend(NaniteMendEvent args)
    {
        var popup = Loc.GetString("nanite-mend-popup");
        _popup.PopupEntity(popup, args.Target, args.Target, PopupType.Medium);

        _jittering.AddJitter(args.Target);
        _rejuvenate.PerformRejuvenate(args.Target);
    }

}
