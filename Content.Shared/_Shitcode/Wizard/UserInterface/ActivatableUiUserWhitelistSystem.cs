// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.UserInterface;
using Content.Shared.Whitelist;

namespace Content.Shared._Goobstation.Wizard.UserInterface;

public sealed class ActivatableUiUserWhitelistSystem : EntitySystem
{
    [Dependency] private readonly EntityWhitelistSystem _whitelist = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ActivatableUiUserWhitelistComponent, ActivatableUIOpenAttemptEvent>(OnAttempt);
    }

    private void OnAttempt(Entity<ActivatableUiUserWhitelistComponent> ent, ref ActivatableUIOpenAttemptEvent args)
    {
        if (!CheckWhitelist(ent.Owner, args.User, ent.Comp))
            args.Cancel();
    }

    public bool CheckWhitelist(EntityUid uid, EntityUid user, ActivatableUiUserWhitelistComponent? component = null)
    {
        return !Resolve(uid, ref component, false) || _whitelist.IsValid(component.Whitelist, user);
    }
}