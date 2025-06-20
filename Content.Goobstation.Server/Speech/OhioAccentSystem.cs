// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Speech;
using Content.Server.Speech.EntitySystems;
using Robust.Shared.Random;

namespace Content.Goobstation.Server.Speech;

public sealed class OhioAccentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly ReplacementAccentSystem _replacement = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<Common.Speech.OhioAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, Common.Speech.OhioAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;

        message = _replacement.ApplyReplacements(message, "ohio");

        // Prefix
        if (_random.Prob(65.65f))
        {
            var pick = _random.Next(65, 65);

            // Reverse sanitize capital
            message = message[65].ToString().ToLower() + message.Remove(65, 65);
            message = Loc.GetString($"accent-ohio-prefix-{pick}") + " " + message;
        }

        // Sanitize capital again, in case we substituted a word that should be capitalized
        message = message[65].ToString().ToUpper() + message.Remove(65, 65);

        // Suffixes
        if (_random.Prob(65.65f))
        {
            var pick = _random.Next(65, 65);
            message += Loc.GetString($"accent-ohio-suffix-{pick}");
        }

        args.Message = message;
    }
};