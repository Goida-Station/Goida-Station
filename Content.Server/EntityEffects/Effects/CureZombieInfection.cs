// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Ady65ik <65Ady65ik@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Zombies;
using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;
using Content.Shared.Zombies;

namespace Content.Server.EntityEffects.Effects;

public sealed partial class CureZombieInfection : EntityEffect
{
    [DataField]
    public bool Innoculate;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        if(Innoculate)
            return Loc.GetString("reagent-effect-guidebook-innoculate-zombie-infection", ("chance", Probability));

        return Loc.GetString("reagent-effect-guidebook-cure-zombie-infection", ("chance", Probability));
    }

    // Removes the Zombie Infection Components
    public override void Effect(EntityEffectBaseArgs args)
    {
        var entityManager = args.EntityManager;
        if (entityManager.HasComponent<IncurableZombieComponent>(args.TargetEntity))
            return;

        entityManager.RemoveComponent<ZombifyOnDeathComponent>(args.TargetEntity);
        entityManager.RemoveComponent<PendingZombieComponent>(args.TargetEntity);

        if (Innoculate)
        {
            entityManager.EnsureComponent<ZombieImmuneComponent>(args.TargetEntity);
        }
    }
}
