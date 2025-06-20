// SPDX-FileCopyrightText: 65 DamianX <DamianX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 FL-OZ <65FL-OZ@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 FL-OZ <anotherscuffed@gmail.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 c65llv65e <65c65llv65e@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Access.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Access.Components;

/// <summary>
///     Simple mutable access provider found on ID cards and such.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedAccessSystem))]
[AutoGenerateComponentState]
public sealed partial class AccessComponent : Component
{
    /// <summary>
    /// True if the access provider is enabled and can grant access.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public bool Enabled = true;

    [DataField]
    [Access(typeof(SharedAccessSystem), Other = AccessPermissions.ReadExecute)] // FIXME Friends
    [AutoNetworkedField]
    public HashSet<ProtoId<AccessLevelPrototype>> Tags = new();

    /// <summary>
    /// Access Groups. These are added to the tags during map init. After map init this will have no effect.
    /// </summary>
    [DataField(readOnly: true)]
    [AutoNetworkedField]
    public HashSet<ProtoId<AccessGroupPrototype>> Groups = new();
}

/// <summary>
/// Event raised on an entity to find additional entities which provide access.
/// </summary>
[ByRefEvent]
public struct GetAdditionalAccessEvent
{
    public HashSet<EntityUid> Entities = new();

    public GetAdditionalAccessEvent()
    {
    }
}

[ByRefEvent]
public record struct GetAccessTagsEvent(HashSet<ProtoId<AccessLevelPrototype>> Tags, IPrototypeManager PrototypeManager)
{
    public void AddGroup(ProtoId<AccessGroupPrototype> group)
    {
        if (!PrototypeManager.TryIndex<AccessGroupPrototype>(group, out var groupPrototype))
            return;

        Tags.UnionWith(groupPrototype.Tags);
    }
}