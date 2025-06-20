// SPDX-FileCopyrightText: 65 Clyybber <darkmine65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Administration;
using Content.Shared.Administration.Events;
using Robust.Shared.Network;

namespace Content.Client.Administration.Systems
{
    public sealed partial class AdminSystem : EntitySystem
    {
        public event Action<List<PlayerInfo>>? PlayerListChanged;

        private Dictionary<NetUserId, PlayerInfo>? _playerList;
        public IReadOnlyList<PlayerInfo> PlayerList
        {
            get
            {
                if (_playerList != null) return _playerList.Values.ToList();

                return new List<PlayerInfo>();
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            InitializeOverlay();
            SubscribeNetworkEvent<FullPlayerListEvent>(OnPlayerListChanged);
            SubscribeNetworkEvent<PlayerInfoChangedEvent>(OnPlayerInfoChanged);
        }

        public override void Shutdown()
        {
            base.Shutdown();
            ShutdownOverlay();
        }

        private void OnPlayerInfoChanged(PlayerInfoChangedEvent ev)
        {
            if(ev.PlayerInfo == null) return;

            if (_playerList == null) _playerList = new();

            _playerList[ev.PlayerInfo.SessionId] = ev.PlayerInfo;
            PlayerListChanged?.Invoke(_playerList.Values.ToList());
        }

        private void OnPlayerListChanged(FullPlayerListEvent msg)
        {
            _playerList = msg.PlayersInfo.ToDictionary(x => x.SessionId, x => x);
            PlayerListChanged?.Invoke(msg.PlayersInfo);
        }
    }
}