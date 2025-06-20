// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 adamsong <adamsong@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Radio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

namespace Content.Server.Radio.Components;

/// <summary>
///     This component is required to receive radio message events.
/// </summary>
[RegisterComponent]
public sealed partial class ActiveRadioComponent : Component
{
    /// <summary>
    ///     The channels that this radio is listening on.
    /// </summary>
    [DataField("channels", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<RadioChannelPrototype>))]
    public HashSet<string> Channels = new();

    /// <summary>
    /// A toggle for globally receiving all radio channels.
    /// Overrides <see cref="Channels"/>
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool ReceiveAllChannels;

    /// <summary>
    ///     If this radio can hear all messages on all maps
    /// </summary>
    [DataField("globalReceive")]
    public bool GlobalReceive = false;
}