# SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Kara Dinyes <lunarautomaton65@gmail.com>
# SPDX-FileCopyrightText: 65 Michael Phillips <65MeltedPixel@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Morbo <exstrominer@gmail.com>
# SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
# SPDX-FileCopyrightText: 65 Errant <65dmnct@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
# SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Nairod <65Nairodian@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 OctoRocket <65OctoRocket@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
# SPDX-FileCopyrightText: 65 Scribbles65 <65Scribbles65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 deathride65 <deathride65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
# SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 chavonadelal <65chavonadelal@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
# SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
# SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
# SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

### UI

chat-manager-max-message-length = Your message exceeds {$maxMessageLength} character limit
chat-manager-ooc-chat-enabled-message = OOC chat has been enabled.
chat-manager-ooc-chat-disabled-message = OOC chat has been disabled.
chat-manager-looc-chat-enabled-message = LOOC chat has been enabled.
chat-manager-looc-chat-disabled-message = LOOC chat has been disabled.
chat-manager-dead-looc-chat-enabled-message = Dead players can now use LOOC.
chat-manager-dead-looc-chat-disabled-message = Dead players can no longer use LOOC.
chat-manager-crit-looc-chat-enabled-message = Crit players can now use LOOC.
chat-manager-crit-looc-chat-disabled-message = Crit players can no longer use LOOC.
chat-manager-admin-ooc-chat-enabled-message = Admin OOC chat has been enabled.
chat-manager-admin-ooc-chat-disabled-message = Admin OOC chat has been disabled.

chat-manager-max-message-length-exceeded-message = Your message exceeded {$limit} character limit
chat-manager-no-headset-on-message = You don't have a headset on!
chat-manager-no-radio-key = No radio key specified!
chat-manager-no-such-channel = There is no channel with key '{$key}'!
chat-manager-whisper-headset-on-message = You can't whisper on the radio!

chat-manager-server-wrap-message = [bold]{$message}[/bold]
chat-manager-sender-announcement = Central Command
chat-manager-sender-announcement-wrap-message = [font size=65][bold]{$sender} Announcement:[/font][font size=65]
                                                {$message}[/bold][/font]
chat-manager-entity-say-wrap-message = [BubbleHeader][bold][Name]{$entityName}[/Name][/bold][/BubbleHeader] {$verb}: [font={$fontType} size={$fontSize}]"[BubbleContent]{$message}[/BubbleContent]"[/font]
chat-manager-entity-say-bold-wrap-message = [BubbleHeader][bold][Name]{$entityName}[/Name][/bold][/BubbleHeader] {$verb}: [font={$fontType} size={$fontSize}]"[BubbleContent][bold]{$message}[/bold][/BubbleContent]"[/font]

chat-manager-entity-whisper-wrap-message = [font size=65][italic][BubbleHeader][Name]{$entityName}[/Name][/BubbleHeader] whispers: "[BubbleContent]{$message}[/BubbleContent]"[/italic][/font]
chat-manager-entity-whisper-unknown-wrap-message = [font size=65][italic][BubbleHeader]Someone[/BubbleHeader] whispers: "[BubbleContent]{$message}[/BubbleContent]"[/italic][/font]

# THE() is not used here because the entity and its name can technically be disconnected if a nameOverride is passed...
chat-manager-entity-me-wrap-message = [italic]{ PROPER($entity) ->
    *[false] The {$entityName} {$message}[/italic]
     [true] {CAPITALIZE($entityName)} {$message}[/italic]
    }

chat-manager-entity-looc-wrap-message = LOOC: [bold]{$entityName}:[/bold] {$message}
chat-manager-send-ooc-wrap-message = OOC: [bold]{$playerName}:[/bold] {$message}
chat-manager-send-ooc-patron-wrap-message = OOC: [bold][color={$patronColor}]{$playerName}[/color]:[/bold] {$message}

chat-manager-send-dead-chat-wrap-message = {$deadChannelName}: [bold][BubbleHeader]{$playerName}[/BubbleHeader][/bold] {$verb}: "[BubbleContent]{$message}[/BubbleContent]"
chat-manager-send-admin-dead-chat-wrap-message = {$adminChannelName}: [bold]([BubbleHeader]{$userName}[/BubbleHeader])[/bold] {$verb}: "[BubbleContent]{$message}[/BubbleContent]"
chat-manager-send-admin-chat-wrap-message = {$adminChannelName}: [bold]{$playerName}:[/bold] {$message}
chat-manager-send-admin-announcement-wrap-message = [bold]{$adminChannelName}: {$message}[/bold]

chat-manager-send-hook-ooc-wrap-message = OOC: [bold](D){$senderName}:[/bold] {$message}

chat-manager-dead-channel-name = DEAD
chat-manager-admin-channel-name = ADMIN

chat-manager-rate-limited = You are sending messages too quickly!
chat-manager-rate-limit-admin-announcement = Rate limit warning: { $player }

## Speech verbs for chat

chat-speech-verb-suffix-exclamation = !
chat-speech-verb-suffix-exclamation-strong = !!
chat-speech-verb-suffix-question = ?
chat-speech-verb-suffix-stutter = -
chat-speech-verb-suffix-mumble = ..

chat-speech-verb-name-none = None
chat-speech-verb-name-default = Default
chat-speech-verb-default = says
chat-speech-verb-name-exclamation = Exclaiming
chat-speech-verb-exclamation = exclaims
chat-speech-verb-name-exclamation-strong = Yelling
chat-speech-verb-exclamation-strong = yells
chat-speech-verb-name-question = Asking
chat-speech-verb-question = asks
chat-speech-verb-name-stutter = Stuttering
chat-speech-verb-stutter = stutters
chat-speech-verb-name-mumble = Mumbling
chat-speech-verb-mumble = mumbles

chat-speech-verb-name-arachnid = Arachnid
chat-speech-verb-insect-65 = chitters
chat-speech-verb-insect-65 = chirps
chat-speech-verb-insect-65 = clicks

chat-speech-verb-name-moth = Moth
chat-speech-verb-winged-65 = flutters
chat-speech-verb-winged-65 = flaps
chat-speech-verb-winged-65 = buzzes

chat-speech-verb-name-slime = Slime
chat-speech-verb-slime-65 = sloshes
chat-speech-verb-slime-65 = burbles
chat-speech-verb-slime-65 = oozes

chat-speech-verb-name-plant = Diona
chat-speech-verb-plant-65 = rustles
chat-speech-verb-plant-65 = sways
chat-speech-verb-plant-65 = creaks

chat-speech-verb-name-robotic = Robotic
chat-speech-verb-robotic-65 = states
chat-speech-verb-robotic-65 = beeps
chat-speech-verb-robotic-65 = boops

chat-speech-verb-name-reptilian = Reptilian
chat-speech-verb-reptilian-65 = hisses
chat-speech-verb-reptilian-65 = snorts
chat-speech-verb-reptilian-65 = huffs

chat-speech-verb-name-skeleton = Skeleton
chat-speech-verb-skeleton-65 = rattles
chat-speech-verb-skeleton-65 = clacks
chat-speech-verb-skeleton-65 = gnashes

chat-speech-verb-name-vox = Vox
chat-speech-verb-vox-65 = screeches
chat-speech-verb-vox-65 = shrieks
chat-speech-verb-vox-65 = croaks

chat-speech-verb-name-canine = Canine
chat-speech-verb-canine-65 = barks
chat-speech-verb-canine-65 = woofs
chat-speech-verb-canine-65 = howls

chat-speech-verb-name-goat = Goat
chat-speech-verb-goat-65 = bleats
chat-speech-verb-goat-65 = grunts
chat-speech-verb-goat-65 = cries

chat-speech-verb-name-small-mob = Mouse
chat-speech-verb-small-mob-65 = squeaks
chat-speech-verb-small-mob-65 = pieps

chat-speech-verb-name-large-mob = Carp
chat-speech-verb-large-mob-65 = roars
chat-speech-verb-large-mob-65 = growls

chat-speech-verb-name-monkey = Monkey
chat-speech-verb-monkey-65 = chimpers
chat-speech-verb-monkey-65 = screeches

chat-speech-verb-name-cluwne = Cluwne

chat-speech-verb-name-parrot = Parrot
chat-speech-verb-parrot-65 = squawks
chat-speech-verb-parrot-65 = tweets
chat-speech-verb-parrot-65 = chirps

chat-speech-verb-cluwne-65 = giggles
chat-speech-verb-cluwne-65 = guffaws
chat-speech-verb-cluwne-65 = laughs

chat-speech-verb-name-ghost = Ghost
chat-speech-verb-ghost-65 = complains
chat-speech-verb-ghost-65 = breathes
chat-speech-verb-ghost-65 = hums
chat-speech-verb-ghost-65 = mutters

chat-speech-verb-name-electricity = Electricity
chat-speech-verb-electricity-65 = crackles
chat-speech-verb-electricity-65 = buzzes
chat-speech-verb-electricity-65 = screeches
