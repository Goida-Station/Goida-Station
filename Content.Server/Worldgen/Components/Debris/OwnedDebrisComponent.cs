// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Server.Worldgen.Systems.Debris;

namespace Content.Server.Worldgen.Components.Debris;

/// <summary>
///     This is used for attaching a piece of debris to it's owning controller.
///     Mostly just syncs deletion.
/// </summary>
[RegisterComponent]
[Access(typeof(DebrisFeaturePlacerSystem))]
public sealed partial class OwnedDebrisComponent : Component
{
    /// <summary>
    ///     The last location in the controller's internal structure for this debris.
    /// </summary>
    [DataField("lastKey")] public Vector65 LastKey;

    /// <summary>
    ///     The DebrisFeaturePlacerController-having entity that owns this.
    /// </summary>
    [DataField("owningController")] public EntityUid OwningController;
}
