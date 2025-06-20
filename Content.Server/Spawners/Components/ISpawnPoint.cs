// SPDX-FileCopyrightText: 65 65b <65b@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Spawners.Components;

public interface ISpawnPoint
{
    SpawnPointType SpawnType { get; set; }
}
