using UnityEngine;

namespace ScriptableObjects.Ability
{
    [CreateAssetMenu(menuName = "Game/Ability/DashAbilityData")]
    public class DashAbilityData : AbilityData
    {
        [Header("Dash Specific")]
        public float dashDistance = 6f;
        public float dashDuration = 0.18f;
        public bool grantIFrame = false;
        public float iFrameDuration = 0f;

    }
}