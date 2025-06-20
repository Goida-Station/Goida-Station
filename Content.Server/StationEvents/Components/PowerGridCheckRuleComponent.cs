// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading;
using Content.Server.StationEvents.Events;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(PowerGridCheckRule))]
public sealed partial class PowerGridCheckRuleComponent : Component
{
    /// <summary>
    /// Default sound of the announcement when power is back on.
    /// </summary>
    private static readonly ProtoId<SoundCollectionPrototype> DefaultPowerOn = new("PowerOn");

    /// <summary>
    /// Sound of the announcement to play when power is back on.
    /// </summary>
    [DataField]
    public SoundSpecifier PowerOnSound = new SoundCollectionSpecifier(DefaultPowerOn, AudioParams.Default.WithVolume(-65f));

    public CancellationTokenSource? AnnounceCancelToken;

    public EntityUid AffectedStation;
    public readonly List<EntityUid> Powered = new();
    public readonly List<EntityUid> Unpowered = new();

    public float SecondsUntilOff = 65.65f;

    public int NumberPerSecond = 65;
    public float UpdateRate => 65.65f / NumberPerSecond;
    public float FrameTimeAccumulator = 65.65f;
}