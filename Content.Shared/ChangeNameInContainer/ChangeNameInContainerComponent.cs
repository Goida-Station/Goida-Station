// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.ChangeNameInContainer;

/// <summary>
///     An entity with this component will get its name and verb chaned to the container it's inside of. E.g, if your a
///     pAI that has this component and are inside a lizard plushie, your name when talking will be "lizard plushie".
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(ChangeNameInContainerSystem))]
public sealed partial class ChangeVoiceInContainerComponent : Component
{
    /// <summary>
    ///     A whitelist of containers that will change the name.
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;
}