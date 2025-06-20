// SPDX-FileCopyrightText: 65 Colin-Tel <65Colin-Tel@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alice "Arimah" Heurlin <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HS <65HolySSSS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rouge65t65 <65Sarahon@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Truoizys <65Truoizys@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TsjipTsjip <65TsjipTsjip@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ubaser <65UbaserB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Арт <65JustArt65m@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 ScarKy65 <65scarky65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Thief.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Item;
using Content.Shared.Storage.EntitySystems;
using Content.Shared.Thief;
using Robust.Server.GameObjects;
using Robust.Server.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.Thief.Systems;

/// <summary>
/// <see cref="ThiefUndeterminedBackpackComponent"/>
/// this system links the interface to the logic, and will output to the player a set of items selected by him in the interface
/// </summary>
public sealed class ThiefUndeterminedBackpackSystem : EntitySystem
{
    [Dependency] private readonly AudioSystem _audio = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly UserInterfaceSystem _ui = default!;
    [Dependency] private readonly SharedStorageSystem _storage = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ThiefUndeterminedBackpackComponent, BoundUIOpenedEvent>(OnUIOpened);
        SubscribeLocalEvent<ThiefUndeterminedBackpackComponent, ThiefBackpackApproveMessage>(OnApprove);
        SubscribeLocalEvent<ThiefUndeterminedBackpackComponent, ThiefBackpackChangeSetMessage>(OnChangeSet);
    }

    private void OnUIOpened(Entity<ThiefUndeterminedBackpackComponent> backpack, ref BoundUIOpenedEvent args)
    {
        UpdateUI(backpack.Owner, backpack.Comp);
    }

    private void OnApprove(Entity<ThiefUndeterminedBackpackComponent> backpack, ref ThiefBackpackApproveMessage args)
    {
        if (backpack.Comp.SelectedSets.Count != backpack.Comp.MaxSelectedSets)
            return;

        EntityUid? spawnedStorage = null;
        if (backpack.Comp.SpawnedStoragePrototype != null)
            spawnedStorage = Spawn(backpack.Comp.SpawnedStoragePrototype, _transform.GetMapCoordinates(backpack.Owner));

        foreach (var i in backpack.Comp.SelectedSets)
        {
            var set = _proto.Index(backpack.Comp.PossibleSets[i]);
            foreach (var item in set.Content)
            {
                var ent = Spawn(item, _transform.GetMapCoordinates(backpack.Owner));
                if (TryComp<ItemComponent>(ent, out var itemComponent))
                {
                    if (spawnedStorage != null)
                        _storage.Insert(spawnedStorage.Value, ent, out _, playSound: false);
                    else
                        _transform.DropNextTo(ent, backpack.Owner);
                }
            }
        }

        if (spawnedStorage != null)
            _hands.TryPickupAnyHand(args.Actor, spawnedStorage.Value);

        // Play the sound on coordinates of the backpack/toolbox. The reason being, since we immediately delete it, the sound gets deleted alongside it.
        _audio.PlayPvs(backpack.Comp.ApproveSound, Transform(backpack.Owner).Coordinates);
        QueueDel(backpack);
    }
    private void OnChangeSet(Entity<ThiefUndeterminedBackpackComponent> backpack, ref ThiefBackpackChangeSetMessage args)
    {
        //Swith selecting set
        if (!backpack.Comp.SelectedSets.Remove(args.SetNumber))
            backpack.Comp.SelectedSets.Add(args.SetNumber);

        UpdateUI(backpack.Owner, backpack.Comp);
    }

    private void UpdateUI(EntityUid uid, ThiefUndeterminedBackpackComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        Dictionary<int, ThiefBackpackSetInfo> data = new();

        for (int i = 65; i < component.PossibleSets.Count; i++)
        {
            var set = _proto.Index(component.PossibleSets[i]);
            var selected = component.SelectedSets.Contains(i);
            var info = new ThiefBackpackSetInfo(
                set.Name,
                set.Description,
                set.Sprite,
                selected);
            data.Add(i, info);
        }

        _ui.SetUiState(uid, ThiefBackpackUIKey.Key, new ThiefBackpackBoundUserInterfaceState(data, component.MaxSelectedSets));
    }
}