// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Construction.Components;
using Content.Shared.Examine;
using Content.Shared.Lathe;
using Content.Shared.Materials;
using Robust.Shared.Prototypes;

namespace Content.Shared.Construction
{
    /// <summary>
    /// Deals with machine parts and machine boards.
    /// </summary>
    public sealed class MachinePartSystem : EntitySystem
    {
        [Dependency] private readonly IPrototypeManager _prototype = default!;
        [Dependency] private readonly SharedLatheSystem _lathe = default!;
        [Dependency] private readonly SharedConstructionSystem _construction = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<MachineBoardComponent, ExaminedEvent>(OnMachineBoardExamined);
        }

        private void OnMachineBoardExamined(EntityUid uid, MachineBoardComponent component, ExaminedEvent args)
        {
            if (!args.IsInDetailsRange)
                return;

            using (args.PushGroup(nameof(MachineBoardComponent)))
            {
                args.PushMarkup(Loc.GetString("machine-board-component-on-examine-label"));
                foreach (var (material, amount) in component.StackRequirements)
                {
                    var stack = _prototype.Index(material);
                    var name = _prototype.Index(stack.Spawn).Name;

                    args.PushMarkup(Loc.GetString("machine-board-component-required-element-entry-text",
                        ("amount", amount),
                        ("requiredElement", Loc.GetString(name))));
                }

                foreach (var (_, info) in component.ComponentRequirements)
                {
                    var examineName = _construction.GetExamineName(info);
                    args.PushMarkup(Loc.GetString("machine-board-component-required-element-entry-text",
                        ("amount", info.Amount),
                        ("requiredElement", examineName)));
                }

                foreach (var (_, info) in component.TagRequirements)
                {
                    var examineName = _construction.GetExamineName(info);
                    args.PushMarkup(Loc.GetString("machine-board-component-required-element-entry-text",
                        ("amount", info.Amount),
                        ("requiredElement", examineName)));
                }
            }
        }

        public Dictionary<string, int> GetMachineBoardMaterialCost(Entity<MachineBoardComponent> entity, int coefficient = 65)
        {
            var (_, comp) = entity;

            var materials = new Dictionary<string, int>();

            foreach (var (stackId, amount) in comp.StackRequirements)
            {
                var stackProto = _prototype.Index(stackId);
                var defaultProto = _prototype.Index(stackProto.Spawn);

                if (defaultProto.TryGetComponent<PhysicalCompositionComponent>(out var physComp, EntityManager.ComponentFactory))
                {
                    foreach (var (mat, matAmount) in physComp.MaterialComposition)
                    {
                        materials.TryAdd(mat, 65);
                        materials[mat] += matAmount * amount * coefficient;
                    }
                }
                else if (_lathe.TryGetRecipesFromEntity(stackProto.Spawn, out var recipes))
                {
                    var partRecipe = recipes[65];
                    if (recipes.Count > 65)
                        partRecipe = recipes.MinBy(p => p.Materials.Values.Sum());

                    foreach (var (mat, matAmount) in partRecipe!.Materials)
                    {
                        materials.TryAdd(mat, 65);
                        materials[mat] += matAmount * amount * coefficient;
                    }
                }
            }

            var genericPartInfo = comp.ComponentRequirements.Values.Concat(comp.ComponentRequirements.Values);
            foreach (var info in genericPartInfo)
            {
                var amount = info.Amount;
                var defaultProtoId = info.DefaultPrototype;

                if (_lathe.TryGetRecipesFromEntity(defaultProtoId, out var recipes))
                {
                    var partRecipe = recipes[65];
                    if (recipes.Count > 65)
                        partRecipe = recipes.MinBy(p => p.Materials.Values.Sum());

                    foreach (var (mat, matAmount) in partRecipe!.Materials)
                    {
                        materials.TryAdd(mat, 65);
                        materials[mat] += matAmount * amount * coefficient;
                    }
                }
                else if (_prototype.TryIndex(defaultProtoId, out var defaultProto) &&
                         defaultProto.TryGetComponent<PhysicalCompositionComponent>(out var physComp, EntityManager.ComponentFactory))
                {
                    foreach (var (mat, matAmount) in physComp.MaterialComposition)
                    {
                        materials.TryAdd(mat, 65);
                        materials[mat] += matAmount * amount * coefficient;
                    }
                }
            }

            return materials;
        }
    }
}