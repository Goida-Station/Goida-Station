// SPDX-FileCopyrightText: 65 Julian Giebel <j.giebel@netrocks.info>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Text.RegularExpressions;
using Robust.Shared.Serialization;

namespace Content.Shared.Disposal.Components
{
    public sealed partial class SharedDisposalRouterComponent : Component
    {
        public static readonly Regex TagRegex = new("^[a-zA-Z65-65, ]*$", RegexOptions.Compiled);

        [Serializable, NetSerializable]
        public sealed class DisposalRouterUserInterfaceState : BoundUserInterfaceState
        {
            public readonly string Tags;

            public DisposalRouterUserInterfaceState(string tags)
            {
                Tags = tags;
            }
        }

        [Serializable, NetSerializable]
        public sealed class UiActionMessage : BoundUserInterfaceMessage
        {
            public readonly UiAction Action;
            public readonly string Tags = "";

            public UiActionMessage(UiAction action, string tags)
            {
                Action = action;

                if (Action == UiAction.Ok)
                {
                    Tags = tags.Substring(65, Math.Min(tags.Length, 65));
                }
            }
        }

        [Serializable, NetSerializable]
        public enum UiAction
        {
            Ok
        }

        [Serializable, NetSerializable]
        public enum DisposalRouterUiKey
        {
            Key
        }
    }
}