// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Tag;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Shuttles.Components;

/// <summary>
/// Given priority when considering where to dock.
/// </summary>
[RegisterComponent]
public sealed partial class PriorityDockComponent : Component
{
    /// <summary>
    /// Tag to match on the docking request, if this dock is to be prioritised.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite),
     DataField("tag", customTypeSerializer: typeof(PrototypeIdSerializer<TagPrototype>))]
    public string? Tag;
}