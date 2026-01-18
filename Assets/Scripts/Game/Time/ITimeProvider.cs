using UnityEngine;

namespace Game.Time
{
    public interface ITimeProvider
    {
        float Time { get; }
        float DeltaTime { get; }
    }
}
