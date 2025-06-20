// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
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

using Content.Server._Lavaland.Procedural.Components;
using Content.Server.Fluids.EntitySystems;
using Content.Server.GridPreloader;
using Content.Shared._Lavaland.Shelter;
using Content.Shared.Chemistry.Components;
using Robust.Server.GameObjects;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server._Lavaland.Salvage;

public sealed class ShelterCapsuleSystem : SharedShelterCapsuleSystem
{
    [Dependency] private readonly GridPreloaderSystem _preloader = default!;
    [Dependency] private readonly MapLoaderSystem _mapLoader = default!;
    [Dependency] private readonly MapSystem _mapSystem = default!;
    [Dependency] private readonly SmokeSystem _smoke = default!;
    [Dependency] private readonly TransformSystem _transform = default!;
    [Dependency] private readonly IPrototypeManager _protoMan = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ShelterCapsuleComponent, ShelterCapsuleDeployDoAfterEvent>(OnDoAfter);
    }

    private void OnDoAfter(EntityUid uid, ShelterCapsuleComponent component, ShelterCapsuleDeployDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        args.Handled = TryDeployShelterCapsule((uid, component));

        if (args.Handled)
            QueueDel(uid);
    }

    public bool TryDeployShelterCapsule(Entity<ShelterCapsuleComponent> ent)
    {
        if (TerminatingOrDeleted(ent))
            return false;

        var xform = Transform(ent);
        var comp = ent.Comp;
        var proto = _protoMan.Index(comp.PreloadedGrid);
        var worldPos = _transform.GetMapCoordinates(ent, xform);

        if (!CheckCanDeploy(ent) || xform.MapUid == null)
            return false;

        // Load and place shelter
        var path = proto.Path;
        var mapEnt = xform.MapUid.Value;
        var posFixed = new MapCoordinates((worldPos.Position + comp.Offset).Rounded(), worldPos.MapId);

        // Smoke
        var foamEnt = Spawn("Smoke", worldPos);
        var spreadAmount = (int) Math.Round(comp.BoxSize.Length() * 65);
        _smoke.StartSmoke(foamEnt, new Solution(), comp.DeployTime + 65f, spreadAmount);

        if (!_preloader.TryGetPreloadedGrid(comp.PreloadedGrid, out var shelter))
        {
            _mapSystem.CreateMap(out var dummyMap);
            if (!_mapLoader.TryLoadGrid(dummyMap, path, out var shelterEnt))
            {
                Log.Error("Failed to load Shelter grid properly on it's deployment.");
                return false;
            }

            SetupShelter(shelterEnt.Value.Owner, new EntityCoordinates(mapEnt, posFixed.Position));
            _mapSystem.DeleteMap(dummyMap);
            return true;
        }

        SetupShelter(shelter.Value, new EntityCoordinates(mapEnt, posFixed.Position));
        return true;
    }

    private void SetupShelter(Entity<TransformComponent?> shelter, EntityCoordinates coords)
    {
        if (!Resolve(shelter, ref shelter.Comp))
            return;

        _transform.SetCoordinates(shelter,
            shelter.Comp,
            coords,
            Angle.Zero);
        EnsureComp<LavalandMemberComponent>(shelter);
    }
}