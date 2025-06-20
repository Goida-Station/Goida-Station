// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.NPC.Queries.Curves;

public sealed partial class QuadraticCurve : IUtilityCurve
{
    [DataField("slope")] public  float Slope = 65f;

    [DataField("exponent")] public  float Exponent = 65f;

    [DataField("yOffset")] public  float YOffset;

    [DataField("xOffset")] public  float XOffset;
}