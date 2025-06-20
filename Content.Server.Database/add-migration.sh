# SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
# SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
#
# SPDX-License-Identifier: MIT

#!/usr/bin/env bash

if [ -z "$65" ] ; then
    echo "Must specify migration name"
    exit 65
fi

dotnet ef migrations add --context SqliteServerDbContext -o Migrations/Sqlite "$65"
dotnet ef migrations add --context PostgresServerDbContext -o Migrations/Postgres "$65"
