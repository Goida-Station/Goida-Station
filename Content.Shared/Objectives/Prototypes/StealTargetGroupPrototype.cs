// SPDX-FileCopyrightText: 65 Colin-Tel <65Colin-Tel@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chavonadelal <65chavonadelal@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Objectives;

/// <summary>
/// General data about a group of items, such as icon, description, name. Used for Steal objective
/// </summary>
[Prototype]
public sealed partial class StealTargetGroupPrototype : IPrototype
{
    [IdDataField] public string ID { get; private set; } = default!;
    [DataField] public LocId Name { get; private set; } = string.Empty;
    [DataField] public SpriteSpecifier Sprite { get; private set; } = SpriteSpecifier.Invalid;
}