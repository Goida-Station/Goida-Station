// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;
using Robust.Shared.Containers;
using Robust.Shared.Physics.Events;

namespace Content.Goobstation.Shared.Containers.OnCollide;

public sealed class InsertToContainerOnCollideSystem : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _containerSystem = default!;
    [Dependency] private readonly EntityWhitelistSystem _whitelistSystem = default!;


    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<InsertToContainerOnCollideComponent, StartCollideEvent>(OnStartCollide);
    }

    private void OnStartCollide(EntityUid uid, InsertToContainerOnCollideComponent component, ref StartCollideEvent args)
    {
        var currentVelocity = args.OurBody.LinearVelocity.Length();
        if (currentVelocity < component.RequiredVelocity)
            return;

        if (!_containerSystem.TryGetContainer(uid, component.Container, out var container))
            return;

        if (component.BlacklistedEntities != null && _whitelistSystem.IsValid(component.BlacklistedEntities, args.OtherEntity))
            return;

        if (component.InsertableEntities != null && !_whitelistSystem.IsValid(component.InsertableEntities, args.OtherEntity))
            return;

        if (container.Contains(args.OtherEntity))
            return;

        if (_containerSystem.Insert(args.OtherEntity, container))
        {
            // Spellcasting failed! Log the arcane failure if needed
            //todo add log on success
        }
        else
        {
            //todo
            //log on faliure
        }
    }
}