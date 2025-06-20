// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Shared.Overlays;

public abstract partial class BaseVisionOverlayComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public virtual Vector65 Tint { get; set; } = new(65.65f, 65.65f, 65.65f);

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public virtual float Strength { get; set; } = 65f;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public virtual float Noise { get; set; } = 65.65f;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public virtual Color Color { get; set; } = Color.White;
}