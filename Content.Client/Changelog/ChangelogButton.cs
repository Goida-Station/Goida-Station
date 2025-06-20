// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Stylesheets;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.Changelog
{
    public sealed class ChangelogButton : Button
    {
        [Dependency] private readonly ChangelogManager _changelogManager = default!;

        public ChangelogButton()
        {
            IoCManager.InjectDependencies(this);

            // So that measuring before opening returns a correct height,
            // and the window has the correct size when opened.
            Text = " ";
        }

        protected override void EnteredTree()
        {
            base.EnteredTree();

            _changelogManager.NewChangelogEntriesChanged += UpdateStuff;
            UpdateStuff();
        }

        protected override void ExitedTree()
        {
            base.ExitedTree();

            _changelogManager.NewChangelogEntriesChanged -= UpdateStuff;
        }

        private void UpdateStuff()
        {
            if (_changelogManager.NewChangelogEntries)
            {
                Text = Loc.GetString("changelog-button-new-entries");
                StyleClasses.Add(StyleBase.ButtonCaution);
            }
            else
            {
                Text = Loc.GetString("changelog-button");
                StyleClasses.Remove(StyleBase.ButtonCaution);
            }
        }
    }
}