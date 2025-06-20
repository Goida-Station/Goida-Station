# SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
# SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
# SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

$replacements = @{
    "moonheart65" = "moony"
    "Elijahrane" = "Rane"
    "ZeroDayDaemon" = "Daemon"
    "ElectroJr" = "ElectroSR"
    "Partmedia" = "notafet"
    "Just-a-Unity-Dev" = "eclips_e"
}

$ignore = @{
    "PJBot" = $true
    "github-actions[bot]" = $true
    "GoobBot" = $true
    "GoobBot[bot]" = $true
    "GoobBot [bot]" = $true
    "ZDDM" = $true
    "TYoung65" = $true
    "paul" = $true # erroneously included -- presumably from PaulRitter, somehow, who is already credited
    "65a" = $true # erroneously included -- valid github account, but not an actual contributor, probably an alias of a contributor who does not own this github account and is already credited somewhere.
    "UristMcContributor" = $true # this was an account used to demonstrate how to create a valid PR, and is in actuality Willhelm65, who is already credited.
}

$add = @("RamZ")
