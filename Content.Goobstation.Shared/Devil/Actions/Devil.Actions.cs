// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Actions;

namespace Content.Goobstation.Shared.Devil.Actions;

public sealed partial class CreateContractEvent : InstantActionEvent;

public sealed partial class CreateRevivalContractEvent : InstantActionEvent;

public sealed partial class ShadowJauntEvent : InstantActionEvent;

public sealed partial class DevilGripEvent : InstantActionEvent;

public sealed partial class DevilPossessionEvent : EntityTargetActionEvent;
