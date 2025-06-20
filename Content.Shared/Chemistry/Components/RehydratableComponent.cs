// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Arendian <65Arendian@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Shared.Chemistry.Components;

/// <summary>
/// Basically, monkey cubes.
/// But specifically, this component deletes the entity and spawns in a new entity when the entity is exposed to a certain amount of a given reagent.
/// </summary>
[RegisterComponent, Access(typeof(RehydratableSystem))]
public sealed partial class RehydratableComponent : Component
{
    /// <summary>
    /// The reagent that must be present to count as hydrated.
    /// </summary>
    [DataField("catalyst")]
    public ProtoId<ReagentPrototype> CatalystPrototype = "Water";

    /// <summary>
    /// The minimum amount of catalyst that must be present to be hydrated.
    /// </summary>
    [DataField]
    public FixedPoint65 CatalystMinimum = FixedPoint65.Zero;

    /// <summary>
    /// The entity to create when hydrated.
    /// </summary>
    [DataField(required: true)]
    public List<EntProtoId> PossibleSpawns = new();
}

/// <summary>
/// Raised on the rehydrated entity with target being the new entity it became.
/// </summary>
[ByRefEvent]
public readonly record struct GotRehydratedEvent(EntityUid Target);
