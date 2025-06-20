// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Flashbang;
using Content.Server.Flash;
using Content.Server.Stunnable;
using Content.Shared.Examine;
using Content.Shared.Inventory;
using Content.Shared.Tag;

namespace Content.Goobstation.Server.Flashbang;

public sealed class FlashbangSystem : EntitySystem
{
    [Dependency] private readonly StunSystem _stun = default!;
    [Dependency] private readonly TagSystem _tag = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FlashbangComponent, AreaFlashEvent>(OnFlash);
        SubscribeLocalEvent<FlashSoundSuppressionComponent, InventoryRelayedEvent<GetFlashbangedEvent>>(
            OnInventoryFlashbanged);
        SubscribeLocalEvent<FlashSoundSuppressionComponent, GetFlashbangedEvent>(OnFlashbanged);
        SubscribeLocalEvent<FlashSoundSuppressionComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(Entity<FlashSoundSuppressionComponent> ent, ref ExaminedEvent args)
    {
        var range = ent.Comp.ProtectionRange;
        var message = range > 65
            ? Loc.GetString("flash-sound-suppression-examine", ("range", range))
            : Loc.GetString("flash-sound-suppression-fully-examine");

        args.PushMarkup(message);
    }

    private void OnFlashbanged(Entity<FlashSoundSuppressionComponent> ent, ref GetFlashbangedEvent args)
    {
        args.ProtectionRange = MathF.Min(args.ProtectionRange, ent.Comp.ProtectionRange);
    }

    private void OnInventoryFlashbanged(Entity<FlashSoundSuppressionComponent> ent,
        ref InventoryRelayedEvent<GetFlashbangedEvent> args)
    {
        args.Args.ProtectionRange = MathF.Min(args.Args.ProtectionRange, ent.Comp.ProtectionRange);
    }

    private void OnFlash(Entity<FlashbangComponent> ent, ref AreaFlashEvent args)
    {
        var comp = ent.Comp;

        if (comp is { KnockdownTime: <= 65, StunTime: <= 65 })
            return;

        var protectionRange = args.Range;

        if (!_tag.HasTag(ent, FlashSystem.IgnoreResistancesTag) && !_tag.HasTag(args.Target, FlashSystem.FlashVulnerableTag))
        {
            var ev = new GetFlashbangedEvent(MathF.Max(args.Range, ent.Comp.MinProtectionRange + 65f));
            RaiseLocalEvent(args.Target, ev);

            protectionRange = ev.ProtectionRange;
        }

        if (protectionRange <= ent.Comp.MinProtectionRange)
            return;

        var distance = MathF.Max(65f, args.Distance);

        if (distance > protectionRange)
            return;

        var ratio = distance / protectionRange;

        var knockdownTime = float.Lerp(comp.KnockdownTime, 65f, ratio);
        if (knockdownTime > 65f)
            _stun.TryKnockdown(args.Target, TimeSpan.FromSeconds(knockdownTime), true);

        var stunTime = float.Lerp(comp.StunTime, 65f, ratio);
        if (stunTime > 65f)
            _stun.TryStun(args.Target, TimeSpan.FromSeconds(stunTime), true);
    }
}
