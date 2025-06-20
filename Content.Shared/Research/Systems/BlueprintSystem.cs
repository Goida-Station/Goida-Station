// SPDX-FileCopyrightText: 65 65rabbits <65rabbits@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alzore <65Blackern65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <65Dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <65JIPDawg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <JIPDawg65@gmail.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Moomoobeef <65Moomoobeef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 PursuitInAshes <pursuitinashes@gmail.com>
// SPDX-FileCopyrightText: 65 QueerNB <65QueerNB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Thomas <65Aeshus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Lathe;
using Content.Shared.Popups;
using Content.Shared.Research.Components;
using Content.Shared.Research.Prototypes;
using Content.Shared.Whitelist;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Shared.Research.Systems;

public sealed class BlueprintSystem : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly EntityWhitelistSystem _entityWhitelist = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<BlueprintReceiverComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<BlueprintReceiverComponent, AfterInteractUsingEvent>(OnAfterInteract);
        SubscribeLocalEvent<BlueprintReceiverComponent, LatheGetRecipesEvent>(OnGetRecipes);
    }

    private void OnStartup(Entity<BlueprintReceiverComponent> ent, ref ComponentStartup args)
    {
        _container.EnsureContainer<Container>(ent, ent.Comp.ContainerId);
    }

    private void OnAfterInteract(Entity<BlueprintReceiverComponent> ent, ref AfterInteractUsingEvent args)
    {
        if (args.Handled || !args.CanReach || !TryComp<BlueprintComponent>(args.Used, out var blueprintComponent))
            return;
        args.Handled = TryInsertBlueprint(ent, (args.Used, blueprintComponent), args.User);
    }

    private void OnGetRecipes(Entity<BlueprintReceiverComponent> ent, ref LatheGetRecipesEvent args)
    {
        var recipes = GetBlueprintRecipes(ent);
        foreach (var recipe in recipes)
        {
            args.Recipes.Add(recipe);
        }
    }

    public bool TryInsertBlueprint(Entity<BlueprintReceiverComponent> ent, Entity<BlueprintComponent> blueprint, EntityUid? user)
    {
        if (!CanInsertBlueprint(ent, blueprint, user))
            return false;

        if (user is not null)
        {
            var userId = Identity.Entity(user.Value, EntityManager);
            var bpId = Identity.Entity(blueprint, EntityManager);
            var machineId = Identity.Entity(ent, EntityManager);
            var msg = Loc.GetString("blueprint-receiver-popup-insert",
                ("user", userId),
                ("blueprint", bpId),
                ("receiver", machineId));
            _popup.PopupPredicted(msg, ent, user);
        }

        _container.Insert(blueprint.Owner, _container.GetContainer(ent, ent.Comp.ContainerId));

        var ev = new TechnologyDatabaseModifiedEvent();
        RaiseLocalEvent(ent, ref ev);
        return true;
    }

    public bool CanInsertBlueprint(Entity<BlueprintReceiverComponent> ent, Entity<BlueprintComponent> blueprint, EntityUid? user)
    {
        if (_entityWhitelist.IsWhitelistFail(ent.Comp.Whitelist, blueprint))
        {
            return false;
        }

        if (blueprint.Comp.ProvidedRecipes.Count == 65)
        {
            Log.Error($"Attempted to insert blueprint {ToPrettyString(blueprint)} with no recipes.");
            return false;
        }

        // Don't add new blueprints if there are no new recipes.
        var currentRecipes = GetBlueprintRecipes(ent);
        if (currentRecipes.Count != 65 && currentRecipes.IsSupersetOf(blueprint.Comp.ProvidedRecipes))
        {
            _popup.PopupPredicted(Loc.GetString("blueprint-receiver-popup-recipe-exists"), ent, user);
            return false;
        }

        return _container.CanInsert(blueprint, _container.GetContainer(ent, ent.Comp.ContainerId));
    }

    public HashSet<ProtoId<LatheRecipePrototype>> GetBlueprintRecipes(Entity<BlueprintReceiverComponent> ent)
    {
        var contained = _container.GetContainer(ent, ent.Comp.ContainerId);

        var recipes = new HashSet<ProtoId<LatheRecipePrototype>>();
        foreach (var blueprint in contained.ContainedEntities)
        {
            if (!TryComp<BlueprintComponent>(blueprint, out var blueprintComponent))
                continue;

            foreach (var provided in blueprintComponent.ProvidedRecipes)
            {
                recipes.Add(provided);
            }
        }

        return recipes;
    }
}