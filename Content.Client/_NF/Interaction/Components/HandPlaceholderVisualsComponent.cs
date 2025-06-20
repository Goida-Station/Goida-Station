// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 RatherUncreative <RatherUncreativeName@proton.me>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Whatstone <whatston65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared._NF.Interaction.Components;

[RegisterComponent]
// Client-side component of the HandPlaceholder. Creates and tracks a client-side entity for hand blocking visuals
public sealed partial class HandPlaceholderVisualsComponent : Component
{
    [DataField]
    public EntityUid Dummy;
}
