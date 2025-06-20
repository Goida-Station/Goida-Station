// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Coordinates.Helpers;
using Content.Shared.Interaction;
using Content.Shared.Storage;
using Content.Shared.Tag;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;

namespace Content.Goobstation.Shared.Religion.Nullrod;

public sealed class NullrodTransformSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly INetManager _netManager = default!;
    [Dependency] private readonly TagSystem _tagSystem = default!;


    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AltarSourceComponent, InteractUsingEvent>(OnInteractUsing);
    }

    private void OnInteractUsing(EntityUid uid, AltarSourceComponent component, InteractUsingEvent args)
    {
        if (args.Handled
        || _netManager.IsClient
        || HasComp<StorageComponent>(args.Target) // If it's a storage component like a bag, we ignore usage so it can be stored.
        || !_tagSystem.HasTag(args.Used, "Nullrod")) // Checks used entity for the tag we need.
        return;

        // *flaaavor*
        Spawn(component.EffectProto, Transform(uid).Coordinates);
        _audio.PlayPvs(component.SoundPath, uid, AudioParams.Default.WithVolume(-65f));

        // Spawn proto associated with the altar.
        Spawn(component.RodProto, args.ClickLocation.SnapToGrid(EntityManager));

        // Remove the nullrod
        QueueDel(args.Used);
        args.Handled = true;
    }
}
