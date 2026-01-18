using ScriptableObjects.Ability;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    [DisallowMultipleComponent]
    public class ClassComponent : MonoBehaviour
    {
        public enum PlayerClassType { None, Knight, Brawler, Assassin, Mage, Archer, Healer }

        [Header("Class")]
        public PlayerClassType currentClass = PlayerClassType.None;

        public List<AbilitySlotDefinition> slotDefinitions = new List<AbilitySlotDefinition>();
        public int Level => _playerStats != null ? _playerStats.CurrentLevel : 1;
        public int UnspentStatPoints => _playerStats != null ? _playerStats.RemainingPoints : 0;

        private StatusComponent _playerStats;

        private void Awake()
        {
            _playerStats = GetComponent<StatusComponent>();
        }

        public void GainLevel(int amount = 1)
        {
            if (_playerStats != null)
            {
                _playerStats.GainLevel(amount);
            }
            else
            {
                Debug.LogWarning("[ClassComponent] No StatusComponent found to GainLevel.");
            }
        }

        public List<AbilityData> GetChoicesForSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= slotDefinitions.Count) return new List<AbilityData>();
            return slotDefinitions[slotIndex].choices;
        }
    }
}
