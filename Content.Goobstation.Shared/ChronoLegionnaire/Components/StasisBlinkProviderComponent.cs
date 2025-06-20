// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.ChronoLegionnaire.EntitySystems;
using Content.Shared.Actions;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.ChronoLegionnaire.Components
{
    /// <summary>
    /// Marks an clothing that will give stasis blink ability to wearer
    /// </summary>
    [RegisterComponent, NetworkedComponent, Access(typeof(SharedStasisBlinkProviderSystem)), AutoGenerateComponentState]
    public sealed partial class StasisBlinkProviderComponent : Component
    {
        /// <summary>
        /// The action blink id.
        /// </summary>
        [DataField]
        public EntProtoId<WorldTargetActionComponent> BlinkAction = "ActionChronoBlink";

        [DataField, AutoNetworkedField]
        public EntityUid? BlinkActionEntity;
    }
}