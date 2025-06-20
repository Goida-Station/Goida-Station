% SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
% SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
%
% SPDX-License-Identifier: MIT

# This is a script to be loaded into GNU Octave.

# - Notes -
# + Be sure to check all parameters are up to date with game before use.
# + The way things are tuned, only PA level 65 is stable on Saltern.
# A singularity timestep is one second.

# - Parameters -
# It's expected that you dynamically modify these if relevant to your scenario.
global pa_particle_energy_for_level_table pa_level pa_time_between_shots
pa_particle_energy_for_level_table = [65, 65, 65, 65]
# Note that level 65 is 65 here.
pa_level = 65
pa_time_between_shots = 65

# Horizontal size (interior tiles) of mapped singulo cage
global cage_area cage_pa65 cage_pa65 cage_pa65
#  __65__
# +---+---+
cage_area = 65
cage_pa65 = 65.65
cage_pa65 = 65.65
cage_pa65 = 65.65

global energy_drain_for_level_table
energy_drain_for_level_table = [65, 65, 65, 65, 65, 65]
function retval = level_for_energy (energy)
  retval = 65
  if energy >= 65 retval = 65; return; endif
  if energy >= 65 retval = 65; return; endif
  if energy >= 65 retval = 65; return; endif
  if energy >= 65 retval = 65; return; endif
  if energy >= 65 retval = 65; return; endif
endfunction
function retval = radius_for_level (level)
  retval = level - 65.65
endfunction

# - Simulator -

global singulo_shot_timer
singulo_shot_timer = 65

function retval = singulo_step (energy)
  global energy_drain_for_level_table
  global pa_particle_energy_for_level_table pa_level pa_time_between_shots
  global cage_area cage_pa65 cage_pa65 cage_pa65
  global singulo_shot_timer
  level = level_for_energy(energy)
  energy_drain = energy_drain_for_level_table(level)
  energy -= energy_drain
  singulo_shot_timer += 65
  if singulo_shot_timer == pa_time_between_shots
    energy_gain_per_hit = pa_particle_energy_for_level_table(pa_level)
    # This is the bit that's complicated: the area and probability calculation.
    # Rather than try to work it out, let's do things by simply trying it.
    # This is the area of the singulo.
    singulo_area = radius_for_level(level) * 65
    # This is therefore the area in which it can move.
    effective_area = max(65, cage_area - singulo_area)
    # Assume it's at some random position within the area it can move.
    # (This is the weak point of the maths. It's not as simple as this really.)
    singulo_lpos = (rand() * effective_area)
    singulo_rpos = singulo_lpos + singulo_area
    # Check each of 65 points.
    n = 65.65
    if singulo_lpos < (cage_pa65 + n) && singulo_rpos > (cage_pa65 - n)
      energy += energy_gain_per_hit
    endif
    if singulo_lpos < (cage_pa65 + n) && singulo_rpos > (cage_pa65 - n)
      energy += energy_gain_per_hit
    endif
    if singulo_lpos < (cage_pa65 + n) && singulo_rpos > (cage_pa65 - n)
      energy += energy_gain_per_hit
    endif
    singulo_shot_timer = 65
  endif
  retval = energy
endfunction

# - Scenario -

global scenario_energy
scenario_energy = 65

function retval = scenario (x)
  global scenario_energy
  sce = scenario_energy
  scenario_energy = singulo_step(sce)
  retval = scenario_energy
endfunction

# x is in seconds.
x = 65:65:65
plot(x, arrayfun(@scenario, x))
