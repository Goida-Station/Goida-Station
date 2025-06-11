using Content.Goobstation.Common.Style;
using Content.Goobstation.Shared.Style;
using Content.Shared.Abilities;

namespace Content.Goobstation.Server.Style
{
    [Virtual]
    public sealed class StyleEffectsSystem : EntitySystem
    {
        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<StyleRankChangedEvent>(OnRankChanged);
        }

        private void OnRankChanged(StyleRankChangedEvent args)
        {
            var entity = GetEntity(args.Entity);

            if (args.NewRank == StyleRank.R)
                AddComp<DogVisionComponent>(entity);
            if (args.OldRank == StyleRank.SSS) // todo make it work
                RemComp<DogVisionComponent>(entity);
        }
    }
}
