// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Anomaly.Effects;
using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;

namespace Content.Shared.Anomaly.Components;

/// <summary>
/// On contact with an entity, if it meets the conditions, it will transfer the specified components to it
/// </summary>
[RegisterComponent, Access(typeof(SharedInnerBodyAnomalySystem))]
public sealed partial class InnerBodyAnomalyInjectorComponent : Component
{
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// components that will be automatically removed after “curing”
    /// </summary>
    [DataField(required: true)]
    public ComponentRegistry InjectionComponents = default!;
}