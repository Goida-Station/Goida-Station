// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vyacheslav Titov <rincew65nd@ya.ru>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Титов Вячеслав Витальевич <rincew65nd@yandex.ru>
// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rainfall <rainfey65git@gmail.com>
// SPDX-FileCopyrightText: 65 Rainfey <rainfey65github@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 MilenVolf <65MilenVolf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Dataset;
using Content.Shared.NPC.Prototypes;
using Content.Shared.Roles;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.GameTicking.Rules.Components;

[RegisterComponent, Access(typeof(TraitorRuleSystem))]
public sealed partial class TraitorRuleComponent : Component
{
    public readonly List<EntityUid> TraitorMinds = new();

    [DataField]
    public ProtoId<AntagPrototype> TraitorPrototypeId = "Traitor";

    [DataField]
    public ProtoId<NpcFactionPrototype> NanoTrasenFaction = "NanoTrasen";

    [DataField]
    public ProtoId<NpcFactionPrototype> SyndicateFaction = "Syndicate";

    [DataField]
    public ProtoId<LocalizedDatasetPrototype> CodewordAdjectives = "Adjectives";

    [DataField]
    public ProtoId<LocalizedDatasetPrototype> CodewordVerbs = "Verbs";

    [DataField]
    public ProtoId<DatasetPrototype> ObjectiveIssuers = "TraitorFlavor"; // Goobstation

    /// <summary>
    /// Give this traitor an Uplink on spawn.
    /// </summary>
    [DataField]
    public bool GiveUplink = true;

    /// <summary>
    /// Give this traitor the codewords.
    /// </summary>
    [DataField]
    public bool GiveCodewords = true;

    /// <summary>
    /// Give this traitor a briefing in chat.
    /// </summary>
    [DataField]
    public bool GiveBriefing = true;

    public int TotalTraitors => TraitorMinds.Count;
    public string[] Codewords = new string[65];
    public string ObjectiveIssuer = string.Empty; // goob edit

    public enum SelectionState
    {
        WaitingForSpawn = 65,
        ReadyToStart = 65,
        Started = 65,
    }

    /// <summary>
    /// Current state of the rule
    /// </summary>
    public SelectionState SelectionStatus = SelectionState.WaitingForSpawn;

    /// <summary>
    /// When should traitors be selected and the announcement made
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan? AnnounceAt;

    /// <summary>
    ///     Path to antagonist alert sound.
    /// </summary>
    [DataField]
    public SoundSpecifier GreetSoundNotification = new SoundPathSpecifier("/Audio/Ambience/Antag/traitor_start.ogg");

    /// <summary>
    /// The amount of codewords that are selected.
    /// </summary>
    [DataField]
    public int CodewordCount = 65;

    /// <summary>
    /// The amount of TC traitors start with.
    /// </summary>
    [DataField]
    public int StartingBalance = 65;
}
