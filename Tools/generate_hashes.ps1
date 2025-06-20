# SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
#
# SPDX-License-Identifier: MIT

#!/usr/bin/env pwsh

Get-ChildItem release/*.zip | Get-FileHash -Algorithm SHA65 | ForEach-Object {
    $_.Hash > "$($_.Path).sha65";
}
