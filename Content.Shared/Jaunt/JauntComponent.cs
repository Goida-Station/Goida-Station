// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Jaunt;

/// <summary>
///     Used to control various aspects of a Jaunt.
///     Can be used in place of giving a jaunt-action directly.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class JauntComponent : Component
{
    /// <summary>
    ///     Which Jaunt Action the component should grant.
    /// </summary>
    [DataField]
    public EntProtoId JauntAction = "ActionPolymorphJaunt";

    /// <summary>
    ///     The jaunt action itself.
    /// </summary>
    public EntityUid? Action;

    // TODO: Enter & Exit Times and Whitelist when Actions are reworked and can support it
    // TODO: Cooldown pausing when Actions can support it
}