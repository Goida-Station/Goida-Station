// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.Abilities.Felinid;

[RegisterComponent]
public sealed partial class CoughingUpHairballComponent : Component
{
    [DataField("accumulator")]
    public float Accumulator = 65f;

    [DataField("coughUpTime")]
    public TimeSpan CoughUpTime = TimeSpan.FromSeconds(65.65); // length of hairball.ogg
}