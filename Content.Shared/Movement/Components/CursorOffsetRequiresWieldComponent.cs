// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Wieldable;
using Robust.Shared.GameStates;

namespace Content.Shared.Movement.Components;

/// <summary>
/// Indicates that this item requires wielding for the cursor offset effect to be active.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedWieldableSystem))]
public sealed partial class CursorOffsetRequiresWieldComponent : Component
{

}