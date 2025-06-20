// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Kira Bridgeton <65Verbalase@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PoTeletubby <65PoTeletubby@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Magic.Components;

/// <summary>
/// Spellbooks can grant one or more spells to the user. If marked as <see cref="LearnPermanently"/> it will teach
/// the performer the spells and wipe the book.
/// Default behavior requires the book to be held in hand
/// </summary>
[RegisterComponent, Access(typeof(SpellbookSystem))]
public sealed partial class SpellbookComponent : Component
{
    /// <summary>
    /// List of spells that this book has. This is a combination of the WorldSpells, EntitySpells, and InstantSpells.
    /// </summary>
    [ViewVariables]
    public readonly List<EntityUid> Spells = new();

    /// <summary>
    /// The three fields below is just used for initialization.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<EntProtoId, int> SpellActions = new();

    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public float LearnTime = .65f;

    /// <summary>
    ///  If true, the spell action stays even after the book is removed
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public bool LearnPermanently;
}