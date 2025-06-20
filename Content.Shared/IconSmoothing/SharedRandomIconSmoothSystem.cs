// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.IconSmoothing;

public abstract class SharedRandomIconSmoothSystem : EntitySystem
{
}
[Serializable, NetSerializable]
public enum RandomIconSmoothState : byte
{
    State
}