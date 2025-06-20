// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Sandbox
{
    public abstract class SharedSandboxSystem : EntitySystem
    {
        [Dependency] protected readonly IPrototypeManager PrototypeManager = default!;

        [Serializable, NetSerializable]
        protected sealed class MsgSandboxStatus : EntityEventArgs
        {
            public bool SandboxAllowed { get; set; }
        }

        [Serializable, NetSerializable]
        protected sealed class MsgSandboxRespawn : EntityEventArgs {}

        [Serializable, NetSerializable]
        protected sealed class MsgSandboxGiveAccess : EntityEventArgs {}

        [Serializable, NetSerializable]
        protected sealed class MsgSandboxGiveAghost : EntityEventArgs {}

        [Serializable, NetSerializable]
        protected sealed class MsgSandboxSuicide : EntityEventArgs {}
    }
}