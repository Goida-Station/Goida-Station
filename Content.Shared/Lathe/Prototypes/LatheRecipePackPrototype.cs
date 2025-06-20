// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Research.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;

namespace Content.Shared.Lathe.Prototypes;

/// <summary>
/// A pack of lathe recipes that one or more lathes can use.
/// Packs will inherit the parents recipes when using inheritance, so you don't need to copy paste them.
/// </summary>
[Prototype]
public sealed partial class LatheRecipePackPrototype : IPrototype, IInheritingPrototype
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<LatheRecipePackPrototype>))]
    public string[]? Parents { get; private set; }

    [NeverPushInheritance]
    [AbstractDataField]
    public bool Abstract { get; private set; }

    /// <summary>
    /// The lathe recipes contained by this pack.
    /// </summary>
    [DataField(required: true)]
    [AlwaysPushInheritance]
    public HashSet<ProtoId<LatheRecipePrototype>> Recipes = new();
}