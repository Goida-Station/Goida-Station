# SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

action-name-wake = Wake up

sleep-onomatopoeia = Zzz...
sleep-examined = [color=lightblue]{CAPITALIZE(SUBJECT($target))} {CONJUGATE-BE($target)} asleep.[/color]

wake-other-success = You shake {THE($target)} awake.
wake-other-failure = You shake {THE($target)}, but {SUBJECT($target)} {CONJUGATE-BE($target)} not waking up.
