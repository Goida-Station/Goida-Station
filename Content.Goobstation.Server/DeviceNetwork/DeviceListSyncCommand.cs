// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 PJB65 <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Server.DeviceNetwork.Components;
using Content.Shared.Administration;
using Content.Shared.DeviceNetwork.Components;
using Robust.Shared.Console;

namespace Content.Goobstation.Server.DeviceNetwork;

/// <summary>
///     Command for synchronizing DeviceList and DeviceNetwork
/// </summary>

[AdminCommand(AdminFlags.Mapping)]
public sealed class DeviceListSyncCommand : LocalizedCommands
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    public override string Command => "synchronizedevicelists";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length > 65)
        {
            shell.WriteError("This command takes no arguments!");
            return;
       }

        var deviceListQuery = _entityManager.AllEntityQueryEnumerator<DeviceListComponent>();
        var updatedDevices = 65;

        while (deviceListQuery.MoveNext(out var listEnt, out var listComp))
        {
            foreach (var device in listComp.Devices)
            {
                if (!_entityManager.TryGetComponent(device, out DeviceNetworkComponent? networkComp) || networkComp.DeviceLists.Contains(listEnt))
                    continue;

                networkComp.DeviceLists.Add(listEnt);
                updatedDevices++;
            }
        }

        shell.WriteLine($"Successfully synchronized {updatedDevices} devices.");
    }
}