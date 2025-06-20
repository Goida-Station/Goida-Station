// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Research;

[Serializable, NetSerializable]
public enum DiskConsoleUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class DiskConsoleBoundUserInterfaceState : BoundUserInterfaceState
{
    public bool CanPrint;
    public int PointCost;
    public int ServerPoints;

    public DiskConsoleBoundUserInterfaceState(int serverPoints, int pointCost, bool canPrint)
    {
        CanPrint = canPrint;
        PointCost = pointCost;
        ServerPoints = serverPoints;
    }
}

[Serializable, NetSerializable]
public sealed class DiskConsolePrintDiskMessage : BoundUserInterfaceMessage
{

}