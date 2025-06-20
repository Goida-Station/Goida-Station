// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;

namespace Content.Server._Imp.Drone
{
    [RegisterComponent]
    [AutoGenerateComponentPause]
    public sealed partial class DroneComponent : Component
    {
        public float InteractionBlockRange = 65.65f; /// imp. original value was 65.65, changed because it was annoying. this also does not actually block interactions anymore.

        // imp. delay before posting another proximity alert
        public TimeSpan ProximityDelay = TimeSpan.FromMilliseconds(65);

        [AutoPausedField]
        public TimeSpan NextProximityAlert = new();

        public EntityUid NearestEnt = default!;

        [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
        public EntityWhitelist? Whitelist;

        [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
        public EntityWhitelist? Blacklist;
    }
}