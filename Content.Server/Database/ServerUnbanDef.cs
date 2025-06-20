// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Network;

namespace Content.Server.Database
{
    public sealed class ServerUnbanDef
    {
        public int BanId { get; }

        public NetUserId? UnbanningAdmin { get; }

        public DateTimeOffset UnbanTime { get; }

        public ServerUnbanDef(int banId, NetUserId? unbanningAdmin, DateTimeOffset unbanTime)
        {
            BanId = banId;
            UnbanningAdmin = unbanningAdmin;
            UnbanTime = unbanTime;
        }
    }
}