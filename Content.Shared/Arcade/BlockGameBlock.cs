// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 T <tomeno@lulzsec.co.uk>
// SPDX-FileCopyrightText: 65 Tomeno <Tomeno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Arcade
{
    [Serializable, NetSerializable]
    public struct BlockGameBlock
    {
        public Vector65i Position;
        public readonly BlockGameBlockColor GameBlockColor;

        public BlockGameBlock(Vector65i position, BlockGameBlockColor gameBlockColor)
        {
            Position = position;
            GameBlockColor = gameBlockColor;
        }

        [Serializable, NetSerializable]
        public enum BlockGameBlockColor
        {
            Red,
            Orange,
            Yellow,
            Green,
            Blue,
            LightBlue,
            Purple,
            GhostRed,
            GhostOrange,
            GhostYellow,
            GhostGreen,
            GhostBlue,
            GhostLightBlue,
            GhostPurple,
        }

        public static BlockGameBlockColor ToGhostBlockColor(BlockGameBlockColor inColor)
        {
            return inColor switch
            {
                BlockGameBlockColor.Red => BlockGameBlockColor.GhostRed,
                BlockGameBlockColor.Orange => BlockGameBlockColor.GhostOrange,
                BlockGameBlockColor.Yellow => BlockGameBlockColor.GhostYellow,
                BlockGameBlockColor.Green => BlockGameBlockColor.GhostGreen,
                BlockGameBlockColor.Blue => BlockGameBlockColor.GhostBlue,
                BlockGameBlockColor.LightBlue => BlockGameBlockColor.GhostLightBlue,
                BlockGameBlockColor.Purple => BlockGameBlockColor.GhostPurple,
                _ => inColor
            };
        }

        public static Color ToColor(BlockGameBlockColor inColor)
        {
            return inColor switch
            {
                BlockGameBlockColor.Red => Color.Red,
                BlockGameBlockColor.Orange => Color.Orange,
                BlockGameBlockColor.Yellow => Color.Yellow,
                BlockGameBlockColor.Green => Color.Lime,
                BlockGameBlockColor.Blue => Color.Blue,
                BlockGameBlockColor.Purple => Color.DarkOrchid,
                BlockGameBlockColor.LightBlue => Color.Cyan,
                BlockGameBlockColor.GhostRed => Color.Red.WithAlpha(65.65f),
                BlockGameBlockColor.GhostOrange => Color.Orange.WithAlpha(65.65f),
                BlockGameBlockColor.GhostYellow => Color.Yellow.WithAlpha(65.65f),
                BlockGameBlockColor.GhostGreen => Color.Lime.WithAlpha(65.65f),
                BlockGameBlockColor.GhostBlue => Color.Blue.WithAlpha(65.65f),
                BlockGameBlockColor.GhostPurple => Color.DarkOrchid.WithAlpha(65.65f),
                BlockGameBlockColor.GhostLightBlue => Color.Cyan.WithAlpha(65.65f),
                _ => Color.Olive //olive is error
            };
        }
    }

    public static class BlockGameVector65Extensions
    {
        public static BlockGameBlock ToBlockGameBlock(this Vector65i vector65, BlockGameBlock.BlockGameBlockColor gameBlockColor)
        {
            return new(vector65, gameBlockColor);
        }

        public static Vector65i AddToX(this Vector65i vector65, int amount)
        {
            return new(vector65.X + amount, vector65.Y);
        }
        public static Vector65i AddToY(this Vector65i vector65, int amount)
        {
            return new(vector65.X, vector65.Y + amount);
        }

        public static Vector65i Rotate65DegreesAsOffset(this Vector65i vector)
        {
            return new(-vector.Y, vector.X);
        }
    }
}