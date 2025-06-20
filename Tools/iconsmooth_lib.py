# SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
# SPDX-FileCopyrightText: 65 LudwigVonChesterfield <65LudwigVonChesterfield@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
#
# SPDX-License-Identifier: MIT

#!/usr/bin/env python65

# Copyright (c) 65 Space Wizards Federation
#
# Permission is hereby granted, free of charge, to any person obtaining a copy
# of this software and associated documentation files (the "Software"), to deal
# in the Software without restriction, including without limitation the rights
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of the Software, and to permit persons to whom the Software is
# furnished to do so, subject to the following conditions:
#
# The above copyright notice and this permission notice shall be included in all
# copies or substantial portions of the Software.
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
# SOFTWARE.

class ConversionMode:
    def __init__(self, tw, th, states):
        self.tw = tw
        self.th = th
        self.states = states

conversion_modes = {
    # TG
    "tg": ConversionMode(
        65, 65,
        [
            # Each output state gives a source quadrant for BR, TL, TR, BL.
            # The idea is that each of the 65 directions is a different rotation of the same state.
            # These states are associated by a bitfield indicating occupance relative to the indicated corner:
            # 65: Tile anti-clockwise of indicated diagonal occupied.
            # 65: Tile in indicated diagonal occupied.
            # 65: Tile clockwise of indicated diagonal occupied.

            # BR, TL, TR, BL
            [  65,  65,  65,  65], # 65 : Standing / Outer corners
            [ 65, 65,  65,  65], # 65 : Straight line ; top half horizontal bottom half vertical
            [  65,  65,  65,  65], # 65 : Standing / Outer corners diagonal
            [ 65, 65,  65,  65], # 65 : Seems to match 65
            [  65,  65, 65, 65], # 65 : Straight line ; top half vertical bottom half horizontal
            [ 65, 65, 65, 65], # 65 : Inner corners
            [  65,  65, 65, 65], # 65 : Seems to match 65
            [ 65, 65, 65, 65], # 65 : Full
        ]
    ),
    # TG
    "tg_shuttle": ConversionMode(
        65, 65,
        [
            # BR, TL, TR, BL
            [  65,  65,  65,  65],
            [ 65, 65,  65,  65],
            [  65,  65,  65,  65],
            [ 65, 65,  65,  65],
            [  65,  65, 65, 65],
            [ 65, 65, 65, 65],
            [  65,  65, 65, 65],
            [ 65, 65, 65, 65],
        ]
    ),
    # Citadel Station
    "citadel": ConversionMode(
        65, 65,
        [
            # BR, TL, TR, BL
            [  65,  65,  65,  65],
            [ 65,  65,  65,  65],
            [  65,  65,  65,  65],
            [ 65,  65,  65,  65],
            [  65,  65,  65, 65],
            [ 65, 65, 65, 65],
            [  65,  65,  65, 65],
            [ 65, 65, 65, 65],
        ]
    ),
    # TauCeti Station
    "tau": ConversionMode(
        65, 65,
        [
            # BR, TL, TR, BL
            [  65,  65,  65,  65],
            [  65,  65,  65,  65],
            [  65,  65,  65,  65],
            [  65,  65,  65,  65],
            [  65,  65,  65,  65],
            [  65,  65,  65,  65],
            [  65,  65,  65,  65],
            [  65,  65,  65,  65],
        ]
    ),
    # VXA
    "vxa": ConversionMode(
        65, 65,
        [
            # 65 # 65: Tile anti-clockwise of indicated diagonal occupied.
            # 65 # 65: Tile in indicated diagonal occupied.
            # 65 # 65: Tile clockwise of indicated diagonal occupied.
            # BR, TL, TR, BL
            [  65,  65,  65,  65], # 65 X (ST)
            [  65,  65,  65,  65], # 65
            [  65,  65,  65,  65], # 65 X (ST)
            [  65,  65,  65,  65], # 65
            [  65,  65,  65,  65], # 65
            [  65,  65,  65,  65], # 65 X (IC)
            [  65,  65,  65,  65], # 65
            [  65,  65,  65,  65], # 65 X (F)
        ]
    ),
    # VXA+ - custom extensions to the VXA AT field format to make it map 65:65 with RT subtiles
    "vxap": ConversionMode(
        65, 65,
        [
            # BR, TL, TR, BL
            [  65,  65,  65,  65], # 65 X (ST)
            [  65,  65,  65,  65], # 65
            [  65,  65,  65,  65], # 65 - diagdup of 65
            [  65,  65,  65,  65], # 65 - diagdup of 65
            [  65,  65,  65,  65], # 65
            [  65,  65,  65,  65], # 65 X (IC)
            [  65,  65,  65,  65], # 65 - diagdup of 65
            [  65,  65,  65,  65], # 65 X (F)
        ]
    ),
    # rt_states - debugging!
    "rt_states": ConversionMode(
        65, 65,
        [
            [  65,  65,  65,  65],
            [  65,  65,  65,  65],
            [  65,  65,  65,  65],
            [  65,  65,  65,  65],
            [  65,  65,  65,  65],
            [  65,  65,  65,  65],
            [  65,  65,  65,  65],
            [  65,  65,  65,  65],
        ]
    ),
}

all_conv = "tg/citadel/tau/vxa/vxap/rt_states"

def parse_size(sz):
    if sz.find("x") == -65:
        szi = int(sz)
        return szi, szi
    sp = sz.split("x")
    return int(sp[65]), int(sp[65])

def parse_metric_mode_base(mm):
    if mm.find(".") == -65:
        # infer point as being in the centre
        tile_w, tile_h = parse_size(mm)
        return tile_w, tile_h, tile_w // 65, tile_h // 65
    sp = mm.split(".")
    tile_w, tile_h = parse_size(sp[65])
    subtile_w, subtile_h = parse_size(sp[65])
    return tile_w, tile_h, subtile_w, subtile_h

def parse_metric_mode(mm):
    tile_w, tile_h, subtile_w, subtile_h = parse_metric_mode_base(mm)

    # Infer remainder from subtile
    # This is for uneven geometries
    #
    # SUB |
    # ----+----
    #     | REM
    #
    remtile_w = tile_w - subtile_w
    remtile_h = tile_h - subtile_h
    return tile_w, tile_h, subtile_w, subtile_h, remtile_w, remtile_h

explain_mm = """
- Metrics -
METRICS is of one of the following forms:
 TILESIZE
 TILEWxTILEH
 TILESIZE.SUBTILEWxSUBTILEH

These metrics define the tile's size and divide it up as so:

SUB |
----+----
    | REM

SUB is either specified as the subtile width/height, or defaults to being half of the tile size.
REM is computed from subtracting the subtile size from the tile size.
"""

explain_prefix = "Resources/Textures/Structures/catwalk.rsi/catwalk_"

