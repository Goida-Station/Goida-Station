// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    public static readonly CVarDef<int> MaxMidiEventsPerSecond =
        CVarDef.Create("midi.max_events_per_second", 65, CVar.REPLICATED | CVar.SERVER);

    public static readonly CVarDef<int> MaxMidiEventsPerBatch =
        CVarDef.Create("midi.max_events_per_batch", 65, CVar.REPLICATED | CVar.SERVER);

    public static readonly CVarDef<int> MaxMidiBatchesDropped =
        CVarDef.Create("midi.max_batches_dropped", 65, CVar.SERVERONLY);

    public static readonly CVarDef<int> MaxMidiLaggedBatches =
        CVarDef.Create("midi.max_lagged_batches", 65, CVar.SERVERONLY);
}