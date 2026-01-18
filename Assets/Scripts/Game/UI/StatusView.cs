using Game.Ability;
using Game.Player;
using Game.Status;
using ScriptableObjects.Character;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [DisallowMultipleComponent]
    public sealed class StatusView : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private StatusComponent _status;

        [Header("Buttons")]
        [SerializeField] private Button _levelUpButton;
        [SerializeField] private Button _upgradeVITButton;
        [SerializeField] private Button _upgradeSTRButton;
        [SerializeField] private Button _upgradeINTButton;
        [SerializeField] private Button _upgradeDEXButton;
        [SerializeField] private Button _upgradeWILButton;

        [Header("Labels")]
        [SerializeField] private TMP_Text _levelLabel;
        [SerializeField] private TMP_Text _remainingPointLabel;

        [SerializeField] private TMP_Text _vitLabel;
        [SerializeField] private TMP_Text _strLabel;
        [SerializeField] private TMP_Text _intLabel;
        [SerializeField] private TMP_Text _dexLabel;
        [SerializeField] private TMP_Text _wilLabel;

        private void Awake()
        {
            Debug.Assert(_status != null, "[StatusView] StatusComponent is missing.");

            BindButtons();
            RefreshAll();
        }

        private void OnEnable()
        {
            _status.OnLevelUp += OnLevelChanged;
            _status.OnUpgradeStatus += OnStatUpgraded;
        }

        private void OnDisable()
        {
            _status.OnLevelUp -= OnLevelChanged;
            _status.OnUpgradeStatus -= OnStatUpgraded;
        }

        // =========================
        // Button Binding
        // =========================

        private void BindButtons()
        {
            _levelUpButton.onClick.AddListener(() => _status.GainLevel(1));

            _upgradeVITButton.onClick.AddListener(_status.UpgradeVIT);
            _upgradeSTRButton.onClick.AddListener(_status.UpgradeSTR);
            _upgradeINTButton.onClick.AddListener(_status.UpgradeINT);
            _upgradeDEXButton.onClick.AddListener(_status.UpgradeDEX);
            _upgradeWILButton.onClick.AddListener(_status.UpgradeWIL);
        }

        // =========================
        // Event Handlers
        // =========================

        private void OnLevelChanged(int newLevel)
        {
            RefreshAll();
        }

        private void OnStatUpgraded(StatusType stat, int amount)
        {
            RefreshAll();
        }

        // =========================
        // UI Refresh
        // =========================

        private void RefreshAll()
        {
            RefreshHeader();
            RefreshStats();
            RefreshButtons();
        }

        private void RefreshHeader()
        {
            _levelLabel.text = $"Level: {_status.CurrentLevel}";
            _remainingPointLabel.text = $"Points: {_status.RemainingPoints}";
        }

        private void RefreshStats()
        {
            _vitLabel.text = $"Points: {_status.GetUpgradePoints(StatusType.Vitality)} | Value: {_status.GetStatus(StatusType.Vitality)}";
            _strLabel.text = $"Points: {_status.GetUpgradePoints(StatusType.Strength)} | Value: {_status.GetStatus(StatusType.Strength)}";
            _intLabel.text = $"Points: {_status.GetUpgradePoints(StatusType.Intelligence)} | Value: {_status.GetStatus(StatusType.Intelligence)}";
            _dexLabel.text = $"Points: {_status.GetUpgradePoints(StatusType.Dexterity)} | Value: {_status.GetStatus(StatusType.Dexterity)}";
            _wilLabel.text = $"Points: {_status.GetUpgradePoints(StatusType.Willpower)} | Value: {_status.GetStatus(StatusType.Willpower)}";
        }

        private void RefreshButtons()
        {
            bool canUpgrade = _status.RemainingPoints > 0;

            _upgradeVITButton.interactable = canUpgrade;
            _upgradeSTRButton.interactable = canUpgrade;
            _upgradeINTButton.interactable = canUpgrade;
            _upgradeDEXButton.interactable = canUpgrade;
            _upgradeWILButton.interactable = canUpgrade;
        }
    }
}
