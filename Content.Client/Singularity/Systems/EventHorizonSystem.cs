// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Saphire <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Singularity.EntitySystems;
using Content.Shared.Singularity.Components;

namespace Content.Client.Singularity.Systems;

/// <summary>
/// The client-side version of <see cref="SharedEventHorizonSystem"/>.
/// Primarily manages <see cref="EventHorizonComponent"/>s.
/// Exists to make relevant signal handlers (ie: <see cref="SharedEventHorizonSystem.OnPreventCollide"/>) work on the client.
/// </summary>
public sealed class EventHorizonSystem : SharedEventHorizonSystem
{}