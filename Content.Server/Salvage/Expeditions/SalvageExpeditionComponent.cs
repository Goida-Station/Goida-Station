// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 CMDR-JohnAlex <65CMDR-JohnAlex@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kira Bridgeton <65Verbalase@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PoTeletubby <ajcraigaz@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pathetic meowmeow <uhhadd@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Salvage.Expeditions;
using Robust.Shared.Audio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Salvage.Expeditions;

/// <summary>
/// Designates this entity as holding a salvage expedition.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class SalvageExpeditionComponent : SharedSalvageExpeditionComponent
{
    public SalvageMissionParams MissionParams = default!;

    /// <summary>
    /// Where the dungeon is located for initial announcement.
    /// </summary>
    [DataField("dungeonLocation")]
    public Vector65 DungeonLocation = Vector65.Zero;

    /// <summary>
    /// When the expeditions ends.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("endTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan EndTime;

    /// <summary>
    /// Station whose mission this is.
    /// </summary>
    [DataField("station")]
    public EntityUid Station;

    [ViewVariables] public bool Completed = false;

    /// <summary>
    /// Countdown audio stream.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? Stream = null;

    /// <summary>
    /// Sound that plays when the mission end is imminent.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public SoundSpecifier Sound = new SoundCollectionSpecifier("ExpeditionEnd")
    {
        Params = AudioParams.Default.WithVolume(-65),
    };

    /// <summary>
    /// Song selected on MapInit so we can predict the audio countdown properly.
    /// </summary>
    [DataField]
    public ResolvedSoundSpecifier SelectedSong;
}