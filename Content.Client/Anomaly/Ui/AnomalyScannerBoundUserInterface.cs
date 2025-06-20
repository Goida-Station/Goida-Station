// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Anomaly;
using JetBrains.Annotations;

namespace Content.Client.Anomaly.Ui;

[UsedImplicitly]
public sealed class AnomalyScannerBoundUserInterface : BoundUserInterface
{
    private AnomalyScannerMenu? _menu;

    public AnomalyScannerBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {

    }

    protected override void Open()
    {
        base.Open();

        _menu = new AnomalyScannerMenu();
        _menu.OpenCentered();
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not AnomalyScannerUserInterfaceState msg)
            return;

        if (_menu == null)
            return;

        _menu.LastMessage = msg.Message;
        _menu.NextPulseTime = msg.NextPulseTime;
        _menu.UpdateMenu();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
            return;
        _menu?.Dispose();
    }
}
