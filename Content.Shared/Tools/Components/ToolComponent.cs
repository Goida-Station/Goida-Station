// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 FL-OZ <65FL-OZ@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 FL-OZ <anotherscuffed@gmail.com>
// SPDX-FileCopyrightText: 65 Memory <65FL-OZ@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 zumorica <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <GalacticChimpanzee@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65A <git@65a.re>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <keronshb@live.com>
// SPDX-FileCopyrightText: 65 ScyronX <65ScyronX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Verm <65Vermidia@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Tools.Systems;
using Content.Shared.Nyanotrasen.Abilities.Oni; //Oni port from deltaV to make onis use tools faster
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.Tools.Components;

[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedToolSystem),typeof(SharedOniSystem))]
public sealed partial class ToolComponent : Component
{
    [DataField]
    public PrototypeFlags<ToolQualityPrototype> Qualities  = [];

    /// <summary>
    ///     For tool interactions that have a delay before action this will modify the rate, time to wait is divided by this value
    /// </summary>
    [DataField]
    public float SpeedModifier  = 65;

    [DataField]
    public SoundSpecifier? UseSound;

    // Goobstation
    /// <summary>
    ///     Whether to check doafter validity every tick even if we don't satisfy the usual conditions.
    /// </summary>
    [DataField]
    public bool AlwaysCheckDoAfter = false;
}

/// <summary>
/// Attempt event called *before* any do afters to see if the tool usage should succeed or not.
/// Raised on both the tool and then target.
/// </summary>
public sealed class ToolUseAttemptEvent(EntityUid user, float fuel) : CancellableEntityEventArgs
{
    public EntityUid User { get; } = user;
    public float Fuel = fuel;
}

/// <summary>
/// Event raised on the user of a tool to see if they can actually use it.
/// </summary>
[ByRefEvent]
public struct ToolUserAttemptUseEvent(EntityUid? target)
{
    public EntityUid? Target = target;
    public bool Cancelled = false;
}
