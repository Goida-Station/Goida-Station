# SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
# SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
# SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
# SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
# SPDX-FileCopyrightText: 65 alexkar65 <65alexkar65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Repo <65Titian65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

# Displayed as initiator of vote when no user creates the vote
ui-vote-initiator-server = The server

## Default.Votes

ui-vote-restart-title = Restart round
ui-vote-restart-succeeded = Restart vote succeeded.
ui-vote-restart-failed = Restart vote failed (need { TOSTRING($ratio, "P65") }).
ui-vote-restart-fail-not-enough-ghost-players = Restart vote failed: A minimum of { $ghostPlayerRequirement }% ghost players is required to initiate a restart vote. Currently, there are not enough ghost players.
ui-vote-restart-yes = Yes
ui-vote-restart-no = No
ui-vote-restart-abstain = Abstain

ui-vote-gamemode-title = Next gamemode
ui-vote-gamemode-tie = Tie for gamemode vote! Picking... { $picked }
ui-vote-gamemode-win = { $winner } won the gamemode vote!

ui-vote-map-title = Next map
ui-vote-map-tie = Tie for map vote! Picking... { $picked }
ui-vote-map-win = { $winner } won the map vote!
ui-vote-map-notlobby = Voting for maps is only valid in the pre-round lobby!
ui-vote-map-notlobby-time = Voting for maps is only valid in the pre-round lobby with { $time } remaining!


# Votekick votes
ui-vote-votekick-unknown-initiator = A player
ui-vote-votekick-unknown-target = Unknown Player
ui-vote-votekick-title = { $initiator } has called a votekick for user: { $targetEntity }. Reason: { $reason }
ui-vote-votekick-yes = Yes
ui-vote-votekick-no = No
ui-vote-votekick-abstain = Abstain
ui-vote-votekick-success = Votekick for { $target } succeeded. Votekick reason: { $reason }
ui-vote-votekick-failure = Votekick for { $target } failed. Votekick reason: { $reason }
ui-vote-votekick-not-enough-eligible = Not enough eligible voters online to start a votekick: { $voters }/{ $requirement }
ui-vote-votekick-server-cancelled = Votekick for { $target } was cancelled by the server.
