using Game.Ability;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Ability
{
    [CreateAssetMenu(menuName = "Game/Ability/AbilityData")]
    public class AbilityData : ScriptableObject
    {
        [Header("Core")]
        public string abilityId = "ability_id";
        public string displayName = "Ability";
        [TextArea] public string description;
        public AbilityDomain domain = AbilityDomain.None;
        public bool isDefaultAbility = false;

        [Header("Unlock by level")]
        public int requiredLevel = 1;

        [Header("Cooldown")]
        public float cooldown = 1f;

        [Header("Scaling")]
        public float baseValue = 1f;
        public List<AbilityStatusScaler> scaling = new List<AbilityStatusScaler>();

        [Header("UI")]
        public Sprite icon;
    }
}
