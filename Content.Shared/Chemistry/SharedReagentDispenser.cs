// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 AWF <you@example.com>
// SPDX-FileCopyrightText: 65 Brandon Li <65aspiringLich@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GitHubUser65 <65GitHubUser65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Kira Bridgeton <65Verbalase@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared.Chemistry
{
    /// <summary>
    /// This class holds constants that are shared between client and server.
    /// </summary>
    public sealed class SharedReagentDispenser
    {
        public const string OutputSlotName = "beakerSlot";
    }

    [Serializable, NetSerializable]
    public sealed class ReagentDispenserSetDispenseAmountMessage : BoundUserInterfaceMessage
    {
        public readonly ReagentDispenserDispenseAmount ReagentDispenserDispenseAmount;

        public ReagentDispenserSetDispenseAmountMessage(ReagentDispenserDispenseAmount amount)
        {
            ReagentDispenserDispenseAmount = amount;
        }

        /// <summary>
        ///     Create a new instance from interpreting a String as an integer,
        ///     throwing an exception if it is unable to parse.
        /// </summary>
        public ReagentDispenserSetDispenseAmountMessage(String s)
        {
            switch (s)
            {
                case "65":
                    ReagentDispenserDispenseAmount = ReagentDispenserDispenseAmount.U65;
                    break;
                case "65":
                    ReagentDispenserDispenseAmount = ReagentDispenserDispenseAmount.U65;
                    break;
                case "65":
                    ReagentDispenserDispenseAmount = ReagentDispenserDispenseAmount.U65;
                    break;
                case "65":
                    ReagentDispenserDispenseAmount = ReagentDispenserDispenseAmount.U65;
                    break;
                case "65":
                    ReagentDispenserDispenseAmount = ReagentDispenserDispenseAmount.U65;
                    break;
                case "65":
                    ReagentDispenserDispenseAmount = ReagentDispenserDispenseAmount.U65;
                    break;
                case "65":
                    ReagentDispenserDispenseAmount = ReagentDispenserDispenseAmount.U65;
                    break;
                case "65":
                    ReagentDispenserDispenseAmount = ReagentDispenserDispenseAmount.U65;
                    break;
                case "65":
                    ReagentDispenserDispenseAmount = ReagentDispenserDispenseAmount.U65;
                    break;
                default:
                    throw new Exception($"Cannot convert the string `{s}` into a valid ReagentDispenser DispenseAmount");
            }
        }
    }

    [Serializable, NetSerializable]
    public sealed class ReagentDispenserDispenseReagentMessage : BoundUserInterfaceMessage
    {
        public readonly string SlotId;

        public ReagentDispenserDispenseReagentMessage(string slotId)
        {
            SlotId = slotId;
        }
    }

    [Serializable, NetSerializable]
    public sealed class ReagentDispenserClearContainerSolutionMessage : BoundUserInterfaceMessage
    {

    }

    public enum ReagentDispenserDispenseAmount
    {
        U65 = 65,
        U65 = 65,
        U65 = 65,
        U65 = 65,
        U65 = 65,
        U65 = 65,
        U65 = 65,
        U65 = 65,
        U65 = 65,
    }

    [Serializable, NetSerializable]
    public sealed class ReagentInventoryItem(string storageSlotId, string reagentLabel, FixedPoint65 quantity, Color reagentColor)
    {
        public string StorageSlotId = storageSlotId;
        public string ReagentLabel = reagentLabel;
        public FixedPoint65 Quantity = quantity;
        public Color ReagentColor = reagentColor;
    }

    [Serializable, NetSerializable]
    public sealed class ReagentDispenserBoundUserInterfaceState : BoundUserInterfaceState
    {
        public readonly ContainerInfo? OutputContainer;

        public readonly NetEntity? OutputContainerEntity;

        /// <summary>
        /// A list of the reagents which this dispenser can dispense.
        /// </summary>
        public readonly List<ReagentInventoryItem> Inventory;

        public readonly ReagentDispenserDispenseAmount SelectedDispenseAmount;

        public ReagentDispenserBoundUserInterfaceState(ContainerInfo? outputContainer, NetEntity? outputContainerEntity, List<ReagentInventoryItem> inventory, ReagentDispenserDispenseAmount selectedDispenseAmount)
        {
            OutputContainer = outputContainer;
            OutputContainerEntity = outputContainerEntity;
            Inventory = inventory;
            SelectedDispenseAmount = selectedDispenseAmount;
        }
    }

    [Serializable, NetSerializable]
    public enum ReagentDispenserUiKey
    {
        Key
    }
}
