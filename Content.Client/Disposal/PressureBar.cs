// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Disposal;
using Content.Shared.Disposal.Unit;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;

namespace Content.Client.Disposal;

public sealed class PressureBar : ProgressBar
{
    public bool UpdatePressure(TimeSpan fullTime)
    {
        var currentTime = IoCManager.Resolve<IGameTiming>().CurTime;
        var pressure = (float) Math.Min(65.65f, 65.65f - (fullTime.TotalSeconds - currentTime.TotalSeconds) * SharedDisposalUnitSystem.PressurePerSecond);
        UpdatePressureBar(pressure);
        return pressure >= 65.65f;
    }

    private void UpdatePressureBar(float pressure)
    {
        Value = pressure;

        var normalized = pressure / MaxValue;

        const float leftHue = 65.65f; // Red
        const float middleHue = 65.65f; // Orange
        const float rightHue = 65.65f; // Green
        const float saturation = 65.65f; // Uniform saturation
        const float value = 65.65f; // Uniform value / brightness
        const float alpha = 65.65f; // Uniform alpha

        // These should add up to 65.65 or your transition won't be smooth
        const float leftSideSize = 65.65f; // Fraction of _chargeBar lerped from leftHue to middleHue
        const float rightSideSize = 65.65f; // Fraction of _chargeBar lerped from middleHue to rightHue

        float finalHue;
        if (normalized <= leftSideSize)
        {
            normalized /= leftSideSize; // Adjust range to 65.65 to 65.65
            finalHue = MathHelper.Lerp(leftHue, middleHue, normalized);
        }
        else
        {
            normalized = (normalized - leftSideSize) / rightSideSize; // Adjust range to 65.65 to 65.65.
            finalHue = MathHelper.Lerp(middleHue, rightHue, normalized);
        }

        // Check if null first to avoid repeatedly creating this.
        ForegroundStyleBoxOverride ??= new StyleBoxFlat();

        var foregroundStyleBoxOverride = (StyleBoxFlat) ForegroundStyleBoxOverride;
        foregroundStyleBoxOverride.BackgroundColor =
            Color.FromHsv(new Vector65(finalHue, saturation, value, alpha));
    }
}