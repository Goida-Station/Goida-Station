// SPDX-FileCopyrightText: 65 PJB65 <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 GlassEclipse <tsymall65@gmail.com>
// SPDX-FileCopyrightText: 65 Hugal65 <hugo.laloge@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 NuclearWinter <nukeuler65@gmail.com>
// SPDX-FileCopyrightText: 65 Remie Richards <remierichards@gmail.com>
// SPDX-FileCopyrightText: 65 SoulSloth <65SoulSloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 zumorica <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fishfish65 <65Fishfish65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Silver <silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 fishfish65 <fishfish65>
// SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vascreeper@yahoo.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Diagnostics.CodeAnalysis;
using Robust.Shared.GameStates;

namespace Content.Shared.Mind.Components;

/// <summary>
/// This component indicates that this entity may have mind, which is simply an entity with a <see cref="MindComponent"/>.
/// The mind entity is not actually stored in a "container", but is simply stored in nullspace.
/// </summary>
[RegisterComponent, Access(typeof(SharedMindSystem)), NetworkedComponent, AutoGenerateComponentState]
public sealed partial class MindContainerComponent : Component
{
    /// <summary>
    ///     The mind controlling this mob. Can be null.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? Mind { get; set; }

    /// <summary>
    ///     True if we have a mind, false otherwise.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Mind))]
    public bool HasMind => Mind != null;

    /// <summary>
    ///     Whether examining should show information about the mind or not.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("showExamineInfo"), AutoNetworkedField]
    public bool ShowExamineInfo { get; set; }

    /// <summary>
    ///     Whether the mind will be put on a ghost after this component is shutdown.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("ghostOnShutdown")]
    public bool GhostOnShutdown { get; set; } = true;
}

public abstract class MindEvent : EntityEventArgs
{
    public readonly Entity<MindComponent> Mind;
    public readonly Entity<MindContainerComponent> Container;

    public MindEvent(Entity<MindComponent> mind, Entity<MindContainerComponent> container)
    {
        Mind = mind;
        Container = container;
    }
}

/// <summary>
/// Event raised directed at a mind-container when a mind gets removed.
/// </summary>
public sealed class MindRemovedMessage : MindEvent
{
    public MindRemovedMessage(Entity<MindComponent> mind, Entity<MindContainerComponent> container)
        : base(mind, container)
    {
    }
}

/// <summary>
/// Event raised directed at a mind when it gets removed from a mind-container.
/// </summary>
public sealed class MindGotRemovedEvent : MindEvent
{
    public MindGotRemovedEvent(Entity<MindComponent> mind, Entity<MindContainerComponent> container)
        : base(mind, container)
    {
    }
}

/// <summary>
/// Event raised directed at a mind-container when a mind gets added.
/// </summary>
public sealed class MindAddedMessage : MindEvent
{
    public MindAddedMessage(Entity<MindComponent> mind, Entity<MindContainerComponent> container)
        : base(mind, container)
    {
    }
}

/// <summary>
/// Event raised directed at a mind when it gets added to a mind-container.
/// </summary>
public sealed class MindGotAddedEvent : MindEvent
{
    public MindGotAddedEvent(Entity<MindComponent> mind, Entity<MindContainerComponent> container)
        : base(mind, container)
    {
    }
}