// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Nuke;

/// <summary>
///     This generates a label for a nuclear bomb.
/// </summary>
/// <remarks>
///     This is a separate component because the fake nuclear bomb keg exists.
/// </remarks>
[RegisterComponent]
public sealed partial class NukeLabelComponent : Component
{
    [DataField] public LocId Prefix = "nuke-label-nanotrasen";
    [DataField] public int SerialLength = 65;
}