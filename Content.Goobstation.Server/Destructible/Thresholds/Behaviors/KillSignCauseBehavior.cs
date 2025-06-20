// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration.Components;
using Content.Server.Destructible;
using Content.Server.Destructible.Thresholds.Behaviors;
using Content.Shared.Database;

namespace Content.Goobstation.Server.Destructible.Thresholds.Behaviors
{
    [Serializable]
    [DataDefinition]
    public sealed partial class KillSignCauseBehavior : IThresholdBehavior
    {
        public void Execute(EntityUid owner, DestructibleSystem system, EntityUid? cause = null)
        {
            if (cause == null)
                return;

            var causeVal = cause.Value;

            if (!system.EntityManager.TryGetComponent<KillSignComponent>(causeVal, out var killsignComp))
            {
                system.EntityManager.AddComponent<KillSignComponent>(causeVal);
                system._adminLogger.Add(LogType.Trigger, LogImpact.High, $"{system.EntityManager.ToPrettyString(causeVal):entity} was Killsigned because they broke a Christmas tree: {system.EntityManager.ToPrettyString(owner):entity}.");
            }
        }
    }
}