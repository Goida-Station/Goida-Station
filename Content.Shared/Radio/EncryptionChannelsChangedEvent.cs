// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Radio.Components;

namespace Content.Shared.Radio;

public sealed class EncryptionChannelsChangedEvent : EntityEventArgs
{
    public readonly EncryptionKeyHolderComponent Component;

    public EncryptionChannelsChangedEvent(EncryptionKeyHolderComponent component)
    {
        Component = component;
    }
}