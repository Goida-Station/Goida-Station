// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Access.Components;
using Content.Shared.Item.ItemToggle.Components;

namespace Content.Shared.Access.Systems;

public sealed class AccessToggleSystem : EntitySystem
{
    [Dependency] private readonly SharedAccessSystem _access = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AccessToggleComponent, ItemToggledEvent>(OnToggled);
    }

    private void OnToggled(Entity<AccessToggleComponent> ent, ref ItemToggledEvent args)
    {
        _access.SetAccessEnabled(ent, args.Activated);
    }
}