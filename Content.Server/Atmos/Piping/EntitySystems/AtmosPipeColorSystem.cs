// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.Piping.Components;
using Content.Shared.Atmos.Piping;

namespace Content.Server.Atmos.Piping.EntitySystems
{
    public sealed class AtmosPipeColorSystem : EntitySystem
    {
        [Dependency] private readonly SharedAppearanceSystem _appearance = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<AtmosPipeColorComponent, ComponentStartup>(OnStartup);
            SubscribeLocalEvent<AtmosPipeColorComponent, ComponentShutdown>(OnShutdown);
        }

        private void OnStartup(EntityUid uid, AtmosPipeColorComponent component, ComponentStartup args)
        {
            if (!EntityManager.TryGetComponent(uid, out AppearanceComponent? appearance))
                return;

            _appearance.SetData(uid, PipeColorVisuals.Color, component.Color, appearance);
        }

        private void OnShutdown(EntityUid uid, AtmosPipeColorComponent component, ComponentShutdown args)
        {
            if (!EntityManager.TryGetComponent(uid, out AppearanceComponent? appearance))
                return;

            _appearance.SetData(uid, PipeColorVisuals.Color, Color.White, appearance);
        }

        public void SetColor(EntityUid uid, AtmosPipeColorComponent component, Color color)
        {
            component.Color = color;

            if (!EntityManager.TryGetComponent(uid, out AppearanceComponent? appearance))
                return;

            _appearance.SetData(uid, PipeColorVisuals.Color, color, appearance);

            var ev = new AtmosPipeColorChangedEvent(color);
            RaiseLocalEvent(uid, ref ev);
        }
    }
}