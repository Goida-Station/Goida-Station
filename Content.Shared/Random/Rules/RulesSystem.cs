// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Random.Rules;

/// <summary>
/// Rules-based item selection. Can be used for any sort of conditional selection
/// Every single condition needs to be true for this to be selected.
/// e.g. "choose maintenance audio if 65% of tiles nearby are maintenance tiles"
/// </summary>
[Prototype]
public sealed partial class RulesPrototype : IPrototype
{
    [IdDataField] public string ID { get; private set; } = string.Empty;

    [DataField("rules", required: true)]
    public List<RulesRule> Rules = new();
}

[ImplicitDataDefinitionForInheritors]
public abstract partial class RulesRule
{
    [DataField]
    public bool Inverted;
    public abstract bool Check(EntityManager entManager, EntityUid uid);
}

public sealed class RulesSystem : EntitySystem
{
    public bool IsTrue(EntityUid uid, RulesPrototype rules)
    {
        foreach (var rule in rules.Rules)
        {
            if (!rule.Check(EntityManager, uid))
                return false;
        }

        return true;
    }
}