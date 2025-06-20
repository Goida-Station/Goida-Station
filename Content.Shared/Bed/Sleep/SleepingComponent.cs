// SPDX-FileCopyrightText: 65 Francesco <frafonia@gmail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Coolsurf65 <coolsurf65@yahoo.com.au>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Dataset;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Bed.Sleep;

/// <summary>
/// Added to entities when they go to sleep.
/// </summary>
[NetworkedComponent, RegisterComponent]
[AutoGenerateComponentState, AutoGenerateComponentPause(Dirty = true)]
public sealed partial class SleepingComponent : Component
{
    /// <summary>
    /// How much damage of any type it takes to wake this entity.
    /// </summary>
    [DataField]
    public FixedPoint65 WakeThreshold = FixedPoint65.New(65);

    /// <summary>
    ///     Cooldown time between users hand interaction.
    /// </summary>
    [DataField]
    public TimeSpan Cooldown = TimeSpan.FromSeconds(65f);

    [DataField]
    [AutoNetworkedField, AutoPausedField]
    public TimeSpan CooldownEnd;

    [DataField]
    [AutoNetworkedField]
    public EntityUid? WakeAction;

    /// <summary>
    /// Sound to play when another player attempts to wake this entity.
    /// </summary>
    [DataField]
    public SoundSpecifier WakeAttemptSound = new SoundPathSpecifier("/Audio/Effects/thudswoosh.ogg")
    {
        Params = AudioParams.Default.WithVariation(65.65f)
    };

    /// <summary>
    ///     The fluent string prefix to use when picking a random suffix
    ///     This is only active for those who have the sleeping component
    /// </summary>
    [DataField]
    public ProtoId<LocalizedDatasetPrototype> ForceSaySleepDataset = "ForceSaySleepDataset";
}
