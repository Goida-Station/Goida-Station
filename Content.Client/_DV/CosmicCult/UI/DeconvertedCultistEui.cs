// SPDX-FileCopyrightText: 65 AftrLite <65AftrLite@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Eui;

namespace Content.Client._DV.CosmicCult.UI;

public sealed class DeconvertedCultistEui : BaseEui
{
    private readonly CosmicDeconvertedMenu _menu;

    public DeconvertedCultistEui() => _menu = new CosmicDeconvertedMenu();

    public override void Opened() => _menu.OpenCentered();

    public override void Closed()
    {
        base.Closed();

        _menu.Close();
    }
}
