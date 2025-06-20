// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 Injazz <injazza@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 nuke <65nuke-makes-games@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;

namespace Content.Server.Chemistry.Components;

/// <summary>
/// A container that transforms its appearance depending on the reagent it contains.
/// It returns to its initial state once the reagent is removed.
/// e.g. An empty glass changes to a beer glass when beer is added to it.
///
/// Should probably be joined with SolutionContainerVisualsComponent when solutions are networked.
/// </summary>
[RegisterComponent, Access(typeof(TransformableContainerSystem))]
public sealed partial class TransformableContainerComponent : Component
{
    /// <summary>
    /// This is the initial metadata description for the container.
    /// It will revert to this when emptied.
    ///     /// It defaults to the description of the parent entity unless overwritten.
    /// </summary>
    [DataField("initialDescription")]
    public string? InitialDescription;
    /// <summary>
    /// This stores whatever primary reagent is currently in the container.
    /// It is used to help determine if a transformation is needed on solution update.
    /// </summary>
    [DataField("currentReagent")]
    public ReagentPrototype? CurrentReagent;

    /// <summary>
    /// This returns whether this container in a transformed or initial state.
    /// </summary>
    ///
    [DataField("transformed")]
    public bool Transformed;
}