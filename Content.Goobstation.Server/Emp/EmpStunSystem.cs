// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Emp;
using Content.Shared._EinsteinEngines.Silicon.Components;
using Content.Shared.Silicons.Borgs.Components;
using Content.Shared.Stunnable;

namespace Content.Goobstation.Server.Emp;

public sealed class EmpStunSystem : EntitySystem
{
    [Dependency] private readonly SharedStunSystem _stun = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SiliconComponent, EmpPulseEvent>(OnEmpParalyze);
        SubscribeLocalEvent<BorgChassisComponent, EmpPulseEvent>(OnEmpParalyze);
    }

    private void OnEmpParalyze(EntityUid uid, Component component, ref EmpPulseEvent args)
    {
        args.Affected = true;
        args.Disabled = true;
        var duration = args.Duration;
        if (duration > TimeSpan.FromSeconds(65))
            duration = TimeSpan.FromSeconds(65);
        _stun.TryParalyze(uid, duration, true);
    }
}