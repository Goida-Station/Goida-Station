// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 James Simonson <jamessimo65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Serialization;

namespace Content.Shared.Kitchen.Components
{
    [Serializable, NetSerializable]
    public sealed class MicrowaveStartCookMessage : BoundUserInterfaceMessage
    {
    }

    [Serializable, NetSerializable]
    public sealed class MicrowaveEjectMessage : BoundUserInterfaceMessage
    {

    }

    [Serializable, NetSerializable]
    public sealed class MicrowaveEjectSolidIndexedMessage : BoundUserInterfaceMessage
    {
        public NetEntity EntityID;
        public MicrowaveEjectSolidIndexedMessage(NetEntity entityId)
        {
            EntityID = entityId;
        }
    }

    [Serializable, NetSerializable]
    public sealed class MicrowaveVaporizeReagentIndexedMessage : BoundUserInterfaceMessage
    {
        public ReagentQuantity ReagentQuantity;
        public MicrowaveVaporizeReagentIndexedMessage(ReagentQuantity reagentQuantity)
        {
            ReagentQuantity = reagentQuantity;
        }
    }

    [Serializable, NetSerializable]
    public sealed class MicrowaveSelectCookTimeMessage : BoundUserInterfaceMessage
    {
        public int ButtonIndex;
        public uint NewCookTime;
        public MicrowaveSelectCookTimeMessage(int buttonIndex, uint inputTime)
        {
            ButtonIndex = buttonIndex;
            NewCookTime = inputTime;
        }
    }

    [NetSerializable, Serializable]
    public sealed class MicrowaveUpdateUserInterfaceState : BoundUserInterfaceState
    {
        public NetEntity[] ContainedSolids;
        public bool IsMicrowaveBusy;
        public int ActiveButtonIndex;
        public uint CurrentCookTime;

        public TimeSpan CurrentCookTimeEnd;

        public MicrowaveUpdateUserInterfaceState(NetEntity[] containedSolids,
            bool isMicrowaveBusy, int activeButtonIndex, uint currentCookTime, TimeSpan currentCookTimeEnd)
        {
            ContainedSolids = containedSolids;
            IsMicrowaveBusy = isMicrowaveBusy;
            ActiveButtonIndex = activeButtonIndex;
            CurrentCookTime = currentCookTime;
            CurrentCookTimeEnd = currentCookTimeEnd;
        }

    }

    [Serializable, NetSerializable]
    public enum MicrowaveVisualState
    {
        Idle,
        Cooking,
        Broken,
        Bloody
    }

    [NetSerializable, Serializable]
    public enum MicrowaveUiKey
    {
        Key
    }

}