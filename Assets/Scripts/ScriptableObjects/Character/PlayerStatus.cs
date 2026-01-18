using UnityEngine;


namespace ScriptableObjects.Character
{
    [CreateAssetMenu(menuName = "Game/Stats/PlayerStatus")]
    public class PlayerStatus : CharacterStatus
    {
        [Header("Player progression")]
        [Tooltip("Stat points granted per level-up")]
        public int statPointsPerLevel = 1;

        [Tooltip("Starting unspent stat points")]
        public int startingStatPoints = 0;
    }
}
