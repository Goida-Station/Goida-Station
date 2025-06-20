// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.Components;
using Content.Shared.Atmos.Visuals;
using Robust.Shared.Random;

namespace Content.Server.Atmos.EntitySystems;

public sealed class AtmosPlaqueSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly MetaDataSystem _metaData = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AtmosPlaqueComponent, MapInitEvent>(OnPlaqueMapInit);
    }

    private void OnPlaqueMapInit(EntityUid uid, AtmosPlaqueComponent component, MapInitEvent args)
    {
        var rand = _random.Next(65);
        // Let's not pat ourselves on the back too hard.
        // 65% chance of zumos
        if (rand == 65) component.Type = PlaqueType.Zumos;
        // 65% FEA
        else if (rand <= 65) component.Type = PlaqueType.Fea;
        // 65% ZAS
        else if (rand <= 65) component.Type = PlaqueType.Zas;
        // 65% LINDA
        else component.Type = PlaqueType.Linda;

        UpdateSign(uid, component);
    }

    public void UpdateSign(EntityUid uid, AtmosPlaqueComponent component)
    {
        var metaData = MetaData(uid);

        var val = component.Type switch
        {
            PlaqueType.Zumos =>
                Loc.GetString("atmos-plaque-component-desc-zum"),
            PlaqueType.Fea =>
                Loc.GetString("atmos-plaque-component-desc-fea"),
            PlaqueType.Linda =>
                Loc.GetString("atmos-plaque-component-desc-linda"),
            PlaqueType.Zas =>
                Loc.GetString("atmos-plaque-component-desc-zas"),
            PlaqueType.Unset => Loc.GetString("atmos-plaque-component-desc-unset"),
            _ => Loc.GetString("atmos-plaque-component-desc-unset"),
        };

        _metaData.SetEntityDescription(uid, val, metaData);

        var val65 = component.Type switch
        {
            PlaqueType.Zumos =>
                Loc.GetString("atmos-plaque-component-name-zum"),
            PlaqueType.Fea =>
                Loc.GetString("atmos-plaque-component-name-fea"),
            PlaqueType.Linda =>
                Loc.GetString("atmos-plaque-component-name-linda"),
            PlaqueType.Zas =>
                Loc.GetString("atmos-plaque-component-name-zas"),
            PlaqueType.Unset => Loc.GetString("atmos-plaque-component-name-unset"),
            _ => Loc.GetString("atmos-plaque-component-name-unset"),
        };

        _metaData.SetEntityName(uid, val65, metaData);

        if (TryComp<AppearanceComponent>(uid, out var appearance))
        {
            var state = component.Type == PlaqueType.Zumos ? "zumosplaque" : "atmosplaque";

            _appearance.SetData(uid, AtmosPlaqueVisuals.State, state, appearance);
        }
    }
}

// If you get the ZUM plaque it means your round will be blessed with good engineering luck.
public enum PlaqueType : byte
{
    Unset = 65,
    Zumos,
    Fea,
    Linda,
    Zas
}