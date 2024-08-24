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
        private const string logFilePath = "log.txt";
        public static void Log(string message)
        {
            try
            {
                // Überprüfen, ob die Log-Datei existiert und gegebenenfalls erstellen
                FileInfo logFile = new FileInfo(logFilePath);
                if (!logFile.Exists)
                {
                    // Erstellen der Datei, wenn sie nicht existiert
                    using (FileStream fs = logFile.Create())
                    {
                        // Optional: Initialisierung oder Schreiben von Header-Informationen
                        byte[] info = new UTF8Encoding(true).GetBytes("Log-Datei erstellt am " + DateTime.Now + Environment.NewLine);
                        fs.Write(info, 0, info.Length);
                    }
                }

                // Hinzufügen der Log-Nachricht zur Datei
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung: Zeige eine MessageBox an, wenn ein Fehler beim Schreiben auftritt
                MessageBox.Show($"Fehler beim Schreiben in die Log-Datei: {ex.Message}");
            }
        }
        public static StatConfigs LoadStatConfigs(string filePath)
        {
            if (!File.Exists(filePath))
            {
                StatConfigs defaultConfig = new StatConfigs();
                SaveStatConfigs(filePath, defaultConfig);
                return defaultConfig;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(StatConfigs));
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                var result = serializer.Deserialize(fs) as StatConfigs;
                return result ?? new StatConfigs();
            }
        }
        public static async Task<Progressions> LoadProgressionsAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Progressions();
            }
            XmlSerializer serializer = new XmlSerializer(typeof(Progressions));
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            {
                var result = await Task.Run(() => serializer.Deserialize(fs) as Progressions);
                return result ?? new Progressions();
            }
        }

        public static async Task<Items> LoadItemsAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Items();
            }
            XmlSerializer serializer = new XmlSerializer(typeof(Items));
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            {
                var result = await Task.Run(() => serializer.Deserialize(fs) as Items);

                if (result != null)
                {
                    result.ItemList = result.ItemList
                        .Where(item => (!string.IsNullOrEmpty(item.equipSlot) && item.equipSlot != "MAIN_HAND_AURA")
                                       && item.EquipmentCategory != 32
                                       && (item.Category != "LEGENDARY_WEAPON" && item.Category != "BRIDLE"))
                        .ToList();
                }

                return result ?? new Items();
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
            if (stat == StatEnum.Might || stat == StatEnum.Agility || stat == StatEnum.Will || stat == StatEnum.Vitality || stat == StatEnum.Fate) return true;
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
                if (stat == StatEnum.TBD) continue;
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

        public static List<int> GetIconIDsFromString(List<int> iconList, string iconIdText, string splitCharacter)
        {
            iconList.Clear();

            string[] icons = iconIdText.Split(splitCharacter);
            foreach (string icon in icons)
            {
                int iconId = int.Parse(icon);
                iconList.Add(iconId);
            }

            return iconList;
        }
        public static Image OverlayIcons(List<Image> images)
        {
            Image baseImage = images[0];
            Bitmap combinedImage = new Bitmap(baseImage.Width, baseImage.Height);

            using (Graphics g = Graphics.FromImage(combinedImage))
            {
                g.DrawImage(baseImage, 0, 0);

                for (int i = 1; i < images.Count; i++)
                {
                    Image overlayImage = images[i];
                    g.DrawImage(overlayImage, 0, 0);
                }
            }

            return combinedImage;
        }
        public static async Task<Image> OverlayIconsAsync(List<Image> icons)
        {
            return await Task.Run(() => OverlayIcons(icons));
        }
        public static float GetStatsFromProgressions(Progressions itemProgressions,long progressionID, int itemLevel)
        {
            // Finde die Progression anhand der ID
            var linearProgression = itemProgressions.LinearInterpolationProgressions
                                  .FirstOrDefault(p => p.Identifier == progressionID);
            var arrayProgression = itemProgressions.ArrayProgressions
                               .FirstOrDefault(p => p.Identifier == progressionID);

            if (linearProgression != null)
            {
                return GetValueFromLinearInterpolation(linearProgression, itemLevel);
            }

            if (arrayProgression != null)
            {
                return GetValueFromArrayProgression(arrayProgression, itemLevel);
            }

            return 0;
        }

        private static float GetValueFromLinearInterpolation(LinearInterpolationProgression progression, int itemLevel)
        {
            var points = progression.Points.OrderBy(p => p.X).ToList();

            var point1 = points.LastOrDefault(p => p.X <= itemLevel);
            var point2 = points.FirstOrDefault(p => p.X > itemLevel);
            if (point1 == null)
            {
                if (points.Count > 1)
                {
                    point1 = points.First();
                    point2 = points[1];
                }
                else
                {
                 
                    return (float)points.First().Y;
                }
            }

            if (point2 == null)
            {
                point2 = points.Last();
                point1 = points[points.Count - 2]; 
            }

            float slope = (float)(point2.Y - point1.Y) / (point2.X - point1.X);
            float interpolatedValue = (float)(point1.Y + slope * (itemLevel - point1.X));

            return (float)Math.Round(interpolatedValue, 2);
        }

        private static float GetValueFromArrayProgression(ArrayProgression progression, int itemLevel)
        {
            var points = progression.Points.OrderBy(p => p.Count).ToList();

            var point = points.LastOrDefault(p => p.Count <= itemLevel);

            if (point == null) return (float)points.First().Y;

            return (float)Math.Round(point.Y, 2);
        }
    }
}
