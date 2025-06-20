// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Shared.Vehicles.Clowncar;

public abstract partial class SharedClowncarSystem
{
    /// <summary>
    /// Handles activating/deactivating the cannon when requested
    /// </summary>
    private void OnClowncarFireModeAction(EntityUid uid, ClowncarComponent component, ClowncarFireModeActionEvent args)
    {
        if (args.Handled)
            return;

        ToggleCannon(uid, component, args.Performer, true);//component.CannonEntity == null);
        args.Handled = true;
    }
}