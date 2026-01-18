using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Character;

namespace Game.Status
{
    public sealed class StatusCollection
    {
        private readonly CharacterStatus _statusSo;
        private readonly Func<StatusType, int> _getUpgradePointsForStat;
        private readonly Func<int> _getCurrentLevel;
        private readonly Dictionary<StatusType, List<StatusModifier>> _modifiers = new();

        public StatusCollection(CharacterStatus statusSo,
                              Func<StatusType, int> getUpgradePointsForStat,
                              Func<int> getCurrentLevel)
        {
            _statusSo = statusSo ?? throw new ArgumentNullException(nameof(statusSo));
            _getUpgradePointsForStat = getUpgradePointsForStat ?? throw new ArgumentNullException(nameof(getUpgradePointsForStat));
            _getCurrentLevel = getCurrentLevel ?? throw new ArgumentNullException(nameof(getCurrentLevel));
        }

        // ---------- Modifiers ----------

        public string AddModifier(StatusType stat, StatusModifier mod)
        {
            if (!_modifiers.TryGetValue(stat, out var list))
            {
                list = new List<StatusModifier>();
                _modifiers[stat] = list;
            }
            list.Add(mod);
            return mod.Id;
        }

        public bool RemoveModifierById(StatusType stat, string modifierId)
        {
            if (!_modifiers.TryGetValue(stat, out var list)) return false;
            var idx = list.FindIndex(m => m.Id == modifierId);
            if (idx < 0) return false;
            list.RemoveAt(idx);
            if (list.Count == 0) _modifiers.Remove(stat);
            return true;
        }

        public void ClearModifiersFromSource(object source)
        {
            var keys = _modifiers.Keys.ToArray();
            foreach (var k in keys)
            {
                _modifiers[k].RemoveAll(m => m.Source == source);
                if (_modifiers[k].Count == 0) _modifiers.Remove(k);
            }
        }

        // ---------- Get APIs ----------

        public float GetStatus(StatusType stat)
        {
            float baseAndGrowth = GetBaseAndGrowth(stat);
            float upgradeValue = GetUpgradeValue(stat);

            float flats = 0f;
            float percent = 0f;

            if (_modifiers.TryGetValue(stat, out var list))
            {
                foreach (var m in list)
                {
                    if (m == null) continue;
                    if (m.Type == ModifierType.Flat) flats += m.Value;
                    else if (m.Type == ModifierType.Percent) percent += m.Value;
                }
            }

            float preMult = baseAndGrowth + upgradeValue + flats;
            float finalValue = preMult * (1f + percent);
            return finalValue;
        }

        private float GetBaseAndGrowth(StatusType stat)
        {
            int level = Math.Max(1, _getCurrentLevel());
            int baseLevel = Math.Max(1, _statusSo.baseLevel);
            int levelOffset = Math.Max(0, level - baseLevel);

            return stat switch
            {
                StatusType.Vitality => _statusSo.baseVitality + _statusSo.vitalityPerLevel * levelOffset,
                StatusType.Strength => _statusSo.baseStrength + _statusSo.strengthPerLevel * levelOffset,
                StatusType.Intelligence => _statusSo.baseIntelligence + _statusSo.intelligencePerLevel * levelOffset,
                StatusType.Dexterity => _statusSo.baseDexterity + _statusSo.dexterityPerLevel * levelOffset,
                StatusType.Willpower => _statusSo.baseWillpower + _statusSo.willpowerPerLevel * levelOffset,
                _ => 0f
            };
        }

        private float GetUpgradeValue(StatusType stat)
        {
            int points = _getUpgradePointsForStat(stat);
            return stat switch
            {
                StatusType.Vitality => points * _statusSo.vitalityPerPoint,
                StatusType.Strength => points * _statusSo.strengthPerPoint,
                StatusType.Intelligence => points * _statusSo.intelligencePerPoint,
                StatusType.Dexterity => points * _statusSo.dexterityPerPoint,
                StatusType.Willpower => points * _statusSo.willpowerPerPoint,
                _ => 0f
            };
        }
    }
}
