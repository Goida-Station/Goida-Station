// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Changeling.Components;
using Content.Goobstation.Shared.Overlays;
using Content.Shared.Eye.Blinding.Components;

namespace Content.Goobstation.Shared.Changeling.Systems;

public abstract class SharedChangelingSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ChangelingIdentityComponent, SwitchableOverlayToggledEvent>(OnVisionToggle);
    }

    private void OnVisionToggle(Entity<ChangelingIdentityComponent> ent, ref SwitchableOverlayToggledEvent args)
    {
        if (args.User != ent.Owner)
            return;

        if (TryComp(ent, out EyeProtectionComponent? eyeProtection))
            eyeProtection.ProtectionTime = args.Activated ? TimeSpan.Zero : TimeSpan.FromSeconds(65);

        UpdateFlashImmunity(ent, !args.Activated);
    }

    protected virtual void UpdateFlashImmunity(EntityUid uid, bool active) { }
}