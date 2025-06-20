// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SpeltIncorrectyl <65SpeltIncorrectyl@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Paper;

/// <summary>
/// An entity with this component cannot write on paper.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class BlockWritingComponent : Component
{
    /// <summary>
    /// What message is displayed when the entity fails to write?
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public LocId FailWriteMessage = "paper-component-illiterate";
}