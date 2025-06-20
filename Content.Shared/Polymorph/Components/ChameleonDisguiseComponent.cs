// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Polymorph.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Polymorph.Components;

/// <summary>
/// Component added to disguise entities.
/// Used by client to copy over appearance from the disguise's source entity.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedChameleonProjectorSystem))]
[AutoGenerateComponentState(true)]
public sealed partial class ChameleonDisguiseComponent : Component
{
    /// <summary>
    /// The user of this disguise.
    /// </summary>
    [DataField]
    public EntityUid User;

    /// <summary>
    /// The projector that created this disguise.
    /// </summary>
    [DataField]
    public EntityUid Projector;

    /// <summary>
    /// The disguise source entity for copying the sprite.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid SourceEntity;

    /// <summary>
    /// The source entity's prototype.
    /// Used as a fallback if the source entity was deleted.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntProtoId? SourceProto;
}