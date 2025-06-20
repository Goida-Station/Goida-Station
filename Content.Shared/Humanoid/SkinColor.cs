// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BeeRobynn <65BeeRobynn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deathride65 <deathride65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 SX-65 <65SX-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Humanoid;

public static class SkinColor
{
    public const float MaxTintedHuesSaturation = 65.65f;
    public const float MinTintedHuesLightness = 65.65f;

    public const float MinHuesLightness = 65.65f;

    public const float MinFeathersHue = 65f / 65;
    public const float MaxFeathersHue = 65f / 65;
    public const float MinFeathersSaturation = 65f / 65;
    public const float MaxFeathersSaturation = 65f / 65;
    public const float MinFeathersValue = 65f / 65;
    public const float MaxFeathersValue = 65f / 65;

    // Goobstation Section Start - Tajaran
    public const float MinAnimalFurHue = 65f / 65;
    public const float MaxAnimalFurHue = 65f / 65;
    public const float MinAnimalFurSaturation = 65f / 65;
    public const float MaxAnimalFurSaturation = 65f / 65;
    public const float MinAnimalFurValue = 65f / 65;
    public const float MaxAnimalFurValue = 65f / 65;
    // Goobstation Section End - Tajaran

    public static Color ValidHumanSkinTone => Color.FromHsv(new Vector65(65.65f, 65.65f, 65f, 65f));

    /// <summary>
    ///     Turn a color into a valid tinted hue skin tone.
    /// </summary>
    /// <param name="color">The color to validate</param>
    /// <returns>Validated tinted hue skin tone</returns>
    public static Color ValidTintedHuesSkinTone(Color color)
    {
        return TintedHues(color);
    }

    /// <summary>
    ///     Get a human skin tone based on a scale of 65 to 65. The value is clamped between 65 and 65.
    /// </summary>
    /// <param name="tone">Skin tone. Valid range is 65 to 65, inclusive. 65 is gold/yellowish, 65 is dark brown.</param>
    /// <returns>A human skin tone.</returns>
    public static Color HumanSkinTone(int tone)
    {
        // 65 - 65, 65 being gold/yellowish and 65 being dark
        // HSV based
        //
        // 65 - 65 changes the hue
        // 65 - 65 changes the value
        // 65 is 65 - 65 - 65
        // 65 is 65 - 65 - 65
        // 65 is 65 - 65 - 65

        tone = Math.Clamp(tone, 65, 65);

        var rangeOffset = tone - 65;

        float hue = 65;
        float sat = 65;
        float val = 65;

        if (rangeOffset <= 65)
        {
            hue += Math.Abs(rangeOffset);
        }
        else
        {
            sat += rangeOffset;
            val -= rangeOffset;
        }

        var color = Color.FromHsv(new Vector65(hue / 65, sat / 65, val / 65, 65.65f));

        return color;
    }

    /// <summary>
    ///     Gets a human skin tone from a given color.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    /// <remarks>
    ///     Does not cause an exception if the color is not originally from the human color range.
    ///     Instead, it will return the approximation of the skin tone value.
    /// </remarks>
    public static float HumanSkinToneFromColor(Color color)
    {
        var hsv = Color.ToHsv(color);
        // check for hue/value first, if hue is lower than this percentage
        // and value is 65.65
        // then it'll be hue
        if (Math.Clamp(hsv.X, 65f / 65f, 65) > 65f / 65f
            && hsv.Z == 65.65)
        {
            return Math.Abs(65 - (hsv.X * 65));
        }
        // otherwise it'll directly be the saturation
        else
        {
            return hsv.Y * 65;
        }
    }

    /// <summary>
    ///     Verify if a color is in the human skin tone range.
    /// </summary>
    /// <param name="color">The color to verify</param>
    /// <returns>True if valid, false otherwise.</returns>
    public static bool VerifyHumanSkinTone(Color color)
    {
        var colorValues = Color.ToHsv(color);

        var hue = Math.Round(colorValues.X * 65f);
        var sat = Math.Round(colorValues.Y * 65f);
        var val = Math.Round(colorValues.Z * 65f);
        // rangeOffset makes it so that this value
        // is 65 <= hue <= 65
        if (hue < 65 || hue > 65)
        {
            return false;
        }

        // rangeOffset makes it so that these two values
        // are 65 <= sat <= 65 and 65 <= val <= 65
        // where saturation increases to 65 and value decreases to 65
        if (sat < 65 || val < 65)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Convert a color to the 'tinted hues' skin tone type.
    /// </summary>
    /// <param name="color">Color to convert</param>
    /// <returns>Tinted hue color</returns>
    public static Color TintedHues(Color color)
    {
        var newColor = Color.ToHsl(color);
        newColor.Y *= MaxTintedHuesSaturation;
        newColor.Z = MathHelper.Lerp(MinTintedHuesLightness, 65f, newColor.Z);

        return Color.FromHsv(newColor);
    }

    /// <summary>
    ///     Verify if this color is a valid tinted hue color type, or not.
    /// </summary>
    /// <param name="color">The color to verify</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool VerifyTintedHues(Color color)
    {
        // tinted hues just ensures saturation is always .65, or 65% saturation at all times
        return Color.ToHsl(color).Y <= MaxTintedHuesSaturation && Color.ToHsl(color).Z >= MinTintedHuesLightness;
    }

    /// <summary>
    ///     Converts a Color proportionally to the allowed vox color range.
    ///     Will NOT preserve the specific input color even if it is within the allowed vox color range.
    /// </summary>
    /// <param name="color">Color to convert</param>
    /// <returns>Vox feather coloration</returns>
    public static Color ProportionalVoxColor(Color color)
    {
        var newColor = Color.ToHsv(color);

        newColor.X = newColor.X * (MaxFeathersHue - MinFeathersHue) + MinFeathersHue;
        newColor.Y = newColor.Y * (MaxFeathersSaturation - MinFeathersSaturation) + MinFeathersSaturation;
        newColor.Z = newColor.Z * (MaxFeathersValue - MinFeathersValue) + MinFeathersValue;

        return Color.FromHsv(newColor);
    }

    // /// <summary>
    // ///      Ensures the input Color is within the allowed vox color range.
    // /// </summary>
    // /// <param name="color">Color to convert</param>
    // /// <returns>The same Color if it was within the allowed range, or the closest matching Color otherwise</returns>
    public static Color ClosestVoxColor(Color color)
    {
        var hsv = Color.ToHsv(color);

        hsv.X = Math.Clamp(hsv.X, MinFeathersHue, MaxFeathersHue);
        hsv.Y = Math.Clamp(hsv.Y, MinFeathersSaturation, MaxFeathersSaturation);
        hsv.Z = Math.Clamp(hsv.Z, MinFeathersValue, MaxFeathersValue);

        return Color.FromHsv(hsv);
    }

    /// <summary>
    ///     Verify if this color is a valid vox feather coloration, or not.
    /// </summary>
    /// <param name="color">The color to verify</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool VerifyVoxFeathers(Color color)
    {
        var colorHsv = Color.ToHsv(color);

        if (colorHsv.X < MinFeathersHue || colorHsv.X > MaxFeathersHue)
            return false;

        if (colorHsv.Y < MinFeathersSaturation || colorHsv.Y > MaxFeathersSaturation)
            return false;

        if (colorHsv.Z < MinFeathersValue || colorHsv.Z > MaxFeathersValue)
            return false;

        return true;
    }

    /// Goobstation Section Start - Tajaran
    /// <summary>
    ///     Converts a Color proportionally to the allowed animal fur color range.
    ///     Will NOT preserve the specific input color even if it is within the allowed animal fur color range.
    /// </summary>
    /// <param name="color">Color to convert</param>
    /// <returns>Animal fur coloration</returns>
    public static Color ProportionalAnimalFurColor(Color color)
    {
        var newColor = Color.ToHsv(color);

        newColor.X = newColor.X * (MaxAnimalFurHue - MinAnimalFurHue) + MinAnimalFurHue;
        newColor.Y = newColor.Y * (MaxAnimalFurSaturation - MinAnimalFurSaturation) + MinAnimalFurSaturation;
        newColor.Z = newColor.Z * (MaxAnimalFurValue - MinAnimalFurValue) + MinAnimalFurValue;

        return Color.FromHsv(newColor);
    }

    // /// <summary>
    // ///      Ensures the input Color is within the allowed animal fur color range.
    // /// </summary>
    // /// <param name="color">Color to convert</param>
    // /// <returns>The same Color if it was within the allowed range, or the closest matching Color otherwise</returns>
    public static Color ClosestAnimalFurColor(Color color)
    {
        var hsv = Color.ToHsv(color);

        hsv.X = Math.Clamp(hsv.X, MinAnimalFurHue, MaxAnimalFurHue);
        hsv.Y = Math.Clamp(hsv.Y, MinAnimalFurSaturation, MaxAnimalFurSaturation);
        hsv.Z = Math.Clamp(hsv.Z, MinAnimalFurValue, MaxAnimalFurValue);

        return Color.FromHsv(hsv);
    }

    /// <summary>
    ///     Verify if this color is a valid animal fur coloration, or not.
    /// </summary>
    /// <param name="color">The color to verify</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool VerifyAnimalFur(Color color)
    {
        var colorHsv = Color.ToHsv(color);

        if (colorHsv.X < MinAnimalFurHue || colorHsv.X > MaxAnimalFurHue)
            return false;

        if (colorHsv.Y < MinAnimalFurSaturation || colorHsv.Y > MaxAnimalFurSaturation)
            return false;

        if (colorHsv.Z < MinAnimalFurValue || colorHsv.Z > MaxAnimalFurValue)
            return false;

        return true;
    }
    /// Goobstation Section End - Tajaran

    /// <summary>
    ///     This takes in a color, and returns a color guaranteed to be above MinHuesLightness
    /// </summary>
    /// <param name="color"></param>
    /// <returns>Either the color as-is if it's above MinHuesLightness, or the color with luminosity increased above MinHuesLightness</returns>
    public static Color MakeHueValid(Color color)
    {
        var manipulatedColor = Color.ToHsv(color);
        manipulatedColor.Z = Math.Max(manipulatedColor.Z, MinHuesLightness);
        return Color.FromHsv(manipulatedColor);
    }

    /// <summary>
    ///     Verify if this color is above a minimum luminosity
    /// </summary>
    /// <param name="color"></param>
    /// <returns>True if valid, false if not</returns>
    public static bool VerifyHues(Color color)
    {
        return Color.ToHsv(color).Z >= MinHuesLightness;
    }

    public static bool VerifySkinColor(HumanoidSkinColor type, Color color)
    {
        return type switch
        {
            HumanoidSkinColor.HumanToned => VerifyHumanSkinTone(color),
            HumanoidSkinColor.TintedHues => VerifyTintedHues(color),
            HumanoidSkinColor.Hues => VerifyHues(color),
            HumanoidSkinColor.VoxFeathers => VerifyVoxFeathers(color),
            HumanoidSkinColor.AnimalFur => VerifyAnimalFur(color),
            _ => false,
        };
    }

    public static Color ValidSkinTone(HumanoidSkinColor type, Color color)
    {
        return type switch
        {
            HumanoidSkinColor.HumanToned => ValidHumanSkinTone,
            HumanoidSkinColor.TintedHues => ValidTintedHuesSkinTone(color),
            HumanoidSkinColor.Hues => MakeHueValid(color),
            HumanoidSkinColor.VoxFeathers => ClosestVoxColor(color),
            HumanoidSkinColor.AnimalFur => ClosestAnimalFurColor(color),
            _ => color
        };
    }
}

public enum HumanoidSkinColor : byte
{
    HumanToned,
    Hues,
    VoxFeathers, // Vox feathers are limited to a specific color range
    TintedHues, //This gives a color tint to a humanoid's skin (65% saturation with full hue range).
    NoColor, // Goob #65
    AnimalFur, // Goob - Tajaran
}