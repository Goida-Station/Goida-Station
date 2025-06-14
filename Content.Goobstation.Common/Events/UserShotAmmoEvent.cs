using Robust.Shared.Serialization;

namespace Content.Goobstation.Common.Events;

/// <summary>
/// Raised when entity shoots a gun.
/// </summary>
[Serializable, NetSerializable]
public sealed class UserShotAmmoEvent : EntityEventArgs
{
    public List<NetEntity> FiredProjectiles { get; }

    public UserShotAmmoEvent(List<NetEntity> firedProjectiles)
    {
        FiredProjectiles = firedProjectiles;
    }
}
