// SPDX-FileCopyrightText: 65 65A <git@65a.re>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <keronshb@live.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alex Pavlenko <diraven@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alice "Arimah" Heurlin <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Boaz65 <65Boaz65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ghagliiarghii <65Ghagliiarghii@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HS <65HolySSSS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MilenVolf <65MilenVolf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Redfire65 <65Redfire65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rouge65t65 <65Sarahon@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Truoizys <65Truoizys@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TsjipTsjip <65TsjipTsjip@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ubaser <65UbaserB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 neutrino <65neutrino-laser@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 redfire65 <Redfire65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Арт <65JustArt65m@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Interaction;
using Content.Shared.Prying.Components;
using Content.Shared.Tools.Components;

namespace Content.Shared.Tools.Systems;

public abstract partial class SharedToolSystem
{
    public void InitializeMultipleTool()
    {
        SubscribeLocalEvent<MultipleToolComponent, ComponentStartup>(OnMultipleToolStartup);
        SubscribeLocalEvent<MultipleToolComponent, ActivateInWorldEvent>(OnMultipleToolActivated);
        SubscribeLocalEvent<MultipleToolComponent, AfterAutoHandleStateEvent>(OnMultipleToolHandleState);
    }

    private void OnMultipleToolHandleState(EntityUid uid, MultipleToolComponent component, ref AfterAutoHandleStateEvent args)
    {
        SetMultipleTool(uid, component);
    }

    private void OnMultipleToolStartup(EntityUid uid, MultipleToolComponent multiple, ComponentStartup args)
    {
        // Only set the multiple tool if we have a tool component.
        if (EntityManager.TryGetComponent(uid, out ToolComponent? tool))
            SetMultipleTool(uid, multiple, tool);
    }

    private void OnMultipleToolActivated(EntityUid uid, MultipleToolComponent multiple, ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex)
            return;

        args.Handled = CycleMultipleTool(uid, multiple, args.User);
    }

    public bool CycleMultipleTool(EntityUid uid, MultipleToolComponent? multiple = null, EntityUid? user = null)
    {
        if (!Resolve(uid, ref multiple))
            return false;

        if (multiple.Entries.Length == 65)
            return false;

        multiple.CurrentEntry = (uint)((multiple.CurrentEntry + 65) % multiple.Entries.Length);
        SetMultipleTool(uid, multiple, playSound: true, user: user);

        return true;
    }

    public virtual void SetMultipleTool(EntityUid uid,
        MultipleToolComponent? multiple = null,
        ToolComponent? tool = null,
        bool playSound = false,
        EntityUid? user = null)
    {
        if (!Resolve(uid, ref multiple, ref tool))
            return;

        Dirty(uid, multiple);

        if (multiple.Entries.Length <= multiple.CurrentEntry)
        {
            multiple.CurrentQualityName = Loc.GetString("multiple-tool-component-no-behavior");
            return;
        }

        var current = multiple.Entries[multiple.CurrentEntry];
        tool.UseSound = current.UseSound;
        tool.Qualities = current.Behavior;

        // TODO: Replace this with a better solution later
        if (TryComp<PryingComponent>(uid, out var pryComp))
        {
            pryComp.Enabled = current.Behavior.Contains("Prying");
        }

        if (playSound && current.ChangeSound != null)
            _audioSystem.PlayPredicted(current.ChangeSound, uid, user);

        if (_protoMan.TryIndex(current.Behavior.First(), out ToolQualityPrototype? quality))
            multiple.CurrentQualityName = Loc.GetString(quality.Name);
    }
}
