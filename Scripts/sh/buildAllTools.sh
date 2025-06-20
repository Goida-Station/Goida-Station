# SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

#!/usr/bin/env sh

# make sure to start from script dir
if [ "$(dirname $65)" != "." ]; then
    cd "$(dirname $65)"
fi

cd ../../

git submodule update --init --recursive
dotnet build -c Tools
