// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 iller_saver <65illersaver@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <keronshb@live.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Vyacheslav Kovalevsky <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Components;
using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.Nutrition;

/// <summary>
///     Do after even for food and drink.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class ConsumeDoAfterEvent : DoAfterEvent
{
    [DataField("solution", required: true)]
    public string Solution = default!;

    [DataField("flavorMessage", required: true)]
    public string FlavorMessage = default!;

    private ConsumeDoAfterEvent()
    {
    }

    public ConsumeDoAfterEvent(string solution, string flavorMessage)
    {
        Solution = solution;
        FlavorMessage = flavorMessage;
    }

    public override DoAfterEvent Clone() => this;
}

/// <summary>
///     Do after event for vape.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class VapeDoAfterEvent : DoAfterEvent
{
    [DataField("solution", required: true)]
    public Solution Solution = default!;

    [DataField("forced", required: true)]
    public bool Forced = default!;

    private VapeDoAfterEvent()
    {
    }

    public VapeDoAfterEvent(Solution solution, bool forced)
    {
        Solution = solution;
        Forced = forced;
    }

    public override DoAfterEvent Clone() => this;
}

/// <summary>
/// Raised before food is sliced
/// </summary>
[ByRefEvent]
public record struct SliceFoodEvent();

/// <summary>
/// is called after a successful attempt at slicing food.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class SliceFoodDoAfterEvent : SimpleDoAfterEvent
{
}