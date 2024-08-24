using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EssenceValueCalculator
{
    internal static class EssenceValueCalculatorFunctions
    {
        public static float ReturnEssenceValueFromDictionary(Dictionary<StatEnum, float> stats, int essenceItemLevel, Classes playerClass, Settings settings, StatConfigs statConfig, string essenceFilePath)
        {
            float essenceValue = 0;
            string usedConfigName = settings?.setting?.usedConfigName;

            var selectedConfig = statConfig?.Configs
                .FirstOrDefault(c => c.Name.Equals(usedConfigName, StringComparison.OrdinalIgnoreCase));

            foreach (var kvp in stats)
            {
                var statElement = selectedConfig?.Stats.FirstOrDefault(s => s.Name.Equals(kvp.Key.ToString().Replace("_", " "), StringComparison.OrdinalIgnoreCase));
                bool isActive = statElement?.Active ?? false;
                float valueMultiplier = isActive ? statElement.Value : 1.0f;
                
                if (kvp.Key == StatEnum.Basic_EssenceSlot || kvp.Key == StatEnum.Armour)
                {
                    if (kvp.Key == StatEnum.Armour)
                    {
                        essenceValue += kvp.Value / Utility.GetEssenceStatValue(StatEnum.Physical_Mitigation, essenceItemLevel, essenceFilePath, settings) * valueMultiplier;
                        essenceValue += kvp.Value / Utility.GetEssenceStatValue(StatEnum.Tactical_Mitigation, essenceItemLevel, essenceFilePath, settings) * valueMultiplier;
                    }
                    else if (kvp.Key == StatEnum.Basic_EssenceSlot)
                    {
                        essenceValue += kvp.Value;
                    }

                    continue;
                }
                



                if (isActive == false) continue;
                if (kvp.Key == StatEnum.Morale || kvp.Key == StatEnum.Power)
                {
                    if (kvp.Key == StatEnum.Power)
                    {
                        if (playerClass == Classes.Beorning)
                        {
                            essenceValue += 0;
                        }
                        else
                        {
                            essenceValue += kvp.Value / Utility.GetEssenceStatValue(StatEnum.Fate, essenceItemLevel, essenceFilePath, settings) * valueMultiplier;
                        }
                    }
                    else if (kvp.Key == StatEnum.Morale)
                    {
                        essenceValue += (kvp.Value / 4.5f) / Utility.GetEssenceStatValue(StatEnum.Vitality, essenceItemLevel, essenceFilePath, settings) * valueMultiplier;
                    }
                }
                else
                {
                    if (!Utility.isMainStat(kvp.Key))
                    {
                        essenceValue += kvp.Value / Utility.GetEssenceStatValue(kvp.Key, essenceItemLevel, essenceFilePath, settings) * valueMultiplier;
                    }
                }
            }

            return essenceValue;
        }

        public static Dictionary<StatEnum, float>? CalculateMainstat(Dictionary<StatEnum, float> mainStatCalc,StatEnum statEnum, float statValue, Classes classe, Stats characterStatDerivations)
        {
            if (statEnum == StatEnum.Power || statEnum == StatEnum.Morale) return null;
            mainStatCalc.Clear();
            var baseStats = new HashSet<StatEnum>
            {
                StatEnum.Critical_Rating,
                StatEnum.Physical_Mastery,
                StatEnum.Tactical_Mastery,
                StatEnum.Physical_Mitigation,
                StatEnum.Tactical_Mitigation,
                StatEnum.Critical_Defence,
                StatEnum.Finesse,
                StatEnum.Block,
                StatEnum.Parry,
                StatEnum.Evade,
                StatEnum.Outgoing_Healing,
                StatEnum.Incoming_Healing,
                StatEnum.Resistance,
                StatEnum.Morale,
                StatEnum.Power
            };

            string mainStat = statEnum.ToString().Replace("_", " ");
            string playerClass = classe.ToString()?.Replace("_", " ");

            var classData = characterStatDerivations.Classes
                .FirstOrDefault(c => c.Name.Equals(playerClass, StringComparison.OrdinalIgnoreCase));

            var mainstat = classData?.Mainstats.FirstOrDefault(m => m.Name != null && m.Name.Equals(mainStat, StringComparison.OrdinalIgnoreCase));

            if (mainstat != null && mainstat.Stats != null)
            {
                foreach (var stat in mainstat.Stats)
                {
                    if (stat != null)
                    {
                        string statName = stat.Name?.Replace(" ", "_");
                        if (statName != null && Enum.TryParse(statName, out StatEnum statEnumValue))
                        {
                            mainStatCalc[statEnumValue] = stat.Value * statValue;
                        }
                    }
                }
            }

            return mainStatCalc;

        }


    }
}
