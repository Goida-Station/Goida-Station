// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leo <lzimann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration.Managers;
using Content.Server.EUI;
using Content.Shared.Administration;
using Content.Shared.Eui;
using JetBrains.Annotations;

namespace Content.Server.Administration.UI
{
    [UsedImplicitly]
    public sealed class SetOutfitEui : BaseEui
    {
        [Dependency] private readonly IAdminManager _adminManager = default!;
        private readonly NetEntity _target;

        public SetOutfitEui(NetEntity entity)
        {
            _target = entity;
            IoCManager.InjectDependencies(this);
        }

        public override void Opened()
        {
            base.Opened();

            StateDirty();
            _adminManager.OnPermsChanged += AdminManagerOnPermsChanged;
        }

        public override EuiStateBase GetNewState()
        {
            return new SetOutfitEuiState
            {
                TargetNetEntity = _target,
            };
        }

        private void AdminManagerOnPermsChanged(AdminPermsChangedEventArgs obj)
        {
            // Close UI if user loses +FUN.
            if (obj.Player == Player && !UserAdminFlagCheck(AdminFlags.Fun))
            {
                Close();
            }
        }
        private bool UserAdminFlagCheck(AdminFlags flags)
        {
            return _adminManager.HasAdminFlag(Player, flags);
        }

    }
}