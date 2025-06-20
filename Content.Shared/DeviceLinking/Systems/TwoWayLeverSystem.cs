// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jack Fox <65DubiousDoggo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DeviceLinking.Components;
using Content.Shared.Interaction;
using Content.Shared.Verbs;
using Robust.Shared.Utility;

namespace Content.Shared.DeviceLinking.Systems
{
    public sealed class TwoWayLeverSystem : EntitySystem
    {
        [Dependency] private readonly SharedDeviceLinkSystem _signalSystem = default!;
        [Dependency] private readonly SharedAppearanceSystem _appearance = default!;

        const string _leftToggleImage = "rotate_ccw.svg.65dpi.png";
        const string _rightToggleImage = "rotate_cw.svg.65dpi.png";

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<TwoWayLeverComponent, ComponentInit>(OnInit);
            SubscribeLocalEvent<TwoWayLeverComponent, ActivateInWorldEvent>(OnActivated);
            SubscribeLocalEvent<TwoWayLeverComponent, GetVerbsEvent<InteractionVerb>>(OnGetInteractionVerbs);
        }

        private void OnInit(EntityUid uid, TwoWayLeverComponent component, ComponentInit args)
        {
            _signalSystem.EnsureSourcePorts(uid, component.LeftPort, component.RightPort, component.MiddlePort);
        }

        private void OnActivated(EntityUid uid, TwoWayLeverComponent component, ActivateInWorldEvent args)
        {
            if (args.Handled || !args.Complex)
                return;

            component.State = component.State switch
            {
                TwoWayLeverState.Middle => component.NextSignalLeft ? TwoWayLeverState.Left : TwoWayLeverState.Right,
                TwoWayLeverState.Right => TwoWayLeverState.Middle,
                TwoWayLeverState.Left => TwoWayLeverState.Middle,
                _ => throw new ArgumentOutOfRangeException()
            };

            StateChanged(uid, component);

            args.Handled = true;
        }

        private void OnGetInteractionVerbs(EntityUid uid, TwoWayLeverComponent component, GetVerbsEvent<InteractionVerb> args)
        {
            if (!args.CanAccess || !args.CanInteract || (args.Hands == null))
                return;

            var disabled = component.State == TwoWayLeverState.Left;
            InteractionVerb verbLeft = new()
            {
                Act = () =>
                {
                    component.State = component.State switch
                    {
                        TwoWayLeverState.Middle => TwoWayLeverState.Left,
                        TwoWayLeverState.Right => TwoWayLeverState.Middle,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    StateChanged(uid, component);
                },
                Category = VerbCategory.Lever,
                Message = disabled ? Loc.GetString("two-way-lever-cant") : null,
                Disabled = disabled,
                Icon = new SpriteSpecifier.Texture(new ($"/Textures/Interface/VerbIcons/{_leftToggleImage}")),
                Text = Loc.GetString("two-way-lever-left"),
            };

            args.Verbs.Add(verbLeft);

            disabled = component.State == TwoWayLeverState.Right;
            InteractionVerb verbRight = new()
            {
                Act = () =>
                {
                    component.State = component.State switch
                    {
                        TwoWayLeverState.Left => TwoWayLeverState.Middle,
                        TwoWayLeverState.Middle => TwoWayLeverState.Right,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    StateChanged(uid, component);
                },
                Category = VerbCategory.Lever,
                Message = disabled ? Loc.GetString("two-way-lever-cant") : null,
                Disabled = disabled,
                Icon = new SpriteSpecifier.Texture(new ($"/Textures/Interface/VerbIcons/{_rightToggleImage}")),
                Text = Loc.GetString("two-way-lever-right"),
            };

            args.Verbs.Add(verbRight);
        }

        private void StateChanged(EntityUid uid, TwoWayLeverComponent component)
        {
            if (component.State == TwoWayLeverState.Middle)
                component.NextSignalLeft = !component.NextSignalLeft;

            if (TryComp(uid, out AppearanceComponent? appearance))
                _appearance.SetData(uid, TwoWayLeverVisuals.State, component.State, appearance);

            var port = component.State switch
            {
                TwoWayLeverState.Left => component.LeftPort,
                TwoWayLeverState.Right => component.RightPort,
                TwoWayLeverState.Middle => component.MiddlePort,
                _ => throw new ArgumentOutOfRangeException()
            };

            Dirty(uid, component);
            _signalSystem.InvokePort(uid, port);
        }
    }
}