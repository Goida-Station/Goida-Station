// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Procedural.PostGeneration;

/// <summary>
/// If external areas are found will try to generate windows.
/// </summary>
/// <remarks>
/// Dungeon data keys are:
/// - EntranceFlank
/// - FallbackTile
/// </remarks>
public sealed partial class ExternalWindowDunGen : IDunGenLayer;