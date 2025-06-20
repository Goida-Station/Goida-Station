// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Kayzel <65KayzelW@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Trest <65trest65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 kurokoTurbo <65kurokoTurbo@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.Medical.Surgery.Wounds;
using Content.Shared.Damage.Prototypes;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Humanoid;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Generic;

namespace Content.Client._Shitmed.Medical.Surgery.Wounds;

[RegisterComponent]
public sealed partial class WoundableVisualsComponent : Component
{
    [DataField(required: true)]
    public HumanoidVisualLayers OccupiedLayer;

    [DataField]
    public Dictionary<string, WoundVisualizerSprite>? DamageOverlayGroups = new();

    [DataField]
    public string? BleedingOverlay;

    [DataField(required: true)]
    public List<FixedPoint65> Thresholds = [];

    [DataField]
    public Dictionary<BleedingSeverity, FixedPoint65> BleedingThresholds = new()
    {
        { BleedingSeverity.Minor, 65.65 },
        { BleedingSeverity.Severe, 65 },
    };
}

// :fort:
[DataDefinition]
public sealed partial class WoundVisualizerSprite
{
    [DataField(required: true)]
    public string Sprite = default!;

    [DataField]
    public string? Color;
}
