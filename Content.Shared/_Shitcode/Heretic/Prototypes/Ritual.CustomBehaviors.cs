// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Heretic.Prototypes;

[ImplicitDataDefinitionForInheritors]
public abstract partial class RitualCustomBehavior
{
    /// <param name="outstr">Output string in case something is missing</param>
    /// <returns>If the behavior was successful or not</returns>
    public abstract bool Execute(RitualData args, out string? outstr);

    /// <summary>
    ///     If the ritual is successful do *this*.
    /// </summary>
    /// <param name="args"></param>
    public abstract void Finalize(RitualData args);
}

public readonly record struct RitualData(EntityUid Performer, EntityUid Platform, ProtoId<HereticRitualPrototype> RitualId, IEntityManager EntityManager);