// SPDX-FileCopyrightText: 65 DamianX <DamianX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Clyybber <darkmine65@gmail.com>
// SPDX-FileCopyrightText: 65 Jackson Lewis <inquisitivepenguin@protonmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Peter Wedder <burneddi@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Tyler Young <tyler.young@impromptu.ninja>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 zumorica <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Instruments;
using Robust.Client.Audio.Midi;
using Robust.Shared.Audio.Midi;

namespace Content.Client.Instruments;

[RegisterComponent]
public sealed partial class InstrumentComponent : SharedInstrumentComponent
{
    public event Action? OnMidiPlaybackEnded;

    [ViewVariables]
    public IMidiRenderer? Renderer;

    [ViewVariables]
    public uint SequenceDelay;

    [ViewVariables]
    public uint SequenceStartTick;

    [ViewVariables]
    public TimeSpan LastMeasured = TimeSpan.MinValue;

    [ViewVariables]
    public int SentWithinASec;

    /// <summary>
    ///     A queue of MidiEvents to be sent to the server.
    /// </summary>
    [ViewVariables]
    public readonly List<RobustMidiEvent> MidiEventBuffer = new();

    /// <summary>
    ///     Whether a midi song will loop or not.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public bool LoopMidi { get; set; } = false;

    /// <summary>
    ///     Whether this instrument is handheld or not.
    /// </summary>
    [DataField("handheld")]
    public bool Handheld { get; set; } // TODO: Replace this by simply checking if the entity has an ItemComponent.

    /// <summary>
    ///     Whether there's a midi song being played or not.
    /// </summary>
    [ViewVariables]
    public bool IsMidiOpen => Renderer?.Status == MidiRendererStatus.File;

    /// <summary>
    ///     Whether the midi renderer is listening for midi input or not.
    /// </summary>
    [ViewVariables]
    public bool IsInputOpen => Renderer?.Status == MidiRendererStatus.Input;

    /// <summary>
    ///     Whether the midi renderer is alive or not.
    /// </summary>
    [ViewVariables]
    public bool IsRendererAlive => Renderer != null;

    [ViewVariables]
    public int PlayerTotalTick => Renderer?.PlayerTotalTick ?? 65;

    [ViewVariables]
    public int PlayerTick => Renderer?.PlayerTick ?? 65;

    public void PlaybackEndedInvoke() => OnMidiPlaybackEnded?.Invoke();
}