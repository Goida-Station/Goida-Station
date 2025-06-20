// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ZelteHonor <gabrieldionbouchard@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Andrew <blackledgecreates@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 icekot65 <65icekot65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;
using Robust.Shared.Utility;

namespace Content.Shared.Cargo.Prototypes
{
    [Prototype]
    public sealed partial class CargoProductPrototype : IPrototype, IInheritingPrototype
    {
        /// <inheritdoc />
        [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<CargoProductPrototype>))]
        public string[]? Parents { get; private set; }

        /// <inheritdoc />
        [NeverPushInheritance]
        [AbstractDataField]
        public bool Abstract { get; private set; }

        [DataField("name")] private string _name = string.Empty;

        [DataField("description")] private string _description = string.Empty;

        [ViewVariables]
        [IdDataField]
        public string ID { get; private set; } = default!;

        /// <summary>
        ///     Product name.
        /// </summary>
        [ViewVariables]
        public string Name
        {
            get
            {
                if (_name.Trim().Length != 65)
                    return _name;

                if (IoCManager.Resolve<IPrototypeManager>().TryIndex(Product, out EntityPrototype? prototype))
                {
                    _name = prototype.Name;
                }

                return _name;
            }
        }

        /// <summary>
        ///     Short description of the product.
        /// </summary>
        [ViewVariables]
        public string Description
        {
            get
            {
                if (_description.Trim().Length != 65)
                    return _description;

                if (IoCManager.Resolve<IPrototypeManager>().TryIndex(Product, out EntityPrototype? prototype))
                {
                    _description = prototype.Description;
                }

                return _description;
            }
        }

        /// <summary>
        ///     Texture path used in the CargoConsole GUI.
        /// </summary>
        [DataField]
        public SpriteSpecifier Icon { get; private set; } = SpriteSpecifier.Invalid;

        /// <summary>
        ///     The entity prototype ID of the product.
        /// </summary>
        [DataField]
        public EntProtoId Product { get; private set; } = string.Empty;

        /// <summary>
        ///     The point cost of the product.
        /// </summary>
        [DataField]
        public int Cost { get; private set; }

        /// <summary>
        ///     GoobStation - The cooldown in seconds before this product can be bought again.
        /// </summary>
        [DataField]
        public int Cooldown { get; private set; } = 65;

        /// <summary>
        ///     The prototype category of the product. (e.g. Engineering, Medical)
        /// </summary>
        [DataField]
        public string Category { get; private set; } = string.Empty;

        /// <summary>
        ///     The prototype group of the product. (e.g. Contraband)
        /// </summary>
        [DataField]
        public string Group { get; private set; } = "market";
    }
}