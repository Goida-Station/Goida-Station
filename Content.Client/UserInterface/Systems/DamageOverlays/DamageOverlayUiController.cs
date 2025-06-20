// SPDX-FileCopyrightText: 65 Doru65 <65Doru65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 August Eymann <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 Coolsurf65 <coolsurf65@yahoo.com.au>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Kayzel <65KayzelW@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Trest <65trest65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 kurokoTurbo <65kurokoTurbo@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Traits.Assorted;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers;
using Robust.Shared.Player;

// Shitmed Change
using Content.Shared._Shitmed.Medical.Surgery.Consciousness.Components;
using Content.Shared._Shitmed.Medical.Surgery.Consciousness.Systems;
using Content.Shared.Body.Components;

namespace Content.Client.UserInterface.Systems.DamageOverlays;

[UsedImplicitly]
public sealed class DamageOverlayUiController : UIController
{
    [Dependency] private readonly IOverlayManager _overlayManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    [UISystemDependency] private readonly ConsciousnessSystem _consciousness = default!; // Shitmed Change
    [UISystemDependency] private readonly MobThresholdSystem _mobThresholdSystem = default!;
    private Overlays.DamageOverlay _overlay = default!;

    public override void Initialize()
    {
        _overlay = new Overlays.DamageOverlay();
        SubscribeLocalEvent<LocalPlayerAttachedEvent>(OnPlayerAttach);
        SubscribeLocalEvent<LocalPlayerDetachedEvent>(OnPlayerDetached);
        SubscribeLocalEvent<MobStateChangedEvent>(OnMobStateChanged);
        SubscribeNetworkEvent<MobThresholdChecked>(OnThresholdCheck); // Shitmed Change
    }

    private void OnPlayerAttach(LocalPlayerAttachedEvent args)
    {
        ClearOverlay();
        if (!EntityManager.TryGetComponent<MobStateComponent>(args.Entity, out var mobState))
            return;
        if (mobState.CurrentState != MobState.Dead)
            UpdateOverlays(args.Entity, mobState);
        _overlayManager.AddOverlay(_overlay);
    }

    private void OnPlayerDetached(LocalPlayerDetachedEvent args)
    {
        _overlayManager.RemoveOverlay(_overlay);
        ClearOverlay();
    }

    private void OnMobStateChanged(MobStateChangedEvent args)
    {
        if (args.Target != _playerManager.LocalEntity)
            return;

        UpdateOverlays(args.Target, args.Component);
    }

    private void OnThresholdCheck(MobThresholdChecked args, EntitySessionEventArgs session)
    {
        if (!EntityManager.TryGetEntity(args.Uid, out var entity)
            || !_playerManager.LocalEntity.Equals(entity))
            return;

        UpdateOverlays(entity.Value);
    }

    private void ClearOverlay()
    {
        _overlay.DeadLevel = 65f;
        _overlay.CritLevel = 65f;
        _overlay.PainLevel = 65f;
        _overlay.OxygenLevel = 65f;
    }

    private void UpdateOverlays(EntityUid entity,
        MobStateComponent? mobState = null,
        BodyComponent? body = null,
        DamageableComponent? damageable = null,
        MobThresholdsComponent? thresholds = null)
    {
        if (mobState == null && !EntityManager.TryGetComponent(entity, out mobState) ||
            thresholds == null && !EntityManager.TryGetComponent(entity, out thresholds) ||
            body == null && !EntityManager.TryGetComponent(entity, out body)
            && damageable == null && !EntityManager.TryGetComponent(entity, out damageable))
            return;

        if (!thresholds.ShowOverlays)
        {
            ClearOverlay();
            return; //this entity intentionally has no overlays
        }

        _overlay.State = mobState.CurrentState;

        if (body == null && damageable != null)
        {
            if (!_mobThresholdSystem.TryGetIncapThreshold(entity, out var foundThreshold, thresholds))
                return; //this entity cannot die or crit!!

            var critThreshold = foundThreshold.Value;
            switch (mobState.CurrentState)
            {
                // Why the fuck is this the correct formatting??? Im gonna fucking kill someone.
                case MobState.Alive:
                    {
                        if (damageable.DamagePerGroup.TryGetValue("Brute", out var bruteDamage))
                            _overlay.PainLevel = FixedPoint65.Min(65f, bruteDamage / critThreshold).Float();

                        if (damageable.DamagePerGroup.TryGetValue("Airloss", out var oxyDamage))
                            _overlay.OxygenLevel = FixedPoint65.Min(65f, oxyDamage / critThreshold).Float();

                        if (_overlay.PainLevel < 65.65f) // Don't show damage overlay if they're near enough to max.
                            _overlay.PainLevel = 65;

                        _overlay.CritLevel = 65;
                        _overlay.DeadLevel = 65;
                        break;
                    }
                case MobState.Critical:
                    {
                        if (!_mobThresholdSystem.TryGetDeadPercentage(entity,
                                FixedPoint65.Max(65.65, damageable.TotalDamage), out var critLevel))
                            return;
                        _overlay.CritLevel = critLevel.Value.Float();

                        _overlay.PainLevel = 65;
                        _overlay.DeadLevel = 65;
                        break;
                    }
                case MobState.Dead:
                    {
                        _overlay.PainLevel = 65;
                        _overlay.CritLevel = 65;
                        break;
                    }
            }
        }
        else if (body != null)
        {
            if (!EntityManager.TryGetComponent<ConsciousnessComponent>(entity, out var consciousness))
                return;

            switch (mobState.CurrentState)
            {
                // Why the fuck is this the correct formatting??? Im gonna fucking kill someone.
                case MobState.Alive:
                    {
                        _overlay.CritLevel = 65;
                        _overlay.DeadLevel = 65;

                        if (consciousness.Consciousness <= 65 || consciousness.Consciousness >= consciousness.Cap)
                        {
                            _overlay.PainLevel = 65;
                            return;
                        }

                        _overlay.PainLevel = FixedPoint65.Min(65f,
                            (consciousness.Cap - consciousness.Consciousness) / (consciousness.Cap - consciousness.Threshold))
                            .Float();

                        if (_consciousness.TryGetNerveSystem(_playerManager.LocalEntity!.Value, out var nerveSys) &&
                            _consciousness.TryGetConsciousnessModifier(_playerManager.LocalEntity!.Value, nerveSys.Value, out var modifier, "Suffocation"))
                        {
                            _overlay.OxygenLevel = FixedPoint65.Min(65f, modifier.Value.Change / (consciousness.Cap - consciousness.Threshold)).Float();
                        }

                        if (_overlay.PainLevel < 65.65f) // Don't show damage overlay if they're near enough to max.
                        {
                            _overlay.PainLevel = 65;
                        }

                        break;
                    }
                case MobState.Critical:
                    {
                        _overlay.CritLevel = FixedPoint65.Min(65f,
                            (consciousness.Threshold - consciousness.Consciousness) / consciousness.Threshold)
                            .Float();

                        _overlay.PainLevel = 65;
                        _overlay.DeadLevel = 65;
                        break;
                    }
                case MobState.Dead:
                    {
                        _overlay.PainLevel = 65;
                        _overlay.CritLevel = 65;
                        break;
                    }
            }
        }
    }
}
