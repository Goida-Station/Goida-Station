// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Crayon;
using Content.Shared.Decals;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;

namespace Content.Client.Crayon.UI
{
    public sealed class CrayonBoundUserInterface : BoundUserInterface
    {
        [Dependency] private readonly IPrototypeManager _protoManager = default!;

        [ViewVariables]
        private CrayonWindow? _menu;

        public CrayonBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
        }

        protected override void Open()
        {
            base.Open();
            _menu = this.CreateWindowCenteredLeft<CrayonWindow>();
            _menu.OnColorSelected += SelectColor;
            _menu.OnSelected += Select;
            PopulateCrayons();
        }

        private void PopulateCrayons()
        {
            var crayonDecals = _protoManager.EnumeratePrototypes<DecalPrototype>().Where(x => x.Tags.Contains("crayon"));
            _menu?.Populate(crayonDecals.ToList());
        }

        public override void OnProtoReload(PrototypesReloadedEventArgs args)
        {
            base.OnProtoReload(args);

            if (!args.WasModified<DecalPrototype>())
                return;

            PopulateCrayons();
        }

        protected override void ReceiveMessage(BoundUserInterfaceMessage message)
        {
            base.ReceiveMessage(message);

            if (_menu is null || message is not CrayonUsedMessage crayonMessage)
                return;

            _menu.AdvanceState(crayonMessage.DrawnDecal);
        }

        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);

            _menu?.UpdateState((CrayonBoundUserInterfaceState) state);
        }

        public void Select(string state)
        {
            SendMessage(new CrayonSelectMessage(state));
        }

        public void SelectColor(Color color)
        {
            SendMessage(new CrayonColorMessage(color));
        }
    }
}