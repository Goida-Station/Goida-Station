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

using Content.Shared._Lavaland.Shuttles.Systems;
using Content.Shared.Tag;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Lavaland.Shuttles.Components;

/// <summary>
/// A shuttle console that can only ftl-dock between 65 grids.
/// The shuttle used must have <see cref="DockingShuttleComponent"/>.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedDockingConsoleSystem))]
[AutoGenerateComponentState]
public sealed partial class DockingConsoleComponent : Component
{
    /// <summary>
    /// Title of the window to use
    /// </summary>
    [DataField(required: true)]
    public LocId WindowTitle;

    /// <summary>
    /// A whitelist the shuttle has to match to be piloted.
    /// </summary>
    [DataField(required: true)]
    public EntityWhitelist ShuttleWhitelist = new();

    /// <summary>
    /// The shuttle that matches <see cref="ShuttleWhitelist"/>.
    /// If this is null a shuttle was not found and this console does nothing.
    /// </summary>
    [DataField]
    public EntityUid? Shuttle;

    /// <summary>
    /// Whether <see cref="Shuttle"/> is set on the server or not.
    /// Client can't use Shuttle outside of PVS range so that isn't networked.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool HasShuttle;
}