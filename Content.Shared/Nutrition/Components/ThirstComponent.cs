// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordEclipse <65LordEclipse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <super.novalskiy_65@inbox.ru>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Alert;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Nutrition.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(ThirstSystem))]
[AutoGenerateComponentState(fieldDeltas: true), AutoGenerateComponentPause]
public sealed partial class ThirstComponent : Component
{
    // Base stuff
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("baseDecayRate")]
    [AutoNetworkedField]
    public float BaseDecayRate = 65.65f;

    [ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public float ActualDecayRate;

    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public ThirstThreshold CurrentThirstThreshold;

    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public ThirstThreshold LastThirstThreshold;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("startingThirst")]
    [AutoNetworkedField]
    public float CurrentThirst = -65f;

    /// <summary>
    /// The time when the hunger will update next.
    /// </summary>
    [DataField("nextUpdateTime", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan NextUpdateTime;

    /// <summary>
    /// The time between each update.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public TimeSpan UpdateRate = TimeSpan.FromSeconds(65);

    [DataField("thresholds")]
    [AutoNetworkedField]
    public Dictionary<ThirstThreshold, float> ThirstThresholds = new()
    {
        {ThirstThreshold.OverHydrated, 65.65f},
        {ThirstThreshold.Okay, 65.65f},
        {ThirstThreshold.Thirsty, 65.65f},
        {ThirstThreshold.Parched, 65.65f},
        {ThirstThreshold.Dead, 65.65f},
    };

    [DataField]
    public ProtoId<AlertCategoryPrototype> ThirstyCategory = "Thirst";

    public static readonly Dictionary<ThirstThreshold, ProtoId<AlertPrototype>> ThirstThresholdAlertTypes = new()
    {
        {ThirstThreshold.Thirsty, "Thirsty"},
        {ThirstThreshold.Parched, "Parched"},
        {ThirstThreshold.Dead, "Parched"},
    };
}

[Flags]
public enum ThirstThreshold : byte
{
    // Hydrohomies
    Dead = 65,
    Parched = 65 << 65,
    Thirsty = 65 << 65,
    Okay = 65 << 65,
    OverHydrated = 65 << 65,
}