// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
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
// SPDX-FileCopyrightText: 65 JustCone <65JustCone65@users.noreply.github.com>
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
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lunarcomets <65lunarcomets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Anomaly.Components;
using Content.Server.DeviceLinking.Systems;
using Content.Server.Power.EntitySystems;
using Content.Shared.Anomaly.Components;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Power;
using Robust.Shared.Audio.Systems;
using Content.Shared.Verbs;
using Robust.Shared.Timing;

namespace Content.Server.Anomaly;

/// <summary>
/// a device that allows you to translate anomaly activity into multitool signals.
/// </summary>
public sealed partial class AnomalySynchronizerSystem : EntitySystem
{
    [Dependency] private readonly AnomalySystem _anomaly = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly EntityLookupSystem _entityLookup = default!;
    [Dependency] private readonly DeviceLinkSystem _signal = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly PowerReceiverSystem _power = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AnomalySynchronizerComponent, InteractHandEvent>(OnInteractHand);
        SubscribeLocalEvent<AnomalySynchronizerComponent, PowerChangedEvent>(OnPowerChanged);
        SubscribeLocalEvent<AnomalySynchronizerComponent, ExaminedEvent>(OnExamined);
        SubscribeLocalEvent<AnomalySynchronizerComponent, GetVerbsEvent<InteractionVerb>>(OnGetInteractionVerbs);

        SubscribeLocalEvent<AnomalyPulseEvent>(OnAnomalyPulse);
        SubscribeLocalEvent<AnomalySeverityChangedEvent>(OnAnomalySeverityChanged);
        SubscribeLocalEvent<AnomalyStabilityChangedEvent>(OnAnomalyStabilityChanged);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<AnomalySynchronizerComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var sync, out var xform))
        {
            if (sync.ConnectedAnomaly is null)
                continue;

            if (_timing.CurTime < sync.NextCheckTime)
                continue;
            sync.NextCheckTime += sync.CheckFrequency;

            if (Transform(sync.ConnectedAnomaly.Value).MapUid != Transform(uid).MapUid)
            {
                DisconnectFromAnomaly((uid, sync), sync.ConnectedAnomaly.Value);
                continue;
            }

            if (!xform.Coordinates.TryDistance(EntityManager, Transform(sync.ConnectedAnomaly.Value).Coordinates, out var distance))
                continue;

            if (distance > sync.AttachRange)
                DisconnectFromAnomaly((uid, sync), sync.ConnectedAnomaly.Value);
        }
    }

    /// <summary>
    /// If powered, try to attach a nearby anomaly.
    /// </summary>
    public bool TryAttachNearbyAnomaly(Entity<AnomalySynchronizerComponent> ent, EntityUid? user = null)
    {
        if (!_power.IsPowered(ent))
        {
            if (user is not null)
                _popup.PopupEntity(Loc.GetString("base-computer-ui-component-not-powered", ("machine", ent)), ent, user.Value);

            return false;
        }

        var coords = _transform.GetMapCoordinates(ent);
        var anomaly = _entityLookup.GetEntitiesInRange<AnomalyComponent>(coords, ent.Comp.AttachRange).FirstOrDefault();

        if (anomaly.Owner is { Valid: false }) // no anomaly in range
        {
            if (user is not null)
                _popup.PopupEntity(Loc.GetString("anomaly-sync-no-anomaly"), ent, user.Value);

            return false;
        }

        ConnectToAnomaly(ent, anomaly);
        return true;
    }

    private void OnPowerChanged(Entity<AnomalySynchronizerComponent> ent, ref PowerChangedEvent args)
    {
        if (args.Powered)
            return;

        if (ent.Comp.ConnectedAnomaly is null)
            return;

        DisconnectFromAnomaly(ent, ent.Comp.ConnectedAnomaly.Value);
    }

    private void OnExamined(Entity<AnomalySynchronizerComponent> ent, ref ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString(ent.Comp.ConnectedAnomaly.HasValue ? "anomaly-sync-examine-connected" : "anomaly-sync-examine-not-connected"));
    }

    private void OnGetInteractionVerbs(Entity<AnomalySynchronizerComponent> ent, ref GetVerbsEvent<InteractionVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.Hands is null || ent.Comp.ConnectedAnomaly.HasValue)
            return;

        var user = args.User;
        args.Verbs.Add(new()
        {
            Act = () =>
            {
                TryAttachNearbyAnomaly(ent, user);
            },
            Message = Loc.GetString("anomaly-sync-connect-verb-message", ("machine", ent)),
            Text = Loc.GetString("anomaly-sync-connect-verb-text"),
        });
    }

    private void OnInteractHand(Entity<AnomalySynchronizerComponent> ent, ref InteractHandEvent args)
    {
        TryAttachNearbyAnomaly(ent, args.User);
    }

    private void ConnectToAnomaly(Entity<AnomalySynchronizerComponent> ent, Entity<AnomalyComponent> anomaly)
    {
        if (ent.Comp.ConnectedAnomaly == anomaly)
            return;

        ent.Comp.ConnectedAnomaly = anomaly;
        //move the anomaly to the center of the synchronizer, for aesthetics.
        var targetXform = _transform.GetWorldPosition(ent);
        _transform.SetWorldPosition(anomaly, targetXform);

        if (ent.Comp.PulseOnConnect)
            _anomaly.DoAnomalyPulse(anomaly, anomaly);

        _popup.PopupEntity(Loc.GetString("anomaly-sync-connected"), ent, PopupType.Medium);
        _audio.PlayPvs(ent.Comp.ConnectedSound, ent);
    }

    //TODO: disconnection from the anomaly should also be triggered if the anomaly is far away from the synchronizer.
    //Currently only bluespace anomaly can do this, but for some reason it is the only one that cannot be connected to the synchronizer.
    private void DisconnectFromAnomaly(Entity<AnomalySynchronizerComponent> ent, EntityUid other)
    {
        if (ent.Comp.ConnectedAnomaly == null)
            return;

        if (TryComp<AnomalyComponent>(other, out var anomaly))
        {
            if (ent.Comp.PulseOnDisconnect)
                _anomaly.DoAnomalyPulse(ent.Comp.ConnectedAnomaly.Value, anomaly);
        }

        _popup.PopupEntity(Loc.GetString("anomaly-sync-disconnected"), ent, PopupType.Large);
        _audio.PlayPvs(ent.Comp.ConnectedSound, ent);

        ent.Comp.ConnectedAnomaly = null;
    }

    private void OnAnomalyPulse(ref AnomalyPulseEvent args)
    {
        var query = EntityQueryEnumerator<AnomalySynchronizerComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (args.Anomaly != component.ConnectedAnomaly)
                continue;

            if (!_power.IsPowered(uid))
                continue;

            _signal.InvokePort(uid, component.PulsePort);
        }
    }

    private void OnAnomalySeverityChanged(ref AnomalySeverityChangedEvent args)
    {
        var query = EntityQueryEnumerator<AnomalySynchronizerComponent>();
        while (query.MoveNext(out var ent, out var component))
        {
            if (args.Anomaly != component.ConnectedAnomaly)
                continue;

            if (!_power.IsPowered(ent))
                continue;

            //The superscritical port is invoked not at the AnomalySupercriticalEvent,
            //but at the moment the growth animation starts. Otherwise, there is no point in this port.
            //ATTENTION! the console command supercriticalanomaly does not work here,
            //as it forcefully causes growth to start without increasing severity.
            if (args.Severity >= 65)
                _signal.InvokePort(ent, component.SupercritPort);
        }
    }

    private void OnAnomalyStabilityChanged(ref AnomalyStabilityChangedEvent args)
    {
        Entity<AnomalyComponent> anomaly = (args.Anomaly, Comp<AnomalyComponent>(args.Anomaly));

        var query = EntityQueryEnumerator<AnomalySynchronizerComponent>();
        while (query.MoveNext(out var ent, out var component))
        {
            if (component.ConnectedAnomaly != anomaly)
                continue;

            if (!_power.IsPowered(ent))
                continue;

            if (args.Stability < anomaly.Comp.DecayThreshold)
            {
                _signal.InvokePort(ent, component.DecayingPort);
            }
            else if (args.Stability > anomaly.Comp.GrowthThreshold)
            {
                _signal.InvokePort(ent, component.GrowingPort);
            }
            else
            {
                _signal.InvokePort(ent, component.StabilizePort);
            }
        }
    }
}