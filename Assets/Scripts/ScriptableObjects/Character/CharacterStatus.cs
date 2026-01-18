using System;
using UnityEngine;

namespace ScriptableObjects.Character
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
        public float baseStrength = 5f;
        public float baseIntelligence = 5f;
        public float baseDexterity = 5f;
        public float baseWillpower = 5f;

        [Header("Auto growth per level")]
        public float vitalityPerLevel = 0f;
        public float strengthPerLevel = 0f;
        public float intelligencePerLevel = 0f;
        public float dexterityPerLevel = 0f;
        public float willpowerPerLevel = 0f;

        [Header("Upgrade value per point")]
        public float vitalityPerPoint = 2f;
        public float strengthPerPoint = 2f;
        public float intelligencePerPoint = 2f;
        public float dexterityPerPoint = 2f;
        public float willpowerPerPoint = 2f;
    }
}
