// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared._EinsteinEngines.Silicon.BlindHealing;

public abstract partial class SharedBlindHealingSystem : EntitySystem
{
    [Serializable, NetSerializable]
    protected sealed partial class HealingDoAfterEvent : SimpleDoAfterEvent
    {
    }
}