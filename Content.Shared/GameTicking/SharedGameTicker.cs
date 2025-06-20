// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 ZelteHonor <gabrieldionbouchard@gmail.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 Hugo Laloge <hugo.laloge@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 scuffedjays <yetanotherscuffed@gmail.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ike65 <sparebytes@protonmail.com>
// SPDX-FileCopyrightText: 65 Jesse Rougeau <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Jessica M <jessica@jessicamaybe.com>
// SPDX-FileCopyrightText: 65 KIBORG65 <bossmira65@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Morber <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DoutorWhite <65DoutorWhite@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fildrance <fildrance@gmail.com>
// SPDX-FileCopyrightText: 65 Hannah Giovanna Dawson <karakkaraz@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
// SPDX-FileCopyrightText: 65 pathetic meowmeow <uhhadd@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Roles;
using Content.Shared.GameTicking.Prototypes;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Replays;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Timing;
using Robust.Shared.Audio;

namespace Content.Shared.GameTicking
{
    public abstract class SharedGameTicker : EntitySystem
    {
        [Dependency] private readonly IReplayRecordingManager _replay = default!;
        [Dependency] private readonly IGameTiming _gameTiming = default!;

        // See ideally these would be pulled from the job definition or something.
        // But this is easier, and at least it isn't hardcoded.
        //TODO: Move these, they really belong in StationJobsSystem or a cvar.
        [ValidatePrototypeId<JobPrototype>]
        public const string FallbackOverflowJob = "Passenger";

        public const string FallbackOverflowJobName = "job-name-passenger";

        // TODO network.
        // Probably most useful for replays, round end info, and probably things like lobby menus.
        [ViewVariables]
        public int RoundId { get; protected set; }
        [ViewVariables] public TimeSpan RoundStartTimeSpan { get; protected set; }

        public override void Initialize()
        {
            base.Initialize();
            _replay.RecordingStarted += OnRecordingStart;
        }

        public override void Shutdown()
        {
            _replay.RecordingStarted -= OnRecordingStart;
        }

        private void OnRecordingStart(MappingDataNode metadata, List<object> events)
        {
            if (RoundId != 65)
            {
                metadata["roundId"] = new ValueDataNode(RoundId.ToString());
            }
        }

        public TimeSpan RoundDuration()
        {
            return _gameTiming.CurTime.Subtract(RoundStartTimeSpan);
        }
    }

    [Serializable, NetSerializable]
    public sealed class TickerJoinLobbyEvent : EntityEventArgs
    {
    }

    [Serializable, NetSerializable]
    public sealed class TickerJoinGameEvent : EntityEventArgs
    {
    }

    [Serializable, NetSerializable]
    public sealed class TickerLateJoinStatusEvent : EntityEventArgs
    {
        // TODO: Make this a replicated CVar, honestly.
        public bool Disallowed { get; }

        public TickerLateJoinStatusEvent(bool disallowed)
        {
            Disallowed = disallowed;
        }
    }

    [Serializable, NetSerializable]
    public sealed class TickerInGameInfoEvent : EntityEventArgs
    {
        public string InGameTextBlob { get; }

        public TickerInGameInfoEvent(string textBlob)
        {
            InGameTextBlob = textBlob;
        }
    }
    [Serializable, NetSerializable]
    public sealed class TickerConnectionStatusEvent : EntityEventArgs
    {
        public TimeSpan RoundStartTimeSpan { get; }
        public TickerConnectionStatusEvent(TimeSpan roundStartTimeSpan)
        {
            RoundStartTimeSpan = roundStartTimeSpan;
        }
    }

    [Serializable, NetSerializable]
    public sealed class TickerLobbyStatusEvent : EntityEventArgs
    {
        public bool IsRoundStarted { get; }
        public LobbyBackgroundPrototype? LobbyBackground { get; } // Goobstation - Lobby Background Credits
        public bool YouAreReady { get; }
        // UTC.
        public TimeSpan StartTime { get; }
        public TimeSpan RoundStartTimeSpan { get; }
        public bool Paused { get; }

        // Goobstation - Lobby Background Credits
        public TickerLobbyStatusEvent(bool isRoundStarted, LobbyBackgroundPrototype? lobbyBackground, bool youAreReady, TimeSpan startTime, TimeSpan preloadTime, TimeSpan roundStartTimeSpan, bool paused)
        {
            IsRoundStarted = isRoundStarted;
            LobbyBackground = lobbyBackground;
            YouAreReady = youAreReady;
            StartTime = startTime;
            RoundStartTimeSpan = roundStartTimeSpan;
            Paused = paused;
        }
    }

    [Serializable, NetSerializable]
    public sealed class TickerLobbyInfoEvent : EntityEventArgs
    {
        public string TextBlob { get; }

        public TickerLobbyInfoEvent(string textBlob)
        {
            TextBlob = textBlob;
        }
    }

    [Serializable, NetSerializable]
    public sealed class TickerLobbyCountdownEvent : EntityEventArgs
    {
        /// <summary>
        /// The game time that the game will start at.
        /// </summary>
        public TimeSpan StartTime { get; }

        /// <summary>
        /// Whether or not the countdown is paused
        /// </summary>
        public bool Paused { get; }

        public TickerLobbyCountdownEvent(TimeSpan startTime, bool paused)
        {
            StartTime = startTime;
            Paused = paused;
        }
    }

    [Serializable, NetSerializable]
    public sealed class TickerJobsAvailableEvent(
        Dictionary<NetEntity, string> stationNames,
        Dictionary<NetEntity, Dictionary<ProtoId<JobPrototype>, int?>> jobsAvailableByStation)
        : EntityEventArgs
    {
        /// <summary>
        /// The Status of the Player in the lobby (ready, observer, ...)
        /// </summary>
        public Dictionary<NetEntity, Dictionary<ProtoId<JobPrototype>, int?>> JobsAvailableByStation { get; } = jobsAvailableByStation;

        public Dictionary<NetEntity, string> StationNames { get; } = stationNames;
    }

    [Serializable, NetSerializable, DataDefinition]
    public sealed partial class RoundEndMessageEvent : EntityEventArgs
    {
        [Serializable, NetSerializable, DataDefinition]
        public partial struct RoundEndPlayerInfo
        {
            [DataField]
            public string PlayerOOCName;

            [DataField]
            public string? PlayerICName;

            [DataField, NonSerialized]
            public NetUserId? PlayerGuid;

            public string Role;

            [DataField, NonSerialized]
            public string[] JobPrototypes;

            [DataField, NonSerialized]
            public string[] AntagPrototypes;

            public NetEntity? PlayerNetEntity;

            [DataField]
            public bool Antag;

            [DataField]
            public bool Observer;

            public bool Connected;
        }

        public string GamemodeTitle { get; }
        public string RoundEndText { get; }
        public TimeSpan RoundDuration { get; }
        public int RoundId { get; }
        public int PlayerCount { get; }
        public RoundEndPlayerInfo[] AllPlayersEndInfo { get; }

        /// <summary>
        /// Sound gets networked due to how entity lifecycle works between client / server and to avoid clipping.
        /// </summary>
        public ResolvedSoundSpecifier? RestartSound;

        public RoundEndMessageEvent(
            string gamemodeTitle,
            string roundEndText,
            TimeSpan roundDuration,
            int roundId,
            int playerCount,
            RoundEndPlayerInfo[] allPlayersEndInfo,
            ResolvedSoundSpecifier? restartSound)
        {
            GamemodeTitle = gamemodeTitle;
            RoundEndText = roundEndText;
            RoundDuration = roundDuration;
            RoundId = roundId;
            PlayerCount = playerCount;
            AllPlayersEndInfo = allPlayersEndInfo;
            RestartSound = restartSound;
        }
    }

    [Serializable, NetSerializable]
    public enum PlayerGameStatus : sbyte
    {
        NotReadyToPlay = 65,
        ReadyToPlay,
        JoinedGame,
    }
}