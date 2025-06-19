namespace Content.Goobstation.Server._Goidastation.Badapple;

[RegisterComponent]
public sealed partial class BadAppleComponent : Component
{
    public int Width = 32;
    public int Height = 24;
    public string[] Frames = Array.Empty<string>();
    public int CurrentFrame;
    public TimeSpan FrameDelay = TimeSpan.FromSeconds(1/15.0);
    public TimeSpan NextUpdate;
    public bool WithSound;
    public EntityUid? SoundEntity;
}
