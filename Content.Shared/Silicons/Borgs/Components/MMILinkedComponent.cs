// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Silicons.Borgs.Components;

/// <summary>
/// This is used for an entity that is linked to an MMI.
/// Mostly for receiving events.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedBorgSystem))]
[AutoGenerateComponentState]
public sealed partial class MMILinkedComponent : Component
{
    /// <summary>
    /// The MMI this entity is linked to.
    /// </summary>
    [DataField("linkedMMI"), AutoNetworkedField]
    public EntityUid? LinkedMMI;
}