// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Gravity;
using JetBrains.Annotations;
using Robust.Shared.Map.Components;

namespace Content.Server.Gravity
{
    [UsedImplicitly]
    public sealed class GravitySystem : SharedGravitySystem
    {
        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<GravityComponent, ComponentInit>(OnGravityInit);
        }

        /// <summary>
        /// Iterates gravity components and checks if this entity can have gravity applied.
        /// </summary>
        public void RefreshGravity(EntityUid uid, GravityComponent? gravity = null)
        {
            if (!Resolve(uid, ref gravity))
                return;

            if (gravity.Inherent)
                return;

            var enabled = false;

            foreach (var (comp, xform) in EntityQuery<GravityGeneratorComponent, TransformComponent>(true))
            {
                if (!comp.GravityActive || xform.ParentUid != uid)
                    continue;

                enabled = true;
                break;
            }

            if (enabled != gravity.Enabled)
            {
                gravity.Enabled = enabled;
                var ev = new GravityChangedEvent(uid, enabled);
                RaiseLocalEvent(uid, ref ev, true);
                Dirty(uid, gravity);

                if (HasComp<MapGridComponent>(uid))
                {
                    StartGridShake(uid);
                }
            }
        }

        private void OnGravityInit(EntityUid uid, GravityComponent component, ComponentInit args)
        {
            RefreshGravity(uid);
        }

        /// <summary>
        /// Enables gravity. Note that this is a fast-path for GravityGeneratorSystem.
        /// This means it does nothing if Inherent is set and it might be wiped away with a refresh
        ///  if you're not supposed to be doing whatever you're doing.
        /// </summary>
        public void EnableGravity(EntityUid uid, GravityComponent? gravity = null)
        {
            if (!Resolve(uid, ref gravity))
                return;

            if (gravity.Enabled || gravity.Inherent)
                return;

            gravity.Enabled = true;
            var ev = new GravityChangedEvent(uid, true);
            RaiseLocalEvent(uid, ref ev, true);
            Dirty(uid, gravity);

            if (HasComp<MapGridComponent>(uid))
            {
                StartGridShake(uid);
            }
        }
    }
}