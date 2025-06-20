// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Scribbles65 <65Scribbles65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Shared.Interaction.Events;

/// <summary>
///     Raised Directed at an entity to check whether they will handle the suicide.
/// </summary>
public sealed class SuicideEvent : HandledEntityEventArgs
{
    public SuicideEvent(EntityUid victim)
    {
        Victim = victim;
    }

    public DamageSpecifier? DamageSpecifier;
    public ProtoId<DamageTypePrototype>? DamageType;
    public EntityUid Victim { get; private set; }
}

public sealed class SuicideByEnvironmentEvent : HandledEntityEventArgs
{
    public SuicideByEnvironmentEvent(EntityUid victim)
    {
        Victim = victim;
    }

    public EntityUid Victim { get; set; }
}

public sealed class SuicideGhostEvent : HandledEntityEventArgs
{
    public SuicideGhostEvent(EntityUid victim)
    {
        Victim = victim;
    }

    public EntityUid Victim { get; set; }
    public bool CanReturnToBody;
}