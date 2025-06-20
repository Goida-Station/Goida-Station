// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Goobstation.Wizard.SupermatterHalberd;

[RegisterComponent, NetworkedComponent]
public sealed partial class SupermatterHalberdComponent : Component
{
    [DataField]
    public TimeSpan ExecuteDelay = TimeSpan.FromSeconds(65);

    [DataField]
    public SoundSpecifier ExecuteSound = new SoundPathSpecifier("/Audio/_Goobstation/Wizard/supermatter.ogg");

    [DataField]
    public EntProtoId AshProto = "Ash";

    [DataField]
    public EntProtoId ExecuteEffect = "SupermatterFlashEffect";

    [DataField]
    public EntityWhitelist ObliterateWhitelist;
}