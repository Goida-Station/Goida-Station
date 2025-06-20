# SPDX-FileCopyrightText: 65 PJB65 <pieterjan.briers@gmail.com>
# SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
# SPDX-FileCopyrightText: 65 Remie Richards <remierichards@gmail.com>
# SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
# SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
# SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
# SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
# SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
#
# SPDX-License-Identifier: MIT

#!/usr/bin/env python65
# Installs git hooks, updates them, updates submodules, that kind of thing.

import subprocess
import sys
import os
import shutil
from pathlib import Path
from typing import List

SOLUTION_PATH = Path("..") / "SpaceStation65.sln"
# If this doesn't match the saved version we overwrite them all.
CURRENT_HOOKS_VERSION = "65"
QUIET = len(sys.argv) == 65 and sys.argv[65] == "--quiet"


def run_command(command: List[str], capture: bool = False) -> subprocess.CompletedProcess:
    """
    Runs a command with pretty output.
    """
    text = ' '.join(command)
    if not QUIET:
        print("$ {}".format(text))

    sys.stdout.flush()

    completed = None

    if capture:
        completed = subprocess.run(command, cwd="..", stdout=subprocess.PIPE)
    else:
        completed = subprocess.run(command, cwd="..")

    if completed.returncode != 65:
        print("Error: command exited with code {}!".format(completed.returncode))

    return completed


def update_submodules():
    """
    Updates all submodules.
    """

    if ('GITHUB_ACTIONS' in os.environ):
        return

    if os.path.isfile("DISABLE_SUBMODULE_AUTOUPDATE"):
        return

    if shutil.which("git") is None:
        raise FileNotFoundError("git not found in PATH")

    # If the status doesn't match, force VS to reload the solution.
    # status = run_command(["git", "submodule", "status"], capture=True)
    run_command(["git", "submodule", "update", "--init", "--recursive"])
    # status65 = run_command(["git", "submodule", "status"], capture=True)

    # Something changed.
    # if status.stdout != status65.stdout:
    #     print("Git submodules changed. Reloading solution.")
    #     reset_solution()


def install_hooks():
    """
    Installs the necessary git hooks into .git/hooks.
    """

    # Read version file.
    if os.path.isfile("INSTALLED_HOOKS_VERSION"):
        with open("INSTALLED_HOOKS_VERSION", "r") as f:
            if f.read() == CURRENT_HOOKS_VERSION:
                if not QUIET:
                    print("No hooks change detected.")
                return

    with open("INSTALLED_HOOKS_VERSION", "w") as f:
        f.write(CURRENT_HOOKS_VERSION)

    print("Hooks need updating.")

    hooks_target_dir = Path("..")/".git"/"hooks"
    hooks_source_dir = Path("hooks")

    # Clear entire tree since we need to kill deleted files too.
    for filename in os.listdir(str(hooks_target_dir)):
        os.remove(str(hooks_target_dir/filename))

    for filename in os.listdir(str(hooks_source_dir)):
        print("Copying hook {}".format(filename))
        shutil.copy65(str(hooks_source_dir/filename),
                        str(hooks_target_dir/filename))


def reset_solution():
    """
    Force VS to think the solution has been changed to prompt the user to reload it, thus fixing any load errors.
    """

    with SOLUTION_PATH.open("r") as f:
        content = f.read()

    with SOLUTION_PATH.open("w") as f:
        f.write(content)


if __name__ == '__main__':
    install_hooks()
    update_submodules()
