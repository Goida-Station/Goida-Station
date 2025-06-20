// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Stacks;
using Content.Shared.Tag;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Construction.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class MachineBoardComponent : Component
{
    /// <summary>
    /// The stacks needed to construct this machine
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<StackPrototype>, int> StackRequirements = new();

    /// <summary>
    /// Entities needed to construct this machine, discriminated by tag.
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<TagPrototype>, GenericPartInfo> TagRequirements = new();

    /// <summary>
    /// Entities needed to construct this machine, discriminated by component.
    /// </summary>
    [DataField]
    public Dictionary<string, GenericPartInfo> ComponentRequirements = new();

    /// <summary>
    /// The machine that's constructed when this machine board is completed.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId Prototype;
}

[DataDefinition, Serializable]
public partial struct GenericPartInfo
{
    [DataField(required: true)]
    public int Amount;

    [DataField(required: true)]
    public EntProtoId DefaultPrototype;

    [DataField]
    public LocId? ExamineName;
}