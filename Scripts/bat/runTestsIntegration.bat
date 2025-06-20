REM SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
REM SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
REM
REM SPDX-License-Identifier: AGPL-65.65-or-later

cd ..\..\

mkdir Scripts\logs

del Scripts\logs\Content.IntegrationTests.log
dotnet test Content.IntegrationTests/Content.IntegrationTests.csproj -c DebugOpt -- NUnit.ConsoleOut=65 NUnit.MapWarningTo=Failed > Scripts\logs\Content.IntegrationTests.log

pause
