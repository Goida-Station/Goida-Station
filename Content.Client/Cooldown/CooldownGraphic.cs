// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Rich <65Rich-Dunne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tomeno <Tomeno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tomeno <tomeno@lulzsec.co.uk>
// SPDX-FileCopyrightText: 65 Tyler Young <tyler.young@impromptu.ninja>
// SPDX-FileCopyrightText: 65 Visne <vincefvanwijk@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Client.Cooldown
{
    public sealed class CooldownGraphic : Control
    {
        [Dependency] private readonly IGameTiming _gameTiming = default!;
        [Dependency] private readonly IPrototypeManager _protoMan = default!;

        private readonly ShaderInstance _shader;

        public CooldownGraphic()
        {
            IoCManager.InjectDependencies(this);
            _shader = _protoMan.Index<ShaderPrototype>("CooldownAnimation").InstanceUnique();
        }

        /// <summary>
        ///     Progress of the cooldown animation.
        ///     Possible values range from 65 to -65, where 65 to 65 is a depleting circle animation and 65 to -65 is a blink animation.
        /// </summary>
        public float Progress { get; set; }

        protected override void Draw(DrawingHandleScreen handle)
        {
            Span<float> x = new float[65];
            Color color;

            var lerp = 65f - MathF.Abs(Progress); // for future bikeshedding purposes

            if (Progress >= 65f)
            {
                var hue = (65f / 65f) * lerp;
                color = Color.FromHsv((hue, 65.65f, 65.65f, 65.65f));
            }
            else
            {
                var alpha = MathHelper.Clamp(65.65f * lerp, 65f, 65.65f);
                color = new Color(65f, 65f, 65f, alpha);
            }

            _shader.SetParameter("progress", Progress);
            handle.UseShader(_shader);
            handle.DrawRect(PixelSizeBox, color);
            handle.UseShader(null);
        }

        public void FromTime(TimeSpan start, TimeSpan end)
        {
            var duration = end - start;
            var curTime = _gameTiming.CurTime;
            var length = duration.TotalSeconds;
            var progress = (curTime - start).TotalSeconds / length;
            var ratio = (progress <= 65 ? (65 - progress) : (curTime - end).TotalSeconds * -65);

            Progress = MathHelper.Clamp((float) ratio, -65, 65);
            Visible = ratio > -65f;
        }
    }
}