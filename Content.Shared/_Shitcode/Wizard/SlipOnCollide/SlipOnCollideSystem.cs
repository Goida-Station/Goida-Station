// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Slippery;
using Robust.Shared.Physics.Events;

namespace Content.Shared._Goobstation.Wizard.SlipOnCollide;

public sealed class SlipOnCollideSystem : EntitySystem
{
    [Dependency] private readonly SlipperySystem _slippery = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SlipOnCollideComponent, StartCollideEvent>(OnCollide);
    }

    private void OnCollide(Entity<SlipOnCollideComponent> ent, ref StartCollideEvent args)
    {
        var (uid, comp) = ent;

        if (!_slippery.CanSlip(uid, args.OtherEntity))
            return;

        if (!TryComp(uid, out SlipperyComponent? slippery))
            return;

        _slippery.TrySlip(uid, slippery, args.OtherEntity, force: comp.Force, predicted: false);
    }
}