// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Instruments.UI;

[Serializable, NetSerializable]
public sealed class InstrumentBandRequestBuiMessage : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class InstrumentBandResponseBuiMessage : BoundUserInterfaceMessage
{
    public (NetEntity, string)[] Nearby { get; set; }

    public InstrumentBandResponseBuiMessage((NetEntity, string)[] nearby)
    {
        Nearby = nearby;
    }
}