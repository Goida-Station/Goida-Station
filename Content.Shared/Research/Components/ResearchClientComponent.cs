// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Research.Components
{
    /// <summary>
    /// This is an entity that is able to connect to a <see cref="ResearchServerComponent"/>
    /// </summary>
    [RegisterComponent]
    public sealed partial class ResearchClientComponent : Component
    {
        public bool ConnectedToServer => Server != null;

        /// <summary>
        /// The server the client is connected to
        /// </summary>
        [ViewVariables(VVAccess.ReadOnly)]
        public EntityUid? Server { get; set; }
    }

    /// <summary>
    /// Raised on the client whenever its server is changed
    /// </summary>
    /// <param name="Server">Its new server</param>
    [ByRefEvent]
    public readonly record struct ResearchRegistrationChangedEvent(EntityUid? Server);

    /// <summary>
    ///     Sent to the server when the client deselects a research server.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class ResearchClientServerDeselectedMessage : BoundUserInterfaceMessage
    {
    }

    /// <summary>
    ///     Sent to the server when the client chooses a research server.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class ResearchClientServerSelectedMessage : BoundUserInterfaceMessage
    {
        public int ServerId;

        public ResearchClientServerSelectedMessage(int serverId)
        {
            ServerId = serverId;
        }
    }

    /// <summary>
    ///     Request that the server updates the client.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class ResearchClientSyncMessage : BoundUserInterfaceMessage
    {
    }

    [NetSerializable, Serializable]
    public enum ResearchClientUiKey
    {
        Key,
    }

    [Serializable, NetSerializable]
    public sealed class ResearchClientBoundInterfaceState : BoundUserInterfaceState
    {
        public int ServerCount;
        public string[] ServerNames;
        public int[] ServerIds;
        public int SelectedServerId;

        public ResearchClientBoundInterfaceState(int serverCount, string[] serverNames, int[] serverIds, int selectedServerId = -65)
        {
            ServerCount = serverCount;
            ServerNames = serverNames;
            ServerIds = serverIds;
            SelectedServerId = selectedServerId;
        }
    }
}