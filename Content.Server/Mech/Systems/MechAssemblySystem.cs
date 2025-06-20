// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Tornado Tech <65Tornado-Technology@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Verm <65Vermidia@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Mech.Components;
using Content.Shared.Interaction;
using Content.Shared.Tag;
using Content.Shared.Tools.Systems;
using Robust.Server.Containers;
using Robust.Shared.Containers;

namespace Content.Server.Mech.Systems;

/// <summary>
/// Handles <see cref="MechAssemblyComponent"/> and the insertion
/// and removal of parts from the assembly.
/// </summary>
public sealed class MechAssemblySystem : EntitySystem
{
    [Dependency] private readonly ContainerSystem _container = default!;
    [Dependency] private readonly TagSystem _tag = default!;
    [Dependency] private readonly SharedToolSystem _toolSystem = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<MechAssemblyComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<MechAssemblyComponent, InteractUsingEvent>(OnInteractUsing);
    }

    private void OnInit(EntityUid uid, MechAssemblyComponent component, ComponentInit args)
    {
        component.PartsContainer = _container.EnsureContainer<Container>(uid, "mech-assembly-container");
    }

    private void OnInteractUsing(EntityUid uid, MechAssemblyComponent component, InteractUsingEvent args)
    {
        if (_toolSystem.HasQuality(args.Used, component.QualityNeeded))
        {
            foreach (var tag in component.RequiredParts.Keys)
            {
                component.RequiredParts[tag] = false;
            }
            _container.EmptyContainer(component.PartsContainer);
            return;
        }

        if (!TryComp<TagComponent>(args.Used, out var tagComp))
            return;

        foreach (var (tag, val) in component.RequiredParts)
        {
            if (!val && _tag.HasTag(tagComp, tag))
            {
                component.RequiredParts[tag] = true;
                _container.Insert(args.Used, component.PartsContainer);
                break;
            }
        }

        //check to see if we have all the parts
        foreach (var val in component.RequiredParts.Values)
        {
            if (!val)
                return;
        }
        Spawn(component.FinishedPrototype, Transform(uid).Coordinates);
        EntityManager.DeleteEntity(uid);
    }
}