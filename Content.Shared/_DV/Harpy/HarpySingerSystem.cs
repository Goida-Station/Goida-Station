// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Actions;

namespace Content.Shared._DV.Harpy
{
    public class HarpySingerSystem : EntitySystem
    {
        [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<HarpySingerComponent, ComponentStartup>(OnStartup);
            SubscribeLocalEvent<HarpySingerComponent, ComponentShutdown>(OnShutdown);
        }

        private void OnStartup(EntityUid uid, HarpySingerComponent component, ComponentStartup args)
        {
            _actionsSystem.AddAction(uid, ref component.MidiAction, component.MidiActionId);
        }

        private void OnShutdown(EntityUid uid, HarpySingerComponent component, ComponentShutdown args)
        {
            _actionsSystem.RemoveAction(uid, component.MidiAction);
        }
    }
}
