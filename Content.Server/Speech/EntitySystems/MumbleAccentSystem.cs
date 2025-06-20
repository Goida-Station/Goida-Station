// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class MumbleAccentSystem : EntitySystem
{
    [Dependency] private readonly ReplacementAccentSystem _replacement = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MumbleAccentComponent, AccentGetEvent>(OnAccentGet);
    }

    public string Accentuate(string message, MumbleAccentComponent component)
    {
        return _replacement.ApplyReplacements(message, "mumble");
    }

    private void OnAccentGet(EntityUid uid, MumbleAccentComponent component, AccentGetEvent args)
    {
        args.Message = Accentuate(args.Message, component);
    }
}