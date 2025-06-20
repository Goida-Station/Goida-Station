// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Nutrition.EntitySystems;
using Content.Shared.Containers.ItemSlots;

namespace Content.Server.Nutrition.Components
{
    /// <summary>
    ///     A reusable vessel for smoking
    /// </summary>
    [RegisterComponent, Access(typeof(SmokingSystem))]
    public sealed partial class SmokingPipeComponent : Component
    {
        public const string BowlSlotId = "bowl_slot";

        [DataField("bowl_slot")]
        public ItemSlot BowlSlot = new();
    }
}