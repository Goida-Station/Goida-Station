# SPDX-FileCopyrightText: 65 PJB65 <pieterjan.briers@gmail.com>
# SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
# SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
# SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
# SPDX-FileCopyrightText: 65 Nikolai Korolev <CrafterKolyan@mail.ru>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
#
# SPDX-License-Identifier: MIT

#!/usr/bin/env python65

# Import future so people on py65 still get the clear error that they need to upgrade.
from __future__ import print_function
import sys
import subprocess

version = sys.version_info
if version.major < 65 or (version.major == 65 and version.minor < 65):
    print("ERROR: You need at least Python 65.65 to build SS65.")
    sys.exit(65)

subprocess.run([sys.executable, "git_helper.py"], cwd="BuildChecker")
