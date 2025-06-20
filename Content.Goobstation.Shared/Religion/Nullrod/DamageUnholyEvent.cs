// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Shared.Religion.Nullrod;

[ByRefEvent]
public sealed class DamageUnholyEvent(EntityUid target, EntityUid? origin = null) : EntityEventArgs
{
    public readonly EntityUid Target = target;

    public bool ShouldTakeHoly = false;

    public EntityUid? Origin = origin;
}
