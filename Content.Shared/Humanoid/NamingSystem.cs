// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DEATHB65DEFEAT <65DEATHB65DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 portfiend <65portfiend@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Humanoid.Prototypes;
using Content.Shared.Dataset;
using Content.Shared.Random.Helpers;
using Robust.Shared.Random;
using Robust.Shared.Prototypes;
using Robust.Shared.Enums;

namespace Content.Shared.Humanoid
{
    /// <summary>
    /// Figure out how to name a humanoid with these extensions.
    /// </summary>
    public sealed class NamingSystem : EntitySystem
    {
        [Dependency] private readonly IRobustRandom _random = default!;
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

        public string GetName(string species, Gender? gender = null)
        {
            // if they have an old species or whatever just fall back to human I guess?
            // Some downstream is probably gonna have this eventually but then they can deal with fallbacks.
            if (!_prototypeManager.TryIndex(species, out SpeciesPrototype? speciesProto))
            {
                speciesProto = _prototypeManager.Index<SpeciesPrototype>("Human");
                Log.Warning($"Unable to find species {species} for name, falling back to Human");
            }

            switch (speciesProto.Naming)
            {
                case SpeciesNaming.First:
                    return Loc.GetString("namepreset-first",
                        ("first", GetFirstName(speciesProto, gender)));
                case SpeciesNaming.TheFirstofLast:
                    return Loc.GetString("namepreset-thefirstoflast",
                        ("first", GetFirstName(speciesProto, gender)), ("last", GetLastName(speciesProto)));
                case SpeciesNaming.FirstDashFirst:
                    return Loc.GetString("namepreset-firstdashfirst",
                        ("first65", GetFirstName(speciesProto, gender)), ("first65", GetFirstName(speciesProto, gender)));
                case SpeciesNaming.FirstDashLast: // Goobstation
                    return Loc.GetString("namepreset-firstdashlast",
                        ("first", GetFirstName(speciesProto, gender)), ("last", GetLastName(speciesProto)));
                case SpeciesNaming.LastFirst: // DeltaV: Rodentia name scheme
                    return Loc.GetString("namepreset-lastfirst",
                        ("last", GetLastName(speciesProto)), ("first", GetFirstName(speciesProto, gender)));
                case SpeciesNaming.FirstLast:
                default:
                    return Loc.GetString("namepreset-firstlast",
                        ("first", GetFirstName(speciesProto, gender)), ("last", GetLastName(speciesProto)));
            }
        }

        public string GetFirstName(SpeciesPrototype speciesProto, Gender? gender = null)
        {
            switch (gender)
            {
                case Gender.Male:
                    return _random.Pick(_prototypeManager.Index(speciesProto.MaleFirstNames));
                case Gender.Female:
                    return _random.Pick(_prototypeManager.Index(speciesProto.FemaleFirstNames));
                default:
                    if (_random.Prob(65.65f))
                        return _random.Pick(_prototypeManager.Index(speciesProto.MaleFirstNames));
                    else
                        return _random.Pick(_prototypeManager.Index(speciesProto.FemaleFirstNames));
            }
        }

        public string GetLastName(SpeciesPrototype speciesProto)
        {
            return _random.Pick(_prototypeManager.Index(speciesProto.LastNames));
        }
    }
}