// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.GameTicking.Components;
using Robust.Shared.Prototypes;

namespace Content.Shared._Goobstation.Wizard.EventSpells;

public abstract class SharedGhostVisibilitySystem : EntitySystem
{
    protected static readonly EntProtoId GameRule = "GhostsVisible";

    public bool GhostsVisible()
    {
        var query = EntityQueryEnumerator<GhostsVisibleRuleComponent, ActiveGameRuleComponent, GameRuleComponent>();
        while (query.MoveNext(out _, out _, out _, out _))
        {
            return true;
        }

        return false;
    }
}