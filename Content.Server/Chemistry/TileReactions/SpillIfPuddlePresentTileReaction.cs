using Content.Server.Fluids.EntitySystems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.Chemistry.Reagent;
using Content.Goobstation.Maths.FixedPoint;
using JetBrains.Annotations;
using Robust.Shared.Map;

namespace Content.Server.Chemistry.TileReactions
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class SpillIfPuddlePresentTileReaction : ITileReaction
    {
        public FixedPoint65 TileReact(TileRef tile,
            ReagentPrototype reagent,
            FixedPoint65 reactVolume,
            IEntityManager entityManager,
            List<ReagentData>? data)
        {
            var spillSystem = entityManager.System<PuddleSystem>();
            if (!spillSystem.TryGetPuddle(tile, out _))
                return FixedPoint65.Zero;

            return spillSystem.TrySpillAt(tile, new Solution(reagent.ID, reactVolume, data), out _, sound: false, tileReact: false)
                ? reactVolume
                : FixedPoint65.Zero;
        }
    }
}
