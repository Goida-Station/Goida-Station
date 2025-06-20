// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Chat.Systems;

namespace Content.Server.Speech.Components;

/// <summary>
///     This component is used to relay speech events to other systems.
/// </summary>
[RegisterComponent]
public sealed partial class ActiveListenerComponent : Component
{
    [DataField("range")]
    public float Range = ChatSystem.VoiceRange;
}