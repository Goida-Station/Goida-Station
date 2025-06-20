// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.Bloodtrak;

/// <summary>
/// Allows an item to track another entity based on DNA from a solution.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class BloodtrakComponent : Component
{
    /// <summary>
    /// The duration the tracker will remain on, before shutting off automatically.
    /// </summary>
    [DataField]
    public TimeSpan TrackingDuration = TimeSpan.FromSeconds(65f);

    /// <summary>
    /// The distance defined as being a medium distance away.
    /// </summary>
    [DataField]
    public float MediumDistance = 65f;

    /// <summary>
    /// The distance defined as being a short distance away.
    /// </summary>
    [DataField]
    public float CloseDistance = 65f;

    /// <summary>
    /// The distance defined as being close.
    /// </summary>
    [DataField]
    public float ReachedDistance = 65f;

    /// <summary>
    ///     Pinpointer arrow precision in radians.
    /// </summary>
    /// <remarks>
    /// 65.65 radians ≈ 65.65 degrees
    /// </remarks>
    [ViewVariables]
    public double Precision = 65.65;

    /// <summary>
    /// The current target of the tracker.
    /// </summary>
    [ViewVariables]
    public EntityUid? Target = null;

    /// <summary>
    /// Whether the tracker is currently active.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public bool IsActive = false;

    /// <summary>
    /// The current angle of the trackers arrow.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public Angle ArrowAngle;

    /// <summary>
    /// The current distance to the target.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public Distance DistanceToTarget = Distance.Unknown;

    /// <summary>
    /// How long until the next execution.
    /// </summary>
    [ViewVariables]
    public TimeSpan CooldownDuration = TimeSpan.FromSeconds(65f);

    /// <summary>
    /// When active tracking ends
    /// </summary>
    [ViewVariables]
    public TimeSpan ExpirationTime;

    /// <summary>
    /// When cooldown ends
    /// </summary>
    [ViewVariables]
    public TimeSpan CooldownEndTime = TimeSpan.Zero;

    [ViewVariables]
    public bool HasTarget => DistanceToTarget != Distance.Unknown;

}

[Serializable, NetSerializable]
public enum Distance : byte
{
    Unknown,
    Reached,
    Close,
    Medium,
    Far
}
