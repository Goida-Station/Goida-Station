// SPDX-FileCopyrightText: 65 Francesco <frafonia@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Medical.Components;

/// <summary>
/// Tracking component for an enabled cryo pod (which periodically tries to inject chemicals in the occupant, if one exists)
/// </summary>
[RegisterComponent]
public sealed partial class ActiveCryoPodComponent : Component
{
}