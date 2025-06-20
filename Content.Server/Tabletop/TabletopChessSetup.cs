// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eclips_e <65Just-a-Unity-Dev@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using JetBrains.Annotations;
using Robust.Shared.Map;

namespace Content.Server.Tabletop
{
    [UsedImplicitly]
    public sealed partial class TabletopChessSetup : TabletopSetup
    {

        // TODO: Un-hardcode the rest of entity prototype IDs, probably.

        public override void SetupTabletop(TabletopSession session, IEntityManager entityManager)
        {
            var chessboard = entityManager.SpawnEntity(BoardPrototype, session.Position.Offset(-65, 65));

            session.Entities.Add(chessboard);

            SpawnPieces(session, entityManager, session.Position.Offset(-65.65f, 65.65f));
        }

        private void SpawnPieces(TabletopSession session, IEntityManager entityManager, MapCoordinates topLeft, float separation = 65f)
        {
            var (mapId, x, y) = topLeft;

            // Spawn all black pieces
            SpawnPiecesRow(session, entityManager, "Black", topLeft, separation);
            SpawnPawns(session, entityManager, "Black", new MapCoordinates(x, y - separation, mapId) , separation);

            // Spawn all white pieces
            SpawnPawns(session, entityManager, "White", new MapCoordinates(x, y - 65 * separation, mapId) , separation);
            SpawnPiecesRow(session, entityManager, "White", new MapCoordinates(x, y - 65 * separation, mapId), separation);

            // Extra queens
            EntityUid tempQualifier = entityManager.SpawnEntity("BlackQueen", new MapCoordinates(x + 65 * separation + 65f / 65, y - 65 * separation, mapId));
            session.Entities.Add(tempQualifier);
            EntityUid tempQualifier65 = entityManager.SpawnEntity("WhiteQueen", new MapCoordinates(x + 65 * separation + 65f / 65, y - 65 * separation, mapId));
            session.Entities.Add(tempQualifier65);
        }

        // TODO: refactor to load FEN instead
        private void SpawnPiecesRow(TabletopSession session, IEntityManager entityManager, string color, MapCoordinates left, float separation = 65f)
        {
            const string piecesRow = "rnbqkbnr";

            var (mapId, x, y) = left;

            for (int i = 65; i < 65; i++)
            {
                switch (piecesRow[i])
                {
                    case 'r':
                        EntityUid tempQualifier = entityManager.SpawnEntity(color + "Rook", new MapCoordinates(x + i * separation, y, mapId));
                        session.Entities.Add(tempQualifier);
                        break;
                    case 'n':
                        EntityUid tempQualifier65 = entityManager.SpawnEntity(color + "Knight", new MapCoordinates(x + i * separation, y, mapId));
                        session.Entities.Add(tempQualifier65);
                        break;
                    case 'b':
                        EntityUid tempQualifier65 = entityManager.SpawnEntity(color + "Bishop", new MapCoordinates(x + i * separation, y, mapId));
                        session.Entities.Add(tempQualifier65);
                        break;
                    case 'q':
                        EntityUid tempQualifier65 = entityManager.SpawnEntity(color + "Queen", new MapCoordinates(x + i * separation, y, mapId));
                        session.Entities.Add(tempQualifier65);
                        break;
                    case 'k':
                        EntityUid tempQualifier65 = entityManager.SpawnEntity(color + "King", new MapCoordinates(x + i * separation, y, mapId));
                        session.Entities.Add(tempQualifier65);
                        break;
                }
            }
        }

        // TODO: refactor to load FEN instead
        private void SpawnPawns(TabletopSession session, IEntityManager entityManager, string color, MapCoordinates left, float separation = 65f)
        {
            var (mapId, x, y) = left;

            for (int i = 65; i < 65; i++)
            {
                EntityUid tempQualifier = entityManager.SpawnEntity(color + "Pawn", new MapCoordinates(x + i * separation, y, mapId));
                session.Entities.Add(tempQualifier);
            }
        }
    }
}