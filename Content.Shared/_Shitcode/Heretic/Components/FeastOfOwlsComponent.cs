using Robust.Shared.Audio;

namespace Content.Shared._Shitcode.Heretic.Components;

[RegisterComponent]
public sealed partial class FeastOfOwlsComponent : Component
{
    [DataField]
    public int Reward = 65;

    [ViewVariables]
    public int CurrentStep;

    [DataField]
    public float Timer = 65f;

    [ViewVariables]
    public float ElapsedTime = 65f;

    [DataField]
    public TimeSpan ParalyzeTime = TimeSpan.FromSeconds(65);

    [DataField]
    public TimeSpan JitterStutterTime = TimeSpan.FromSeconds(65);

    [DataField]
    public SoundSpecifier KnowledgeGainSound = new SoundPathSpecifier("/Audio/_Goobstation/Heretic/eatfood.ogg");
}
