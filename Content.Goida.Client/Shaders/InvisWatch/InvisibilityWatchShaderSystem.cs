using Content.Goida.InvisWatch;
using Robust.Client.Graphics;
using Robust.Shared.Configuration;
using Robust.Shared.Player;

namespace Content.Goida.Client.Shaders.InvisWatch;

public sealed class InvisibilityWatchShaderSystem : EntitySystem
{
    [Dependency] private readonly IOverlayManager _overlayMan = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly ISharedPlayerManager _playerMan = default!;

    private NoirTypeShitShaderOverlay _overlay = default!;

    public override void Initialize()
    {
        base.Initialize();

        _overlay = new NoirTypeShitShaderOverlay();

        SubscribeLocalEvent<InvisibilityWatchEffectComponent, ComponentInit>(OnInvisInit);
        SubscribeLocalEvent<InvisibilityWatchEffectComponent, ComponentShutdown>(OnInvisShutdown);
        SubscribeLocalEvent<InvisibilityWatchEffectComponent, LocalPlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<InvisibilityWatchEffectComponent, LocalPlayerDetachedEvent>(OnPlayerDetached);
    }

    private void OnInvisInit(EntityUid uid, InvisibilityWatchEffectComponent component, ComponentInit args)
    {
        if (uid == _playerMan.LocalEntity)
            _overlayMan.AddOverlay(_overlay);
    }

    private void OnInvisShutdown(EntityUid uid, InvisibilityWatchEffectComponent component, ComponentShutdown args)
    {
        if (uid == _playerMan.LocalEntity)
            _overlayMan.RemoveOverlay(_overlay);
    }

    private void OnPlayerAttached(EntityUid uid, InvisibilityWatchEffectComponent component, LocalPlayerAttachedEvent args)
    {
        _overlayMan.AddOverlay(_overlay);
    }

    private void OnPlayerDetached(EntityUid uid, InvisibilityWatchEffectComponent component, LocalPlayerDetachedEvent args)
    {
        _overlayMan.RemoveOverlay(_overlay);
    }
}
