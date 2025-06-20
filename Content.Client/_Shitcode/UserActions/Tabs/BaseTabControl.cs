// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Client.UserInterface;

namespace Content.Client._Shitcode.UserActions.Tabs;

[Virtual]
public class BaseTabControl : Control
{
    public virtual bool UpdateState() { return true; }
}
