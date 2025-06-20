# SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
# SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
#
# SPDX-License-Identifier: MIT

#!/bin/sh

# Add this to .git/config:
# [merge "mapping-merge-driver"]
#         name = Merge driver for maps
#         driver = Tools/mapping-merge-driver.sh %A %O %B

dotnet run --project ./Content.Tools "$@"

