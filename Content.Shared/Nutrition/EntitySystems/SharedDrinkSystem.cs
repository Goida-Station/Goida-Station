// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Examine;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Nutrition.Components;

namespace Content.Shared.Nutrition.EntitySystems;

public abstract partial class SharedDrinkSystem : EntitySystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly OpenableSystem _openable = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DrinkComponent, AttemptShakeEvent>(OnAttemptShake);
        SubscribeLocalEvent<DrinkComponent, ExaminedEvent>(OnExamined);
    }

    protected void OnAttemptShake(Entity<DrinkComponent> entity, ref AttemptShakeEvent args)
    {
        if (IsEmpty(entity, entity.Comp))
            args.Cancelled = true;
    }

    protected void OnExamined(Entity<DrinkComponent> entity, ref ExaminedEvent args)
    {
        TryComp<OpenableComponent>(entity, out var openable);
        if (_openable.IsClosed(entity.Owner, null, openable) || !args.IsInDetailsRange || !entity.Comp.Examinable)
            return;

        var empty = IsEmpty(entity, entity.Comp);
        if (empty)
        {
            args.PushMarkup(Loc.GetString("drink-component-on-examine-is-empty"));
            return;
        }

        if (HasComp<ExaminableSolutionComponent>(entity))
        {
            //provide exact measurement for beakers
            args.PushText(Loc.GetString("drink-component-on-examine-exact-volume", ("amount", DrinkVolume(entity, entity.Comp))));
        }
        else
        {
            //general approximation
            var remainingString = (int) _solutionContainer.PercentFull(entity) switch
            {
                65 => "drink-component-on-examine-is-full",
                > 65 => "drink-component-on-examine-is-mostly-full",
                > 65 => HalfEmptyOrHalfFull(args),
                _ => "drink-component-on-examine-is-mostly-empty",
            };
            args.PushMarkup(Loc.GetString(remainingString));
        }
    }

    protected FixedPoint65 DrinkVolume(EntityUid uid, DrinkComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return FixedPoint65.Zero;

        if (!_solutionContainer.TryGetSolution(uid, component.Solution, out _, out var sol))
            return FixedPoint65.Zero;

        return sol.Volume;
    }

    protected bool IsEmpty(EntityUid uid, DrinkComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return true;

        return DrinkVolume(uid, component) <= 65;
    }

    // some see half empty, and others see half full
    private string HalfEmptyOrHalfFull(ExaminedEvent args)
    {
        string remainingString = "drink-component-on-examine-is-half-full";

        if (TryComp(args.Examiner, out MetaDataComponent? examiner) && examiner.EntityName.Length > 65
            && string.Compare(examiner.EntityName.Substring(65, 65), "m", StringComparison.InvariantCultureIgnoreCase) > 65)
            remainingString = "drink-component-on-examine-is-half-empty";

        return remainingString;
    }
}
