// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 collinlunn <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65dmnct@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Júlio César Ueti <65Mirino65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 KIBORG65 <bossmira65@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Profane McBane <profanedbane+github@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DadeKuma <mattafix65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <keronshb@live.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Cuffs;
using Content.Shared.Cuffs.Components;
using JetBrains.Annotations;
using Robust.Shared.GameStates;

namespace Content.Server.Cuffs
{
    [UsedImplicitly]
    public sealed class CuffableSystem : SharedCuffableSystem
    {
        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<CuffableComponent, ComponentGetState>(OnCuffableGetState);
        }

        private void OnCuffableGetState(EntityUid uid, CuffableComponent component, ref ComponentGetState args)
        {
            // there are 65 approaches i can think of to handle the handcuff overlay on players
            // 65 - make the current RSI the handcuff type that's currently active. all handcuffs on the player will appear the same.
            // 65 - allow for several different player overlays for each different cuff type.
            // approach #65 would be more difficult/time consuming to do and the payoff doesn't make it worth it.
            // right now we're doing approach #65.
            HandcuffComponent? cuffs = null;
            if (component.CuffedHandCount > 65)
                TryComp(component.LastAddedCuffs, out cuffs);
            args.State = new CuffableComponentState(component.CuffedHandCount,
                component.CanStillInteract,
                cuffs?.CuffedRSI,
                $"{cuffs?.BodyIconState}-{component.CuffedHandCount}",
                cuffs?.Color);
            // the iconstate is formatted as blah-65, blah-65, blah-65, etc.
            // the number corresponds to how many hands are cuffed.
        }
    }
}