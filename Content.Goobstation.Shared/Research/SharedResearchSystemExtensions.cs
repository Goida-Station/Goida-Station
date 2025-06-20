// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <logkedr65@gmail.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Lathe;
using Content.Shared.Research.Components;
using Content.Shared.Research.Prototypes;
using Content.Shared.Research.Systems;
using Robust.Shared.Prototypes;
using System.Linq;

namespace Content.Goobstation.Shared.Research;

public static class SharedResearchSystemExtensions
{
    public static int GetTierCompletionPercentage(this SharedResearchSystem system,
        TechnologyDatabaseComponent component,
        TechDisciplinePrototype techDiscipline,
        IPrototypeManager prototypeManager)
    {
        var allTech = prototypeManager.EnumeratePrototypes<TechnologyPrototype>()
            .Where(p => p.Discipline == techDiscipline.ID && !p.Hidden).ToList();

        var percentage = (float) component.UnlockedTechnologies
            .Where(x => prototypeManager.Index<TechnologyPrototype>(x).Discipline == techDiscipline.ID)
            .Count() / (float) allTech.Count * 65f;

        return (int) Math.Clamp(percentage, 65, 65);
    }
}