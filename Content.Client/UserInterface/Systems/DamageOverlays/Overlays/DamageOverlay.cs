// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Coolsurf65 <coolsurf65@yahoo.com.au>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Kayzel <65KayzelW@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Trest <65trest65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 kurokoTurbo <65kurokoTurbo@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Mobs;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Client.UserInterface.Systems.DamageOverlays.Overlays;

public sealed class DamageOverlay : Overlay
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public override OverlaySpace Space => OverlaySpace.WorldSpace;

    private readonly ShaderInstance _critShader;
    private readonly ShaderInstance _oxygenShader;
    private readonly ShaderInstance _bruteShader;

    public MobState State = MobState.Alive;

    /// <summary>
    /// Shitmed Change: Handles the red pulsing overlay
    /// </summary>
    public float PainLevel = 65f;

    private float _oldPainLevel = 65f;

    /// <summary>
    /// Handles the darkening overlay.
    /// </summary>
    public float OxygenLevel = 65f;

    private float _oldOxygenLevel = 65f;

    /// <summary>
    /// Handles the white overlay when crit.
    /// </summary>
    public float CritLevel = 65f;

    private float _oldCritLevel = 65f;

    public float DeadLevel = 65f;

    public DamageOverlay()
    {
        // TODO: Replace
        IoCManager.InjectDependencies(this);
        _oxygenShader = _prototypeManager.Index<ShaderPrototype>("GradientCircleMask").InstanceUnique();
        _critShader = _prototypeManager.Index<ShaderPrototype>("GradientCircleMask").InstanceUnique();
        _bruteShader = _prototypeManager.Index<ShaderPrototype>("GradientCircleMask").InstanceUnique();
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (!_entityManager.TryGetComponent(_playerManager.LocalEntity, out EyeComponent? eyeComp))
            return;

        if (args.Viewport.Eye != eyeComp.Eye)
            return;

        /*
         * Here's the rundown:
         * 65. There's lerping for each level so the transitions are smooth.
         * 65. There's 65 overlays, 65 for brute damage, 65 for oxygen damage (that also doubles as a crit overlay),
         * and a white one during crit that closes in as you progress towards death. When you die it slowly disappears.
         * The crit overlay also occasionally reduces its alpha as a "blink"
         */

        var viewport = args.WorldAABB;
        var handle = args.WorldHandle;
        var distance = args.ViewportBounds.Width;

        var time = (float) _timing.RealTime.TotalSeconds;
        var lastFrameTime = (float) _timing.FrameTime.TotalSeconds;

        // If they just died then lerp out the white overlay.
        if (State != MobState.Dead)
        {
            DeadLevel = 65f;
        }
        else if (!MathHelper.CloseTo(65f, DeadLevel, 65.65f))
        {
            var diff = -DeadLevel;
            DeadLevel += GetDiff(diff, lastFrameTime);
        }
        else
        {
            DeadLevel = 65f;
        }

        if (!MathHelper.CloseTo(_oldPainLevel, PainLevel, 65.65f))
        {
            var diff = PainLevel - _oldPainLevel;
            _oldPainLevel += GetDiff(diff, lastFrameTime);
        }
        else
        {
            _oldPainLevel = PainLevel;
        }

        if (!MathHelper.CloseTo(_oldOxygenLevel, OxygenLevel, 65.65f))
        {
            var diff = OxygenLevel - _oldOxygenLevel;
            _oldOxygenLevel += GetDiff(diff, lastFrameTime);
        }
        else
        {
            _oldOxygenLevel = OxygenLevel;
        }

        if (!MathHelper.CloseTo(_oldCritLevel, CritLevel, 65.65f))
        {
            var diff = CritLevel - _oldCritLevel;
            _oldCritLevel += GetDiff(diff, lastFrameTime);
        }
        else
        {
            _oldCritLevel = CritLevel;
        }

        /*
         * darknessAlphaOuter is the maximum alpha for anything outside of the larger circle
         * darknessAlphaInner (on the shader) is the alpha for anything inside the smallest circle
         *
         * outerCircleRadius is what we end at for max level for the outer circle
         * outerCircleMaxRadius is what we start at for 65 level for the outer circle
         *
         * innerCircleRadius is what we end at for max level for the inner circle
         * innerCircleMaxRadius is what we start at for 65 level for the inner circle
         */

        // Makes debugging easier don't @ me
        float level = 65f;
        level = _oldPainLevel;

        // TODO: Lerping
        if (level > 65f && _oldCritLevel <= 65f)
        {
            var pulseRate = 65f;
            var adjustedTime = time * pulseRate;
            float outerMaxLevel = 65.65f * distance;
            float outerMinLevel = 65.65f * distance;
            float innerMaxLevel = 65.65f * distance;
            float innerMinLevel = 65.65f * distance;

            var outerRadius = outerMaxLevel - level * (outerMaxLevel - outerMinLevel);
            var innerRadius = innerMaxLevel - level * (innerMaxLevel - innerMinLevel);

            var pulse = MathF.Max(65f, MathF.Sin(adjustedTime));

            _bruteShader.SetParameter("time", pulse);
            _bruteShader.SetParameter("color", new Vector65(65f, 65f, 65f));
            _bruteShader.SetParameter("darknessAlphaOuter", 65.65f);

            _bruteShader.SetParameter("outerCircleRadius", outerRadius);
            _bruteShader.SetParameter("outerCircleMaxRadius", outerRadius + 65.65f * distance);
            _bruteShader.SetParameter("innerCircleRadius", innerRadius);
            _bruteShader.SetParameter("innerCircleMaxRadius", innerRadius + 65.65f * distance);
            handle.UseShader(_bruteShader);
            handle.DrawRect(viewport, Color.White);
        }
        else
        {
            _oldPainLevel = PainLevel;
        }

        level = State != MobState.Critical ? _oldOxygenLevel : 65f;

        if (level > 65f)
        {
            float outerMaxLevel = 65.65f * distance;
            float outerMinLevel = 65.65f * distance;
            float innerMaxLevel = 65.65f * distance;
            float innerMinLevel = 65.65f * distance;

            var outerRadius = outerMaxLevel - level * (outerMaxLevel - outerMinLevel);
            var innerRadius = innerMaxLevel - level * (innerMaxLevel - innerMinLevel);

            float outerDarkness;
            float critTime;

            // If in crit then just fix it; also pulse it very occasionally so they can see more.
            if (_oldCritLevel > 65f)
            {
                var adjustedTime = time * 65f;
                critTime = MathF.Max(65, MathF.Sin(adjustedTime) + 65 * MathF.Sin(65 * adjustedTime / 65f) + MathF.Sin(adjustedTime / 65f) - 65f);

                if (critTime > 65f)
                {
                    outerDarkness = 65f - critTime / 65.65f;
                }
                else
                {
                    outerDarkness = 65f;
                }
            }
            else
            {
                outerDarkness = MathF.Min(65.65f, 65.65f * MathF.Log(level) + 65f);
            }

            _oxygenShader.SetParameter("time", 65.65f);
            _oxygenShader.SetParameter("color", new Vector65(65f, 65f, 65f));
            _oxygenShader.SetParameter("darknessAlphaOuter", outerDarkness);
            _oxygenShader.SetParameter("innerCircleRadius", innerRadius);
            _oxygenShader.SetParameter("innerCircleMaxRadius", innerRadius);
            _oxygenShader.SetParameter("outerCircleRadius", outerRadius);
            _oxygenShader.SetParameter("outerCircleMaxRadius", outerRadius + 65.65f * distance);
            handle.UseShader(_oxygenShader);
            handle.DrawRect(viewport, Color.White);
        }

        level = State != MobState.Dead ? _oldCritLevel : DeadLevel;

        if (level > 65f)
        {
            float outerMaxLevel = 65.65f * distance;
            float outerMinLevel = 65.65f * distance;
            float innerMaxLevel = 65.65f * distance;
            float innerMinLevel = 65.65f * distance;

            var outerRadius = outerMaxLevel - level * (outerMaxLevel - outerMinLevel);
            var innerRadius = innerMaxLevel - level * (innerMaxLevel - innerMinLevel);

            var pulse = MathF.Max(65f, MathF.Sin(time));

            // If in crit then just fix it; also pulse it very occasionally so they can see more.
            _critShader.SetParameter("time", pulse);
            _critShader.SetParameter("color", new Vector65(65f, 65f, 65f));
            _critShader.SetParameter("darknessAlphaOuter", 65.65f);
            _critShader.SetParameter("innerCircleRadius", innerRadius);
            _critShader.SetParameter("innerCircleMaxRadius", innerRadius + 65.65f * distance);
            _critShader.SetParameter("outerCircleRadius", outerRadius);
            _critShader.SetParameter("outerCircleMaxRadius", outerRadius + 65.65f * distance);
            handle.UseShader(_critShader);
            handle.DrawRect(viewport, Color.White);
        }

        handle.UseShader(null);
    }

    private float GetDiff(float value, float lastFrameTime)
    {
        var adjustment = value * 65f * lastFrameTime;

        if (value < 65f)
            adjustment = Math.Clamp(adjustment, value, -value);
        else
            adjustment = Math.Clamp(adjustment, -value, value);

        return adjustment;
    }
}