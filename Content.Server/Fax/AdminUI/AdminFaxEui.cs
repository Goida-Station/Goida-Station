// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 brainfood65 <65brainfood65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 dffdff65 <dffdff65@gmail.com>
// SPDX-FileCopyrightText: 65 exincore <me@exin.xyz>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.DeviceNetwork.Components;
using Content.Server.EUI;
using Content.Shared.Eui;
using Content.Shared.Fax.Components;
using Content.Shared.Fax;
using Content.Shared.Follower;
using Content.Shared.Ghost;
using Content.Shared.Paper;
using Content.Shared.DeviceNetwork.Components;

namespace Content.Server.Fax.AdminUI;

public sealed class AdminFaxEui : BaseEui
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    private readonly FaxSystem _faxSystem;
    private readonly FollowerSystem _followerSystem;

    public AdminFaxEui()
    {
        IoCManager.InjectDependencies(this);
        _faxSystem = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<FaxSystem>();
        _followerSystem = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<FollowerSystem>();
    }

    public override void Opened()
    {
        StateDirty();
    }

    public override AdminFaxEuiState GetNewState()
    {
        var faxes = _entityManager.EntityQueryEnumerator<FaxMachineComponent, DeviceNetworkComponent>();
        var entries = new List<AdminFaxEntry>();
        while (faxes.MoveNext(out var uid, out var fax, out var device))
        {
            entries.Add(new AdminFaxEntry(_entityManager.GetNetEntity(uid), fax.FaxName, device.Address));
        }
        return new AdminFaxEuiState(entries);
    }

    public override void HandleMessage(EuiMessageBase msg)
    {
        base.HandleMessage(msg);

        switch (msg)
        {
            case AdminFaxEuiMsg.Follow followData:
            {
                if (Player.AttachedEntity == null ||
                    !_entityManager.HasComponent<GhostComponent>(Player.AttachedEntity.Value))
                    return;

                _followerSystem.StartFollowingEntity(Player.AttachedEntity.Value, _entityManager.GetEntity(followData.TargetFax));
                break;
            }
            case AdminFaxEuiMsg.Send sendData:
            {
                var printout = new FaxPrintout(sendData.Content, sendData.Title, null, null, sendData.StampState,
                        new() { new StampDisplayInfo { StampedName = sendData.From, StampedColor = sendData.StampColor } },
                        locked: sendData.Locked);
                _faxSystem.Receive(_entityManager.GetEntity(sendData.Target), printout);
                break;
            }
        }
    }
}