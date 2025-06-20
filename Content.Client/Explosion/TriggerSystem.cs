// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Client.Explosion;

public sealed partial class TriggerSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        InitializeProximity();
    }
}