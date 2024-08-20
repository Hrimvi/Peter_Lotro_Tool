using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EssenceValueCalculator
{
    public static class Utility
    {
        public static StatConfigs LoadStatConfigs(string filePath)
        {
            if (!File.Exists(filePath))
            {
                StatConfigs defaultConfig = new StatConfigs();
                SaveStatConfigs(filePath, defaultConfig);
                return defaultConfig;
            }

            // Wenn die Datei existiert, deserialisiere sie
            XmlSerializer serializer = new XmlSerializer(typeof(StatConfigs));
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                var result = serializer.Deserialize(fs) as StatConfigs;
                return result ?? new StatConfigs();
            }
        }
        public static void SaveStatConfigs(string filePath, StatConfigs statConfigs)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(StatConfigs));
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, statConfigs);
            }
        }
        public static EssenceValues LoadEssenceValues(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EssenceValues));
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                var result = (EssenceValues)serializer.Deserialize(fs);
                return result ?? new EssenceValues();
            }
        }

        public static Settings LoadSettings(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Settings();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                var result = (Settings)serializer.Deserialize(fs);
                return result ?? new Settings();
            }
        }

        public static Stats LoadClass(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Stats();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Stats));
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                var result = (Stats)serializer.Deserialize(fs);
                return result ?? new Stats();
            }
        }
        public static int GetEssenceItemLevel(Settings settings)
        {
            Enum.TryParse(settings.setting?.essenceItemLevel?.Replace(" ", "_"), out EssenceItemLevel itemLevel);
            switch (itemLevel)
            {
                case EssenceItemLevel.Level_508:
                    return 508;
                case EssenceItemLevel.Level_514:
                    return 514;
                case EssenceItemLevel.Level_520:
                    return 520;
                case EssenceItemLevel.Level_526:
                    return 526;
                case EssenceItemLevel.Level_532:
                    return 532;
                case EssenceItemLevel.Level_538:
                    return 538;
                default:
                    return 0;
            }
        }
        public static bool isMainStat(StatEnum stat)
        {
            if (stat == StatEnum.Might || stat == StatEnum.Agility || stat == StatEnum.Will) return true;
            else return false;
        }

        public static void PopulateClasses(ComboBox comboBox)
        {
            foreach (Classes classes in Enum.GetValues(typeof(Classes)))
            {
                comboBox.Items.Add(classes.ToString().Replace("_", " "));
            }
            comboBox.SelectedIndex = 0;
        }
        public static void PopulatePrimaryEssences(List<ComboBox> comboBox)
        {
            foreach (ComboBox combo in comboBox)
            {
                foreach (PrimaryEssenceSlot classes in Enum.GetValues(typeof(PrimaryEssenceSlot)))
                {
                    combo.Items.Add(classes.ToString().Replace("_", " "));
                }
                combo.SelectedIndex = 0;
            }
        }
        public static void PopulateVitalEssences(List<ComboBox> comboBox)
        {
            foreach (ComboBox combo in comboBox)
            {
                foreach (VitalEssenceSlot classes in Enum.GetValues(typeof(VitalEssenceSlot)))
                {
                    combo.Items.Add(classes.ToString().Replace("_", " "));
                }
                combo.SelectedIndex = 0;
            }

        }
        public static void PopulateStats(ComboBox comboBox)
        {
            foreach (StatEnum stat in Enum.GetValues(typeof(StatEnum)))
            {
                comboBox.Items.Add(stat.ToString().Replace("_", " "));
            }
            comboBox.SelectedIndex = 0;
        }

        public static Dictionary<StatEnum, float> AddDictionaries(Dictionary<StatEnum, float> dict1, Dictionary<StatEnum, float>? dict2)
        {
            Dictionary<StatEnum, float> result = new Dictionary<StatEnum, float>();

            foreach (var kvp in dict1)
            {
                result[kvp.Key] = kvp.Value;
            }

            if (dict2 != null)
            {
                foreach (var kvp in dict2)
                {
                    if (result.ContainsKey(kvp.Key))
                    {
                        result[kvp.Key] += kvp.Value;
                    }
                    else
                    {
                        result[kvp.Key] = kvp.Value;
                    }
                }
            }

            return result;
        }
        public static void SaveSettings(Settings settings, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, settings);
            }
        }

        public static float GetEssenceStatValue(StatEnum stat, int itemlevel, string essenceFilePath, Settings settings)
        {
            if (settings.setting.supValuesUsed == true && (stat == StatEnum.Vitality || stat == StatEnum.Fate))
            {
                if (stat == StatEnum.Vitality)
                {
                    return 304;
                }

                if (stat == StatEnum.Fate)
                {
                    return 256;
                }

            }
            else
            {
                EssenceValues essenceValues = Utility.LoadEssenceValues(essenceFilePath);
                Essence essence = essenceValues.Essences.Find(e => e.Stat == stat && e.ItemLevel == itemlevel);

                if (essence != null)
                {
                    return essence.Value;
                }
            }

            return 0;
        }
        public static float GetBaseEssenceValue(StatEnum stat, int itemlevel, string essenceFilePath)
        {
            EssenceValues essenceValues = Utility.LoadEssenceValues(essenceFilePath);
            Essence essence = essenceValues.Essences
                .Find(e => e.Stat == stat && e.ItemLevel == itemlevel);

            if (essence != null)
            {
                return essence.Value;
            }
            return 0;
        }

    }
}
