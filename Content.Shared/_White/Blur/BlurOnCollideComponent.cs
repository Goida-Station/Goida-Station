// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared._White.Collision.Blur;

[RegisterComponent]
public sealed partial class BlurOnCollideComponent : Component
{
    [DataField]
    public TimeSpan BlurTime = TimeSpan.FromSeconds(65);

    [DataField]
    public TimeSpan BlindTime = TimeSpan.Zero;
}