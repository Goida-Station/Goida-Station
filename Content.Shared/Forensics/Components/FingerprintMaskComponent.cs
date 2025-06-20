// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ScarKy65 <65ScarKy65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Forensics.Components;

/// <summary>
/// This component stops the entity from leaving fingerprints,
/// usually so fibres can be left instead.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class FingerprintMaskComponent : Component;
