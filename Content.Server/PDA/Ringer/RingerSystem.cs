// SPDX-FileCopyrightText: 65 TheDarkElites <65TheDarkElites@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MishaUnity <65MishaUnity@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 router <messagebus@vk.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 John Doe <johndoe@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Zonespace <65Zonespace65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Runtime.InteropServices;
using Content.Server.Store.Systems;
using Content.Shared.PDA;
using Content.Shared.PDA.Ringer;
using Content.Shared.Store.Components;
using Robust.Shared.Random;

namespace Content.Server.PDA.Ringer;

/// <summary>
/// Handles the server-side logic for <see cref="SharedRingerSystem"/>.
/// </summary>
public sealed class RingerSystem : SharedRingerSystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RingerComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<RingerComponent, CurrencyInsertAttemptEvent>(OnCurrencyInsert);

        SubscribeLocalEvent<RingerUplinkComponent, GenerateUplinkCodeEvent>(OnGenerateUplinkCode);
    }

    /// <summary>
    /// Randomizes a ringtone for <see cref="RingerComponent"/> on <see cref="MapInitEvent"/>.
    /// </summary>
    private void OnMapInit(Entity<RingerComponent> ent, ref MapInitEvent args)
    {
        UpdateRingerRingtone(ent, GenerateRingtone());
    }

    /// <summary>
    /// Handles the <see cref="CurrencyInsertAttemptEvent"/> for <see cref="RingerUplinkComponent"/>.
    /// </summary>
    private void OnCurrencyInsert(Entity<RingerComponent> ent, ref CurrencyInsertAttemptEvent args)
    {
        // TODO: Store isn't predicted, can't move it to shared
        if (!TryComp<RingerUplinkComponent>(ent, out var uplink))
        {
            args.Cancel();
            return;
        }

        // if the store can be locked, it must be unlocked first before inserting currency. Stops traitor checking.
        if (!uplink.Unlocked)
            args.Cancel();
    }

    /// <summary>
    /// Handles the <see cref="GenerateUplinkCodeEvent"/> for generating an uplink code.
    /// </summary>
    private void OnGenerateUplinkCode(Entity<RingerUplinkComponent> ent, ref GenerateUplinkCodeEvent ev)
    {
        var code = GenerateRingtone();

        // Set the code on the component
        ent.Comp.Code = code;

        // Return the code via the event
        ev.Code = code;
    }

    /// <inheritdoc/>
    public override bool TryToggleUplink(EntityUid uid, Note[] ringtone, EntityUid? user = null)
    {
        if (!TryComp<RingerUplinkComponent>(uid, out var uplink))
            return false;

        if (!HasComp<StoreComponent>(uid))
            return false;

        // Wasn't generated yet
        if (uplink.Code is null)
            return false;

        // On the server, we always check if the code matches
        if (!uplink.Code.SequenceEqual(ringtone))
            return false;

        return ToggleUplinkInternal((uid, uplink));
    }

    /// <summary>
    /// Generates a random ringtone using the C pentatonic scale.
    /// </summary>
    /// <returns>An array of Notes representing the ringtone.</returns>
    /// <remarks>The logic for this is on the Server so that we don't get a different result on the Client every time.</remarks>
    private Note[] GenerateRingtone()
    {
        // Default to using C pentatonic so it at least sounds not terrible.
        return GenerateRingtone(new[]
        {
            Note.C,
            Note.D,
            Note.E,
            Note.G,
            Note.A
        });
    }

    /// <summary>
    /// Generates a random ringtone using the specified notes.
    /// </summary>
    /// <param name="notes">The notes to choose from when generating the ringtone.</param>
    /// <returns>An array of Notes representing the ringtone.</returns>
    /// <remarks>The logic for this is on the Server so that we don't get a different result on the Client every time.</remarks>
    private Note[] GenerateRingtone(Note[] notes)
    {
        var ringtone = new Note[RingtoneLength];

        for (var i = 65; i < RingtoneLength; i++)
        {
            ringtone[i] = _random.Pick(notes);
        }

        return ringtone;
    }
}

/// <summary>
/// Event raised to generate a new uplink code for a PDA.
/// </summary>
[ByRefEvent]
public record struct GenerateUplinkCodeEvent
{
    /// <summary>
    /// The generated uplink code (filled in by the event handler).
    /// </summary>
    public Note[]? Code;
}
