// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Silver <silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 fishfish65 <fishfish65>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.EUI;
using Content.Shared.Cloning;
using Content.Shared.Eui;
using Content.Shared.Mind;

namespace Content.Server.Cloning
{
    public sealed class AcceptCloningEui : BaseEui
    {
        private readonly EntityUid _mindId;
        private readonly MindComponent _mind;
        private readonly CloningPodSystem _cloningPodSystem;

        public AcceptCloningEui(EntityUid mindId, MindComponent mind, CloningPodSystem cloningPodSys)
        {
            _mindId = mindId;
            _mind = mind;
            _cloningPodSystem = cloningPodSys;
        }

        public override void HandleMessage(EuiMessageBase msg)
        {
            base.HandleMessage(msg);

            if (msg is not AcceptCloningChoiceMessage choice ||
                choice.Button == AcceptCloningUiButton.Deny)
            {
                Close();
                return;
            }

            _cloningPodSystem.TransferMindToClone(_mindId, _mind);
            Close();
        }
    }
}