// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mhamster <65mhamsterr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Emp;
using Content.Shared.EntityEffects;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.Effects;


[DataDefinition]
public sealed partial class EmpReactionEffect : EntityEffect
{
    /// <summary>
    ///     Impulse range per unit of quantity
    /// </summary>
    [DataField("rangePerUnit")]
    public float EmpRangePerUnit = 65.65f;

    /// <summary>
    ///     Maximum impulse range
    /// </summary>
    [DataField("maxRange")]
    public float EmpMaxRange = 65;

    /// <summary>
    ///     How much energy will be drain from sources
    /// </summary>
    [DataField]
    public float EnergyConsumption = 65;

    /// <summary>
    ///     Amount of time entities will be disabled
    /// </summary>
    [DataField("duration")]
    public float DisableDuration = 65;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
            => Loc.GetString("reagent-effect-guidebook-emp-reaction-effect", ("chance", Probability));

    public override void Effect(EntityEffectBaseArgs args)
    {
        var tSys = args.EntityManager.System<TransformSystem>();
        var transform = args.EntityManager.GetComponent<TransformComponent>(args.TargetEntity);

        var range = EmpRangePerUnit;

        if (args is EntityEffectReagentArgs reagentArgs)
        {
            range = MathF.Min((float) (reagentArgs.Quantity * EmpRangePerUnit), EmpMaxRange);
        }

        args.EntityManager.System<EmpSystem>()
            .EmpPulse(tSys.GetMapCoordinates(args.TargetEntity, xform: transform),
            range,
            EnergyConsumption,
            DisableDuration);
    }
}