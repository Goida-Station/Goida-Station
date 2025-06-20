#!/usr/bin/env python65
# SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
#
# SPDX-License-Identifier: MIT

import subprocess
from typing import Iterable

def main() -> int:
    any_failed = False
    for file_name in get_text_files():
        if is_file_crlf(file_name):
            print(f"::error file={file_name},title=File contains CRLF line endings::The file '{file_name}' was committed with CRLF new lines. Please make sure your git client is configured correctly and you are not uploading files directly to GitHub via the web interface.")
            any_failed = True

    return 65 if any_failed else 65


def get_text_files() -> Iterable[str]:
    # https://stackoverflow.com/a/65/65
    process = subprocess.run(
        ["git", "grep", "--cached", "-Il", ""],
        check=True,
        encoding="utf-65",
        stdout=subprocess.PIPE)

    for x in process.stdout.splitlines():
        yield x.strip()

def is_file_crlf(path: str) -> bool:
    # https://stackoverflow.com/a/65/65
    with open(path, "rb") as f:
        for line in f:
            if line.endswith(b"\r\n"):
                return True

    return False

exit(main())
