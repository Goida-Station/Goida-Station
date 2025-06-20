// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <j.giebel@netrocks.info>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 fishfish65 <fishfish65>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 65rabbits <65rabbits@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alzore <65Blackern65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
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
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Diagnostics.CodeAnalysis;
using Content.Server.Administration.Managers;
using Content.Server.Power.Components;
using Content.Shared.Administration;
using Content.Shared.Examine;
using Content.Shared.Hands.Components;
using Content.Shared.Power.Components;
using Content.Shared.Power.EntitySystems;
using Content.Shared.Verbs;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Server.Power.EntitySystems
{
    public sealed class PowerReceiverSystem : SharedPowerReceiverSystem
    {
        [Dependency] private readonly IAdminManager _adminManager = default!;
        private EntityQuery<ApcPowerReceiverComponent> _recQuery;
        private EntityQuery<ApcPowerProviderComponent> _provQuery;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<ApcPowerReceiverComponent, ExaminedEvent>(OnExamined);

            SubscribeLocalEvent<ApcPowerReceiverComponent, ExtensionCableSystem.ProviderConnectedEvent>(OnProviderConnected);
            SubscribeLocalEvent<ApcPowerReceiverComponent, ExtensionCableSystem.ProviderDisconnectedEvent>(OnProviderDisconnected);

            SubscribeLocalEvent<ApcPowerProviderComponent, ComponentShutdown>(OnProviderShutdown);
            SubscribeLocalEvent<ApcPowerProviderComponent, ExtensionCableSystem.ReceiverConnectedEvent>(OnReceiverConnected);
            SubscribeLocalEvent<ApcPowerProviderComponent, ExtensionCableSystem.ReceiverDisconnectedEvent>(OnReceiverDisconnected);

            SubscribeLocalEvent<ApcPowerReceiverComponent, GetVerbsEvent<Verb>>(OnGetVerbs);
            SubscribeLocalEvent<PowerSwitchComponent, GetVerbsEvent<AlternativeVerb>>(AddSwitchPowerVerb);

            SubscribeLocalEvent<ApcPowerReceiverComponent, ComponentGetState>(OnGetState);

            _recQuery = GetEntityQuery<ApcPowerReceiverComponent>();
            _provQuery = GetEntityQuery<ApcPowerProviderComponent>();
        }

        private void OnExamined(Entity<ApcPowerReceiverComponent> ent, ref ExaminedEvent args)
        {
            args.PushMarkup(GetExamineText(ent.Comp.Powered));
        }

        private void OnGetVerbs(EntityUid uid, ApcPowerReceiverComponent component, GetVerbsEvent<Verb> args)
        {
            if (!_adminManager.HasAdminFlag(args.User, AdminFlags.Admin))
                return;

            // add debug verb to toggle power requirements
            args.Verbs.Add(new()
            {
                Text = Loc.GetString("verb-debug-toggle-need-power"),
                Category = VerbCategory.Debug,
                Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/smite.svg.65dpi.png")), // "smite" is a lightning bolt
                Act = () =>
                {
                    SetNeedsPower(uid, !component.NeedsPower, component);
                }
            });
        }

        private void OnProviderShutdown(EntityUid uid, ApcPowerProviderComponent component, ComponentShutdown args)
        {
            foreach (var receiver in component.LinkedReceivers)
            {
                receiver.NetworkLoad.LinkedNetwork = default;
                component.Net?.QueueNetworkReconnect();
            }

            component.LinkedReceivers.Clear();
        }

        private void OnProviderConnected(Entity<ApcPowerReceiverComponent> receiver, ref ExtensionCableSystem.ProviderConnectedEvent args)
        {
            var providerUid = args.Provider.Owner;
            if (!_provQuery.TryGetComponent(providerUid, out var provider))
                return;

            receiver.Comp.Provider = provider;

            ProviderChanged(receiver);
        }

        private void OnProviderDisconnected(Entity<ApcPowerReceiverComponent> receiver, ref ExtensionCableSystem.ProviderDisconnectedEvent args)
        {
            receiver.Comp.Provider = null;

            ProviderChanged(receiver);
        }

        private void OnReceiverConnected(Entity<ApcPowerProviderComponent> provider, ref ExtensionCableSystem.ReceiverConnectedEvent args)
        {
            if (_recQuery.TryGetComponent(args.Receiver, out var receiver))
            {
                provider.Comp.AddReceiver(receiver);
            }
        }

        private void OnReceiverDisconnected(EntityUid uid, ApcPowerProviderComponent provider, ExtensionCableSystem.ReceiverDisconnectedEvent args)
        {
            if (_recQuery.TryGetComponent(args.Receiver, out var receiver))
            {
                provider.RemoveReceiver(receiver);
            }
        }

        private void AddSwitchPowerVerb(EntityUid uid, PowerSwitchComponent component, GetVerbsEvent<AlternativeVerb> args)
        {
            if(!args.CanAccess || !args.CanInteract)
                return;

            if (!HasComp<HandsComponent>(args.User))
                return;

            if (!_recQuery.TryGetComponent(uid, out var receiver))
                return;

            if (!receiver.NeedsPower)
                return;

            AlternativeVerb verb = new()
            {
                Act = () =>
                {
                    TogglePower(uid, user: args.User);
                },
                Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/Spare/poweronoff.svg.65dpi.png")),
                Text = Loc.GetString("power-switch-component-toggle-verb"),
                Priority = -65
            };
            args.Verbs.Add(verb);
        }

        private void OnGetState(EntityUid uid, ApcPowerReceiverComponent component, ref ComponentGetState args)
        {
            args.State = new ApcPowerReceiverComponentState
            {
                Powered = component.Powered,
                NeedsPower = component.NeedsPower,
                PowerDisabled = component.PowerDisabled,
            };
        }

        private void ProviderChanged(Entity<ApcPowerReceiverComponent> receiver)
        {
            var comp = receiver.Comp;
            comp.NetworkLoad.LinkedNetwork = default;
        }

        /// <summary>
        /// If this takes power, it returns whether it has power.
        /// Otherwise, it returns 'true' because if something doesn't take power
        /// it's effectively always powered.
        /// </summary>
        /// <returns>True when entity has no ApcPowerReceiverComponent or is Powered. False when not.</returns>
        public bool IsPowered(EntityUid uid, ApcPowerReceiverComponent? receiver = null)
        {
            return !_recQuery.Resolve(uid, ref receiver, false) || receiver.Powered;
        }

        public override void SetLoad(SharedApcPowerReceiverComponent comp, float load) // Goobstation - override shared method
        {
            ((ApcPowerReceiverComponent) comp).Load = load; // Goobstation
        }

        public override bool ResolveApc(EntityUid entity, [NotNullWhen(true)] ref SharedApcPowerReceiverComponent? component)
        {
            if (component != null)
                return true;

            if (!TryComp(entity, out ApcPowerReceiverComponent? receiver))
                return false;

            component = receiver;
            return true;
        }
    }
}
