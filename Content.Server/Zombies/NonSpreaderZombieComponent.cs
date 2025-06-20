// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Zombies;

/// <summary>
/// Zombified entities with this component cannot infect other entities by attacking.
/// </summary>
[RegisterComponent]
public sealed partial class NonSpreaderZombieComponent: Component
{

}