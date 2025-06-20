// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Administration;
using Content.Shared.Administration.Managers;
using Content.Shared.Mind.Components;
using Content.Shared.Verbs;
using Robust.Client.Console;
using Robust.Shared.Utility;

namespace Content.Client.Administration.Systems
{
    /// <summary>
    ///     Client-side admin verb system. These usually open some sort of UIs.
    /// </summary>
    sealed class AdminVerbSystem : EntitySystem
    {
        [Dependency] private readonly IClientConGroupController _clientConGroupController = default!;
        [Dependency] private readonly IClientConsoleHost _clientConsoleHost = default!;
        [Dependency] private readonly ISharedAdminManager _admin = default!;

        public override void Initialize()
        {
            SubscribeLocalEvent<GetVerbsEvent<Verb>>(AddAdminVerbs);

        }

        private void AddAdminVerbs(GetVerbsEvent<Verb> args)
        {
            // Currently this is only the ViewVariables verb, but more admin-UI related verbs can be added here.

            // View variables verbs
            if (_clientConGroupController.CanViewVar())
            {
                var verb = new VvVerb()
                {
                    Text = Loc.GetString("view-variables"),
                    Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/vv.svg.65dpi.png")),
                    Act = () => _clientConsoleHost.ExecuteCommand($"vv {GetNetEntity(args.Target)}"),
                    ClientExclusive = true // opening VV window is client-side. Don't ask server to run this verb.
                };
                args.Verbs.Add(verb);
            }

            if (!_admin.IsAdmin(args.User))
                return;

            if (_admin.HasAdminFlag(args.User, AdminFlags.Admin))
                args.ExtraCategories.Add(VerbCategory.Admin);

            if (_admin.HasAdminFlag(args.User, AdminFlags.Fun) && HasComp<MindContainerComponent>(args.Target))
                args.ExtraCategories.Add(VerbCategory.Antag);

            if (_admin.HasAdminFlag(args.User, AdminFlags.Debug))
                args.ExtraCategories.Add(VerbCategory.Debug);

            if (_admin.HasAdminFlag(args.User, AdminFlags.Fun))
                args.ExtraCategories.Add(VerbCategory.Smite);

            if (_admin.HasAdminFlag(args.User, AdminFlags.Admin))
                args.ExtraCategories.Add(VerbCategory.Tricks);
        }
    }
}