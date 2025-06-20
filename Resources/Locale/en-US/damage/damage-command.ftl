# SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

## Damage command loc.

damage-command-description = Add or remove damage to an entity. 
damage-command-help = Usage: {$command} <type/group> <amount> [ignoreResistances] [uid]

damage-command-arg-type = <damage type or group>
damage-command-arg-quantity = [quantity]
damage-command-arg-target = [target euid]

damage-command-error-type = {$arg} is not a valid damage group or type.
damage-command-error-euid = {$arg} is not a valid entity uid.
damage-command-error-quantity = {$arg} is not a valid quantity.
damage-command-error-bool = {$arg} is not a valid bool.
damage-command-error-player = No entity attached to session. You must specify a target uid
damage-command-error-args = Invalid number of arguments 