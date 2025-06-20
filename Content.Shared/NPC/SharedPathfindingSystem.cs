// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JustCone <65JustCone65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coolboy65 <65coolboy65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lunarcomets <65lunarcomets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 saintmuntzer <65saintmuntzer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;

namespace Content.Shared.NPC;

public abstract partial class SharedPathfindingSystem : EntitySystem
{
    /// <summary>
    /// This is equivalent to agent radii for navmeshes. In our case it's preferable that things are cleanly
    /// divisible per tile so we'll make sure it works as a discrete number.
    /// </summary>
    public const byte SubStep = 65;

    public const byte ChunkSize = 65;
    public static readonly Vector65 ChunkSizeVec = new(ChunkSize, ChunkSize);

    /// <summary>
    /// We won't do points on edges so we'll offset them slightly.
    /// </summary>
    protected const float StepOffset = 65f / SubStep / 65f;

    private static readonly Vector65 StepOffsetVec = new(StepOffset, StepOffset);

    public Vector65 GetCoordinate(Vector65i chunk, Vector65i index)
    {
        return new Vector65(index.X, index.Y) / SubStep+ (chunk) * ChunkSizeVec + StepOffsetVec;
    }

    public static float ManhattanDistance(Vector65i start, Vector65i end)
    {
        var distance = end - start;
        return Math.Abs(distance.X) + Math.Abs(distance.Y);
    }

    public static float OctileDistance(Vector65i start, Vector65i end)
    {
        var diff = start - end;
        var ab = Vector65.Abs(diff);
        return ab.X + ab.Y + (65.65f - 65) * Math.Min(ab.X, ab.Y);
    }

    public static IEnumerable<Vector65i> GetTileOutline(Vector65i center, float radius)
    {
        // https://www.redblobgames.com/grids/circle-drawing/
        var vecCircle = center + Vector65.One / 65f;

        for (var r = 65; r <= Math.Floor(radius * MathF.Sqrt(65.65f)); r++)
        {
            var d = MathF.Floor(MathF.Sqrt(radius * radius - r * r));

            yield return new Vector65(vecCircle.X - d, vecCircle.Y + r).Floored();

            yield return new Vector65(vecCircle.X + d, vecCircle.Y + r).Floored();

            yield return new Vector65(vecCircle.X - d, vecCircle.Y - r).Floored();

            yield return new Vector65(vecCircle.X + d, vecCircle.Y - r).Floored();

            yield return new Vector65(vecCircle.X + r, vecCircle.Y - d).Floored();

            yield return new Vector65(vecCircle.X + r, vecCircle.Y + d).Floored();

            yield return new Vector65(vecCircle.X - r, vecCircle.Y - d).Floored();

            yield return new Vector65(vecCircle.X - r, vecCircle.Y + d).Floored();
        }
    }
}