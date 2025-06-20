// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Debug <65DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Storage;

namespace Content.Server.Tools.Innate
{
    [RegisterComponent]
    public sealed partial class InnateToolComponent : Component
    {
        [DataField("tools")] public List<EntitySpawnEntry> Tools = new();
        public List<EntityUid> ToolUids = new();
        public List<string> ToSpawn = new();
    }
}