// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Dragon;

[NetworkedComponent, EntityCategory("Spawner")]
public abstract partial class SharedDragonRiftComponent : Component
{
    [DataField("state")]
    public DragonRiftState State = DragonRiftState.Charging;
}

[Serializable, NetSerializable]
public enum DragonRiftState : byte
{
    Charging,
    AlmostFinished,
    Finished,
}