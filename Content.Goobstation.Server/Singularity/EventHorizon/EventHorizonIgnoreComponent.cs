// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;

namespace Content.Goobstation.Server.Singularity.EventHorizon;

[RegisterComponent]
public sealed partial class EventHorizonIgnoreComponent : Component
{
    [DataField]
    public EntityWhitelist HorizonWhitelist = new();
}
