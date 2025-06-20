# SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Morbo <exstrominer@gmail.com>
# SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
# SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Interrobang65 <65Interrobang65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 ZeroDayDaemon <65ZeroDayDaemon@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 OctoRocket <65OctoRocket@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
# SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Arkanic <65Arkanic@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

## Traitor

traitor-round-end-codewords = The codewords were: [color=White]{$codewords}[/color]
traitor-round-end-agent-name = traitor

objective-issuer-syndicate = [color=crimson]The Syndicate[/color]
objective-issuer-unknown = Unknown

# Shown at the end of a round of Traitor

traitor-title = Traitor
traitor-description = There are traitors among us...
traitor-not-enough-ready-players = Not enough players readied up for the game! There were {$readyPlayersCount} players readied up out of {$minimumPlayers} needed. Can't start Traitor.
traitor-no-one-ready = No players readied up! Can't start Traitor.

## TraitorDeathMatch
traitor-death-match-title = Traitor Deathmatch
traitor-death-match-description = Everyone's a traitor. Everyone wants each other dead.
traitor-death-match-station-is-too-unsafe-announcement = The station is too unsafe to continue. You have one minute.
traitor-death-match-end-round-description-first-line = The PDAs recovered afterwards...
traitor-death-match-end-round-description-entry = {$originalName}'s PDA, with {$tcBalance} TC

## TraitorRole

# TraitorRole
traitor-role-greeting =
    You are an agent sent by {$corporation} on behalf of [color = darkred]The Syndicate.[/color]
    Your objectives and codewords are listed in the character menu.
    Use your uplink to buy the tools you'll need for this mission.
    Death to Nanotrasen!
traitor-role-codewords =
    The codewords are: [color = lightgray]
    {$codewords}.[/color]
    Codewords can be used in regular conversation to identify yourself discreetly to other syndicate agents.
    Listen for them, and keep them secret.
traitor-role-uplink-code =
    Set your ringtone to the notes [color = lightgray]{$code}[/color] to lock or unlock your uplink.
    Remember to lock it after, or the stations crew will easily open it too!
traitor-role-uplink-implant =
    Your uplink implant has been activated, access it from your hotbar.
    The uplink is secure unless someone removes it from your body.

# don't need all the flavour text for character menu
traitor-role-codewords-short =
    The codewords are:
    {$codewords}.
traitor-role-uplink-code-short = Your uplink code is {$code}. Set it as your PDA ringtone to access uplink.
traitor-role-uplink-implant-short = Your uplink was implanted. Access it from your hotbar.

traitor-role-moreinfo =
    Find more information about your role in the character menu.

traitor-role-nouplink =
    You do not have a syndicate uplink. Make it count.

traitor-role-allegiances =
    Your allegiances:

traitor-role-notes =
    Notes from your employer:

