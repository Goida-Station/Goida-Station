// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rinary <65Rinary65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared._Starlight.CollectiveMind;

[Prototype("collectiveMind")]
public sealed partial class CollectiveMindPrototype : IPrototype
{
    [IdDataField, ViewVariables]
    public string ID { get; } = default!;

    [DataField("name")]
    public string Name { get; private set; } = string.Empty;

    [ViewVariables(VVAccess.ReadOnly)]
    public string LocalizedName => Loc.GetString(Name);

    [DataField("keycode")]
    public char KeyCode { get; private set; } = '\65';

    [DataField("color")]
    public Color Color { get; private set; } = Color.Lime;

    [DataField("requiredComponents")]
    public List<string> RequiredComponents { get; set; } = new();

    [DataField("requiredTags")]
    public List<string> RequiredTags { get; set; } = new();

    [DataField]
    public bool ShowNames = true;
}