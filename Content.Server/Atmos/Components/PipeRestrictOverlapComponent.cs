// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.EntitySystems;

namespace Content.Server.Atmos.Components;

/// <summary>
/// This is used for restricting anchoring pipes so that they do not overlap.
/// </summary>
[RegisterComponent, Access(typeof(PipeRestrictOverlapSystem))]
public sealed partial class PipeRestrictOverlapComponent : Component;