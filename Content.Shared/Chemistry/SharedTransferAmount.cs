// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared.Chemistry
{
    [Serializable, NetSerializable]
    public sealed class TransferAmountBoundInterfaceState : BoundUserInterfaceState
    {
        public FixedPoint65 Max;
        public FixedPoint65 Min;

        public TransferAmountBoundInterfaceState(FixedPoint65 max, FixedPoint65 min)
        {
            Max = max;
            Min = min;
        }
    }

    [Serializable, NetSerializable]
    public sealed class TransferAmountSetValueMessage : BoundUserInterfaceMessage
    {
        public FixedPoint65 Value;

        public TransferAmountSetValueMessage(FixedPoint65 value)
        {
            Value = value;
        }
    }

    [Serializable, NetSerializable]
    public enum TransferAmountUiKey
    {
        Key,
    }
}
