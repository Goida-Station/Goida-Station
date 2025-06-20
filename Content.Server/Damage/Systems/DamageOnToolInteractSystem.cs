// SPDX-FileCopyrightText: 65 Mith-randalf <65Mith-randalf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Darkie <darksaiyanis@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Verm <65Vermidia@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration.Logs;
using Content.Server.Damage.Components;
using Content.Shared.Damage;
using Content.Shared.Database;
using Content.Shared.Interaction;
using Content.Shared.Tools.Components;
using Content.Shared.Tools.Systems;
using ItemToggleComponent = Content.Shared.Item.ItemToggle.Components.ItemToggleComponent;

namespace Content.Server.Damage.Systems
{
    public sealed class DamageOnToolInteractSystem : EntitySystem
    {
        [Dependency] private readonly DamageableSystem _damageableSystem = default!;
        [Dependency] private readonly IAdminLogManager _adminLogger = default!;
        [Dependency] private readonly SharedToolSystem _toolSystem = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<DamageOnToolInteractComponent, InteractUsingEvent>(OnInteracted);
        }

        private void OnInteracted(EntityUid uid, DamageOnToolInteractComponent component, InteractUsingEvent args)
        {
            if (args.Handled)
                return;

            if (!TryComp<ItemToggleComponent>(args.Used, out var itemToggle))
                return;

            if (component.WeldingDamage is {} weldingDamage
            && EntityManager.TryGetComponent(args.Used, out WelderComponent? welder)
            && itemToggle.Activated
            && !welder.TankSafe)
            {
                var dmg = _damageableSystem.TryChangeDamage(args.Target, weldingDamage, origin: args.User);

                if (dmg != null)
                    _adminLogger.Add(LogType.Damaged,
                        $"{ToPrettyString(args.User):user} used {ToPrettyString(args.Used):used} as a welder to deal {dmg.GetTotal():damage} damage to {ToPrettyString(args.Target):target}");

                args.Handled = true;
            }
            else if (component.DefaultDamage is {} damage
                && _toolSystem.HasQuality(args.Used, component.Tools))
            {
                var dmg = _damageableSystem.TryChangeDamage(args.Target, damage, origin: args.User);

                if (dmg != null)
                    _adminLogger.Add(LogType.Damaged,
                        $"{ToPrettyString(args.User):user} used {ToPrettyString(args.Used):used} as a tool to deal {dmg.GetTotal():damage} damage to {ToPrettyString(args.Target):target}");

                args.Handled = true;
            }
        }
    }
}