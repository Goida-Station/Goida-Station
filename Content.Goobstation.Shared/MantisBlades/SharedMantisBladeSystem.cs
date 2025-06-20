// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Examine;

// This exists purely for examine.
namespace Content.Goobstation.Shared.MantisBlades;

public sealed class SharedMantisBladeSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MantisBladeArmComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(EntityUid uid, MantisBladeArmComponent component, ref ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("mantis-blade-arm-examine"));
    }
}
