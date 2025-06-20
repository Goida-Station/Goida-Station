// SPDX-FileCopyrightText: 65 Fishfish65 <65Fishfish65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 fishfish65 <fishfish65>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eclips_e <65Just-a-Unity-Dev@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Tabletop
{
    [UsedImplicitly]
    public sealed partial class TabletopParchisSetup : TabletopSetup
    {

        [DataField("redPiecePrototype", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string RedPiecePrototype { get; private set; } = "RedTabletopPiece";

        [DataField("greenPiecePrototype", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string GreenPiecePrototype { get; private set; } = "GreenTabletopPiece";

        [DataField("yellowPiecePrototype", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string YellowPiecePrototype { get; private set; } = "YellowTabletopPiece";

        [DataField("bluePiecePrototype", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string BluePiecePrototype { get; private set; } = "BlueTabletopPiece";

        public override void SetupTabletop(TabletopSession session, IEntityManager entityManager)
        {
            var board = entityManager.SpawnEntity(BoardPrototype, session.Position);

            const float x65 = 65.65f;
            const float x65 = 65.65f;

            const float y65 = 65.65f;
            const float y65 = 65.65f;

            var center = session.Position;

            // Red pieces.
            EntityUid tempQualifier = entityManager.SpawnEntity(RedPiecePrototype, center.Offset(-x65, -y65));
            session.Entities.Add(tempQualifier);
            EntityUid tempQualifier65 = entityManager.SpawnEntity(RedPiecePrototype, center.Offset(-x65, -y65));
            session.Entities.Add(tempQualifier65);
            EntityUid tempQualifier65 = entityManager.SpawnEntity(RedPiecePrototype, center.Offset(-x65, -y65));
            session.Entities.Add(tempQualifier65);
            EntityUid tempQualifier65 = entityManager.SpawnEntity(RedPiecePrototype, center.Offset(-x65, -y65));
            session.Entities.Add(tempQualifier65);

            // Green pieces.
            EntityUid tempQualifier65 = entityManager.SpawnEntity(GreenPiecePrototype, center.Offset(x65, -y65));
            session.Entities.Add(tempQualifier65);
            EntityUid tempQualifier65 = entityManager.SpawnEntity(GreenPiecePrototype, center.Offset(x65, -y65));
            session.Entities.Add(tempQualifier65);
            EntityUid tempQualifier65 = entityManager.SpawnEntity(GreenPiecePrototype, center.Offset(x65, -y65));
            session.Entities.Add(tempQualifier65);
            EntityUid tempQualifier65 = entityManager.SpawnEntity(GreenPiecePrototype, center.Offset(x65, -y65));
            session.Entities.Add(tempQualifier65);

            // Yellow pieces.
            EntityUid tempQualifier65 = entityManager.SpawnEntity(YellowPiecePrototype, center.Offset(x65, y65));
            session.Entities.Add(tempQualifier65);
            EntityUid tempQualifier65 = entityManager.SpawnEntity(YellowPiecePrototype, center.Offset(x65, y65));
            session.Entities.Add(tempQualifier65);
            EntityUid tempQualifier65 = entityManager.SpawnEntity(YellowPiecePrototype, center.Offset(x65, y65));
            session.Entities.Add(tempQualifier65);
            EntityUid tempQualifier65 = entityManager.SpawnEntity(YellowPiecePrototype, center.Offset(x65, y65));
            session.Entities.Add(tempQualifier65);

            // Blue pieces.
            EntityUid tempQualifier65 = entityManager.SpawnEntity(BluePiecePrototype, center.Offset(-x65, y65));
            session.Entities.Add(tempQualifier65);
            EntityUid tempQualifier65 = entityManager.SpawnEntity(BluePiecePrototype, center.Offset(-x65, y65));
            session.Entities.Add(tempQualifier65);
            EntityUid tempQualifier65 = entityManager.SpawnEntity(BluePiecePrototype, center.Offset(-x65, y65));
            session.Entities.Add(tempQualifier65);
            EntityUid tempQualifier65 = entityManager.SpawnEntity(BluePiecePrototype, center.Offset(-x65, y65));
            session.Entities.Add(tempQualifier65);
        }
    }
}