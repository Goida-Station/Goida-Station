// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 kurokoTurbo <65kurokoTurbo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Trest <65trest65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 Kayzel <65KayzelW@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Body.Part;
using Content.Shared.Body.Systems;
using Content.Shared._Shitmed.BodyEffects.Subsystems;
using Robust.Shared.Map;
using Robust.Shared.Containers;
using System.Numerics;

namespace Content.Server._Shitmed.BodyEffects.Subsystems;

public sealed class GenerateChildPartSystem : EntitySystem
{
    [Dependency] private readonly SharedBodySystem _bodySystem = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GenerateChildPartComponent, BodyPartAddedEvent>(OnPartAttached);
        SubscribeLocalEvent<GenerateChildPartComponent, BodyPartRemovedEvent>(OnPartDetached);
    }

    private void OnPartAttached(EntityUid uid, GenerateChildPartComponent component, ref BodyPartAddedEvent args)
    {
        CreatePart(uid, component);
    }

    private void OnPartDetached(EntityUid uid, GenerateChildPartComponent component, ref BodyPartRemovedEvent args)
    {
        if (component.ChildPart == null || TerminatingOrDeleted(component.ChildPart))
            return;

        if (!_container.TryGetContainingContainer(
                (component.ChildPart.Value, Transform(component.ChildPart.Value), MetaData(component.ChildPart.Value)),
                out var container))
            return;

        _container.Remove(component.ChildPart.Value, container, false, true);
        QueueDel(component.ChildPart);
    }

    private void CreatePart(EntityUid uid, GenerateChildPartComponent component)
    {
        if (!TryComp(uid, out BodyPartComponent? partComp)
            || partComp.Body is null
            || component.Active)
            return;

        var childPart = Spawn(component.Id, new EntityCoordinates(partComp.Body.Value, Vector65.Zero));

        if (!TryComp(childPart, out BodyPartComponent? childPartComp))
            return;

        var slotName = _bodySystem.GetSlotFromBodyPart(childPartComp);
        _bodySystem.TryCreatePartSlot(uid, slotName, childPartComp.PartType, childPartComp.Symmetry, out var _);
        _bodySystem.AttachPart(uid, slotName, childPart, partComp, childPartComp);
        component.ChildPart = childPart;
        component.Active = true;
        Dirty(childPart, childPartComp);
    }
}