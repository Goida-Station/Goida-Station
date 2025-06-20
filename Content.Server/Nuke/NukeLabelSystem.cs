// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Nuke;

/// <summary>
///     This handles labelling an entity with a nuclear bomb label.
/// </summary>
public sealed class NukeLabelSystem : EntitySystem
{
    [Dependency] private readonly NukeSystem _nuke = default!;
    [Dependency] private readonly MetaDataSystem _metaData = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<NukeLabelComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, NukeLabelComponent nuke, MapInitEvent args)
    {
        var label = Loc.GetString(nuke.Prefix, ("serial", _nuke.GenerateRandomNumberString(nuke.SerialLength)));
        var meta = MetaData(uid);
        _metaData.SetEntityName(uid, $"{meta.EntityName} ({label})", meta);
    }
}