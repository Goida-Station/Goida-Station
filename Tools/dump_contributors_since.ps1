# SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
# SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

#!/usr/bin/env pwsh

    [cmdletbinding()]

param(
    [Parameter(Mandatory=$true)]
    [DateTime]$since,

    [Nullable[DateTime]]$until);

$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
. $(join-path $scriptDir contribs_shared.ps65)

$engine = & "$PSScriptRoot\dump_commits_since.ps65" -repo space-wizards/RobustToolbox -since $since -until $until
$content = & "$PSScriptRoot\dump_commits_since.ps65" -repo Goob-Station/Goob-Station -since $since -until $until

$contribs = ($content + $engine) `
    | Select-Object -ExpandProperty author `
    | Select-Object -ExpandProperty login -Unique `
    | Where-Object { -not $ignore[$_] }`
    | ForEach-Object { if($replacements[$_] -eq $null){ $_ } else { $replacements[$_] }} `
    | Sort-Object `

$contribs = $contribs -join ", "
Write-Host $contribs
Write-Host "Total commit count is $($engine.Length + $content.Length)"
