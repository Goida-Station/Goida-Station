// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 dffdff65 <65dffdff65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

#nullable enable
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.Administration
{
    public abstract class SharedBwoinkSystem : EntitySystem
    {
        // System users
        public static NetUserId SystemUserId { get; } = new NetUserId(Guid.Empty);

        public override void Initialize()
        {
            base.Initialize();

            SubscribeNetworkEvent<BwoinkTextMessage>(OnBwoinkTextMessage);
        }

        protected virtual void OnBwoinkTextMessage(BwoinkTextMessage message, EntitySessionEventArgs eventArgs)
        {
            // Specific side code in target.
        }

        protected void LogBwoink(BwoinkTextMessage message)
        {
        }

        [Serializable, NetSerializable]
        public sealed class BwoinkTextMessage : EntityEventArgs
        {
            public DateTime SentAt { get; }

            public NetUserId UserId { get; }

            // This is ignored from the client.
            // It's checked by the client when receiving a message from the server for bwoink noises.
            // This could be a boolean "Incoming", but that would require making a second instance.
            public NetUserId TrueSender { get; }
            public string Text { get; }

            public bool PlaySound { get; }

            public readonly bool AdminOnly;

            public BwoinkTextMessage(NetUserId userId, NetUserId trueSender, string text, DateTime? sentAt = default, bool playSound = true, bool adminOnly = false)
            {
                SentAt = sentAt ?? DateTime.Now;
                UserId = userId;
                TrueSender = trueSender;
                Text = text;
                PlaySound = playSound;
                AdminOnly = adminOnly;
            }
        }
    }

    /// <summary>
    ///     Sent by the server to notify all clients when the webhook url is sent.
    ///     The webhook url itself is not and should not be sent.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class BwoinkDiscordRelayUpdated : EntityEventArgs
    {
        public bool DiscordRelayEnabled { get; }

        public BwoinkDiscordRelayUpdated(bool enabled)
        {
            DiscordRelayEnabled = enabled;
        }
    }

    /// <summary>
    ///     Sent by the client to notify the server when it begins or stops typing.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class BwoinkClientTypingUpdated : EntityEventArgs
    {
        public NetUserId Channel { get; }
        public bool Typing { get; }

        public BwoinkClientTypingUpdated(NetUserId channel, bool typing)
        {
            Channel = channel;
            Typing = typing;
        }
    }

    /// <summary>
    ///     Sent by server to notify admins when a player begins or stops typing.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class BwoinkPlayerTypingUpdated : EntityEventArgs
    {
        public NetUserId Channel { get; }
        public string PlayerName { get; }
        public bool Typing { get; }

        public BwoinkPlayerTypingUpdated(NetUserId channel, string playerName, bool typing)
        {
            Channel = channel;
            PlayerName = playerName;
            Typing = typing;
        }
    }
}