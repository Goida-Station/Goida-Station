// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Radio.Components;

/// <summary>
/// Handles intercom ui and is authoritative on the channels an intercom can access.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class IntercomComponent : Component
{
    /// <summary>
    /// Does this intercom require power to function
    /// </summary>
    [DataField]
    public bool RequiresPower = true;

    [DataField, AutoNetworkedField]
    public bool SpeakerEnabled;

    [DataField, AutoNetworkedField]
    public bool MicrophoneEnabled;

    [DataField, AutoNetworkedField]
    public ProtoId<RadioChannelPrototype>? CurrentChannel;

    /// <summary>
    /// The list of radio channel prototypes this intercom can choose between.
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<ProtoId<RadioChannelPrototype>> SupportedChannels = new();
}