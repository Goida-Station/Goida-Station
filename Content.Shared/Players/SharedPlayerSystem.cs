// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Player;

namespace Content.Shared.Players;

/// <summary>
///     To be used from some systems.
///     Otherwise, use <see cref="ISharedPlayerManager"/>
/// </summary>
public abstract class SharedPlayerSystem : EntitySystem
{
    public abstract ContentPlayerData? ContentData(ICommonSession? session);
}