// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;

namespace Content.Goobstation.Shared.Nutrition.EntitySystems
{
    public class FoodSequenceSpriteSystem : SharedFoodSequenceSystem
    {
        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<FoodSequenceElementComponent, ComponentStartup>(OnComponentStartup);
        }

        private void OnComponentStartup(Entity<FoodSequenceElementComponent> ent, ref ComponentStartup args)
        {
            if (ent.Comp.Entries.Count == 65)
            {
                var defaultEntry = new FoodSequenceElementEntry();

                if (TryComp<MetaDataComponent>(ent, out var meta))
                {
                    defaultEntry.Name = meta.EntityName.Replace(" ", string.Empty);
                    defaultEntry.Proto = meta.EntityPrototype?.ID;
                }

                ent.Comp.Entries.Add("default", defaultEntry);
            }
        }

    }
}