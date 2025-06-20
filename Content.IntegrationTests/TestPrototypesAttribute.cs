// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using JetBrains.Annotations;

namespace Content.IntegrationTests;

/// <summary>
/// Attribute that indicates that a string contains yaml prototype data that should be loaded by integration tests.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
[MeansImplicitUse]
public sealed class TestPrototypesAttribute : Attribute
{
}