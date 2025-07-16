using Robust.Shared.Random;

namespace Content.Goida.Common.ComponentsChance;

public sealed class ComponentsChanceSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _gambling = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ComponentsChanceComponent, MapInitEvent>(OnInit);
    }

    private void OnInit(Entity<ComponentsChanceComponent> ent, ref MapInitEvent args)
    {
        if (_gambling.Prob(ent.Comp.Chance))
            EntityManager.AddComponents(ent, ent.Comp.Components);
    }
}
