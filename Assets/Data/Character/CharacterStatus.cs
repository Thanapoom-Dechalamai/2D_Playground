using UnityEngine;

namespace ScriptableObjects.Ability
{
    [CreateAssetMenu(menuName = "Game/Stats/CharacterStatus")]
    public class CharacterStatus : ScriptableObject
    {
        [Header("Identity")]
        public string statusId = "character_status";
        public string displayName = "Character";

        [Header("Level")]
        public int baseLevel = 1;
        public int maxLevel = 100;

        [Header("Base stats")]
        public float baseVitality = 5f;
        public float baseStrength = 10f;
        public float baseIntelligence = 3f;
        public float baseDexterity = 5f;
        public float baseWillpower = 5f;

        [Header("Growth per level (additive)")]
        public float vitalityPerLevel = 1f;
        public float strengthPerLevel = 1f;
        public float intelligencePerLevel = 0.5f;
        public float dexterityPerLevel = 0.8f;
        public float willpowerPerLevel = 0.5f;
    }
}
