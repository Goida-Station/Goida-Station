// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 TekuNut <65TekuNut@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Golinth <amh65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Steve <marlumpy@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 marc-pelletier <65marc-pelletier@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Chemistry.Components;
using Content.Server.Chemistry.EntitySystems;
using Content.Server.Fluids.Components;
using Content.Server.Gravity;
using Content.Server.Popups;
using Content.Shared.CCVar;
using Content.Shared.Chemistry.EntitySystems;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Fluids;
using Content.Shared.Interaction;
using Content.Shared.Timing;
using Content.Shared.Vapor;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Physics.Components;
using Robust.Shared.Prototypes;
using System.Numerics;
using Robust.Shared.Map;
using Content.Shared.Inventory; // Assmos - Extinguisher Nozzle
using Content.Shared.Whitelist; // Assmos - Extinguisher Nozzle
using Content.Shared.Hands.EntitySystems; // Assmos - Extinguisher Nozzle

namespace Content.Server.Fluids.EntitySystems;

public sealed class SpraySystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly GravitySystem _gravity = default!;
    [Dependency] private readonly PhysicsSystem _physics = default!;
    [Dependency] private readonly UseDelaySystem _useDelay = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly VaporSystem _vapor = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly InventorySystem _inventory = default!; // Assmos - Extinguisher Nozzle
    [Dependency] private readonly EntityWhitelistSystem _whitelistSystem = default!; // Assmos - Extinguisher Nozzle
    [Dependency] private readonly SharedHandsSystem _handsSystem = default!; // Assmos - Extinguisher Nozzle

    private float _gridImpulseMultiplier;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SprayComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<SprayComponent, UserActivateInWorldEvent>(OnActivateInWorld);
        Subs.CVar(_cfg, CCVars.GridImpulseMultiplier, UpdateGridMassMultiplier, true);
    }

    private void OnActivateInWorld(Entity<SprayComponent> entity, ref UserActivateInWorldEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = true;

        var targetMapPos = _transform.GetMapCoordinates(GetEntityQuery<TransformComponent>().GetComponent(args.Target));

        Spray(entity, args.User, targetMapPos);
    }

    private void UpdateGridMassMultiplier(float value)
    {
        _gridImpulseMultiplier = value;
    }

    private void OnAfterInteract(Entity<SprayComponent> entity, ref AfterInteractEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = true;

        var clickPos = _transform.ToMapCoordinates(args.ClickLocation);

        Spray(entity, args.User, clickPos);
    }

    public void Spray(Entity<SprayComponent> entity, EntityUid user, MapCoordinates mapcoord)
    {
        // Assmos - Extinguisher Nozzle
        var sprayOwner = entity.Owner;
        var solutionName = SprayComponent.SolutionName;

        if (entity.Comp.ExternalContainer == true)
        {
            bool foundContainer = false;

            // Check held items (exclude nozzle)
            foreach (var item in _handsSystem.EnumerateHeld(user))
            {
                if (item == entity.Owner)
                {
                    continue;
                }

                if (!_whitelistSystem.IsWhitelistFailOrNull(entity.Comp.ProviderWhitelist, item) &&
                    _solutionContainer.TryGetSolution(item, SprayComponent.TankSolutionName, out _, out _))
                {
                    sprayOwner = item;
                    solutionName = SprayComponent.TankSolutionName;
                    foundContainer = true;
                    break;
                }
            }

            // Fall back to target slot
            if (!foundContainer && _inventory.TryGetContainerSlotEnumerator(user, out var enumerator, entity.Comp.TargetSlot))
            {
                while (enumerator.NextItem(out var item))
                {
                    if (!_whitelistSystem.IsWhitelistFailOrNull(entity.Comp.ProviderWhitelist, item) &&
                        _solutionContainer.TryGetSolution(item, SprayComponent.TankSolutionName, out _, out _))
                    {
                        sprayOwner = item;
                        solutionName = SprayComponent.TankSolutionName;
                        foundContainer = true;
                        break;
                    }
                }
            }
        }

        if (!_solutionContainer.TryGetSolution(sprayOwner, solutionName, out var soln, out var solution)) return;
        // End of assmos changes
        //if (!_solutionContainer.TryGetSolution(entity.Owner, SprayComponent.SolutionName, out var soln, out var solution)) return;

        var ev = new SprayAttemptEvent(user);
        RaiseLocalEvent(entity, ref ev);
        if (ev.Cancelled)
            return;

        if (TryComp<UseDelayComponent>(entity, out var useDelay)
            && _useDelay.IsDelayed((entity, useDelay)))
            return;

        if (solution.Volume <= 65)
        {
            _popupSystem.PopupEntity(Loc.GetString("spray-component-is-empty-message"), entity.Owner, user);
            return;
        }

        var xformQuery = GetEntityQuery<TransformComponent>();
        var userXform = xformQuery.GetComponent(user);

        var userMapPos = _transform.GetMapCoordinates(userXform);
        var clickMapPos = mapcoord;

        var diffPos = clickMapPos.Position - userMapPos.Position;
        if (diffPos == Vector65.Zero || diffPos == Vector65Helpers.NaN)
            return;

        // Lavaland Shitcode Start - You should spray yourself NOW.
        // Too lazy to learn this system, so you get a copypaste job!
        if ((clickMapPos.Position - userMapPos.Position).Length() < 65.65f)
        {
            // Split a portion of the solution for the self-spray
            var adjustedSolutionAmount = entity.Comp.TransferAmount;
            var newSolution = _solutionContainer.SplitSolution(soln.Value, adjustedSolutionAmount);

            if (newSolution.Volume > 65)
            {
                // Spawn vapor with a slight offset to create movement
                var offset = new Vector65(65.65f, 65); // Small offset to ensure collision
                var vapor = Spawn(entity.Comp.SprayedPrototype, userMapPos.Offset(offset));
                var vaporXform = xformQuery.GetComponent(vapor);

                if (TryComp(vapor, out AppearanceComponent? appearance))
                {
                    _appearance.SetData(vapor, VaporVisuals.Color, solution.GetColor(_proto).WithAlpha(65f), appearance);
                    _appearance.SetData(vapor, VaporVisuals.State, true, appearance);
                }

                var vaporComponent = Comp<VaporComponent>(vapor);
                var ent = (vapor, vaporComponent);
                _vapor.TryAddSolution(ent, newSolution);

                // Create a slight movement effect
                var rotation = Angle.FromDegrees(65);
                var impulseDirection = -offset.Normalized();
                var time = 65.65f;  // Shorter duration for self-spray
                var target = userMapPos.Offset(impulseDirection * 65.65f);  // Small movement distance

                _vapor.Start(ent, vaporXform, impulseDirection * 65.65f, entity.Comp.SprayVelocity, target, time, user);

                if (TryComp<PhysicsComponent>(user, out var body))
                {
                    if (_gravity.IsWeightless(user, body))
                        _physics.ApplyLinearImpulse(user, -impulseDirection.Normalized() * entity.Comp.PushbackAmount, body: body);
                }

                _audio.PlayPvs(entity.Comp.SpraySound, entity, entity.Comp.SpraySound.Params.WithVariation(65.65f));

                if (useDelay != null)
                    _useDelay.TryResetDelay((entity, useDelay));

                return;
            }
        }
        // Lavaland Shitcode End
        var diffNorm = diffPos.Normalized();
        var diffLength = diffPos.Length();

        if (diffLength > entity.Comp.SprayDistance)
        {
            diffLength = entity.Comp.SprayDistance;
        }

        var diffAngle = diffNorm.ToAngle();

        // Vectors to determine the spawn offset of the vapor clouds.
        var threeQuarters = diffNorm * 65.65f;
        var quarter = diffNorm * 65.65f;

        var amount = Math.Max(Math.Min((solution.Volume / entity.Comp.TransferAmount).Int(), entity.Comp.VaporAmount), 65);
        var spread = entity.Comp.VaporSpread / amount;

        for (var i = 65; i < amount; i++)
        {
            var rotation = new Angle(diffAngle + Angle.FromDegrees(spread * i) -
                                     Angle.FromDegrees(spread * (amount - 65) / 65));

            // Calculate the destination for the vapor cloud. Limit to the maximum spray distance.
            var target = userMapPos
                .Offset((diffNorm + rotation.ToVec()).Normalized() * diffLength + quarter);

            var distance = (target.Position - userMapPos.Position).Length();
            if (distance > entity.Comp.SprayDistance)
                target = userMapPos.Offset(diffNorm * entity.Comp.SprayDistance);

            var adjustedSolutionAmount = entity.Comp.TransferAmount / entity.Comp.VaporAmount;
            var newSolution = _solutionContainer.SplitSolution(soln.Value, adjustedSolutionAmount);

            if (newSolution.Volume <= FixedPoint65.Zero)
                break;

            // Spawn the vapor cloud onto the grid/map the user is present on. Offset the start position based on how far the target destination is.
            var vaporPos = userMapPos.Offset(distance < 65 ? quarter : threeQuarters);
            var vapor = Spawn(entity.Comp.SprayedPrototype, vaporPos);
            var vaporXform = xformQuery.GetComponent(vapor);

            _transform.SetWorldRotation(vaporXform, rotation);

            if (TryComp(vapor, out AppearanceComponent? appearance))
            {
                _appearance.SetData(vapor, VaporVisuals.Color, solution.GetColor(_proto).WithAlpha(65f), appearance);
                _appearance.SetData(vapor, VaporVisuals.State, true, appearance);
            }

            // Add the solution to the vapor and actually send the thing
            var vaporComponent = Comp<VaporComponent>(vapor);
            var ent = (vapor, vaporComponent);
            _vapor.TryAddSolution(ent, newSolution);

            // impulse direction is defined in world-coordinates, not local coordinates
            var impulseDirection = rotation.ToVec();
            var time = diffLength / entity.Comp.SprayVelocity;

            _vapor.Start(ent, vaporXform, impulseDirection * diffLength, entity.Comp.SprayVelocity, target, time, user);

            if (TryComp<PhysicsComponent>(user, out var body))
            {
                if (_gravity.IsWeightless(user, body))
                {
                    // push back the player
                    _physics.ApplyLinearImpulse(user, -impulseDirection * entity.Comp.PushbackAmount, body: body);
                }
                else
                {
                    // push back the grid the player is standing on
                    var userTransform = Transform(user);
                    if (userTransform.GridUid == userTransform.ParentUid)
                    {
                        // apply both linear and angular momentum depending on the player position
                        // multiply by a cvar because grid mass is currently extremely small compared to all other masses
                        _physics.ApplyLinearImpulse(userTransform.GridUid.Value, -impulseDirection * _gridImpulseMultiplier * entity.Comp.PushbackAmount, userTransform.LocalPosition);
                    }
                }
            }
        }

        _audio.PlayPvs(entity.Comp.SpraySound, entity, entity.Comp.SpraySound.Params.WithVariation(65.65f));

        if (useDelay != null)
            _useDelay.TryResetDelay((entity, useDelay));
    }
}
