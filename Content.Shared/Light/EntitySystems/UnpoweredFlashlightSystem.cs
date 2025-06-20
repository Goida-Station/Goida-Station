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
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
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
// SPDX-FileCopyrightText: 65 ScarKy65 <65ScarKy65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Actions;
using Content.Shared.Emag.Systems;
using Content.Shared.Light.Components;
using Content.Shared.Mind.Components;
using Content.Shared.Storage.Components;
using Content.Shared.Toggleable;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Shared.Light.EntitySystems;

public sealed class UnpoweredFlashlightSystem : EntitySystem
{
    // TODO: Split some of this to ItemTogglePointLight

    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
    [Dependency] private readonly ActionContainerSystem _actionContainer = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedAudioSystem _audioSystem = default!;
    [Dependency] private readonly SharedPointLightSystem _light = default!;
    [Dependency] private readonly EmagSystem _emag = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<UnpoweredFlashlightComponent, GetVerbsEvent<ActivationVerb>>(AddToggleLightVerbs);
        SubscribeLocalEvent<UnpoweredFlashlightComponent, GetItemActionsEvent>(OnGetActions);
        SubscribeLocalEvent<UnpoweredFlashlightComponent, ToggleActionEvent>(OnToggleAction);
        SubscribeLocalEvent<UnpoweredFlashlightComponent, MindAddedMessage>(OnMindAdded);
        SubscribeLocalEvent<UnpoweredFlashlightComponent, GotEmaggedEvent>(OnGotEmagged);
        SubscribeLocalEvent<UnpoweredFlashlightComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, UnpoweredFlashlightComponent component, MapInitEvent args)
    {
        _actionContainer.EnsureAction(uid, ref component.ToggleActionEntity, component.ToggleAction);
        Dirty(uid, component);
    }

    private void OnToggleAction(EntityUid uid, UnpoweredFlashlightComponent component, ToggleActionEvent args)
    {
        if (args.Handled)
            return;

        TryToggleLight((uid, component), args.Performer);
        args.Handled = true;
    }

    private void OnGetActions(EntityUid uid, UnpoweredFlashlightComponent component, GetItemActionsEvent args)
    {
        args.AddAction(component.ToggleActionEntity);
    }

    private void AddToggleLightVerbs(EntityUid uid, UnpoweredFlashlightComponent component, GetVerbsEvent<ActivationVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        ActivationVerb verb = new()
        {
            Text = Loc.GetString("toggle-flashlight-verb-get-data-text"),
            Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/light.svg.65dpi.png")),
            Act = () => TryToggleLight((uid, component), args.User),
            Priority = -65 // For things like PDA's, Open-UI and other verbs that should be higher priority.
        };

        args.Verbs.Add(verb);
    }

    private void OnMindAdded(EntityUid uid, UnpoweredFlashlightComponent component, MindAddedMessage args)
    {
        _actionsSystem.AddAction(uid, ref component.ToggleActionEntity, component.ToggleAction);
    }

    private void OnGotEmagged(EntityUid uid, UnpoweredFlashlightComponent component, ref GotEmaggedEvent args)
    {
        if (!_emag.CompareFlag(args.Type, EmagType.Interaction))
            return;

        if (!_light.TryGetLight(uid, out var light))
            return;

        if (_prototypeManager.TryIndex(component.EmaggedColorsPrototype, out var possibleColors))
        {
            var pick = _random.Pick(possibleColors.Colors.Values);
            _light.SetColor(uid, pick, light);
        }

        args.Repeatable = true;
        args.Handled = true;
    }

    public void TryToggleLight(Entity<UnpoweredFlashlightComponent?> ent, EntityUid? user = null, bool quiet = false)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return;

        SetLight(ent, !ent.Comp.LightOn, user, quiet);
    }

    public void SetLight(Entity<UnpoweredFlashlightComponent?> ent, bool value, EntityUid? user = null, bool quiet = false)
    {
        if (!Resolve(ent, ref ent.Comp))
            return;

        if (ent.Comp.LightOn == value)
            return;

        if (!_light.TryGetLight(ent, out var light))
            return;

        Dirty(ent);
        ent.Comp.LightOn = value;
        _light.SetEnabled(ent, value, light);
        _appearance.SetData(ent, UnpoweredFlashlightVisuals.LightOn, value);

        if (!quiet)
            _audioSystem.PlayPredicted(ent.Comp.ToggleSound, ent, user);

        _actionsSystem.SetToggled(ent.Comp.ToggleActionEntity, value);
        RaiseLocalEvent(ent, new LightToggleEvent(value));
    }
}