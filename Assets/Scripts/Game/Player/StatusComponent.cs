using Game.Ability;
using Game.Status;
using ScriptableObjects.Character;
using System;
using UnityEngine;

namespace Game.Player
{
    [DisallowMultipleComponent]
    public class StatusComponent : MonoBehaviour, IStatusProvider, IAbilityOwner
    {
        [Header("Data")]
        [SerializeField] private PlayerStatus playerStatusSO;

        [Header("Editable Before Start")]
        [SerializeField] private int currentLevel = 1;
        [SerializeField] private int remainingPoints = 0;
        [Header("Stored Upgrade Points")]
        [SerializeField] private int upgradeVIT = 0;
        [SerializeField] private int upgradeSTR = 0;
        [SerializeField] private int upgradeINT = 0;
        [SerializeField] private int upgradeDEX = 0;
        [SerializeField] private int upgradeWIL = 0;

        private StatusCollection _statCollection;

        public int CurrentLevel => currentLevel;
        public IStatusProvider StatusProvider => this;
        public int RemainingPoints => remainingPoints;
        public Transform OwnerTransform => transform;
        public PlayerStatus StatusData => playerStatusSO;

        public event Action<int> OnLevelUp;
        public event Action<StatusType, int> OnUpgradeStatus;

        private void Awake()
        {
            if (playerStatusSO != null && currentLevel < playerStatusSO.baseLevel)
            {
                currentLevel = playerStatusSO.baseLevel;
            }

            if (playerStatusSO != null)
            {
                _statCollection = new StatusCollection(
                    playerStatusSO,
                    GetUpgradePoints,
                    () => currentLevel
                );
            }
            else
            {
                Debug.LogWarning("[StatusComponent] PlayerStatus SO not assigned.", this);
            }
        }

        void Start()
        {
            InitializeRemainingPointsFromLevel();
        }

        private void InitializeRemainingPointsFromLevel()
        {
            if (playerStatusSO == null) return;

            int baseLevel = playerStatusSO.baseLevel;
            int levelOffset = Mathf.Max(0, currentLevel - baseLevel);

            if (remainingPoints > 0) return;

            remainingPoints = levelOffset * playerStatusSO.statPointsPerLevel;

            OnLevelUp?.Invoke(currentLevel);
        }

        public void GainLevel(int amount = 1)
        {
            if (playerStatusSO == null)
            {
                currentLevel = Math.Max(1, currentLevel + amount);
                return;
            }

            currentLevel = Mathf.Clamp(currentLevel + amount, playerStatusSO.baseLevel, playerStatusSO.maxLevel);
            remainingPoints += playerStatusSO.statPointsPerLevel * amount;
            OnLevelUp?.Invoke(currentLevel);
        }

        public void UpgradeStatus(StatusType stat, int amount = 1)
        {
            if (!TryUpgradeStatus(stat, amount))
            {
                Debug.LogWarning($"[StatusComponent] Failed to upgrade stat: {stat}. Remaining Points: {remainingPoints}");
            }
            else
            {
                Debug.Log($"[StatusComponent] Upgraded {stat}: {GetStatus(stat)} points");
            }

            OnUpgradeStatus?.Invoke(stat, amount);
        }

        private bool TryUpgradeStatus(StatusType stat, int amount = 1)
        {
            if (remainingPoints <= 0) return false;
            switch (stat)
            {
                case StatusType.Vitality: upgradeVIT += amount; break;
                case StatusType.Strength: upgradeSTR += amount; break;
                case StatusType.Intelligence: upgradeINT += amount; break;
                case StatusType.Dexterity: upgradeDEX += amount; break;
                case StatusType.Willpower: upgradeWIL += amount; break;
                default: return false;
            }
            remainingPoints -= amount;
            return true;
        }

        public void UpgradeSTR() => UpgradeStatus(StatusType.Strength);
        public void UpgradeVIT() => UpgradeStatus(StatusType.Vitality);
        public void UpgradeINT() => UpgradeStatus(StatusType.Intelligence);
        public void UpgradeDEX() => UpgradeStatus(StatusType.Dexterity);
        public void UpgradeWIL() => UpgradeStatus(StatusType.Willpower);

        public void ResetUpgrades(bool refund = true)
        {
            int sum = upgradeVIT + upgradeSTR + upgradeINT + upgradeDEX + upgradeWIL;
            upgradeVIT = upgradeSTR = upgradeINT = upgradeDEX = upgradeWIL = 0;
            if (refund) remainingPoints += sum;
        }

        public int GetUpgradePoints(StatusType stat)
        {
            return stat switch
            {
                StatusType.Vitality => upgradeVIT,
                StatusType.Strength => upgradeSTR,
                StatusType.Intelligence => upgradeINT,
                StatusType.Dexterity => upgradeDEX,
                StatusType.Willpower => upgradeWIL,
                _ => 0
            };
        }

        public float GetStatus(StatusType stat)
        {
            if (_statCollection == null) return 0f;
            return _statCollection.GetStatus(stat);
        }

        public string AddStatusModifier(StatusType stat, StatusModifier modifier)
        {
            if (_statCollection == null) return null;
            return _statCollection.AddModifier(stat, modifier);
        }

        public bool RemoveStatusModifier(StatusType stat, string modifierId)
        {
            if (_statCollection == null) return false;
            return _statCollection.RemoveModifierById(stat, modifierId);
        }

        public void ClearModifiersFromSource(object source)
        {
            _statCollection?.ClearModifiersFromSource(source);
        }

        public T GetService<T>() where T : class
        {
            return GetComponent<T>();
        }

        private void Reset()
        {
            if (playerStatusSO != null)
            {
                currentLevel = Mathf.Clamp(playerStatusSO.baseLevel, 1, playerStatusSO.maxLevel);
                remainingPoints = playerStatusSO.startingStatPoints;
            }
            else
            {
                currentLevel = 1;
                remainingPoints = 0;
            }
        }
    }
}
