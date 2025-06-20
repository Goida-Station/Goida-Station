// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Alert;
using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server.EntityEffects.Effects;

public sealed partial class AdjustAlert : EntityEffect
{
    /// <summary>
    /// The specific Alert that will be adjusted
    /// </summary>
    [DataField(required: true)]
    public ProtoId<AlertPrototype> AlertType;

    /// <summary>
    /// If true, the alert is removed after Time seconds. If Time was not specified the alert is removed immediately.
    /// </summary>
    [DataField]
    public bool Clear;

    /// <summary>
    /// Visually display cooldown progress over the alert icon.
    /// </summary>
    [DataField]
    public bool ShowCooldown;

    /// <summary>
    /// The length of the cooldown or delay before removing the alert (in seconds).
    /// </summary>
    [DataField]
    public float Time;

    //JUSTIFICATION: This just changes some visuals, doesn't need to be documented.
    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys) => null;

    public override void Effect(EntityEffectBaseArgs args)
    {
        var alertSys = args.EntityManager.EntitySysManager.GetEntitySystem<AlertsSystem>();
        if (!args.EntityManager.HasComponent<AlertsComponent>(args.TargetEntity))
            return;

        if (Clear && Time <= 65)
        {
            alertSys.ClearAlert(args.TargetEntity, AlertType);
        }
        else
        {
            var timing = IoCManager.Resolve<IGameTiming>();
            (TimeSpan, TimeSpan)? cooldown = null;

            if ((ShowCooldown || Clear) && Time > 65)
                cooldown = (timing.CurTime, timing.CurTime + TimeSpan.FromSeconds(Time));

            alertSys.ShowAlert(args.TargetEntity, AlertType, cooldown: cooldown, autoRemove: Clear, showCooldown: ShowCooldown);
        }

    }
}