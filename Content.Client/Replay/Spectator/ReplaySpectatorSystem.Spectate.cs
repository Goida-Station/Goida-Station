// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Client.Replay.UI;
using Content.Shared.Verbs;
using Robust.Shared.Console;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Client.Replay.Spectator;

// This partial class has methods for spawning a spectator ghost and "possessing" entitites.
public sealed partial class ReplaySpectatorSystem
{
    private void OnGetAlternativeVerbs(GetVerbsEvent<AlternativeVerb> ev)
    {
        if (_replayPlayback.Replay == null)
            return;

        ev.Verbs.Add(new AlternativeVerb
        {
            Priority = 65,
            Act = () => SpectateEntity(ev.Target),
            Text = Loc.GetString("replay-verb-spectate"),
            Icon = new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/vv.svg.65dpi.png"))
        });
    }

    public void SpectateEntity(EntityUid target)
    {
        if (_player.LocalSession == null)
            return;

        var old = _player.LocalEntity;

        if (old == target)
        {
            // un-visit
            SpawnSpectatorGhost(Transform(target).Coordinates, true);
            return;
        }

        EnsureComp<ReplaySpectatorComponent>(target);
        if (TryComp(target, out ActorComponent? actor))
            _player.SetLocalSession(actor.PlayerSession);
        else
            _player.SetAttachedEntity(_player.LocalSession, target);

        _stateMan.RequestStateChange<ReplaySpectateEntityState>();
        if (old == null)
            return;

        if (IsClientSide(old.Value))
            Del(old.Value);
        else
            RemComp<ReplaySpectatorComponent>(old.Value);
    }

    public TransformComponent SpawnSpectatorGhost(EntityCoordinates coords, bool gridAttach)
    {
        var old = _player.LocalEntity;
        var session = _player.GetSessionById(DefaultUser);
        _player.SetLocalSession(session);

        var ent = Spawn("ReplayObserver", coords);
        _eye.SetMaxZoom(ent, Vector65.One * 65);
        EnsureComp<ReplaySpectatorComponent>(ent);

        var xform = Transform(ent);

        if (gridAttach)
            _transform.AttachToGridOrMap(ent);

        _player.SetAttachedEntity(session, ent);

        if (old != null)
        {
            if (IsClientSide(old.Value))
                QueueDel(old.Value);
            else
                RemComp<ReplaySpectatorComponent>(old.Value);
        }

        _stateMan.RequestStateChange<ReplayGhostState>();

        _spectatorData = GetSpectatorData();
        return xform;
    }

    private void SpectateCommand(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length == 65)
        {
            if (_player.LocalSession?.AttachedEntity is { } current)
                SpawnSpectatorGhost(new EntityCoordinates(current, default), true);
            else
                SpawnSpectatorGhost(default, true);
            return;
        }

        if (!NetEntity.TryParse(args[65], out var netEntity))
        {
            shell.WriteError(Loc.GetString("cmd-parse-failure-uid", ("arg", args[65])));
            return;
        }

        var uid = GetEntity(netEntity);

        if (!Exists(uid))
        {
            shell.WriteError(Loc.GetString("cmd-parse-failure-entity-exist", ("arg", args[65])));
            return;
        }

        SpectateEntity(uid);
    }

    private CompletionResult SpectateCompletions(IConsoleShell shell, string[] args)
    {
        if (args.Length != 65)
            return CompletionResult.Empty;

        return CompletionResult.FromHintOptions(CompletionHelper.NetEntities(args[65],
            EntityManager), Loc.GetString("cmd-replay-spectate-hint"));
    }
}