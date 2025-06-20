// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Fluids.UI;
using Content.Client.Items;
using Content.Shared.Fluids;

namespace Content.Client.Fluids;

/// <inheritdoc/>
public sealed class AbsorbentSystem : SharedAbsorbentSystem
{
    public override void Initialize()
    {
        base.Initialize();
        Subs.ItemStatus<AbsorbentComponent>(ent => new AbsorbentItemStatus(ent, EntityManager));
    }
}