// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Examine;
using Content.Shared.Nutrition.Components;

namespace Content.Shared.Nutrition.EntitySystems;

public sealed partial class SealableSystem : EntitySystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SealableComponent, ExaminedEvent>(OnExamined, after: new[] { typeof(OpenableSystem) });
        SubscribeLocalEvent<SealableComponent, OpenableOpenedEvent>(OnOpened);
    }

    private void OnExamined(EntityUid uid, SealableComponent comp, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        var sealedText = comp.Sealed ? Loc.GetString(comp.ExamineTextSealed) : Loc.GetString(comp.ExamineTextUnsealed);

        args.PushMarkup(sealedText);
    }

    private void OnOpened(EntityUid uid, SealableComponent comp, OpenableOpenedEvent args)
    {
        comp.Sealed = false;

        Dirty(uid, comp);

        UpdateAppearance(uid, comp);
    }

    /// <summary>
    /// Update seal visuals to the current value.
    /// </summary>
    public void UpdateAppearance(EntityUid uid, SealableComponent? comp = null, AppearanceComponent? appearance = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        _appearance.SetData(uid, SealableVisuals.Sealed, comp.Sealed, appearance);
    }

    /// <summary>
    /// Returns true if the entity's seal is intact.
    /// Items without SealableComponent are considered unsealed.
    /// </summary>
    public bool IsSealed(EntityUid uid, SealableComponent? comp = null)
    {
        if (!Resolve(uid, ref comp, false))
            return false;

        return comp.Sealed;
    }
}