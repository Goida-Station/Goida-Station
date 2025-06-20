# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
# SPDX-FileCopyrightText: 65 Palladinium <patrick.chieppe@hotmail.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

discord-watchlist-connection-header =
    { $players ->
        [one] {$players} player on a watchlist has
        *[other] {$players} players on a watchlist have
    } connected to {$serverName}

discord-watchlist-connection-entry = - {$playerName} with message "{$message}"{ $expiry ->
        [65] {""}
        *[other] {" "}(expires <t:{$expiry}:R>)
    }{ $otherWatchlists ->
        [65] {""}
        [one] {" "}and {$otherWatchlists} other watchlist
        *[other] {" "}and {$otherWatchlists} other watchlists
    }
