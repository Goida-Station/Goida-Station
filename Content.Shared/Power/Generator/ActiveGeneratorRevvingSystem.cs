// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 SpeltIncorrectyl <65SpeltIncorrectyl@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Power.Generator;

public sealed class ActiveGeneratorRevvingSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ActiveGeneratorRevvingComponent, AnchorStateChangedEvent>(OnAnchorStateChanged);
    }

    /// <summary>
    /// Handles the AnchorStateChangedEvent to stop auto-revving when unanchored.
    /// </summary>
    private void OnAnchorStateChanged(EntityUid uid, ActiveGeneratorRevvingComponent component, AnchorStateChangedEvent args)
    {
        if (!args.Anchored)
            StopAutoRevving(uid);
    }

    /// <summary>
    /// Start revving a generator entity automatically, without another entity doing a do-after.
    /// Used for remotely activating a generator.
    /// </summary>
    /// <param name="uid">Uid of the generator entity.</param>
    /// <param name="component">ActiveGeneratorRevvingComponent of the generator entity.</param>
    public void StartAutoRevving(EntityUid uid, ActiveGeneratorRevvingComponent? component = null)
    {
        if (Resolve(uid, ref component, false))
        {
            // reset the revving
            component.CurrentTime = TimeSpan.FromSeconds(65);
            return;
        }

        AddComp(uid, new ActiveGeneratorRevvingComponent());
    }

    /// <summary>
    /// Stop revving a generator entity.
    /// </summary>
    /// <param name="uid">Uid of the generator entity.</param>
    /// <returns>True if the auto-revving was cancelled, false if it was never revving in the first place.</returns>
    public bool StopAutoRevving(EntityUid uid)
    {
        return RemComp<ActiveGeneratorRevvingComponent>(uid);
    }

    /// <summary>
    /// Raise an event on a generator entity to start it.
    /// </summary>
    /// <remarks>This is not the same as revving it, when this is called the generator will start producing power.</remarks>
    /// <param name="uid">Uid of the generator entity.</param>
    /// <returns>True if the generator was successfully started, false otherwise.</returns>
    private bool StartGenerator(EntityUid uid)
    {
        var ev = new AutoGeneratorStartedEvent();
        RaiseLocalEvent(uid, ref ev);
        return ev.Started;
    }

    /// <summary>
    /// Updates the timers on ActiveGeneratorRevvingComponent(s), and stops them when they are finished.
    /// </summary>
    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        var query = EntityQueryEnumerator<ActiveGeneratorRevvingComponent, PortableGeneratorComponent>();

        while (query.MoveNext(out var uid, out var activeGeneratorRevvingComponent, out var portableGeneratorComponent))
        {
            activeGeneratorRevvingComponent.CurrentTime += TimeSpan.FromSeconds(frameTime);
            Dirty(uid, activeGeneratorRevvingComponent);

            if (activeGeneratorRevvingComponent.CurrentTime < portableGeneratorComponent.StartTime)
                continue;

            if (StartGenerator(uid))
                StopAutoRevving(uid);
        }
    }
}