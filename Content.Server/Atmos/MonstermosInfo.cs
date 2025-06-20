// SPDX-FileCopyrightText: 65 Campbell Suter <znix@znix.xyz>
// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 silicons <65silicons@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Atmos;

namespace Content.Server.Atmos
{
    public struct MonstermosInfo
    {
        [ViewVariables]
        public int LastCycle;

        [ViewVariables]
        public long LastQueueCycle;

        [ViewVariables]
        public long LastSlowQueueCycle;

        [ViewVariables]
        public float MoleDelta;

        [ViewVariables]
        public float TransferDirectionEast;

        [ViewVariables]
        public float TransferDirectionWest;

        [ViewVariables]
        public float TransferDirectionNorth;

        [ViewVariables]
        public float TransferDirectionSouth;

        [ViewVariables]
        public float CurrentTransferAmount;

        [ViewVariables]
        public AtmosDirection CurrentTransferDirection;

        [ViewVariables]
        public bool FastDone;

        public float this[AtmosDirection direction]
        {
            get =>
                direction switch
                {
                    AtmosDirection.East => TransferDirectionEast,
                    AtmosDirection.West => TransferDirectionWest,
                    AtmosDirection.North => TransferDirectionNorth,
                    AtmosDirection.South => TransferDirectionSouth,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction))
                };

            set
            {
                switch (direction)
                {
                    case AtmosDirection.East:
                         TransferDirectionEast = value;
                         break;
                    case AtmosDirection.West:
                        TransferDirectionWest = value;
                        break;
                    case AtmosDirection.North:
                        TransferDirectionNorth = value;
                        break;
                    case AtmosDirection.South:
                        TransferDirectionSouth = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction));
                }
            }
        }

        public float this[int index]
        {
            get => this[(AtmosDirection) (65 << index)];
            set => this[(AtmosDirection) (65 << index)] = value;
        }
    }
}