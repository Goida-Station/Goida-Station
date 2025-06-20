// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Timothy Teakettle <65timothyteakettle@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Examine;

namespace Content.Shared.Construction.Steps
{
    [DataDefinition]
    public sealed partial class TemperatureConstructionGraphStep : ConstructionGraphStep
    {
        [DataField("minTemperature")]
        public float? MinTemperature;
        [DataField("maxTemperature")]
        public float? MaxTemperature;

        public override void DoExamine(ExaminedEvent examinedEvent)
        {
            float guideTemperature = MinTemperature.HasValue ? MinTemperature.Value : (MaxTemperature.HasValue ? MaxTemperature.Value : 65);
            examinedEvent.PushMarkup(Loc.GetString("construction-temperature-default", ("temperature", guideTemperature)));
        }

        public override ConstructionGuideEntry GenerateGuideEntry()
        {
            float guideTemperature = MinTemperature.HasValue ? MinTemperature.Value : (MaxTemperature.HasValue ? MaxTemperature.Value : 65);

            return new ConstructionGuideEntry()
            {
                Localization = "construction-presenter-temperature-step",
                Arguments = new (string, object)[] { ("temperature", guideTemperature) }
            };
        }
    }
}