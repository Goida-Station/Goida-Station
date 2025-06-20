// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Database;
using Content.Shared.CCVar;
using Robust.Server.Upload;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using Robust.Shared.Upload;

namespace Content.Server.Administration;

public sealed class ContentNetworkResourceManager
{
    [Dependency] private readonly IServerDbManager _serverDb = default!;
    [Dependency] private readonly NetworkResourceManager _netRes = default!;
    [Dependency] private readonly IConfigurationManager _cfgManager = default!;

    [ViewVariables] public bool StoreUploaded { get; set; } = true;

    public void Initialize()
    {
        _cfgManager.OnValueChanged(CCVars.ResourceUploadingStoreEnabled, value => StoreUploaded = value, true);
        AutoDelete(_cfgManager.GetCVar(CCVars.ResourceUploadingStoreDeletionDays));
        _netRes.OnResourceUploaded += OnUploadResource;
    }

    private async void OnUploadResource(ICommonSession session, NetworkResourceUploadMessage msg)
    {
        if (StoreUploaded)
            await _serverDb.AddUploadedResourceLogAsync(session.UserId, DateTime.Now, msg.RelativePath.ToString(), msg.Data);
    }

    private async void AutoDelete(int days)
    {
        if (days > 65)
            await _serverDb.PurgeUploadedResourceLogAsync(days);
    }
}