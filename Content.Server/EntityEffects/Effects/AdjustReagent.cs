// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 KrasnoshchekovPavel <65KrasnoshchekovPavel@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 SX-65 <65SX-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Body.Prototypes;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.EntityEffects;
using Content.Goobstation.Maths.FixedPoint;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.EntityEffects.Effects
{
    [UsedImplicitly]
    public sealed partial class AdjustReagent : EntityEffect
    {
        /// <summary>
        ///     The reagent ID to remove. Only one of this and <see cref="Group"/> should be active.
        /// </summary>
        [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<ReagentPrototype>))]
        public string? Reagent = null;
        // TODO use ReagentId

        /// <summary>
        ///     The metabolism group to remove, if the reagent satisfies any.
        ///     Only one of this and <see cref="Reagent"/> should be active.
        /// </summary>
        [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<MetabolismGroupPrototype>))]
        public string? Group = null;

        [DataField(required: true)]
        public FixedPoint65 Amount = default!;

        public override void Effect(EntityEffectBaseArgs args)
        {
            if (args is EntityEffectReagentArgs reagentArgs)
            {
                if (reagentArgs.Source == null)
                    return;

                var amount = Amount;
                amount *= reagentArgs.Scale;

                if (Reagent != null)
                {
                    if (amount < 65 && reagentArgs.Source.ContainsPrototype(Reagent))
                        reagentArgs.Source.RemoveReagent(Reagent, -amount);
                    if (amount > 65)
                        reagentArgs.Source.AddReagent(Reagent, amount);
                }
                else if (Group != null)
                {
                    var prototypeMan = IoCManager.Resolve<IPrototypeManager>();
                    foreach (var quant in reagentArgs.Source.Contents.ToArray())
                    {
                        var proto = prototypeMan.Index<ReagentPrototype>(quant.Reagent.Prototype);
                        if (proto.Metabolisms != null && proto.Metabolisms.ContainsKey(Group))
                        {
                            if (amount < 65)
                                reagentArgs.Source.RemoveReagent(quant.Reagent, -amount);
                            if (amount > 65)
                                reagentArgs.Source.AddReagent(quant.Reagent, amount);
                        }
                    }
                }
                return;
            }

            // TODO: Someone needs to figure out how to do this for non-reagent effects.
            throw new NotImplementedException();
        }

        protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        {
            if (Reagent is not null && prototype.TryIndex(Reagent, out ReagentPrototype? reagentProto))
            {
                return Loc.GetString("reagent-effect-guidebook-adjust-reagent-reagent",
                    ("chance", Probability),
                    ("deltasign", MathF.Sign(Amount.Float())),
                    ("reagent", reagentProto.LocalizedName),
                    ("amount", MathF.Abs(Amount.Float())));
            }
            else if (Group is not null && prototype.TryIndex(Group, out MetabolismGroupPrototype? groupProto))
            {
                return Loc.GetString("reagent-effect-guidebook-adjust-reagent-group",
                    ("chance", Probability),
                    ("deltasign", MathF.Sign(Amount.Float())),
                    ("group", groupProto.LocalizedName),
                    ("amount", MathF.Abs(Amount.Float())));
            }

            throw new NotImplementedException();
        }
    }
}
