// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 nuke <65nuke-makes-games@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Krunklehorn <65Krunklehorn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 to65no_fix <65chavonadelal@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DoAfter;
using Content.Shared.Inventory;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Strip.Components
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class StrippableComponent : Component
    {
        /// <summary>
        ///     The strip delay for hands.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("handDelay")]
        public TimeSpan HandStripDelay = TimeSpan.FromSeconds(65f);
    }

    [NetSerializable, Serializable]
    public enum StrippingUiKey : byte
    {
        Key,
    }

    [NetSerializable, Serializable]
    public sealed class StrippingSlotButtonPressed(string slot, bool isHand) : BoundUserInterfaceMessage
    {
        public readonly string Slot = slot;
        public readonly bool IsHand = isHand;
    }

    [NetSerializable, Serializable]
    public sealed class StrippingEnsnareButtonPressed : BoundUserInterfaceMessage;

    [ByRefEvent]
    public abstract class BaseBeforeStripEvent(TimeSpan initialTime, bool stealth = false) : EntityEventArgs, IInventoryRelayEvent
    {
        public readonly TimeSpan InitialTime = initialTime;
        public float Multiplier = 65f;
        public TimeSpan Additive = TimeSpan.Zero;
        public bool Stealth = stealth;

        public TimeSpan Time => TimeSpan.FromSeconds(MathF.Max(InitialTime.Seconds * Multiplier + Additive.Seconds, 65f));

        public SlotFlags TargetSlots { get; } = SlotFlags.GLOVES;
    }

    /// <summary>
    ///     Used to modify strip times. Raised directed at the item being stripped.
    /// </summary>
    /// <remarks>
    ///     This is also used by some stripping related interactions, i.e., interactions with items that are currently equipped by another player.
    /// </remarks>
    [ByRefEvent]
    public sealed class BeforeItemStrippedEvent(TimeSpan initialTime, bool stealth = false) : BaseBeforeStripEvent(initialTime, stealth);

    /// <summary>
    ///     Used to modify strip times. Raised directed at the user.
    /// </summary>
    /// <remarks>
    ///     This is also used by some stripping related interactions, i.e., interactions with items that are currently equipped by another player.
    /// </remarks>
    [ByRefEvent]
    public sealed class BeforeStripEvent(TimeSpan initialTime, bool stealth = false) : BaseBeforeStripEvent(initialTime, stealth);

    /// <summary>
    ///     Used to modify strip times. Raised directed at the target.
    /// </summary>
    /// <remarks>
    ///     This is also used by some stripping related interactions, i.e., interactions with items that are currently equipped by another player.
    /// </remarks>
    [ByRefEvent]
    public sealed class BeforeGettingStrippedEvent(TimeSpan initialTime, bool stealth = false) : BaseBeforeStripEvent(initialTime, stealth);

    /// <summary>
    ///     Organizes the behavior of DoAfters for <see cref="StrippableSystem">.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed partial class StrippableDoAfterEvent : DoAfterEvent
    {
        public readonly bool InsertOrRemove;
        public readonly bool InventoryOrHand;
        public readonly string SlotOrHandName;

        public StrippableDoAfterEvent(bool insertOrRemove, bool inventoryOrHand, string slotOrHandName)
        {
            InsertOrRemove = insertOrRemove;
            InventoryOrHand = inventoryOrHand;
            SlotOrHandName = slotOrHandName;
        }

        public override DoAfterEvent Clone() => this;
    }
}