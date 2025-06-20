// SPDX-FileCopyrightText: 65 MisterMecky <mrmecky@hotmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Chemistry.Components;
using Content.Shared.Inventory;

namespace Content.Shared.Chemistry;

public sealed class SolutionScannerSystem : EntitySystem
{
    public override void Initialize()
    {
        SubscribeLocalEvent<SolutionScannerComponent, SolutionScanEvent>(OnSolutionScanAttempt);
        SubscribeLocalEvent<SolutionScannerComponent, InventoryRelayedEvent<SolutionScanEvent>>((e, c, ev) => OnSolutionScanAttempt(e, c, ev.Args));
    }

    private void OnSolutionScanAttempt(EntityUid eid, SolutionScannerComponent component, SolutionScanEvent args)
    {
        args.CanScan = true;
    }
}

public sealed class SolutionScanEvent : EntityEventArgs, IInventoryRelayEvent
{
    public bool CanScan;
    public SlotFlags TargetSlots { get; } = SlotFlags.EYES;
}