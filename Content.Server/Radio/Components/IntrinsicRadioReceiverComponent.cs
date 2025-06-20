// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Radio.Components;

/// <summary>
///     This component allows an entity to directly translate radio messages into chat messages. Note that this does not
///     automatically add an <see cref="ActiveRadioComponent"/>, which is required to receive radio messages on specific
///     channels.
/// </summary>
[RegisterComponent]
public sealed partial class IntrinsicRadioReceiverComponent : Component
{
}