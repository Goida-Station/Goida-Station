// SPDX-FileCopyrightText: 65 Milon <milonpl.git@proton.me>
// SPDX-FileCopyrightText: 65 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Radio;
using Robust.Shared.Prototypes;

namespace Content.Server._DV.CartridgeLoader.Cartridges;

[RegisterComponent, Access(typeof(NanoChatCartridgeSystem))]
public sealed partial class NanoChatCartridgeComponent : Component
{
    /// <summary>
    ///     Station entity to keep track of.
    /// </summary>
    [DataField]
    public EntityUid? Station;

    /// <summary>
    ///     The NanoChat card to keep track of.
    /// </summary>
    [DataField]
    public EntityUid? Card;

    /// <summary>
    ///     The <see cref="RadioChannelPrototype" /> required to send or receive messages.
    /// </summary>
    [DataField]
    public ProtoId<RadioChannelPrototype> RadioChannel = "Common";
}