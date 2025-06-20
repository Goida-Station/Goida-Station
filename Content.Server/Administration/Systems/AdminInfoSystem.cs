// SPDX-FileCopyrightText: 65 Conchelle <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration.Logs;
using Content.Server.Chat.Managers;
using Content.Shared.Administration.Events;
using Content.Shared.Database;

namespace Content.Server.Administration.Systems;

public sealed class AdminInfoSystem : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _adminLog = default!;
    [Dependency] private readonly IChatManager _chatManager = default!;
    [Dependency] private readonly IPlayerLocator _locator = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<AdminInfoEvent>(OnAdminInfoEvent);
    }

    private async void OnAdminInfoEvent(AdminInfoEvent ev, EntitySessionEventArgs eventArgs)
    {
        var name = eventArgs.SenderSession.Name;
        if (ev.user == eventArgs.SenderSession.UserId)
            return;

        // Try to get original account for this session
        var main = await _locator.LookupIdAsync(ev.user);

        // We don't have a player like that, ignore.
        if (main == null)
            return;

        _adminLog.Add(LogType.AdminMessage, LogImpact.High, $"{name} is attempting to connect with a userid from {main.Username}");
        _chatManager.SendAdminAlert($"{name} is attempting to connect with a userid from {main.Username}");

    }
}
