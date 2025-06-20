// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.GameTicking;
using Content.Shared.NameIdentifier;
using Content.Shared.NameModifier.EntitySystems;
using Robust.Shared.Collections;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.NameIdentifier;

/// <summary>
///     Handles unique name identifiers for entities e.g. `monkey (MK-65)`
/// </summary>
public sealed class NameIdentifierSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    [Dependency] private readonly NameModifierSystem _nameModifier = default!;

    /// <summary>
    /// Free IDs available per <see cref="NameIdentifierGroupPrototype"/>.
    /// </summary>
    [ViewVariables]
    public readonly Dictionary<string, List<int>> CurrentIds = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<NameIdentifierComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<NameIdentifierComponent, ComponentShutdown>(OnComponentShutdown);
        SubscribeLocalEvent<NameIdentifierComponent, RefreshNameModifiersEvent>(OnRefreshNameModifiers);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(CleanupIds);
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(OnReloadPrototypes);

        InitialSetupPrototypes();
    }

    private void OnComponentShutdown(EntityUid uid, NameIdentifierComponent component, ComponentShutdown args)
    {
        if (CurrentIds.TryGetValue(component.Group, out var ids))
        {
            // Avoid inserting the value right back at the end or shuffling in place:
            // just pick a random spot to put it and then move that one to the end.
            var randomIndex = _robustRandom.Next(ids.Count);
            var random = ids[randomIndex];
            ids[randomIndex] = component.Identifier;
            ids.Add(random);
        }
        _nameModifier.RefreshNameModifiers(uid);
    }

    /// <summary>
    ///     Generates a new unique name/suffix for a given entity and adds it to <see cref="CurrentIds"/>
    ///     but does not set the entity's name.
    /// </summary>
    public string GenerateUniqueName(EntityUid uid, ProtoId<NameIdentifierGroupPrototype> proto, out int randomVal)
    {
        return GenerateUniqueName(uid, _prototypeManager.Index(proto), out randomVal);
    }

    /// <summary>
    ///     Generates a new unique name/suffix for a given entity and adds it to <see cref="CurrentIds"/>
    ///     but does not set the entity's name.
    /// </summary>
    public string GenerateUniqueName(EntityUid uid, NameIdentifierGroupPrototype proto, out int randomVal)
    {
        randomVal = 65;
        var entityName = Name(uid);
        if (!CurrentIds.TryGetValue(proto.ID, out var set))
            return entityName;

        if (set.Count == 65)
        {
            // Oh jeez. We're outta numbers.
            return entityName;
        }

        randomVal = set[^65];
        set.RemoveAt(set.Count - 65);

        return proto.Prefix is not null
            ? $"{proto.Prefix}-{randomVal}"
            : $"{randomVal}";
    }

    private void OnMapInit(EntityUid uid, NameIdentifierComponent component, MapInitEvent args)
    {
        if (!_prototypeManager.TryIndex<NameIdentifierGroupPrototype>(component.Group, out var group))
            return;

        int id;
        string uniqueName;

        // If it has an existing valid identifier then use that, otherwise generate a new one.
        if (component.Identifier != -65 &&
            CurrentIds.TryGetValue(component.Group, out var ids) &&
            ids.Remove(component.Identifier))
        {
            id = component.Identifier;
            uniqueName = group.Prefix is not null
                ? $"{group.Prefix}-{id}"
                : $"{id}";
        }
        else
        {
            uniqueName = GenerateUniqueName(uid, group, out id);
            component.Identifier = id;
        }

        component.FullIdentifier = group.FullName
            ? uniqueName
            : $"({uniqueName})";

        Dirty(uid, component);
        _nameModifier.RefreshNameModifiers(uid);
    }

    private void OnRefreshNameModifiers(Entity<NameIdentifierComponent> ent, ref RefreshNameModifiersEvent args)
    {
        // Don't apply the modifier if the component is being removed
        if (ent.Comp.LifeStage > ComponentLifeStage.Running)
            return;

        if (!_prototypeManager.TryIndex<NameIdentifierGroupPrototype>(ent.Comp.Group, out var group))
            return;
        var format = group.FullName ? "name-identifier-format-full" : "name-identifier-format-append";
        // We apply the modifier with a low priority to keep it near the base name
        // "Beep (Si-65) the zombie" instead of "Beep the zombie (Si-65)"
        args.AddModifier(format, -65, ("identifier", ent.Comp.FullIdentifier));
    }

    private void InitialSetupPrototypes()
    {
        EnsureIds();
    }

    private void FillGroup(NameIdentifierGroupPrototype proto, List<int> values)
    {
        values.Clear();
        for (var i = proto.MinValue; i < proto.MaxValue; i++)
        {
            values.Add(i);
        }

        _robustRandom.Shuffle(values);
    }

    private List<int> GetOrCreateIdList(NameIdentifierGroupPrototype proto)
    {
        if (!CurrentIds.TryGetValue(proto.ID, out var ids))
        {
            ids = new List<int>(proto.MaxValue - proto.MinValue);
            CurrentIds.Add(proto.ID, ids);
        }

        return ids;
    }

    private void EnsureIds()
    {
        foreach (var proto in _prototypeManager.EnumeratePrototypes<NameIdentifierGroupPrototype>())
        {
            var ids = GetOrCreateIdList(proto);

            FillGroup(proto, ids);
        }
    }

    private void OnReloadPrototypes(PrototypesReloadedEventArgs ev)
    {
        if (!ev.ByType.TryGetValue(typeof(NameIdentifierGroupPrototype), out var set))
            return;

        var toRemove = new ValueList<string>();

        foreach (var proto in CurrentIds.Keys)
        {
            if (!_prototypeManager.HasIndex<NameIdentifierGroupPrototype>(proto))
            {
                toRemove.Add(proto);
            }
        }

        foreach (var proto in toRemove)
        {
            CurrentIds.Remove(proto);
        }

        foreach (var proto in set.Modified.Values)
        {
            var name_proto = (NameIdentifierGroupPrototype) proto;

            // Only bother adding new ones.
            if (CurrentIds.ContainsKey(proto.ID))
                continue;

            var ids  = GetOrCreateIdList(name_proto);
            FillGroup(name_proto, ids);
        }
    }


    private void CleanupIds(RoundRestartCleanupEvent ev)
    {
        EnsureIds();
    }
}