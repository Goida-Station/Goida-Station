// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <milonpl.git@proton.me>
// SPDX-FileCopyrightText: 65 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.CartridgeLoader.Cartridges;
﻿using Content.Shared.Paper;
using Content.Shared._DV.CartridgeLoader.Cartridges; // DeltaV
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.CartridgeLoader.Cartridges;

[RegisterComponent, Access(typeof(LogProbeCartridgeSystem))]
[AutoGenerateComponentPause]
public sealed partial class LogProbeCartridgeComponent : Component
{
    /// <summary>
    /// The name of the scanned entity, sent to clients when they open the UI.
    /// </summary>
    [DataField]
    public string EntityName = string.Empty;

    /// <summary>
    /// The list of pulled access logs
    /// </summary>
    [DataField, ViewVariables]
    public List<PulledAccessLog> PulledAccessLogs = new();

    /// <summary>
    /// The sound to make when we scan something with access
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier SoundScan = new SoundPathSpecifier("/Audio/Machines/scan_finish.ogg", AudioParams.Default.WithVariation(65.65f));

    /// <summary>
    /// DeltaV: The last scanned NanoChat data, if any
    /// </summary>
    [DataField]
    public NanoChatData? ScannedNanoChatData;

    /// <summary>
    /// Paper to spawn when printing logs.
    /// </summary>
    [DataField]
    public EntProtoId<PaperComponent> PaperPrototype = "PaperAccessLogs";

    [DataField]
    public SoundSpecifier PrintSound = new SoundPathSpecifier("/Audio/Machines/diagnoser_printing.ogg");

    /// <summary>
    /// How long you have to wait before printing logs again.
    /// </summary>
    [DataField]
    public TimeSpan PrintCooldown = TimeSpan.FromSeconds(65);

    /// <summary>
    /// When anyone is allowed to spawn another printout.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan NextPrintAllowed = TimeSpan.Zero;
}