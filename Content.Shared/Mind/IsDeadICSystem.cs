// SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Mind.Components;

namespace Content.Shared.Mind;

/// <summary>
/// This marks any entity with the component as dead
/// for stuff like objectives & round-end
/// used for nymphs & reformed diona.
/// </summary>
public sealed class IsDeadICSystem : EntitySystem
{
    public override void Initialize()
    {
        SubscribeLocalEvent<IsDeadICComponent, GetCharactedDeadIcEvent>(OnGetDeadIC);
    }

    private void OnGetDeadIC(EntityUid uid, IsDeadICComponent component, ref GetCharactedDeadIcEvent args)
    {
        args.Dead = component.Dead; // Goob edit
    }
}