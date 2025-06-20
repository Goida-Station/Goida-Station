// SPDX-FileCopyrightText: 65 Psychpsyo <65Psychpsyo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Psychpsyo <psychpsyo@gmail.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Chemistry.Components;

/// <summary>
///     Represents a container that also contains a solution.
///     This means that reactive entities react when inserted into the container.
/// </summary>
[RegisterComponent]
public sealed partial class ReactiveContainerComponent : Component
{
    /// <summary>
    ///     The container that holds the solution.
    /// </summary>
    [DataField(required: true)]
    public string Container = default!;

    /// <summary>
    ///     The solution in the container.
    /// </summary>
    [DataField(required: true)]
    public string Solution = default!;
}