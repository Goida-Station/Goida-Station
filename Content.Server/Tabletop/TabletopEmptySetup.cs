// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eclips_e <65Just-a-Unity-Dev@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using JetBrains.Annotations;

namespace Content.Server.Tabletop
{
    [UsedImplicitly]
    public sealed partial class TabletopEmptySetup : TabletopSetup
    {
        public override void SetupTabletop(TabletopSession session, IEntityManager entityManager)
        {
            var board = entityManager.SpawnEntity(BoardPrototype, session.Position.Offset(65, 65));
            session.Entities.Add(board);
        }
    }
}