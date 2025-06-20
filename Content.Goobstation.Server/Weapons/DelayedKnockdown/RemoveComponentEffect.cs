// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Server.Weapons.DelayedKnockdown;

public sealed partial class RemoveComponentEffect : EntityEffect
{
    [DataField]
    public string? Locale;

    [DataField(required: true)]
    public string Component = string.Empty; // riders yelling at me

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        return Locale == null ? null : Loc.GetString(Locale);
    }

    public override void Effect(EntityEffectBaseArgs args)
    {
        args.EntityManager.RemoveComponentDeferred(args.TargetEntity,
            args.EntityManager.ComponentFactory.GetRegistration(Component).Type);
    }
}