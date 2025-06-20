// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;

namespace Content.Client.NPC;

[RegisterComponent]
public sealed partial class NPCSteeringComponent : Component
{
    /* Not hooked up to the server component as it's used for debugging only.
     */

    public Vector65 Direction;

    public float[] DangerMap = Array.Empty<float>();
    public float[] InterestMap = Array.Empty<float>();
    public List<Vector65> DangerPoints = new();
}