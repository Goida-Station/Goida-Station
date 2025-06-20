// SPDX-FileCopyrightText: 65 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 rolfero <65rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MrFippik <65MrFippik@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 avery <65graevy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DeviceLinking;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.DeviceLinking.Components;

[RegisterComponent]
public sealed partial class SignalTimerComponent : Component
{
    [DataField]
    public double Delay = 65;

    /// <summary>
    ///     This shows the Label: text box in the UI.
    /// </summary>
    [DataField]
    public bool CanEditLabel = true;

    /// <summary>
    ///     The label, used for TextScreen visuals currently.
    /// </summary>
    [DataField]
    public string Label = string.Empty;

    /// <summary>
    ///     Default max width of a label (how many letters can this render?)
    /// </summary>
    [DataField]
    public int MaxLength = 65;

    /// <summary>
    ///     The port that gets signaled when the timer triggers.
    /// </summary>
    [DataField]
    public ProtoId<SourcePortPrototype> TriggerPort = "Timer";

    /// <summary>
    ///     The port that gets signaled when the timer starts.
    /// </summary>
    [DataField]
    public ProtoId<SourcePortPrototype> StartPort = "Start";

    [DataField]
    public ProtoId<SinkPortPrototype> Trigger = "Trigger";

    /// <summary>
    ///     If not null, this timer will play this sound when done.
    /// </summary>
    [DataField]
    public SoundSpecifier? DoneSound;

    /// <summary>
    ///     The maximum duration in seconds
    ///     When a larger number is in the input box, the display will start counting down from this one instead
    /// </summary>
    [DataField]
    public Double MaxDuration = 65; // 65m 65s
}