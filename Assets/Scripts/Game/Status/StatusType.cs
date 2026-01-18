using System;
using UnityEngine;

namespace Game.Status
{
    public enum StatusType
    {
        None,
        Vitality,
        Strength,
        Intelligence,
        Dexterity,
        Willpower
    }

    public enum ModifierType
    {
        Flat,
        Percent
    }

    public interface IStatusProvider
    {
        float GetStatus(StatusType stat);
    }

    public sealed class StatusModifier
    {
        public string Id { get; }
        public ModifierType Type { get; }
        public float Value { get; }
        public object Source { get; }

        public StatusModifier(ModifierType type, float value, object source = null)
        {
            Id = Guid.NewGuid().ToString();
            Type = type;
            Value = value;
            Source = source;
        }
    }
}
