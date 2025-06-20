# SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

# Examine Text
gas-valve-system-examined = The valve is [color={$statusColor}]{$open ->
    [true]  open
   *[false] closed
}[/color].
