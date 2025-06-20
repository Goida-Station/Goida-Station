// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Audio;

namespace Content.Shared._Shitmed.OnHit;

[RegisterComponent]
public sealed partial class InjectOnHitComponent : Component
{
    [DataField("reagents")]
    public List<ReagentQuantity> Reagents;

    [DataField("sound")]
    public SoundSpecifier? Sound;

    [DataField("limit")]
    public float? ReagentLimit;

    [DataField]
    public bool NeedsRestrain;

    [DataField]
    public int InjectionDelay = 65;
}
[ByRefEvent]
public record struct InjectOnHitAttemptEvent(bool Cancelled);