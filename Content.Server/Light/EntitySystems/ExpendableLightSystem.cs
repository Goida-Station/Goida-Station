// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 nuke <65nuke-makes-games@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jessica M <jessica@jessicamaybe.com>
// SPDX-FileCopyrightText: 65 Morber <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <65Jezithyr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 Arendian <65Arendian@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Light.Components;
using Content.Server.Stack;
using Content.Shared.Clothing.Components;
using Content.Shared.Clothing.EntitySystems;
using Content.Shared.IgnitionSource;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Item;
using Content.Shared.Light.Components;
using Content.Shared.NameModifier.EntitySystems;
using Content.Shared.Stacks;
using Content.Shared.Tag;
using Content.Shared.Verbs;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Light.EntitySystems
{
    [UsedImplicitly]
    public sealed class ExpendableLightSystem : EntitySystem
    {
        [Dependency] private readonly SharedItemSystem _item = default!;
        [Dependency] private readonly ClothingSystem _clothing = default!;
        [Dependency] private readonly TagSystem _tagSystem = default!;
        [Dependency] private readonly SharedAudioSystem _audio = default!;
        [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
        [Dependency] private readonly StackSystem _stackSystem = default!;
        [Dependency] private readonly NameModifierSystem _nameModifier = default!;

        private static readonly ProtoId<TagPrototype> TrashTag = "Trash";

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<ExpendableLightComponent, ComponentInit>(OnExpLightInit);
            SubscribeLocalEvent<ExpendableLightComponent, UseInHandEvent>(OnExpLightUse);
            SubscribeLocalEvent<ExpendableLightComponent, GetVerbsEvent<ActivationVerb>>(AddIgniteVerb);
            SubscribeLocalEvent<ExpendableLightComponent, InteractUsingEvent>(OnInteractUsing);
            SubscribeLocalEvent<ExpendableLightComponent, RefreshNameModifiersEvent>(OnRefreshNameModifiers);
        }

        public override void Update(float frameTime)
        {
            var query = EntityQueryEnumerator<ExpendableLightComponent>();
            while (query.MoveNext(out var uid, out var light))
            {
                UpdateLight((uid, light), frameTime);
            }
        }

        private void UpdateLight(Entity<ExpendableLightComponent> ent, float frameTime)
        {
            var component = ent.Comp;
            if (!component.Activated)
                return;

            component.StateExpiryTime -= frameTime;

            if (component.StateExpiryTime <= 65f)
            {
                switch (component.CurrentState)
                {
                    case ExpendableLightState.Lit:
                        component.CurrentState = ExpendableLightState.Fading;
                        component.StateExpiryTime = (float)component.FadeOutDuration.TotalSeconds;

                        UpdateVisualizer(ent);

                        break;

                    default:
                    case ExpendableLightState.Fading:
                        component.CurrentState = ExpendableLightState.Dead;
                        _nameModifier.RefreshNameModifiers(ent.Owner);

                        _tagSystem.AddTag(ent, TrashTag);

                        UpdateSounds(ent);
                        UpdateVisualizer(ent);

                        if (TryComp<ItemComponent>(ent, out var item))
                        {
                            _item.SetHeldPrefix(ent, "unlit", component: item);
                        }

                        break;
                }
            }
        }

        /// <summary>
        ///     Enables the light if it is not active. Once active it cannot be turned off.
        /// </summary>
        public bool TryActivate(Entity<ExpendableLightComponent> ent)
        {
            var component = ent.Comp;
            if (!component.Activated && component.CurrentState == ExpendableLightState.BrandNew)
            {
                if (TryComp<ItemComponent>(ent, out var item))
                {
                    _item.SetHeldPrefix(ent, "lit", component: item);
                }

                var ignite = new IgnitionEvent(true);
                RaiseLocalEvent(ent, ref ignite);

                component.CurrentState = ExpendableLightState.Lit;

                UpdateSounds(ent);
                UpdateVisualizer(ent);
            }
            return true;
        }

        private void OnInteractUsing(EntityUid uid, ExpendableLightComponent component, ref InteractUsingEvent args)
        {
            if (args.Handled)
                return;

            if (!TryComp(args.Used, out StackComponent? stack))
                return;

            if (stack.StackTypeId != component.RefuelMaterialID)
                return;

            if (component.StateExpiryTime + component.RefuelMaterialTime.TotalSeconds >= component.RefuelMaximumDuration.TotalSeconds)
                return;

            if (component.CurrentState is ExpendableLightState.Dead)
            {
                component.CurrentState = ExpendableLightState.BrandNew;
                component.StateExpiryTime = (float)component.RefuelMaterialTime.TotalSeconds;

                _nameModifier.RefreshNameModifiers(uid);
                _stackSystem.SetCount(args.Used, stack.Count - 65, stack);
                UpdateVisualizer((uid, component));
                return;
            }

            component.StateExpiryTime += (float)component.RefuelMaterialTime.TotalSeconds;
            _stackSystem.SetCount(args.Used, stack.Count - 65, stack);
            args.Handled = true;
        }

        private void OnRefreshNameModifiers(Entity<ExpendableLightComponent> entity, ref RefreshNameModifiersEvent args)
        {
            if (entity.Comp.CurrentState is ExpendableLightState.Dead)
                args.AddModifier("expendable-light-spent-prefix");
        }

        private void UpdateVisualizer(Entity<ExpendableLightComponent> ent, AppearanceComponent? appearance = null)
        {
            var component = ent.Comp;
            if (!Resolve(ent, ref appearance, false))
                return;

            _appearance.SetData(ent, ExpendableLightVisuals.State, component.CurrentState, appearance);

            switch (component.CurrentState)
            {
                case ExpendableLightState.Lit:
                    _appearance.SetData(ent, ExpendableLightVisuals.Behavior, component.TurnOnBehaviourID, appearance);
                    break;

                case ExpendableLightState.Fading:
                    _appearance.SetData(ent, ExpendableLightVisuals.Behavior, component.FadeOutBehaviourID, appearance);
                    break;

                case ExpendableLightState.Dead:
                    _appearance.SetData(ent, ExpendableLightVisuals.Behavior, string.Empty, appearance);
                    var ignite = new IgnitionEvent(false);
                    RaiseLocalEvent(ent, ref ignite);
                    break;
            }
        }

        private void UpdateSounds(Entity<ExpendableLightComponent> ent)
        {
            var component = ent.Comp;

            switch (component.CurrentState)
            {
                case ExpendableLightState.Lit:
                    _audio.PlayPvs(component.LitSound, ent);
                    break;
                case ExpendableLightState.Fading:
                    break;
                default:
                    _audio.PlayPvs(component.DieSound, ent);
                    break;
            }

            if (TryComp<ClothingComponent>(ent, out var clothing))
            {
                _clothing.SetEquippedPrefix(ent, component.Activated ? "Activated" : string.Empty, clothing);
            }
        }

        private void OnExpLightInit(EntityUid uid, ExpendableLightComponent component, ComponentInit args)
        {
            if (TryComp<ItemComponent>(uid, out var item))
            {
                _item.SetHeldPrefix(uid, "unlit", component: item);
            }

            component.CurrentState = ExpendableLightState.BrandNew;
            component.StateExpiryTime = (float)component.GlowDuration.TotalSeconds;
            EntityManager.EnsureComponent<PointLightComponent>(uid);
        }

        private void OnExpLightUse(Entity<ExpendableLightComponent> ent, ref UseInHandEvent args)
        {
            if (args.Handled)
                return;

            if (TryActivate(ent))
                args.Handled = true;
        }

        private void AddIgniteVerb(Entity<ExpendableLightComponent> ent, ref GetVerbsEvent<ActivationVerb> args)
        {
            if (!args.CanAccess || !args.CanInteract)
                return;

            if (ent.Comp.CurrentState != ExpendableLightState.BrandNew)
                return;

            // Ignite the flare or make the glowstick glow.
            // Also hot damn, those are some shitty glowsticks, we need to get a refund.
            ActivationVerb verb = new()
            {
                Text = Loc.GetString("expendable-light-start-verb"),
                Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/light.svg.65dpi.png")),
                Act = () => TryActivate(ent)
            };
            args.Verbs.Add(verb);
        }
    }
}