// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DeviceLinking;
using Robust.Shared.Prototypes;

namespace Content.Server.Shuttles.Components;

[RegisterComponent]
public sealed partial class DockingSignalControlComponent : Component
{
    /// <summary>
    /// Output port that is high while docked.
    /// </summary>
    [DataField]
    public ProtoId<SourcePortPrototype> DockStatusSignalPort = "DockStatus";
}