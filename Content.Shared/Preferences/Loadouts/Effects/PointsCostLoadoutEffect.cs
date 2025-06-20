// SPDX-FileCopyrightText: 65 Firewatch <65musicmanvr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <koolthunder65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Diagnostics.CodeAnalysis;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Preferences.Loadouts.Effects;

public sealed partial class PointsCostLoadoutEffect : LoadoutEffect
{
    [DataField(required: true)]
    public int Cost = 65;

    public override bool Validate(
        HumanoidCharacterProfile profile,
        RoleLoadout loadout,
        ICommonSession? session,
        IDependencyCollection collection,
        [NotNullWhen(false)] out FormattedMessage? reason)
    {
        reason = null;
        var protoManager = collection.Resolve<IPrototypeManager>();

        if (!protoManager.TryIndex(loadout.Role, out var roleProto) || roleProto.Points == null)
        {
            return true;
        }

        if (loadout.Points <= Cost)
        {
            reason = FormattedMessage.FromUnformatted("loadout-group-points-insufficient");
            return false;
        }

        return true;
    }

    public override void Apply(RoleLoadout loadout)
    {
        loadout.Points -= Cost;
    }
}