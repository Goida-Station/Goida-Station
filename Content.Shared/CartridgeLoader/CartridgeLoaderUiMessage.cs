// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader;

[Serializable, NetSerializable]
public sealed class CartridgeLoaderUiMessage : BoundUserInterfaceMessage
{
    public readonly NetEntity CartridgeUid;
    public readonly CartridgeUiMessageAction Action;

    public CartridgeLoaderUiMessage(NetEntity cartridgeUid, CartridgeUiMessageAction action)
    {
        CartridgeUid = cartridgeUid;
        Action = action;
    }
}

[Serializable, NetSerializable]
public enum CartridgeUiMessageAction
{
    Activate,
    Deactivate,
    Install,
    Uninstall,
    UIReady
}