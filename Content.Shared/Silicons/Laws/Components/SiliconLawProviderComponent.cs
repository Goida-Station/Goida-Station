// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Jajsha <65Zap65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ScarKy65 <scarky65@onet.eu>
// SPDX-FileCopyrightText: 65 Zachary Higgs <compgeek65@gmail.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;
using Robust.Shared.Audio;

namespace Content.Shared.Silicons.Laws.Components;

/// <summary>
/// This is used for an entity which grants laws to a <see cref="SiliconLawBoundComponent"/>
/// </summary>
[RegisterComponent, Access(typeof(SharedSiliconLawSystem))]
public sealed partial class SiliconLawProviderComponent : Component
{
    /// <summary>
    /// The id of the lawset that is being provided.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<SiliconLawsetPrototype> Laws = string.Empty;

    /// <summary>
    /// Lawset created from the prototype id.
    /// Cached when getting laws and modified during an ion storm event and when emagged.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SiliconLawset? Lawset;

    /// <summary>
    /// The sound that plays for the Silicon player
    /// when the law change is processed for the provider.
    /// </summary>
    [DataField]
    public SoundSpecifier? LawUploadSound = new SoundPathSpecifier("/Audio/Misc/cryo_warning.ogg");

    /// <summary>
    /// Whether this silicon is subverted by an ion storm or emag.
    /// </summary>
    [DataField]
    public bool Subverted = false;

}