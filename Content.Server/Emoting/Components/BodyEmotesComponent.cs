// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Emoting.Systems;
using Content.Shared.Chat.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Emoting.Components;

/// <summary>
///     Component required for entities to be able to do body emotions (clap, flip, etc).
/// </summary>
[RegisterComponent]
[Access(typeof(BodyEmotesSystem))]
public sealed partial class BodyEmotesComponent : Component
{
    /// <summary>
    ///     Emote sounds prototype id for body emotes.
    /// </summary>
    [DataField("soundsId", customTypeSerializer: typeof(PrototypeIdSerializer<EmoteSoundsPrototype>))]
    public string? SoundsId;

    /// <summary>
    ///     Loaded emote sounds prototype used for body emotes.
    /// </summary>
    public EmoteSoundsPrototype? Sounds;
}