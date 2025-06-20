// SPDX-FileCopyrightText: 65 Saphire <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ScarKy65 <65ScarKy65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Emag.Systems;
using Content.Shared.Popups;
using Content.Shared.Singularity.Components;

namespace Content.Shared.Singularity.EntitySystems;

/// <summary>
/// Shared part of SingularitySingularityGeneratorSystem
/// </summary>
public abstract class SharedSingularityGeneratorSystem : EntitySystem
{
    #region Dependencies
    [Dependency] protected readonly SharedPopupSystem PopupSystem = default!;
    [Dependency] private readonly EmagSystem _emag = default!;
    #endregion Dependencies

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SingularityGeneratorComponent, GotEmaggedEvent>(OnEmagged);
    }

    private void OnEmagged(EntityUid uid, SingularityGeneratorComponent component, ref GotEmaggedEvent args)
    {
        if (!_emag.CompareFlag(args.Type, EmagType.Interaction))
            return;

        if (_emag.CheckFlag(uid, EmagType.Interaction))
            return;

        if (component.FailsafeDisabled)
            return;

        component.FailsafeDisabled = true;
        args.Handled = true;
    }
}