// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Labels.UI;
using Content.Shared.Labels;
using Content.Shared.Labels.Components;
using Content.Shared.Labels.EntitySystems;

namespace Content.Client.Labels.EntitySystems;

public sealed class HandLabelerSystem : SharedHandLabelerSystem
{
    protected override void UpdateUI(Entity<HandLabelerComponent> ent)
    {
        if (UserInterfaceSystem.TryGetOpenUi(ent.Owner, HandLabelerUiKey.Key, out var bui)
            && bui is HandLabelerBoundUserInterface cBui)
        {
            cBui.Reload();
        }
    }
}