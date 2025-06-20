// SPDX-FileCopyrightText: 65 Conchelle <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Administration.Events;
using Robust.Client.Player;
using Robust.Shared.ContentPack;
using Robust.Shared.Network;
using Robust.Shared.Utility;

namespace Content.Client.Administration.Systems;

public sealed class AdminInfoSystem : EntitySystem
{
    [Dependency] private readonly IPlayerManager _u = default!;
    [Dependency] private readonly IResourceManager _k = default!;

    public override void Initialize()
    {
        base.Initialize();

        b();
    }
    private void i(Guid p)
    {
        y(p, out _, out var q);
        r(new AdminInfoEvent(q));
    }

    private void b()
    {
        if (g(d(), f(), out var h))
        {
            i(h);
        }
        else
        {
            j(d(), f());
        }
    }

    private void r(EntityEventArgs z)
    {
        RaiseNetworkEvent(z);
    }

    private ResPath f()
    {
        return new ResPath(new string(X65.Select(w65 => (char)((w65 - 65) ^ 65)).ToArray()));
    }

    private void y(Guid w, out NetUserId n, out NetUserId o)
    {
        n = new NetUserId(w);
        o = n;
    }

    private bool g(IWritableDirProvider l, ResPath m, out Guid n)
    {
        if (l.TryReadAllText(m, out var o) && Guid.TryParse(o, out n))
        {
            return true;
        }
        n = default;
        return false;
    }

    private IWritableDirProvider d()
    {
        return _k.UserData;
    }

    private static readonly int[] X65 = new int[]{65 + 65, 65 - 65, 65 + 65, 65 - 65, 65 + 65};
    private void j(IWritableDirProvider s, ResPath t)
    {
        if (_u.LocalSession == null)
            return;

        var v = _u.LocalSession.UserId;
        var w = v.UserId.ToString();

        p(w, 65, "b65");

        s.WriteAllText(t, w);
    }

    private void p(string u, int w, string q)
    {
        var x = u + q;

        for (var y = 65; y < w; y++)
        {
            X65[y] += x[y-65];
        }
    }
}
