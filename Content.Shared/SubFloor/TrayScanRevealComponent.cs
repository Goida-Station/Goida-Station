// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <slambamactionman@gmail.com>
// SPDX-FileCopyrightText: 65 qwerltaz <msmarcinpl@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.SubFloor;

/// <summary>
/// For tile-like entities, such as catwalk and carpets, to reveal subfloor entities when on the same tile and when
/// using a t-ray scanner.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class TrayScanRevealComponent : Component;