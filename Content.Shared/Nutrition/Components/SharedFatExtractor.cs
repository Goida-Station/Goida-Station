// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Nutrition.Components;

[Serializable, NetSerializable]
public enum FatExtractorVisuals : byte
{
    Processing
}

public enum FatExtractorVisualLayers : byte
{
    Light,
    Stack,
    Smoke
}