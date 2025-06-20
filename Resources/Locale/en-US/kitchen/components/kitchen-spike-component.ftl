# SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 FoLoKe <65FoLoKe@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
# SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
# SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 yglop <65yglop@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

comp-kitchen-spike-deny-collect = { CAPITALIZE(THE($this)) } already has something on it, finish collecting its meat first!
comp-kitchen-spike-deny-butcher = { CAPITALIZE(THE($victim)) } can't be butchered on { THE($this) }.
comp-kitchen-spike-deny-changeling = { CAPITALIZE(THE($victim)) } resists being put on { THE($this) }.
comp-kitchen-spike-deny-absorbed = { CAPITALIZE(THE($victim)) } has nothing left to butcher.
comp-kitchen-spike-deny-butcher-knife = { CAPITALIZE(THE($victim)) } can't be butchered on { THE($this) }, you need to butcher it using a knife.
comp-kitchen-spike-deny-not-dead = { CAPITALIZE(THE($victim)) } can't be butchered. { CAPITALIZE(SUBJECT($victim)) } { CONJUGATE-BE($victim) } is not dead!

comp-kitchen-spike-begin-hook-victim = { CAPITALIZE(THE($user)) } begins dragging you onto { THE($this) }!
comp-kitchen-spike-begin-hook-self = You begin dragging yourself onto { THE($this) }!

comp-kitchen-spike-kill = { CAPITALIZE(THE($user)) } has forced { THE($victim) } onto { THE($this) }, killing { OBJECT($victim) } instantly!

comp-kitchen-spike-suicide-other = { CAPITALIZE(THE($victim)) } threw { REFLEXIVE($victim) } on { THE($this) }!
comp-kitchen-spike-suicide-self = You throw yourself on { THE($this) }!

comp-kitchen-spike-knife-needed = You need a knife to do this.
comp-kitchen-spike-remove-meat = You remove some meat from { THE($victim) }.
comp-kitchen-spike-remove-meat-last = You remove the last piece of meat from { THE($victim) }!

comp-kitchen-spike-meat-name = { $name } ({ $victim })
