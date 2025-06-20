// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Speech;
using Robust.Shared.Prototypes;

namespace Content.Server.VoiceMask;

/// <summary>
///     This component is for voice mask items! Adding this component to clothing will give the the voice mask UI
///     and allow the wearer to change their voice and verb at will.
/// </summary>
/// <remarks>
///     DO NOT use this if you do not want the interface.
///     The VoiceOverrideSystem is probably what your looking for (Or you might have to make something similar)!
/// </remarks>
[RegisterComponent]
public sealed partial class VoiceMaskComponent : Component
{
    /// <summary>
    ///     The name that will override an entities default name. If null, it will use the default override.
    /// </summary>
    [DataField]
    public string? VoiceMaskName = null;

    /// <summary>
    ///     The speech verb that will override an entities default one. If null, it will use the entities default verb.
    /// </summary>
    [DataField]
    public ProtoId<SpeechVerbPrototype>? VoiceMaskSpeechVerb;

    /// <summary>
    ///     The action that gets displayed when the voice mask is equipped.
    /// </summary>
    [DataField]
    public EntProtoId Action = "ActionChangeVoiceMask";

    /// <summary>
    ///     Reference to the action.
    /// </summary>
    [DataField]
    public EntityUid? ActionEntity;
    /// <summary>
    ///     if UI Action shud be added on equipt
    /// </summary>
    [DataField]
    public bool EnableAction = true; //Goobstation
}