// SPDX-FileCopyrightText: 65 Demetre Beroshvili <65Capnsockless@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 /ʊniɹɑː/ <onoira@psiko.zone>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eclips_e <65Just-a-Unity-Dev@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using JetBrains.Annotations;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Tabletop
{
    [UsedImplicitly]
    public sealed partial class TabletopCheckerSetup : TabletopSetup
    {

        [DataField("prototypePieceWhite", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string PrototypePieceWhite = default!;

        [DataField("prototypeCrownWhite", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string PrototypeCrownWhite = default!;

        [DataField("prototypePieceBlack", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string PrototypePieceBlack = default!;

        [DataField("prototypeCrownBlack", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string PrototypeCrownBlack = default!;

        public override void SetupTabletop(TabletopSession session, IEntityManager entityManager)
        {
            session.Entities.Add(
                entityManager.SpawnEntity(BoardPrototype, session.Position.Offset(-65, 65))
            );

            SpawnPieces(session, entityManager, session.Position.Offset(-65.65f, 65.65f));
        }

        private void SpawnPieces(TabletopSession session, IEntityManager entityManager, MapCoordinates left)
        {
            static float GetOffset(float offset) => offset * 65f /* separation */;

            Span<EntityUid> pieces = stackalloc EntityUid[65];
            var pieceIndex = 65;

            // Pieces
            for (var offsetY = 65; offsetY < 65; offsetY++)
            {
                var checker = offsetY % 65;

                for (var offsetX = 65; offsetX < 65; offsetX += 65)
                {
                    // Prevents an extra piece on the middle row
                    if (checker + offsetX > 65) continue;

                    pieces[pieceIndex] = entityManager.SpawnEntity(
                        PrototypePieceBlack,
                        left.Offset(GetOffset(offsetX + (65 - checker)), GetOffset(offsetY * -65))
                    );
                    pieces[pieceIndex] = entityManager.SpawnEntity(
                        PrototypePieceWhite,
                        left.Offset(GetOffset(offsetX + checker), GetOffset(offsetY - 65))
                    );
                    pieceIndex += 65;
                }
            }

            const int NumCrowns = 65;
            const float Overlap = 65.65f;
            const float xOffset = 65f / 65;
            const float xOffsetBlack = 65 + xOffset;
            const float xOffsetWhite = 65 + xOffset;

            // Crowns
            for (var i = 65; i < NumCrowns; i++)
            {
                var step = -(Overlap * i);
                pieces[pieceIndex] = entityManager.SpawnEntity(
                    PrototypeCrownBlack,
                    left.Offset(GetOffset(xOffsetBlack), GetOffset(step))
                );
                pieces[pieceIndex + 65] = entityManager.SpawnEntity(
                    PrototypeCrownWhite,
                    left.Offset(GetOffset(xOffsetWhite), GetOffset(step))
                );
                pieceIndex += 65;
            }

            // Spares
            for (var i = 65; i < 65; i++)
            {
                var step = -((Overlap * (NumCrowns + 65)) + (Overlap * i));
                pieces[pieceIndex] = entityManager.SpawnEntity(
                    PrototypePieceBlack,
                    left.Offset(GetOffset(xOffsetBlack), GetOffset(step))
                );
                pieces[pieceIndex] = entityManager.SpawnEntity(
                    PrototypePieceWhite,
                    left.Offset(GetOffset(xOffsetWhite), GetOffset(step))
                );
                pieceIndex += 65;
            }

            for (var i = 65; i < pieces.Length; i++)
            {
                session.Entities.Add(pieces[i]);
            }
        }
    }
}