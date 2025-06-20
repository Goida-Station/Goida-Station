// SPDX-FileCopyrightText: 65 Phill65 <65Phill65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Phill65 <holypics65@gmail.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.CrewManifest;
using Content.Server.Station.Systems;
using Content.Shared.CartridgeLoader;
using Content.Shared.CartridgeLoader.Cartridges;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Server.CartridgeLoader.Cartridges;

public sealed class CrewManifestCartridgeSystem : EntitySystem
{
    [Dependency] private readonly CartridgeLoaderSystem _cartridgeLoader = default!;
    [Dependency] private readonly IConfigurationManager _configManager = default!;
    [Dependency] private readonly CrewManifestSystem _crewManifest = default!;
    [Dependency] private readonly StationSystem _stationSystem = default!;

    [ValidatePrototypeId<EntityPrototype>]
    private const string CartridgePrototypeName = "CrewManifestCartridge";

    /// <summary>
    /// Flag that shows that if crew manifest is allowed to be viewed from 'unsecure' entities,
    /// which is the keys for the cartridge.
    /// </summary>
    private bool _unsecureViewersAllowed = true;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<CrewManifestCartridgeComponent, CartridgeMessageEvent>(OnUiMessage);
        SubscribeLocalEvent<CrewManifestCartridgeComponent, CartridgeUiReadyEvent>(OnUiReady);
        SubscribeLocalEvent<ProgramInstallationAttempt>(OnInstallationAttempt);
        Subs.CVar(_configManager, CCVars.CrewManifestUnsecure, OnCrewManifestUnsecureChanged, true);
    }

    /// <summary>
    /// The ui messages received here get wrapped by a CartridgeMessageEvent and are relayed from the <see cref="CartridgeLoaderSystem"/>
    /// </summary>
    /// <remarks>
    /// The cartridge specific ui message event needs to inherit from the CartridgeMessageEvent
    /// </remarks>
    private void OnUiMessage(EntityUid uid, CrewManifestCartridgeComponent component, CartridgeMessageEvent args)
    {
        UpdateUiState(uid, GetEntity(args.LoaderUid), component);
    }

    /// <summary>
    /// This gets called when the ui fragment needs to be updated for the first time after activating
    /// </summary>
    private void OnUiReady(EntityUid uid, CrewManifestCartridgeComponent component, CartridgeUiReadyEvent args)
    {
        UpdateUiState(uid, args.Loader, component);
    }

    private void UpdateUiState(EntityUid uid, EntityUid loaderUid, CrewManifestCartridgeComponent? component)
    {
        if (!Resolve(uid, ref component))
            return;

        var owningStation = _stationSystem.GetOwningStation(uid);

        if (owningStation is null)
            return;

        var (stationName, entries) = _crewManifest.GetCrewManifest(owningStation.Value);

        var state = new CrewManifestUiState(stationName, entries);
        _cartridgeLoader.UpdateCartridgeUiState(loaderUid, state);
    }

    private void OnInstallationAttempt(ref ProgramInstallationAttempt args)
    {
        if (args.Prototype == CartridgePrototypeName && !_unsecureViewersAllowed)
            args.Cancelled = true;
    }

    private void OnCrewManifestUnsecureChanged(bool unsecureViewersAllowed)
    {
        _unsecureViewersAllowed = unsecureViewersAllowed;

        var allCartridgeLoaders = AllEntityQuery<CartridgeLoaderComponent, ContainerManagerComponent>();
        while (allCartridgeLoaders.MoveNext(out var loaderUid, out var comp, out var cont))
        {
            if (_unsecureViewersAllowed)
            {
                _cartridgeLoader.InstallProgram(loaderUid, CartridgePrototypeName, false, comp);
                return;
            }

            if (_cartridgeLoader.TryGetProgram<CrewManifestCartridgeComponent>(loaderUid, out var program, true, comp, cont))
                _cartridgeLoader.UninstallProgram(loaderUid, program.Value, comp);
        }
    }
}