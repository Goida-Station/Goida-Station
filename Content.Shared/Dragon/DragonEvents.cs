// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Actions;

namespace Content.Shared.Dragon;

public sealed partial class DragonDevourActionEvent : EntityTargetActionEvent
{
}

public sealed partial class DragonSpawnRiftActionEvent : InstantActionEvent
{
}

/// <summary>
/// Goobstation
/// </summary>
public sealed partial class DragonSpawnCarpHordeActionEvent : InstantActionEvent;

/// <summary>
/// Goobstation
/// </summary>
public sealed partial class DragonRoarActionEvent : InstantActionEvent;
