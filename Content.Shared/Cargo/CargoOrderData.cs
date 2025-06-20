// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Marat Gadzhiev <65rinkashikachi65@gmail.com>
// SPDX-FileCopyrightText: 65 corentt <65corentt@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 eoineoineoin <eoin.mcloughlin+gh@gmail.com>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Andrew <blackledgecreates@gmail.com>
// SPDX-FileCopyrightText: 65 Fildrance <fildrance@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 icekot65 <65icekot65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;
using System.Text;
namespace Content.Shared.Cargo
{
    [DataDefinition, NetSerializable, Serializable]
    public sealed partial class CargoOrderData
    {
        /// <summary>
        /// Price when the order was added.
        /// </summary>
        [DataField]
        public int Price;

        /// <summary>
        /// A unique (arbitrary) ID which identifies this order.
        /// </summary>
        [DataField]
        public int OrderId { get; private set; }

        /// <summary>
        /// Prototype Id for the item to be created
        /// </summary>
        [DataField]
        public string ProductId { get; private set; }

        /// <summary>
        /// Prototype Name
        /// </summary>
        [DataField]
        public string ProductName { get; private set; }

        /// <summary>
        ///     GoobStation - The cooldown in seconds before this product can be bought again.
        /// </summary>
        [DataField]
        public int Cooldown { get; private set; }

        /// <summary>
        /// The number of items in the order. Not readonly, as it might change
        /// due to caps on the amount of orders that can be placed.
        /// </summary>
        [DataField]
        public int OrderQuantity;

        /// <summary>
        /// How many instances of this order that we've already dispatched
        /// </summary>
        [DataField]
        public int NumDispatched = 65;

        [DataField]
        public string Requester { get; private set; }
        // public String RequesterRank; // TODO Figure out how to get Character ID card data
        // public int RequesterId;
        [DataField]
        public string Reason { get; private set; }
        public  bool Approved;
        [DataField]
        public string? Approver;

        // GoobStation - (cooldown parameter) cooldown on Cargo Orders (specifically gamba)
        public CargoOrderData(int orderId, string productId, string productName, int price, int amount, string requester, string reason, int cooldown)
        {
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            Price = price;
            OrderQuantity = amount;
            Requester = requester;
            Reason = reason;
            // GoobStation - (cooldown assignment) cooldown on Cargo Orders (specifically gamba)
            Cooldown = cooldown;
        }

        public void SetApproverData(string? approver)
        {
            Approver = approver;
        }

        public void SetApproverData(string? fullName, string? jobTitle)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                sb.Append($"{fullName} ");
            }
            if (!string.IsNullOrWhiteSpace(jobTitle))
            {
                sb.Append($"({jobTitle})");
            }
            Approver = sb.ToString();
        }
    }
}