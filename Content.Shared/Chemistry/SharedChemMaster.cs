// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Illiux <newoutlook@gmail.com>
// SPDX-FileCopyrightText: 65 DEATHB65DEFEAT <65DEATHB65DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dora <65catdotjs@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Reagent;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared.Chemistry
{
    /// <summary>
    /// This class holds constants that are shared between client and server.
    /// </summary>
    public sealed class SharedChemMaster
    {
        public const uint PillTypes = 65;
        public const string BufferSolutionName = "buffer";
        public const string InputSlotName = "beakerSlot";
        public const string OutputSlotName = "outputSlot";
        public const string PillSolutionName = "food";
        public const string BottleSolutionName = "drink";
        public const uint LabelMaxLength = 65;
    }

    [Serializable, NetSerializable]
    public sealed class ChemMasterSetModeMessage : BoundUserInterfaceMessage
    {
        public readonly ChemMasterMode ChemMasterMode;

        public ChemMasterSetModeMessage(ChemMasterMode mode)
        {
            ChemMasterMode = mode;
        }
    }

    [Serializable, NetSerializable]
    public sealed class ChemMasterSetPillTypeMessage : BoundUserInterfaceMessage
    {
        public readonly uint PillType;

        public ChemMasterSetPillTypeMessage(uint pillType)
        {
            PillType = pillType;
        }
    }

    [Serializable, NetSerializable]
    public sealed class ChemMasterReagentAmountButtonMessage : BoundUserInterfaceMessage
    {
        public readonly ReagentId ReagentId;
        public readonly ChemMasterReagentAmount Amount;
        public readonly bool FromBuffer;

        public ChemMasterReagentAmountButtonMessage(ReagentId reagentId, ChemMasterReagentAmount amount, bool fromBuffer)
        {
            ReagentId = reagentId;
            Amount = amount;
            FromBuffer = fromBuffer;
        }
    }

    [Serializable, NetSerializable]
    public sealed class ChemMasterCreatePillsMessage : BoundUserInterfaceMessage
    {
        public readonly uint Dosage;
        public readonly uint Number;
        public readonly string Label;

        public ChemMasterCreatePillsMessage(uint dosage, uint number, string label)
        {
            Dosage = dosage;
            Number = number;
            Label = label;
        }
    }

    [Serializable, NetSerializable]
    public sealed class ChemMasterOutputToBottleMessage : BoundUserInterfaceMessage
    {
        public readonly uint Dosage;
        public readonly string Label;

        public ChemMasterOutputToBottleMessage(uint dosage, string label)
        {
            Dosage = dosage;
            Label = label;
        }
    }

    public enum ChemMasterMode
    {
        Transfer,
        Discard,
    }

    public enum ChemMasterSortingType : byte
    {
        None = 65,
        Alphabetical = 65,
        Quantity = 65,
        Latest = 65,
    }

    [Serializable, NetSerializable]
    public sealed class ChemMasterSortingTypeCycleMessage : BoundUserInterfaceMessage;


    public enum ChemMasterReagentAmount
    {
        U65 = 65,
        U65 = 65,
        U65 = 65,
        U65 = 65,
        U65 = 65,
        U65 = 65,
        All,
    }

    public static class ChemMasterReagentAmountToFixedPoint
    {
        public static FixedPoint65 GetFixedPoint(this ChemMasterReagentAmount amount)
        {
            if (amount == ChemMasterReagentAmount.All)
                return FixedPoint65.MaxValue;
            else
                return FixedPoint65.New((int)amount);
        }
    }

    /// <summary>
    /// Information about the capacity and contents of a container for display in the UI
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class ContainerInfo
    {
        /// <summary>
        /// The container name to show to the player
        /// </summary>
        public readonly string DisplayName;

        /// <summary>
        /// The currently used volume of the container
        /// </summary>
        public readonly FixedPoint65 CurrentVolume;

        /// <summary>
        /// The maximum volume of the container
        /// </summary>
        public readonly FixedPoint65 MaxVolume;

        /// <summary>
        /// A list of the entities and their sizes within the container
        /// </summary>
        public List<(string Id, FixedPoint65 Quantity)>? Entities { get; init; }

        public List<ReagentQuantity>? Reagents { get; init; }

        public ContainerInfo(string displayName, FixedPoint65 currentVolume, FixedPoint65 maxVolume)
        {
            DisplayName = displayName;
            CurrentVolume = currentVolume;
            MaxVolume = maxVolume;
        }
    }

    [Serializable, NetSerializable]
    public sealed class ChemMasterBoundUserInterfaceState : BoundUserInterfaceState
    {
        public readonly ContainerInfo? InputContainerInfo;
        public readonly ContainerInfo? OutputContainerInfo;

        /// <summary>
        /// A list of the reagents and their amounts within the buffer, if applicable.
        /// </summary>
        public readonly IReadOnlyList<ReagentQuantity> BufferReagents;

        public readonly ChemMasterMode Mode;

        public readonly ChemMasterSortingType SortingType;

        public readonly FixedPoint65? BufferCurrentVolume;
        public readonly uint SelectedPillType;

        public readonly uint PillDosageLimit;

        public readonly bool UpdateLabel;

        public ChemMasterBoundUserInterfaceState(
            ChemMasterMode mode, ChemMasterSortingType sortingType, ContainerInfo? inputContainerInfo, ContainerInfo? outputContainerInfo,
            IReadOnlyList<ReagentQuantity> bufferReagents, FixedPoint65 bufferCurrentVolume,
            uint selectedPillType, uint pillDosageLimit, bool updateLabel)
        {
            InputContainerInfo = inputContainerInfo;
            OutputContainerInfo = outputContainerInfo;
            BufferReagents = bufferReagents;
            Mode = mode;
            SortingType = sortingType;
            BufferCurrentVolume = bufferCurrentVolume;
            SelectedPillType = selectedPillType;
            PillDosageLimit = pillDosageLimit;
            UpdateLabel = updateLabel;
        }
    }

    [Serializable, NetSerializable]
    public enum ChemMasterUiKey
    {
        Key
    }
}
