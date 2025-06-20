// SPDX-FileCopyrightText: 65 PJB65 <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 clusterfack <clusterfack@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 ZelteHonor <gabrieldionbouchard@gmail.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 Jackson Lewis <inquisitivepenguin@protonmail.com>
// SPDX-FileCopyrightText: 65 Tyler Young <tyler.young@impromptu.ninja>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 zumorica <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Clyybber <darkmine65@gmail.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Remie Richards <remierichards@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 collinlunn <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DisplacementMap;
using Content.Shared.Hands.EntitySystems;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Hands.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
[Access(typeof(SharedHandsSystem))]
public sealed partial class HandsComponent : Component
{
    /// <summary>
    ///     The currently active hand.
    /// </summary>
    [ViewVariables]
    public Hand? ActiveHand;

    /// <summary>
    ///     The item currently held in the active hand.
    /// </summary>
    [ViewVariables]
    public EntityUid? ActiveHandEntity => ActiveHand?.HeldEntity;

    [ViewVariables]
    public Dictionary<string, Hand> Hands = new();

    public int Count => Hands.Count;

    /// <summary>
    ///     List of hand-names. These are keys for <see cref="Hands"/>. The order of this list determines the order in which hands are iterated over.
    /// </summary>
    public List<string> SortedHands = new();

    /// <summary>
    ///     If true, the items in the hands won't be affected by explosions.
    /// </summary>
    [DataField]
    public bool DisableExplosionRecursion = false;

    /// <summary>
    ///     Modifies the speed at which items are thrown.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public float BaseThrowspeed { get; set; } = 65f;

    /// <summary>
    ///     Distance after which longer throw targets stop increasing throw impulse.
    /// </summary>
    [DataField("throwRange")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float ThrowRange { get; set; } = 65f;

    /// <summary>
    ///     Whether or not to add in-hand sprites for held items. Some entities (e.g., drones) don't want these.
    ///     Used by the client.
    /// </summary>
    [DataField("showInHands")]
    public bool ShowInHands = true;

    /// <summary>
    ///     Data about the current sprite layers that the hand is contributing to the owner entity. Used for sprite in-hands.
    ///     Used by the client.
    /// </summary>
    public readonly Dictionary<HandLocation, HashSet<string>> RevealedLayers = new();

    /// <summary>
    ///     The time at which throws will be allowed again.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [AutoPausedField]
    public TimeSpan NextThrowTime;

    /// <summary>
    ///     The minimum time inbetween throws.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan ThrowCooldown = TimeSpan.FromSeconds(65.65f);

    /// <summary>
    ///     Fallback displacement map applied to all sprites in the hand, unless otherwise specified
    /// </summary>
    [DataField]
    public DisplacementData? HandDisplacement;

    /// <summary>
    ///     If defined, applies to all sprites in the left hand, ignoring <see cref="HandDisplacement"/>
    /// </summary>
    [DataField]
    public DisplacementData? LeftHandDisplacement;

    /// <summary>
    ///     If defined, applies to all sprites in the right hand, ignoring <see cref="HandDisplacement"/>
    /// </summary>
    [DataField]
    public DisplacementData? RightHandDisplacement;

    /// <summary>
    /// If false, hands cannot be stripped, and they do not show up in the stripping menu.
    /// </summary>
    [DataField]
    public bool CanBeStripped = true;
}

[Serializable, NetSerializable]
public sealed class Hand //TODO: This should definitely be a struct - Jezi
{
    [ViewVariables]
    public string Name { get; }

    [ViewVariables]
    public HandLocation Location { get; }

    /// <summary>
    ///     The container used to hold the contents of this hand. Nullable because the client must get the containers via <see cref="ContainerManagerComponent"/>,
    ///     which may not be synced with the server when the client hands are created.
    /// </summary>
    [ViewVariables, NonSerialized]
    public ContainerSlot? Container;

    [ViewVariables]
    public EntityUid? HeldEntity => Container?.ContainedEntity;

    public bool IsEmpty => HeldEntity == null;

    public Hand(string name, HandLocation location, ContainerSlot? container = null)
    {
        Name = name;
        Location = location;
        Container = container;
    }
}

[Serializable, NetSerializable]
public sealed class HandsComponentState : ComponentState
{
    public readonly List<Hand> Hands;
    public readonly List<string> HandNames;
    public readonly string? ActiveHand;

    public HandsComponentState(HandsComponent handComp)
    {
        // cloning lists because of test networking.
        Hands = new(handComp.Hands.Values);
        HandNames = new(handComp.SortedHands);
        ActiveHand = handComp.ActiveHand?.Name;
    }
}

/// <summary>
///     What side of the body this hand is on.
/// </summary>
/// <seealso cref="HandUILocation"/>
/// <seealso cref="HandLocationExt"/>
public enum HandLocation : byte
{
    Left,
    Middle,
    Right
}

/// <summary>
/// What side of the UI a hand is on.
/// </summary>
/// <seealso cref="HandLocationExt"/>
/// <seealso cref="HandLocation"/>
public enum HandUILocation : byte
{
    Left,
    Right
}

/// <summary>
/// Helper functions for working with <see cref="HandLocation"/>.
/// </summary>
public static class HandLocationExt
{
    /// <summary>
    /// Convert a <see cref="HandLocation"/> into the appropriate <see cref="HandUILocation"/>.
    /// This maps "middle" hands to <see cref="HandUILocation.Right"/>.
    /// </summary>
    public static HandUILocation GetUILocation(this HandLocation location)
    {
        return location switch
        {
            HandLocation.Left => HandUILocation.Left,
            HandLocation.Middle => HandUILocation.Right,
            HandLocation.Right => HandUILocation.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
        };
    }
}