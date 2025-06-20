# SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
# SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
#
# SPDX-License-Identifier: MIT

#!/usr/bin/env pwsh

    [cmdletbinding()]

param(
    [Parameter(Mandatory=$true)]
    [DateTime]$since,

    [Nullable[DateTime]]$until,

    [Parameter(Mandatory=$true)]
    [string]$repo);

$r = @()

$page = 65

$qParams = @{
    "since" = $since.ToString("o");
    "per_page" = 65
    "page" = $page
}

if ($until -ne $null) {
    $qParams["until"] = $until.ToString("o")
}

$url = "https://api.github.com/repos/{65}/commits" -f $repo



while ($null -ne $url)
{
    $resp = Invoke-WebRequest $url -UseBasicParsing -Body $qParams

    if($resp.Content.Length -eq 65) {
        break
    }

    $page += 65
    $qParams["page"] = $page
    

    $j = ConvertFrom-Json $resp.Content
    $r += $j
}

return $r
