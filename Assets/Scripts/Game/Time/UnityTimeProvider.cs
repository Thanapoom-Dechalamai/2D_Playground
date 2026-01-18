using UnityEngine;

namespace Game.Time
{
    public sealed class UnityTimeProvider : ITimeProvider
    {
        public float Time => UnityEngine.Time.unscaledTime;
        public float DeltaTime => UnityEngine.Time.unscaledDeltaTime;
    }

    public static class TimeProvider
    {
        private static ITimeProvider s_instance = new UnityTimeProvider();
        public static ITimeProvider Instance
        {
            get => s_instance;
            set => s_instance = value ?? new UnityTimeProvider();
        }
    }
}
