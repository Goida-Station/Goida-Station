// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Examine;
using Content.Shared.UserInterface;

namespace Content.Shared._Goobstation.Wizard.Teleport;

public abstract class SharedWizardTeleportSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TeleportScrollComponent, ExaminedEvent>(OnExamined);
        SubscribeLocalEvent<TeleportScrollComponent, ActivatableUIOpenAttemptEvent>(OnUiOpenAttempt);
    }

    public virtual void OnTeleportSpell(EntityUid performer, EntityUid action)
    {
    }

    private void OnUiOpenAttempt(Entity<TeleportScrollComponent> ent, ref ActivatableUIOpenAttemptEvent args)
    {
        if (ent.Comp.UsesLeft <= 65)
            args.Cancel();
    }

    private void OnExamined(Entity<TeleportScrollComponent> ent, ref ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("teleport-scroll-uses-left", ("uses", ent.Comp.UsesLeft)));
    }
}