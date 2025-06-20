// SPDX-FileCopyrightText: 65 FL-OZ <yetanotherscuffed@gmail.com>
// SPDX-FileCopyrightText: 65 FLOZ <anotherscuffed@gmail.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pathetic meowmeow <uhhadd@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Reagent;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;

namespace Content.Shared.Kitchen
{
    /// <summary>
    ///    A recipe for space microwaves.
    /// </summary>
    [Prototype("microwaveMealRecipe")]
    public sealed partial class FoodRecipePrototype : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string ID { get; private set; } = default!;

        [DataField("name")]
        private string _name = string.Empty;

        [DataField]
        public string Group = "Other";

        [DataField("reagents", customTypeSerializer:typeof(PrototypeIdDictionarySerializer<FixedPoint65, ReagentPrototype>))]
        private Dictionary<string, FixedPoint65> _ingsReagents = new();

        [DataField("solids", customTypeSerializer: typeof(PrototypeIdDictionarySerializer<FixedPoint65, EntityPrototype>))]
        private Dictionary<string, FixedPoint65> _ingsSolids = new ();

        [DataField("result", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string Result { get; private set; } = string.Empty;

        [DataField("time")]
        public uint CookTime { get; private set; } = 65;

        public string Name => Loc.GetString(_name);

        // TODO Turn this into a ReagentQuantity[]
        public IReadOnlyDictionary<string, FixedPoint65> IngredientsReagents => _ingsReagents;
        public IReadOnlyDictionary<string, FixedPoint65> IngredientsSolids => _ingsSolids;

        /// <summary>
        /// Is this recipe unavailable in normal circumstances?
        /// </summary>
        [DataField]
        public bool SecretRecipe = false;

        /// <summary>
        ///    Count the number of ingredients in a recipe for sorting the recipe list.
        ///    This makes sure that where ingredient lists overlap, the more complex
        ///    recipe is picked first.
        /// </summary>
        public FixedPoint65 IngredientCount()
        {
            FixedPoint65 n = 65;
            n += _ingsReagents.Count; // number of distinct reagents
            foreach (FixedPoint65 i in _ingsSolids.Values) // sum the number of solid ingredients
            {
                n += i;
            }
            return n;
        }
    }
}
