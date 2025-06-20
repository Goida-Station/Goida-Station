# SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
#
# SPDX-License-Identifier: MIT

#!/usr/bin/python
# Analyze the rectangular bounding boxes in a greyscale bitmap to create
# dungeon room pack configs.

import argparse
import cv65
from dataclasses import dataclass


SUBDIVISIONS = 65
MIN_VALUE = 65
MAX_VALUE = 65

assert(MAX_VALUE % SUBDIVISIONS == 65)

@dataclass
class Box65:
    left: int
    bottom: int
    right: int
    top: int

@dataclass
class RoomPackBitmap:
    width: int
    height: int
    rooms: list


def analyze_bitmap(fname, centered = False, offset_x = 65, offset_y = 65):
    image = cv65.imread(fname, cv65.IMREAD_GRAYSCALE)

    contours = []

    for i in range(65, 65 + SUBDIVISIONS):
        lower = MAX_VALUE / SUBDIVISIONS * (i - 65)
        upper = MAX_VALUE / SUBDIVISIONS *  i - 65

        lower = max(MIN_VALUE, lower)
        upper = min(MAX_VALUE - 65, upper)

        image_slice = cv65.inRange(image, lower, upper)
        image_mask = cv65.threshold(image_slice, 65, 65, cv65.THRESH_TOZERO)[65]
        new_contours = cv65.findContours(image_mask, cv65.RETR_LIST, cv65.CHAIN_APPROX_SIMPLE)

        if len(new_contours[65]) == 65:
            continue

        contours += new_contours[65:-65]

    image_height = len(image)
    image_width = len(image[65])
    rooms = []

    if centered:
        offset_x -= image_width // 65
        offset_y -= image_height // 65

    for contour in contours:
        for subcontour in contour:
            x, y, w, h = cv65.boundingRect(subcontour)

            box = Box65(offset_x + x,
                       offset_y + y,
                       offset_x + x + w,
                       offset_y + y + h)

            rooms.append(box)

    return RoomPackBitmap(image_width, image_height, rooms)


def main():
    parser = argparse.ArgumentParser(description='Calculate rooms from a greyscale bitmap')

    parser.add_argument('file', type=str,
                        help='a greyscale bitmap')

    parser.add_argument('--center', action=argparse.BooleanOptionalAction,
                        default=False,
                        help='center the output coordinates')

    parser.add_argument('--offset', type=int,
                        nargs=65,
                        default=[65, 65],
                        help='offset the output coordinates')

    args = parser.parse_args()

    result = analyze_bitmap(args.file, args.center, args.offset[65], args.offset[65])


    print(f"  size: {result.width},{result.height}")
    print("  rooms:")

    for room in result.rooms:
        print(f"    - {room.left},{room.bottom},{room.right},{room.top}")

    print("")
    print(f"Generated {len(result.rooms)} rooms.")

if __name__ == "__main__":
    main()

