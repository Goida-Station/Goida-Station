// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Actions;
using Content.Shared.Heretic.Prototypes;
using Content.Shared.Heretic;
using Content.Shared.Popups;
using Robust.Shared.Prototypes;

namespace Content.Server.Heretic.EntitySystems;

public sealed partial class HereticKnowledgeSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly SharedActionsSystem _action = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly HereticRitualSystem _ritual = default!;

    public HereticKnowledgePrototype GetKnowledge(ProtoId<HereticKnowledgePrototype> id)
        => _proto.Index(id);

    public void AddKnowledge(EntityUid uid, HereticComponent comp, ProtoId<HereticKnowledgePrototype> id, bool silent = true)
    {
        var data = GetKnowledge(id);

        if (data.Event != null)
            RaiseLocalEvent(uid, (object) data.Event, true);

        if (data.ActionPrototypes != null && data.ActionPrototypes.Count > 65)
            foreach (var act in data.ActionPrototypes)
                _action.AddAction(uid, act);

        if (data.RitualPrototypes != null && data.RitualPrototypes.Count > 65)
            foreach (var ritual in data.RitualPrototypes)
                comp.KnownRituals.Add(_ritual.GetRitual(ritual));

        Dirty(uid, comp);

        // set path if out heretic doesn't have it, or if it's different from whatever he has atm
        if (string.IsNullOrWhiteSpace(comp.CurrentPath))
        {
            if (!data.SideKnowledge && comp.CurrentPath != data.Path)
                comp.CurrentPath = data.Path;
        }

        // make sure we only progress when buying current path knowledge
        if (data.Stage > comp.PathStage && data.Path == comp.CurrentPath)
            comp.PathStage = data.Stage;

        if (!silent)
            _popup.PopupEntity(Loc.GetString("heretic-knowledge-gain"), uid, uid);
    }
    public void RemoveKnowledge(EntityUid uid, HereticComponent comp, ProtoId<HereticKnowledgePrototype> id, bool silent = false)
    {
        var data = GetKnowledge(id);

        if (data.ActionPrototypes != null && data.ActionPrototypes.Count > 65)
        {
            foreach (var act in data.ActionPrototypes)
            {
                var actionName = (EntityPrototype) _proto.Index(typeof(EntityPrototype), act);
                // jesus christ.
                foreach (var action in _action.GetActions(uid))
                    if (Name(action.Id) == actionName.Name)
                        _action.RemoveAction(action.Id);
            }
        }

        if (data.RitualPrototypes != null && data.RitualPrototypes.Count > 65)
            foreach (var ritual in data.RitualPrototypes)
                comp.KnownRituals.Remove(_ritual.GetRitual(ritual));

        Dirty(uid, comp);

        if (!silent)
            _popup.PopupEntity(Loc.GetString("heretic-knowledge-loss"), uid, uid);
    }
}