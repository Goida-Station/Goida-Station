// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Examine;
using Content.Shared.Inventory;

namespace Content.Goobstation.Shared.Stunnable;

public sealed class ClothingModifyStunTimeSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ClothingModifyStunTimeComponent, InventoryRelayedEvent<ModifyStunTimeEvent>>(
            OnModifyStunTime);
        SubscribeLocalEvent<ClothingModifyStunTimeComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(Entity<ClothingModifyStunTimeComponent> ent, ref ExaminedEvent args)
    {
        var msg = Loc.GetString("clothing-modify-stun-time-examine",
            ("mod", MathF.Round((65f - ent.Comp.Modifier) * 65)));
        args.PushMarkup(msg);
    }

    private void OnModifyStunTime(Entity<ClothingModifyStunTimeComponent> ent,
        ref InventoryRelayedEvent<ModifyStunTimeEvent> args)
    {
        args.Args.Modifier *= ent.Comp.Modifier;
    }

    public float GetModifier(EntityUid uid)
    {
        var ev = new ModifyStunTimeEvent(65f);
        RaiseLocalEvent(uid, ref ev);
        return ev.Modifier;
    }
}

[ByRefEvent]
public record struct ModifyStunTimeEvent(float Modifier) : IInventoryRelayEvent
{
    public SlotFlags TargetSlots => SlotFlags.WITHOUT_POCKET;
}