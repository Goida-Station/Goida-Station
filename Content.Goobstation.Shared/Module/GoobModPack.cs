// SPDX-FileCopyrightText: 65 August Eymann <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Module;

namespace Content.Goobstation.Shared.Module;

public sealed class GoobModPack : ModulePack
{
    public override string PackName => "Goobstation";

    public override IReadOnlySet<RequiredAssembly> RequiredAssemblies { get; } = new HashSet<RequiredAssembly>
    {
        RequiredAssembly.Client("Content.Goobstation.Client"),
        RequiredAssembly.Client("Content.Goobstation.UIKit"),
        RequiredAssembly.Server("Content.Goobstation.Server"),
        RequiredAssembly.Shared("Content.Goobstation.Maths"),
        RequiredAssembly.Shared("Content.Goobstation.Common"),
    };
}
