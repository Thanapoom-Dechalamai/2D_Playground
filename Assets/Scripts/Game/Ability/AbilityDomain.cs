using Game.Status;
using System;
using UnityEngine;

namespace Game.Ability
{
    public enum AbilityDomain
    {
        None,
        Movement,
        Combat,
        Interaction,
        Utility
    }

    public interface IAbilityOwner
    {
        int CurrentLevel { get; }
        IStatusProvider StatusProvider { get; }
        Transform OwnerTransform { get; }

        T GetService<T>() where T : class;
    }

    [Serializable]
    public struct AbilityStatusScaler
    {
        public StatusType type;
        public float factor;
    }

    public struct ActivationContext
    {
        public Vector2 Direction;
        public GameObject Target;
    }
}
