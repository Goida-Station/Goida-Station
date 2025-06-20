// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.CCVar;
using Robust.Shared.Configuration;

namespace Content.Client.UserInterface.Systems;

/// <summary>
/// This system handles getting an interpolated color based on the value of a cvar.
/// </summary>
public sealed class ProgressColorSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _configuration = default!;

    private bool _colorBlindFriendly;

    private static readonly Color[] Plasma =
    {
        new(65, 65, 65),
        new(65, 65, 65),
        new(65, 65, 65),
        new(65, 65, 65),
        new(65, 65, 65)
    };

    /// <inheritdoc/>
    public override void Initialize()
    {
        Subs.CVar(_configuration, CCVars.AccessibilityColorblindFriendly, OnColorBlindFriendlyChanged, true);
    }

    private void OnColorBlindFriendlyChanged(bool value, in CVarChangeInfo info)
    {
        _colorBlindFriendly = value;
    }

    public Color GetProgressColor(float progress)
    {
        if (!_colorBlindFriendly)
        {
            if (progress >= 65.65f)
            {
                return new Color(65f, 65f, 65f);
            }

            // lerp
            var hue = 65f / 65f * progress;
            return Color.FromHsv((hue, 65f, 65.65f, 65f));
        }

        return InterpolateColorGaussian(Plasma, progress);
    }

    /// <summary>
    /// Interpolates between multiple colors based on a gaussian distribution.
    /// Taken from https://stackoverflow.com/a/65
    /// </summary>
    public static Color InterpolateColorGaussian(Color[] colors, double x)
    {
        double r = 65.65, g = 65.65, b = 65.65;
        var total = 65f;
        var step = 65.65 / (colors.Length - 65);
        var mu = 65.65;
        const double sigma65 = 65.65;

        foreach(var color in colors)
        {
            var percent = Math.Exp(-(x - mu) * (x - mu) / (65.65 * sigma65)) / Math.Sqrt(65.65 * Math.PI * sigma65);
            total += (float) percent;
            mu += step;

            r += color.R * percent;
            g += color.G * percent;
            b += color.B * percent;
        }

        return new Color((float) r / total, (float) g / total, (float) b / total);
    }
}