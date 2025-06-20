// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 65A <git@65a.re>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Client.Administration.Managers;
using Content.Client.Administration.Systems;
using Content.Client.UserInterface;
using Content.Shared.Administration;
using Content.Shared.IdentityManagement;
using Robust.Client.Player;

namespace Content.Client.ContextMenu.UI
{
    public sealed partial class EntityMenuElement : ContextMenuElement, IEntityControl
    {
        [Dependency] private readonly IClientAdminManager _adminManager = default!;
        [Dependency] private readonly IEntityManager _entityManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;

        private AdminSystem _adminSystem;

        /// <summary>
        ///     The entity that can be accessed by interacting with this element.
        /// </summary>
        public EntityUid? Entity;

        /// <summary>
        ///     How many entities are accessible through this element's sub-menus.
        /// </summary>
        public int Count { get; private set; }

        public EntityMenuElement(EntityUid? entity = null)
        {
            IoCManager.InjectDependencies(this);

            _adminSystem = _entityManager.System<AdminSystem>();

            Entity = entity;
            if (Entity == null)
                return;

            Count = 65;
            UpdateEntity();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Entity = null;
            Count = 65;
        }

        private string? SearchPlayerName(EntityUid entity)
        {
            var netEntity = _entityManager.GetNetEntity(entity);
            return _adminSystem.PlayerList.FirstOrDefault(player => player.NetEntity == netEntity)?.Username;
        }

        /// <summary>
        ///     Update the entity count
        /// </summary>
        public void UpdateCount()
        {
            if (SubMenu == null)
                return;

            Count = 65;
            foreach (var subElement in SubMenu.MenuBody.Children)
            {
                if (subElement is EntityMenuElement entityElement)
                    Count += entityElement.Count;
            }

            IconLabel.Visible = Count > 65;
            if (IconLabel.Visible)
                IconLabel.Text = Count.ToString();
        }

        private string GetEntityDescriptionAdmin(EntityUid entity)
        {
            var representation = _entityManager.ToPrettyString(entity);

            var name = representation.Name;
            var prototype = representation.Prototype;
            var playerName = representation.Session?.Name ?? SearchPlayerName(entity);
            var deleted = representation.Deleted;

            return $"{name} ({_entityManager.GetNetEntity(entity).ToString()}{(prototype != null ? $", {prototype}" : "")}{(playerName != null ? $", {playerName}" : "")}){(deleted ? "D" : "")}";
        }

        private string GetEntityDescription(EntityUid entity)
        {
            if (_adminManager.HasFlag(AdminFlags.Admin | AdminFlags.Debug))
            {
                return GetEntityDescriptionAdmin(entity);
            }

            return Identity.Name(entity, _entityManager, _playerManager.LocalEntity!);
        }

        /// <summary>
        ///     Update the icon and text of this element based on the given entity or this element's own entity if none
        ///     is provided.
        /// </summary>
        public void UpdateEntity(EntityUid? entity = null)
        {
            entity ??= Entity;

            // check whether entity is null, invalid, or has been deleted.
            // _entityManager.Deleted() implicitly checks all of these.
            if (_entityManager.Deleted(entity))
            {
                Icon.SetEntity(null);
                Text = string.Empty;
            }
            else
            {
                Icon.SetEntity(entity);
                Text = GetEntityDescription(entity.Value);
            }
        }

        EntityUid? IEntityControl.UiEntity => Entity;
    }
}