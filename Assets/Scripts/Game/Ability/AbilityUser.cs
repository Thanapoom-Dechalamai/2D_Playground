using Game.Player;
using ScriptableObjects.Ability;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Ability
{
    [DisallowMultipleComponent]
    public class AbilityUser : MonoBehaviour
    {
        [Header("Slot system")]
        [SerializeField] private int maxSlots = 3;

        [Header("Ability Owned List")]
        [SerializeField] private List<AbilityData> equippedAbilities = new();

        private readonly Dictionary<AbilityData, AbilityBase> _abilityLookup = new();
        private readonly Dictionary<string, AbilityData> _abilityIdLookup = new();

        private IAbilityOwner _owner;
        private ClassComponent _classComponent;

        public int MaxSlots => maxSlots;

        private void Awake()
        {
            _owner = GetComponent<IAbilityOwner>();
            _classComponent = GetComponent<ClassComponent>();

            var abilities = GetComponentsInChildren<AbilityBase>(includeInactive: true);
            foreach (var a in abilities)
            {
                if (a == null) continue;
                var key = a.Data;
                if (key != null)
                {
                    _abilityLookup[key] = a;
                    if (!string.IsNullOrEmpty(key.abilityId)) _abilityIdLookup[key.abilityId] = key;
                }
                else
                {
                    _abilityLookup.TryAdd(null, a);
                }

                if (_owner != null) a.BindOwner(_owner);
            }

            while (equippedAbilities.Count < maxSlots) equippedAbilities.Add(null);
        }

        public int GetUnlockedSlotCount()
        {
            if (_owner == null) return 0;
            return Mathf.Clamp(_owner.CurrentLevel / 10, 0, maxSlots);
        }

        public List<AbilityData> GetChoicesForSlot(int slotIndex)
        {
            if (_classComponent == null) return new List<AbilityData>();
            return _classComponent.GetChoicesForSlot(slotIndex);
        }

        public bool EquipAbilityToSlot(int slotIndex, AbilityData ability)
        {
            int unlocked = GetUnlockedSlotCount();
            if (slotIndex < 0 || slotIndex >= maxSlots) return false;
            if (slotIndex >= unlocked) return false;
            if (ability == null) return false;
            if (!_abilityLookup.ContainsKey(ability)) return false;
            if (!ability.isDefaultAbility && !ability) return false;

            equippedAbilities[slotIndex] = ability;
            return true;
        }

        public bool TryActivateSlot(int slotIndex, ActivationContext ctx)
        {
            if (slotIndex < 0 || slotIndex >= equippedAbilities.Count) return false;
            var data = equippedAbilities[slotIndex];
            if (data == null) return false;
            if (!_abilityLookup.TryGetValue(data, out var ability)) return false;
            return ability.TryActivate(ctx);
        }

        public bool TryActivateById(string abilityId, ActivationContext ctx)
        {
            if (string.IsNullOrEmpty(abilityId)) return false;
            if (!_abilityIdLookup.TryGetValue(abilityId, out var data)) return false;
            if (!_abilityLookup.TryGetValue(data, out var ability)) return false;
            return ability.TryActivate(ctx);
        }

        public AbilityBase GetAbilityByData(AbilityData data)
        {
            _abilityLookup.TryGetValue(data, out var ability);
            return ability;
        }

        public IReadOnlyList<AbilityData> GetEquippedAbilities() => equippedAbilities.AsReadOnly();
    }
}
