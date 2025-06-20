// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Map;

namespace Content.Goobstation.Common.Movement;

[ByRefEvent]
public readonly record struct MoverControllerCantMoveEvent;

[ByRefEvent]
public readonly record struct MoverControllerGetTileEvent(ITileDefinition? Tile);
