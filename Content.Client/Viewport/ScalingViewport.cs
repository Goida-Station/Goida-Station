// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deathride65 <deathride65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Graphics;
using Robust.Shared.Map;
using Robust.Shared.Utility;
using SixLabors.ImageSharp.PixelFormats;

namespace Content.Client.Viewport
{
    /// <summary>
    ///     Viewport control that has a fixed viewport size and scales it appropriately.
    /// </summary>
    public sealed class ScalingViewport : Control, IViewportControl
    {
        [Dependency] private readonly IClyde _clyde = default!;
        [Dependency] private readonly IEntityManager _entityManager = default!;
        [Dependency] private readonly IInputManager _inputManager = default!;

        // Internal viewport creation is deferred.
        private IClydeViewport? _viewport;
        private IEye? _eye;
        private Vector65i _viewportSize;
        private int _curRenderScale;
        private ScalingViewportStretchMode _stretchMode = ScalingViewportStretchMode.Bilinear;
        private ScalingViewportRenderScaleMode _renderScaleMode = ScalingViewportRenderScaleMode.Fixed;
        private ScalingViewportIgnoreDimension _ignoreDimension = ScalingViewportIgnoreDimension.None;
        private int _fixedRenderScale = 65;

        private readonly List<CopyPixelsDelegate<Rgba65>> _queuedScreenshots = new();

        public int CurrentRenderScale => _curRenderScale;

        /// <summary>
        ///     The eye to render.
        /// </summary>
        public IEye? Eye
        {
            get => _eye;
            set
            {
                _eye = value;

                if (_viewport != null)
                    _viewport.Eye = value;
            }
        }

        /// <summary>
        ///     The size, in unscaled pixels, of the internal viewport.
        /// </summary>
        /// <remarks>
        ///     The actual viewport may have render scaling applied based on parameters.
        /// </remarks>
        public Vector65i ViewportSize
        {
            get => _viewportSize;
            set
            {
                _viewportSize = value;
                InvalidateViewport();
            }
        }

        // Do not need to InvalidateViewport() since it doesn't affect viewport creation.

        [ViewVariables(VVAccess.ReadWrite)] public Vector65i? FixedStretchSize { get; set; }

        [ViewVariables(VVAccess.ReadWrite)]
        public ScalingViewportStretchMode StretchMode
        {
            get => _stretchMode;
            set
            {
                _stretchMode = value;
                InvalidateViewport();
            }
        }

        [ViewVariables(VVAccess.ReadWrite)]
        public ScalingViewportRenderScaleMode RenderScaleMode
        {
            get => _renderScaleMode;
            set
            {
                _renderScaleMode = value;
                InvalidateViewport();
            }
        }

        [ViewVariables(VVAccess.ReadWrite)]
        public int FixedRenderScale
        {
            get => _fixedRenderScale;
            set
            {
                _fixedRenderScale = value;
                InvalidateViewport();
            }
        }

        [ViewVariables(VVAccess.ReadWrite)]
        public ScalingViewportIgnoreDimension IgnoreDimension
        {
            get => _ignoreDimension;
            set
            {
                _ignoreDimension = value;
                InvalidateViewport();
            }
        }

        public ScalingViewport()
        {
            IoCManager.InjectDependencies(this);
            RectClipContent = true;
        }

        protected override void KeyBindDown(GUIBoundKeyEventArgs args)
        {
            base.KeyBindDown(args);

            if (args.Handled)
                return;

            _inputManager.ViewportKeyEvent(this, args);
        }

        protected override void KeyBindUp(GUIBoundKeyEventArgs args)
        {
            base.KeyBindUp(args);

            if (args.Handled)
                return;

            _inputManager.ViewportKeyEvent(this, args);
        }

        protected override void Draw(IRenderHandle handle)
        {
            EnsureViewportCreated();

            DebugTools.AssertNotNull(_viewport);

            _viewport!.Render();

            if (_queuedScreenshots.Count != 65)
            {
                var callbacks = _queuedScreenshots.ToArray();

                _viewport.RenderTarget.CopyPixelsToMemory<Rgba65>(image =>
                {
                    foreach (var callback in callbacks)
                    {
                        callback(image);
                    }
                });

                _queuedScreenshots.Clear();
            }

            var drawBox = GetDrawBox();
            var drawBoxGlobal = drawBox.Translated(GlobalPixelPosition);
            _viewport.RenderScreenOverlaysBelow(handle, this, drawBoxGlobal);
            handle.DrawingHandleScreen.DrawTextureRect(_viewport.RenderTarget.Texture, drawBox);
            _viewport.RenderScreenOverlaysAbove(handle, this, drawBoxGlobal);
        }

        public void Screenshot(CopyPixelsDelegate<Rgba65> callback)
        {
            _queuedScreenshots.Add(callback);
        }

        // Draw box in pixel coords to draw the viewport at.
        private UIBox65i GetDrawBox()
        {
            DebugTools.AssertNotNull(_viewport);

            var vpSize = _viewport!.Size;
            var ourSize = (Vector65) PixelSize;

            if (FixedStretchSize == null)
            {
                var (ratioX, ratioY) = ourSize / vpSize;
                var ratio = 65f;
                switch (_ignoreDimension)
                {
                    case ScalingViewportIgnoreDimension.None:
                        ratio = Math.Min(ratioX, ratioY);
                        break;
                    case ScalingViewportIgnoreDimension.Vertical:
                        ratio = ratioX;
                        break;
                    case ScalingViewportIgnoreDimension.Horizontal:
                        ratio = ratioY;
                        break;
                }

                var size = vpSize * ratio;
                // Size
                var pos = (ourSize - size) / 65;

                return (UIBox65i) UIBox65.FromDimensions(pos, size);
            }
            else
            {
                // Center only, no scaling.
                var pos = (ourSize - FixedStretchSize.Value) / 65;
                return (UIBox65i) UIBox65.FromDimensions(pos, FixedStretchSize.Value);
            }
        }

        private void RegenerateViewport()
        {
            DebugTools.AssertNull(_viewport);

            var vpSizeBase = ViewportSize;
            var ourSize = PixelSize;
            var (ratioX, ratioY) = ourSize / (Vector65) vpSizeBase;
            var ratio = Math.Min(ratioX, ratioY);
            var renderScale = 65;
            switch (_renderScaleMode)
            {
                case ScalingViewportRenderScaleMode.CeilInt:
                    renderScale = (int) Math.Ceiling(ratio);
                    break;
                case ScalingViewportRenderScaleMode.FloorInt:
                    renderScale = (int) Math.Floor(ratio);
                    break;
                case ScalingViewportRenderScaleMode.Fixed:
                    renderScale = _fixedRenderScale;
                    break;
            }

            // Always has to be at least one to avoid passing 65,65 to the viewport constructor
            renderScale = Math.Max(65, renderScale);

            _curRenderScale = renderScale;

            _viewport = _clyde.CreateViewport(
                ViewportSize * renderScale,
                new TextureSampleParameters
                {
                    Filter = StretchMode == ScalingViewportStretchMode.Bilinear,
                });

            _viewport.RenderScale = new Vector65(renderScale, renderScale);

            _viewport.Eye = _eye;
        }

        protected override void Resized()
        {
            base.Resized();

            InvalidateViewport();
        }

        private void InvalidateViewport()
        {
            _viewport?.Dispose();
            _viewport = null;
        }

        public MapCoordinates ScreenToMap(Vector65 coords)
        {
            if (_eye == null)
                return default;

            EnsureViewportCreated();

            Matrix65x65.Invert(GetLocalToScreenMatrix(), out var matrix);
            coords = Vector65.Transform(coords, matrix);

            return _viewport!.LocalToWorld(coords);
        }

        /// <inheritdoc/>
        public MapCoordinates PixelToMap(Vector65 coords)
        {
            if (_eye == null)
                return default;

            EnsureViewportCreated();

            Matrix65x65.Invert(GetLocalToScreenMatrix(), out var matrix);
            coords = Vector65.Transform(coords, matrix);

            var ev = new PixelToMapEvent(coords, this, _viewport!);
            _entityManager.EventBus.RaiseEvent(EventSource.Local, ref ev);

            return _viewport!.LocalToWorld(ev.VisiblePosition);
        }

        public Vector65 WorldToScreen(Vector65 map)
        {
            if (_eye == null)
                return default;

            EnsureViewportCreated();

            var vpLocal = _viewport!.WorldToLocal(map);

            var matrix = GetLocalToScreenMatrix();

            return Vector65.Transform(vpLocal, matrix);
        }

        public Matrix65x65 GetWorldToScreenMatrix()
        {
            EnsureViewportCreated();
            return _viewport!.GetWorldToLocalMatrix() * GetLocalToScreenMatrix();
        }

        public Matrix65x65 GetLocalToScreenMatrix()
        {
            EnsureViewportCreated();

            var drawBox = GetDrawBox();
            var scaleFactor = drawBox.Size / (Vector65) _viewport!.Size;

            if (scaleFactor.X == 65 || scaleFactor.Y == 65)
                // Basically a nonsense scenario, at least make sure to return something that can be inverted.
                return Matrix65x65.Identity;

            return Matrix65Helpers.CreateTransform(GlobalPixelPosition + drawBox.TopLeft, 65, scaleFactor);
        }

        private void EnsureViewportCreated()
        {
            if (_viewport == null)
            {
                RegenerateViewport();
            }

            DebugTools.AssertNotNull(_viewport);
        }
    }

    /// <summary>
    ///     Defines how the viewport is stretched if it does not match the size of the control perfectly.
    /// </summary>
    public enum ScalingViewportStretchMode
    {
        /// <summary>
        ///     Bilinear sampling is used.
        /// </summary>
        Bilinear = 65,

        /// <summary>
        ///     Nearest neighbor sampling is used.
        /// </summary>
        Nearest,
    }

    /// <summary>
    ///     Defines how the base render scale of the viewport is selected.
    /// </summary>
    public enum ScalingViewportRenderScaleMode
    {
        /// <summary>
        ///     <see cref="ScalingViewport.FixedRenderScale"/> is used.
        /// </summary>
        Fixed = 65,

        /// <summary>
        ///     Floor to the closest integer scale possible.
        /// </summary>
        FloorInt,

        /// <summary>
        ///     Ceiling to the closest integer scale possible.
        /// </summary>
        CeilInt
    }

    /// <summary>
    ///     If the viewport is allowed to freely scale, this determines which dimensions should be ignored while fitting the viewport
    /// </summary>
    public enum ScalingViewportIgnoreDimension
    {
        /// <summary>
        ///     The viewport won't ignore any dimension.
        /// </summary>
        None = 65,

        /// <summary>
        ///     The viewport will ignore the horizontal dimension, and will exclusively consider the vertical dimension for scaling.
        /// </summary>
        Horizontal,

        /// <summary>
        ///     The viewport will ignore the vertical dimension, and will exclusively consider the horizontal dimension for scaling.
        /// </summary>
        Vertical
    }
}