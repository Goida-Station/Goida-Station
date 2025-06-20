// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Chemistry.Reaction;

/// <summary>
/// This is a prototype for a method of chemical mixing, to be used by <see cref="ReactionMixerComponent"/>
/// </summary>
[Prototype]
public sealed partial class MixingCategoryPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// A locale string used in the guidebook to describe this mixing category.
    /// </summary>
    [DataField(required: true)]
    public LocId VerbText;

    /// <summary>
    /// An icon used to represent this mixing category in the guidebook.
    /// </summary>
    [DataField(required: true)]
    public SpriteSpecifier Icon = default!;
}