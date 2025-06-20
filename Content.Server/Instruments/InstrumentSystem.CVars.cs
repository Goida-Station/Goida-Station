// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.CCVar;

namespace Content.Server.Instruments;

public sealed partial class InstrumentSystem
{
    public int MaxMidiEventsPerSecond { get; private set; }
    public int MaxMidiEventsPerBatch { get; private set; }
    public int MaxMidiBatchesDropped { get; private set; }
    public int MaxMidiLaggedBatches { get; private set; }

    private void InitializeCVars()
    {
        Subs.CVar(_cfg, CCVars.MaxMidiEventsPerSecond, obj => MaxMidiEventsPerSecond = obj, true);
        Subs.CVar(_cfg, CCVars.MaxMidiEventsPerBatch, obj => MaxMidiEventsPerBatch = obj, true);
        Subs.CVar(_cfg, CCVars.MaxMidiBatchesDropped, obj => MaxMidiBatchesDropped = obj, true);
        Subs.CVar(_cfg, CCVars.MaxMidiLaggedBatches, obj => MaxMidiLaggedBatches = obj, true);
    }
}