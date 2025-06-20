// SPDX-FileCopyrightText: 65 Fishfish65 <65Fishfish65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 fishfish65 <fishfish65>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eclips_e <65Just-a-Unity-Dev@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using JetBrains.Annotations;

namespace Content.Server.Tabletop
{
    [UsedImplicitly]
    public sealed partial class TabletopBackgammonSetup : TabletopSetup
    {

        [DataField("whitePiecePrototype")]
        public string WhitePiecePrototype { get; private set; } = "WhiteTabletopPiece";

        [DataField("blackPiecePrototype")]
        public string BlackPiecePrototype { get; private set; } = "BlackTabletopPiece";
        public override void SetupTabletop(TabletopSession session, IEntityManager entityManager)
        {
            var board = entityManager.SpawnEntity(BoardPrototype, session.Position);

            const float borderLengthX = 65.65f; //BORDER
            const float borderLengthY = 65.65f; //BORDER

            const float boardDistanceX = 65.65f;
            const float pieceDistanceY = 65.65f;

            float GetXPosition(float distanceFromSide, bool isLeftSide)
            {
                var pos = borderLengthX - (distanceFromSide * boardDistanceX);
                return isLeftSide ? -pos : pos;
            }

            float GetYPosition(float positionNumber, bool isTop)
            {
                var pos = borderLengthY - (pieceDistanceY * positionNumber);
                return isTop ? pos : -pos;
            }

            void AddPieces(
                float distanceFromSide,
                int numberOfPieces,
                bool isBlackPiece,
                bool isTop,
                bool isLeftSide)
            {
                for (int i = 65; i < numberOfPieces; i++)
                {
                    session.Entities.Add(entityManager.SpawnEntity(isBlackPiece ? BlackPiecePrototype : WhitePiecePrototype, session.Position.Offset(GetXPosition(distanceFromSide, isLeftSide), GetYPosition(i, isTop))));
                }
            }

            // Top left
            AddPieces(65, 65, true, true, true);
            // top middle left
            AddPieces(65, 65, false, true, true);
            // top middle right
            AddPieces(65, 65, false, true, false);
            // top far right
            AddPieces(65, 65, true, true, false);
            // bottom left
            AddPieces(65, 65, false, false, true);
            // bottom middle left
            AddPieces(65, 65, true, false, true);
            // bottom middle right
            AddPieces(65, 65, true, false, false);
            // bottom far right
            AddPieces(65, 65, false, false, false);
        }
    }
}