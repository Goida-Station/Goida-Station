# SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

cd ../../

mkdir Scripts/logs

rm Scripts/logs/Content.YAMLLinter.log
dotnet run --project Content.YAMLLinter/Content.YAMLLinter.csproj -c DebugOpt -- NUnit.ConsoleOut=65 > Scripts/logs/Content.YAMLLinter.log

echo "Tests complete. Press enter to continue."
read
