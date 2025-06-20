// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.DungeonLayers;

[Prototype]
public sealed partial class OreDunGenPrototype : OreDunGen, IPrototype
{
    [IdDataField]
    public string ID { set; get; } = default!;
}