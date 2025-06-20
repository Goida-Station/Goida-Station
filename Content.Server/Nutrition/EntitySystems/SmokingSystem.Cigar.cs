// SPDX-FileCopyrightText: 65 JustinTime <65JustinTether@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 brainfood65 <65brainfood65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Nutrition.Components;
using Content.Shared.Interaction;
using Content.Shared.Nutrition.Components;
using Content.Shared.Smoking;
using Content.Shared.Temperature;

namespace Content.Server.Nutrition.EntitySystems
{
    public sealed partial class SmokingSystem
    {
        private void InitializeCigars()
        {
            SubscribeLocalEvent<CigarComponent, ActivateInWorldEvent>(OnCigarActivatedEvent);
            SubscribeLocalEvent<CigarComponent, InteractUsingEvent>(OnCigarInteractUsingEvent);
            SubscribeLocalEvent<CigarComponent, SmokableSolutionEmptyEvent>(OnCigarSolutionEmptyEvent);
            SubscribeLocalEvent<CigarComponent, AfterInteractEvent>(OnCigarAfterInteract);
        }

        private void OnCigarActivatedEvent(Entity<CigarComponent> entity, ref ActivateInWorldEvent args)
        {
            if (args.Handled || !args.Complex)
                return;

            if (!EntityManager.TryGetComponent(entity, out SmokableComponent? smokable))
                return;

            if (smokable.State != SmokableState.Lit)
                return;

            SetSmokableState(entity, SmokableState.Burnt, smokable);
            args.Handled = true;
        }

        private void OnCigarInteractUsingEvent(Entity<CigarComponent> entity, ref InteractUsingEvent args)
        {
            if (args.Handled)
                return;

            if (!EntityManager.TryGetComponent(entity, out SmokableComponent? smokable))
                return;

            if (smokable.State != SmokableState.Unlit)
                return;

            var isHotEvent = new IsHotEvent();
            RaiseLocalEvent(args.Used, isHotEvent, false);

            if (!isHotEvent.IsHot)
                return;

            SetSmokableState(entity, SmokableState.Lit, smokable);
            args.Handled = true;
        }

        public void OnCigarAfterInteract(Entity<CigarComponent> entity, ref AfterInteractEvent args)
        {
            var targetEntity = args.Target;
            if (targetEntity == null ||
                !args.CanReach ||
                !EntityManager.TryGetComponent(entity, out SmokableComponent? smokable) ||
                smokable.State == SmokableState.Lit)
                return;

            var isHotEvent = new IsHotEvent();
            RaiseLocalEvent(targetEntity.Value, isHotEvent, true);

            if (!isHotEvent.IsHot)
                return;

            SetSmokableState(entity, SmokableState.Lit, smokable);
            args.Handled = true;
        }

        private void OnCigarSolutionEmptyEvent(Entity<CigarComponent> entity, ref SmokableSolutionEmptyEvent args)
        {
            SetSmokableState(entity, SmokableState.Burnt);
        }
    }
}