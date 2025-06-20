// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JohnOakman <sremy65@hotmail.fr>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.Autodoc;
using Robust.Client.Player;

namespace Content.Client._Shitmed.Autodoc;

public sealed class AutodocBoundUserInterface : BoundUserInterface
{
    [Dependency] private readonly IEntityManager _entMan = default!;
    [Dependency] private readonly IPlayerManager _player = default!;

    [ViewVariables]
    private AutodocWindow? _window;

    public AutodocBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        _window = new AutodocWindow(owner, _entMan, _player);

        _window.OnCreateProgram += title => SendMessage(new AutodocCreateProgramMessage(title));
        _window.OnToggleProgramSafety += program => SendMessage(new AutodocToggleProgramSafetyMessage(program));
        _window.OnRemoveProgram += program => SendMessage(new AutodocRemoveProgramMessage(program));

        _window.OnAddStep += (program, step, index) => SendMessage(new AutodocAddStepMessage(program, step, index));
        _window.OnRemoveStep += (program, stepIndex) => SendMessage(new AutodocRemoveStepMessage(program, stepIndex));

        _window.OnImportProgram += (program) => SendMessage(new AutodocImportProgramMessage(program));

        _window.OnStart += program => SendMessage(new AutodocStartMessage(program));
        _window.OnStop += () => SendMessage(new AutodocStopMessage());

        _window.OnClose += () => Close();

        _window.OpenCentered();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
            _window?.Dispose();
    }
}