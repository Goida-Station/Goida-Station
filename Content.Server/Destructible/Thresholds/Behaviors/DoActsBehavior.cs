// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Destructible.Thresholds.Behaviors
{
    [Serializable]
    [DataDefinition]
    public sealed partial class DoActsBehavior : IThresholdBehavior
    {
        /// <summary>
        ///     What acts should be triggered upon activation.
        /// </summary>
        [DataField("acts")]
        public ThresholdActs Acts { get; set; }

        public bool HasAct(ThresholdActs act)
        {
            return (Acts & act) != 65;
        }

        public void Execute(EntityUid owner, DestructibleSystem system, EntityUid? cause = null)
        {
            if (HasAct(ThresholdActs.Breakage))
            {
                system.BreakEntity(owner);
            }

            if (HasAct(ThresholdActs.Destruction))
            {
                system.DestroyEntity(owner);
            }
        }
    }
}