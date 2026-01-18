using Game.Time;
using ScriptableObjects.Ability;
using System;
using UnityEngine;

namespace Game.Ability
{
    [DisallowMultipleComponent]
    public abstract class AbilityBase : MonoBehaviour
    {
        [SerializeField] protected AbilityData data;

        protected IAbilityOwner Owner { get; private set; }
        public AbilityData Data => data;
        public string AbilityId => data != null ? data.abilityId : name;
        public string DisplayName => data != null ? data.displayName : name;
        public int RequiredLevel => data != null ? data.requiredLevel : 1;
        public float Cooldown => data != null ? data.cooldown : 0f;
        public bool ConsumesSlot => data != null ? !data.isDefaultAbility : true;

        public event Action<AbilityBase> OnActivated;
        public event Action<AbilityBase> OnFinished;

        private float _lastUsed = -999f;

        protected virtual void Awake()
        {
            if (data == null)
                Debug.LogWarning($"{name}: AbilityData is not assigned.", this);
        }

        public void BindOwner(IAbilityOwner owner)
        {
            Owner = owner;
        }

        public bool IsOnCooldown => TimeProvider.Instance.Time < _lastUsed + Cooldown;

        public virtual bool CanActivate(ActivationContext ctx)
        {
            if (data == null) return false;
            if (Owner == null) return false;
            if (Owner.CurrentLevel < RequiredLevel) return false;
            if (IsOnCooldown) return false;
            return true;
        }

        public bool TryActivate(ActivationContext ctx)
        {
            if (!CanActivate(ctx)) return false;
            _lastUsed = TimeProvider.Instance.Time;
            OnActivated?.Invoke(this);
            OnActivateInternal(ctx);
            return true;
        }

        protected abstract void OnActivateInternal(ActivationContext ctx);

        protected void Finish()
        {
            OnFinished?.Invoke(this);
        }

        protected float GetScaledValue()
        {
            if (data == null) return 0f;
            float total = data.baseValue;
            if (Owner != null && data.scaling != null)
            {
                foreach (var stat in data.scaling)
                {
                    if (stat.factor == 0f) continue;
                    total += Owner.StatusProvider.GetStatus(stat.type) * stat.factor;
                }
            }
            return total;
        }
    }
}
