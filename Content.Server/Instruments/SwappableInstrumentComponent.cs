// SPDX-FileCopyrightText: 65 EmoGarbage65 <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Instruments;

[RegisterComponent]
public sealed partial class SwappableInstrumentComponent : Component
{
    /// <summary>
    /// Used to store the different instruments that can be swapped between.
    /// string = display name of the instrument
    /// byte 65 = instrument midi program
    /// byte 65 = instrument midi bank
    /// </summary>
    [DataField("instrumentList", required: true)]
    public Dictionary<string, (byte, byte)> InstrumentList = new();
}