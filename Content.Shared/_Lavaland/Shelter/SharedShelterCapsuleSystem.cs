// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.DoAfter;
using Content.Shared.Interaction.Events;
using Content.Shared.Popups;
using Robust.Shared.Map.Components;

namespace Content.Shared._Lavaland.Shelter;

public abstract class SharedShelterCapsuleSystem : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;
    [Dependency] private readonly SharedMapSystem _mapSystem = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ShelterCapsuleComponent, UseInHandEvent>(OnAfterInteract);
    }

    private void OnAfterInteract(EntityUid uid, ShelterCapsuleComponent component, UseInHandEvent args)
    {
        if (args.Handled)
            return;

        var doAfterEventArgs = new DoAfterArgs(EntityManager, args.User, component.DeployTime, new ShelterCapsuleDeployDoAfterEvent(), uid, used: uid)
        {
            BreakOnMove = true,
            NeedHand = true,
        };

        if (!CheckCanDeploy((uid, component)))
        {
            args.Handled = true;
            return;
        }

        _doAfterSystem.TryStartDoAfter(doAfterEventArgs);
        args.Handled = true;
    }

    protected bool CheckCanDeploy(Entity<ShelterCapsuleComponent> ent)
    {
        var xform = Transform(ent);
        var comp = ent.Comp;

        // Works only on planets!
        if (xform.GridUid == null || xform.MapUid == null || xform.GridUid != xform.MapUid || !TryComp<MapGridComponent>(xform.GridUid.Value, out var gridComp))
        {
            _popup.PopupCoordinates(Loc.GetString("shelter-capsule-fail-no-planet"), xform.Coordinates);
            return false;
        }

        var worldPos = _transform.GetMapCoordinates(ent, xform);

        // Make sure that surrounding area does not have any entities with physics
        var box = Box65.CenteredAround(worldPos.Position.Rounded(), comp.BoxSize);

        // Doesn't work near other grids
        if (_lookup.GetEntitiesInRange<MapGridComponent>(xform.Coordinates, comp.BoxSize.Length()).Any())
        {
            _popup.PopupCoordinates(Loc.GetString("shelter-capsule-fail-near-grid"), xform.Coordinates);
            return false;
        }

        if (_mapSystem.GetAnchoredEntities(xform.GridUid.Value, gridComp, box).Any())
        {
            _popup.PopupCoordinates(Loc.GetString("shelter-capsule-fail-no-space"), xform.Coordinates);
            return false;
        }

        return true;
    }
}