// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.IoC;
using Robust.Shared.Network;

namespace Content.Goobstation.Shared.Redial;

public abstract class SharedRedialManager : IPostInjectInit
{
    [Dependency] protected readonly INetManager _netManager = default!;

    public void PostInject()
    {
        Initialize();
    }

    public virtual void Initialize()
    {

    }
}