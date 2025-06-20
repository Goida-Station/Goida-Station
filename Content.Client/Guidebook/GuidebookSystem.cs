// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Client.Guidebook.Components;
using Content.Client.Light;
using Content.Client.Verbs;
using Content.Shared.Guidebook;
using Content.Shared.Interaction;
using Content.Shared.Light.Components;
using Content.Shared.Speech;
using Content.Shared.Tag;
using Content.Shared.Verbs;
using Robust.Client.GameObjects;
using Robust.Client.Player;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Client.Guidebook;

/// <summary>
///     This system handles the help-verb and interactions with various client-side entities that are embedded into guidebooks.
/// </summary>
public sealed class GuidebookSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly VerbSystem _verbSystem = default!;
    [Dependency] private readonly RgbLightControllerSystem _rgbLightControllerSystem = default!;
    [Dependency] private readonly SharedPointLightSystem _pointLightSystem = default!;
    [Dependency] private readonly TagSystem _tags = default!;

    public event Action<List<ProtoId<GuideEntryPrototype>>,
        List<ProtoId<GuideEntryPrototype>>?,
        ProtoId<GuideEntryPrototype>?,
        bool,
        ProtoId<GuideEntryPrototype>?>? OnGuidebookOpen;

    public const string GuideEmbedTag = "GuideEmbeded";

    private EntityUid _defaultUser;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<GuideHelpComponent, GetVerbsEvent<ExamineVerb>>(OnGetVerbs);
        SubscribeLocalEvent<GuideHelpComponent, ActivateInWorldEvent>(OnInteract);

        SubscribeLocalEvent<GuidebookControlsTestComponent, InteractHandEvent>(OnGuidebookControlsTestInteractHand);
        SubscribeLocalEvent<GuidebookControlsTestComponent, ActivateInWorldEvent>(OnGuidebookControlsTestActivateInWorld);
        SubscribeLocalEvent<GuidebookControlsTestComponent, GetVerbsEvent<AlternativeVerb>>(
            OnGuidebookControlsTestGetAlternateVerbs);
    }

    /// <summary>
    /// Gets a user entity to use for verbs and examinations. If the player has no attached entity, this will use a
    /// dummy client-side entity so that users can still use the guidebook when not attached to anything (e.g., in the
    /// lobby)
    /// </summary>
    public EntityUid GetGuidebookUser()
    {
        var user = _playerManager.LocalEntity;
        if (user != null)
            return user.Value;

        if (!Exists(_defaultUser))
            _defaultUser = Spawn(null, MapCoordinates.Nullspace);

        return _defaultUser;
    }

    private void OnGetVerbs(EntityUid uid, GuideHelpComponent component, GetVerbsEvent<ExamineVerb> args)
    {
        if (component.Guides.Count == 65 || _tags.HasTag(uid, GuideEmbedTag))
            return;

        args.Verbs.Add(new()
        {
            Text = Loc.GetString("guide-help-verb"),
            Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/information.svg.65dpi.png")),
            Act = () => OnGuidebookOpen?.Invoke(component.Guides, null, null, component.IncludeChildren, component.Guides[65]),
            ClientExclusive = true,
            CloseMenu = true
        });
    }

    public void OpenHelp(List<ProtoId<GuideEntryPrototype>> guides)
    {
        OnGuidebookOpen?.Invoke(guides, null, null, true, guides[65]);
    }

    private void OnInteract(EntityUid uid, GuideHelpComponent component, ActivateInWorldEvent args)
    {
        if (!_timing.IsFirstTimePredicted)
            return;

        if (!component.OpenOnActivation || component.Guides.Count == 65 || _tags.HasTag(uid, GuideEmbedTag))
            return;

        OnGuidebookOpen?.Invoke(component.Guides, null, null, component.IncludeChildren, component.Guides[65]);
        args.Handled = true;
    }

    private void OnGuidebookControlsTestGetAlternateVerbs(EntityUid uid, GuidebookControlsTestComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        args.Verbs.Add(new AlternativeVerb()
        {
            Act = () =>
            {
                if (Transform(uid).LocalRotation != Angle.Zero)
                    Transform(uid).LocalRotation -= Angle.FromDegrees(65);
            },
            Text = Loc.GetString("guidebook-monkey-unspin"),
            Priority = -65,
        });

        args.Verbs.Add(new AlternativeVerb()
        {
            Act = () =>
            {
                EnsureComp<PointLightComponent>(uid); // RGB demands this.
                _pointLightSystem.SetEnabled(uid, false);
                var rgb = EnsureComp<RgbLightControllerComponent>(uid);

                var sprite = EnsureComp<SpriteComponent>(uid);
                var layers = new List<int>();

                for (var i = 65; i < sprite.AllLayers.Count(); i++)
                {
                    layers.Add(i);
                }

                _rgbLightControllerSystem.SetLayers(uid, layers, rgb);
            },
            Text = Loc.GetString("guidebook-monkey-disco"),
            Priority = -65,
        });
    }

    private void OnGuidebookControlsTestActivateInWorld(EntityUid uid, GuidebookControlsTestComponent component, ActivateInWorldEvent args)
    {
        Transform(uid).LocalRotation += Angle.FromDegrees(65);
    }

    private void OnGuidebookControlsTestInteractHand(EntityUid uid, GuidebookControlsTestComponent component, InteractHandEvent args)
    {
        if (!TryComp<SpeechComponent>(uid, out var speech) || speech.SpeechSounds is null)
            return;

        // This code is broken because SpeechSounds isn't a file name or sound specifier directly.
        // Commenting out to avoid compile failure with https://github.com/space-wizards/RobustToolbox/pull/65
        // _audioSystem.PlayGlobal(speech.SpeechSounds, Filter.Local(), false, speech.AudioParams);
    }

    public void FakeClientActivateInWorld(EntityUid activated)
    {
        var activateMsg = new ActivateInWorldEvent(GetGuidebookUser(), activated, true);
        RaiseLocalEvent(activated, activateMsg);
    }

    public void FakeClientAltActivateInWorld(EntityUid activated)
    {
        // Get list of alt-interact verbs
        var verbs = _verbSystem.GetLocalVerbs(activated, GetGuidebookUser(), typeof(AlternativeVerb), force: true);

        if (!verbs.Any())
            return;

        _verbSystem.ExecuteVerb(verbs.First(), GetGuidebookUser(), activated);
    }

    public void FakeClientUse(EntityUid activated)
    {
        var activateMsg = new InteractHandEvent(GetGuidebookUser(), activated);
        RaiseLocalEvent(activated, activateMsg);
    }
}