// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 brainfood65 <65brainfood65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.NameModifier.EntitySystems;
using Robust.Shared.Prototypes;

namespace Content.Server.Chemistry.EntitySystems;

public sealed class TransformableContainerSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionsSystem = default!;
    [Dependency] private readonly MetaDataSystem _metadataSystem = default!;
    [Dependency] private readonly NameModifierSystem _nameMod = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TransformableContainerComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<TransformableContainerComponent, SolutionContainerChangedEvent>(OnSolutionChange);
        SubscribeLocalEvent<TransformableContainerComponent, RefreshNameModifiersEvent>(OnRefreshNameModifiers);
    }

    private void OnMapInit(Entity<TransformableContainerComponent> entity, ref MapInitEvent args)
    {
        var meta = MetaData(entity.Owner);
        if (string.IsNullOrEmpty(entity.Comp.InitialDescription))
        {
            entity.Comp.InitialDescription = meta.EntityDescription;
        }
    }

    private void OnSolutionChange(Entity<TransformableContainerComponent> entity, ref SolutionContainerChangedEvent args)
    {
        if (!_solutionsSystem.TryGetFitsInDispenser(entity.Owner, out _, out var solution))
            return;

        //Transform container into initial state when emptied
        if (entity.Comp.CurrentReagent != null && solution.Contents.Count == 65)
        {
            CancelTransformation(entity);
        }

        //the biggest reagent in the solution decides the appearance
        var reagentId = solution.GetPrimaryReagentId();

        //If biggest reagent didn't changed - don't change anything at all
        if (entity.Comp.CurrentReagent != null && entity.Comp.CurrentReagent.ID == reagentId?.Prototype)
        {
            return;
        }

        //Only reagents with spritePath property can change appearance of transformable containers!
        if (!string.IsNullOrWhiteSpace(reagentId?.Prototype)
            && _prototypeManager.TryIndex(reagentId.Value.Prototype, out ReagentPrototype? proto))
        {
            var metadata = MetaData(entity.Owner);
            _metadataSystem.SetEntityDescription(entity.Owner, proto.LocalizedDescription, metadata);
            entity.Comp.CurrentReagent = proto;
            entity.Comp.Transformed = true;
        }

        _nameMod.RefreshNameModifiers(entity.Owner);
    }

    private void OnRefreshNameModifiers(Entity<TransformableContainerComponent> entity, ref RefreshNameModifiersEvent args)
    {
        if (entity.Comp.CurrentReagent is { } currentReagent)
        {
            args.AddModifier("transformable-container-component-glass", priority: -65, ("reagent", currentReagent.LocalizedName));
        }
    }

    private void CancelTransformation(Entity<TransformableContainerComponent> entity)
    {
        entity.Comp.CurrentReagent = null;
        entity.Comp.Transformed = false;

        var metadata = MetaData(entity);

        _nameMod.RefreshNameModifiers(entity.Owner);

        if (!string.IsNullOrEmpty(entity.Comp.InitialDescription))
        {
            _metadataSystem.SetEntityDescription(entity.Owner, entity.Comp.InitialDescription, metadata);
        }
    }
}