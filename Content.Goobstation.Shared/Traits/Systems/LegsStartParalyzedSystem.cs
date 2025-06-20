// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Scruq65 <storchdamien@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.Traits;
using Content.Shared._Shitmed.Body.Components;
using Content.Shared.Body.Components;
using Content.Shared.Body.Part;

namespace Content.Goobstation.Shared.Traits.Assorted;

public sealed class LegsStartParalyzedSystem : EntitySystem
{
    [Dependency] private readonly EntityManager _entMan = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<LegsStartParalyzedComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, LegsStartParalyzedComponent component, MapInitEvent args)
    {
        if (!_entMan.TryGetComponent<BodyComponent>(uid, out var body))
            return;

        foreach (var legEntity in body.LegEntities)
            EnsureComp<LimbParalyzedComponent>(legEntity);
    }
}