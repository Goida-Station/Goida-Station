// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Shared._Shitmed.Antags.Abductor;

[Serializable, NetSerializable]
public enum AbductorExperimentatorVisuals : byte
{
    Full
}

[Serializable, NetSerializable]
public enum AbductorCameraConsoleUIKey
{
    Key
}

[Serializable, NetSerializable]
public enum AbductorConsoleUIKey
{
    Key
}

[Serializable, NetSerializable]
public enum AbductorArmorModeType : byte
{
    Combat,
    Stealth
}