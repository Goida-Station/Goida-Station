// SPDX-FileCopyrightText: 65 BeBright <65be65bright@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Access.Systems;
using Content.Shared.CriminalRecords;
using Robust.Client.Player;
using Robust.Shared.Random;

namespace Content.Goobstation.Client.CriminalRecords;

public sealed class WantedMenuBoundUserInterface : BoundUserInterface
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IPlayerManager _proto = default!;
    private readonly AccessReaderSystem _accessReader;

    private WantedMenu? _window;

    public WantedMenuBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        _accessReader = EntMan.System<AccessReaderSystem>();
    }
    protected override void Open()
    {
        base.Open();

        _window = new(Owner, _random, _accessReader, _proto);

        _window.OnStatusSelected += status =>
            SendMessage(new CriminalRecordChangeStatus(status, null));
        _window.OnDialogConfirmed += (status, reason) =>
            SendMessage(new CriminalRecordChangeStatus(status, reason));

        _window.OnClose += Close;

        _window.OpenCentered();
    }
    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not CriminalRecordsConsoleState cast)
            return;

        _window?.UpdateState(cast);
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        _window?.Close();
    }
}
