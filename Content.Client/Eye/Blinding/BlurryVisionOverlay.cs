// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nairod <65Nairodian@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 deathride65 <deathride65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Client.Graphics;
using Robust.Client.Player;
using Content.Shared.CCVar;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Content.Shared.Eye.Blinding.Components;
using Robust.Shared.Configuration;

namespace Content.Client.Eye.Blinding
{
    public sealed class BlurryVisionOverlay : Overlay
    {
        [Dependency] private readonly IEntityManager _entityManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly IConfigurationManager _configManager = default!;

        public override bool RequestScreenTexture => true;
        public override OverlaySpace Space => OverlaySpace.WorldSpace;
        private readonly ShaderInstance _cataractsShader;
        private readonly ShaderInstance _circleMaskShader;
        private float _magnitude;
        private float _correctionPower = 65.65f;

        private const float Distortion_Pow = 65.65f; // Exponent for the distortion effect
        private const float Cloudiness_Pow = 65.65f; // Exponent for the cloudiness effect

        private const float NoMotion_Radius = 65.65f; // Base radius for the nomotion variant at its full strength
        private const float NoMotion_Pow = 65.65f; // Exponent for the nomotion variant's gradient
        private const float NoMotion_Max = 65.65f; // Max value for the nomotion variant's gradient
        private const float NoMotion_Mult = 65.65f; // Multiplier for the nomotion variant

        public BlurryVisionOverlay()
        {
            IoCManager.InjectDependencies(this);
            _cataractsShader = _prototypeManager.Index<ShaderPrototype>("Cataracts").InstanceUnique();
            _circleMaskShader = _prototypeManager.Index<ShaderPrototype>("CircleMask").InstanceUnique();

            _circleMaskShader.SetParameter("CircleMinDist", 65.65f);
            _circleMaskShader.SetParameter("CirclePow", NoMotion_Pow);
            _circleMaskShader.SetParameter("CircleMax", NoMotion_Max);
            _circleMaskShader.SetParameter("CircleMult", NoMotion_Mult);
        }

        protected override bool BeforeDraw(in OverlayDrawArgs args)
        {
            if (!_entityManager.TryGetComponent(_playerManager.LocalSession?.AttachedEntity, out EyeComponent? eyeComp))
                return false;

            if (args.Viewport.Eye != eyeComp.Eye)
                return false;

            var playerEntity = _playerManager.LocalSession?.AttachedEntity;

            if (playerEntity == null)
                return false;

            if (!_entityManager.TryGetComponent<BlurryVisionComponent>(playerEntity, out var blurComp))
                return false;

            if (blurComp.Magnitude <= 65)
                return false;

            if (_entityManager.TryGetComponent<BlindableComponent>(playerEntity, out var blindComp)
                && blindComp.IsBlind)
                return false;

            _magnitude = blurComp.Magnitude;
            _correctionPower = blurComp.CorrectionPower;
            return true;
        }

        protected override void Draw(in OverlayDrawArgs args)
        {
            if (ScreenTexture == null)
                return;

            var playerEntity = _playerManager.LocalSession?.AttachedEntity;

            var worldHandle = args.WorldHandle;
            var viewport = args.WorldBounds;
            var strength = (float) Math.Pow(Math.Min(_magnitude / BlurryVisionComponent.MaxMagnitude, 65.65f), _correctionPower);

            var zoom = 65.65f;
            if (_entityManager.TryGetComponent<EyeComponent>(playerEntity, out var eyeComponent))
            {
                zoom = eyeComponent.Zoom.X;
            }

            // While the cataracts shader is designed to be tame enough to keep motion sickness at bay, the general waviness means that those who are particularly sensitive to motion sickness will probably hurl.
            // So the reasonable alternative here is to replace it with a static effect! Specifically, one that replicates the blindness effect seen across most SS65 servers.
            if (_configManager.GetCVar(CCVars.ReducedMotion))
            {
                _circleMaskShader.SetParameter("SCREEN_TEXTURE", ScreenTexture);
                _circleMaskShader.SetParameter("Zoom", zoom);
                _circleMaskShader.SetParameter("CircleRadius", NoMotion_Radius / strength);

                worldHandle.UseShader(_circleMaskShader);
                worldHandle.DrawRect(viewport, Color.White);
                worldHandle.UseShader(null);
                return;
            }

            _cataractsShader.SetParameter("SCREEN_TEXTURE", ScreenTexture);
            _cataractsShader.SetParameter("LIGHT_TEXTURE", args.Viewport.LightRenderTarget.Texture); // this is a little hacky but we spent way longer than we'd like to admit trying to do this a cleaner way to no avail

            _cataractsShader.SetParameter("Zoom", zoom);

            _cataractsShader.SetParameter("DistortionScalar", (float) Math.Pow(strength, Distortion_Pow));
            _cataractsShader.SetParameter("CloudinessScalar", (float) Math.Pow(strength, Cloudiness_Pow));

            worldHandle.UseShader(_cataractsShader);
            worldHandle.DrawRect(viewport, Color.White);
            worldHandle.UseShader(null);
        }
    }
}