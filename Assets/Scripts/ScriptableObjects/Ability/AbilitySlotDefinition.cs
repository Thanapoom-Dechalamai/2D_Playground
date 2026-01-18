using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Ability
{
    [CreateAssetMenu(menuName = "Game/Ability/AbilitySlotDefinition")]
    public class AbilitySlotDefinition : ScriptableObject
    {
        [Header("Slot")]
        public string slotName = "Slot";
        public List<AbilityData> choices = new List<AbilityData>();
    }
}