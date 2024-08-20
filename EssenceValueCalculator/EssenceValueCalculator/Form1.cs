using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
namespace EssenceValueCalculator
{
   
    public partial class Peter_Tool : Form
    {
        private Dictionary<ComboBox, TextBox> dynamicControls = new Dictionary<ComboBox, TextBox>();

        private const string essenceFilePath = "essence_values.xml";
        private const string characterStatDerivationFilePath = "classStatDerivations.xml";
        private const string settingsFilePath = "settings.xml";
        private const string statConfigFilePath = "statConfigs.xml";

        private int currentYOffset = 0;

        private System.Windows.Forms.Timer updateTimer;
        private Form2? form2;

        private Settings settings;
        private Stats playerStatsPerClass;
        private StatConfigs? statConfig;



        private float essenceValue;

        private Dictionary<StatEnum, float> statCalc = new Dictionary<StatEnum, float>();

        private Dictionary<StatEnum, float> mainStatCalc = new Dictionary<StatEnum, float>();

        public List<ComboBox> primaryBoxes = new List<ComboBox>();
        public List<ComboBox> vitalBoxes = new List<ComboBox>();



        public Peter_Tool()
        {
            InitializeComponent();

            settings = Utility.LoadSettings(settingsFilePath);
            playerStatsPerClass = Utility.LoadClass(characterStatDerivationFilePath);
            

            Utility.PopulateStats(comboBoxStats);
            Utility.PopulateClasses(classBox);

            dynamicControls.Add(comboBoxStats, inputField);

            comboBoxStats.DropDownStyle = ComboBoxStyle.DropDownList;
            classBox.DropDownStyle = ComboBoxStyle.DropDownList;



            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 50; // Intervall auf 1 Sekunde setzen
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start(); // Timer starten

            primaryBoxes.Add(primaryBox1);
            primaryBoxes.Add(primaryBox2);
            primaryBoxes.Add(primaryBox3);

            primaryBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            primaryBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            primaryBox3.DropDownStyle = ComboBoxStyle.DropDownList;

            vitalBoxes.Add(vitalBox1);
            vitalBoxes.Add(vitalBox2);
            vitalBoxes.Add(vitalBox3);

            vitalBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            vitalBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            vitalBox3.DropDownStyle = ComboBoxStyle.DropDownList;

            Utility.PopulatePrimaryEssences(primaryBoxes);

            Utility.PopulateVitalEssences(vitalBoxes);
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            settings = Utility.LoadSettings(settingsFilePath);
            statConfig = Utility.LoadStatConfigs(statConfigFilePath);
            playerStatsPerClass = Utility.LoadClass(characterStatDerivationFilePath);
            float essenceValue = GetEssenceValue();
            essenceValueText.Text = $"Essence-Value: {essenceValue:F2}";
        }
        
        private float GetEssenceValue()
        {
            statCalc.Clear();
            essenceValue = 0;
            int essenceItemlevel = Utility.GetEssenceItemLevel(settings);

            Enum.TryParse(classBox.SelectedItem?.ToString()?.Replace(" ", "_"), out Classes classe);

            //Add direct Stats from Userpanel
            foreach (var kvp in dynamicControls)
            {
                Enum.TryParse(kvp.Key.SelectedItem?.ToString()?.Replace(" ", "_"), out StatEnum stat);
                float.TryParse(kvp.Value.Text, out float statValues);
                if (statCalc.ContainsKey(stat)) statCalc[stat] += statValues;
                else statCalc.Add(stat, statValues);
            }
            statCalc = GetEssenceSocketValues(statCalc);
            //Mainstat decodation
            foreach (var kvp in statCalc)
            {
                if (Utility.isMainStat(kvp.Key))
                {
                    statCalc = Utility.AddDictionaries(statCalc, CalculateMainstat(kvp.Key, kvp.Value));
                    statCalc.Remove(kvp.Key);
                }
            }


            return ReturnEssenceValueFromDictionary(statCalc, essenceItemlevel, classe);
        }
        public float ReturnEssenceValueFromDictionary(Dictionary<StatEnum, float> stats, int essenceItemLevel, Classes playerClass)
        {
            float essenceValue = 0; // Stellen Sie sicher, dass `essenceValue` initialisiert wird.
            string usedConfigName = settings?.setting?.usedConfigName;

            // Finden Sie die aktuelle Konfiguration basierend auf dem Namen
            var selectedConfig = statConfig?.Configs
                .FirstOrDefault(c => c.Name.Equals(usedConfigName, StringComparison.OrdinalIgnoreCase));

            foreach (var kvp in stats)
            {
                // Überprüfen, ob die Stat-Elemente aktiv sind
                var statElement = selectedConfig?.Stats.FirstOrDefault(s => s.Name.Equals(kvp.Key.ToString().Replace("_", " "), StringComparison.OrdinalIgnoreCase));
                bool isActive = statElement?.Active ?? false;
                float valueMultiplier = isActive ? statElement.Value : 1.0f;
                if (isActive == false) continue;
                if (kvp.Key == StatEnum.Max_Morale || kvp.Key == StatEnum.Max_Power)
                {
                    if (kvp.Key == StatEnum.Max_Power)
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
                    else if (kvp.Key == StatEnum.Max_Morale)
                    {
                        essenceValue += (kvp.Value / 4.5f) / Utility.GetEssenceStatValue(StatEnum.Vitality, essenceItemLevel, essenceFilePath, settings) * valueMultiplier;
                    }
                }
                else if (kvp.Key == StatEnum.Armour)
                {
                    essenceValue += kvp.Value / Utility.GetEssenceStatValue(StatEnum.Physical_Mitigation, essenceItemLevel, essenceFilePath, settings) * valueMultiplier;
                    essenceValue += kvp.Value / Utility.GetEssenceStatValue(StatEnum.Tactical_Mitigation, essenceItemLevel, essenceFilePath, settings) * valueMultiplier;
                }
                else if (kvp.Key == StatEnum.Basic_EssenceSlot)
                {
                    essenceValue += kvp.Value;
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

        public Dictionary<StatEnum, float>? CalculateMainstat(StatEnum statEnum, float statValue)
        {
            if (statEnum == StatEnum.Max_Power || statEnum == StatEnum.Max_Morale) return null;
            mainStatCalc.Clear();

            var baseStats = new HashSet<StatEnum>
            {
                StatEnum.Critical_Rating,
                StatEnum.Physical_Mastery,
                StatEnum.Tactical_Mastery,
                StatEnum.Physical_Mitigation,
                StatEnum.Tactical_Mitigation,
                StatEnum.Critical_Defense,
                StatEnum.Finesse,
                StatEnum.Block,
                StatEnum.Parry,
                StatEnum.Evade,
                StatEnum.Outgoing_Healing,
                StatEnum.Incoming_Healing,
                StatEnum.Resistance,
            };

            string mainStat = statEnum.ToString().Replace("_", " ");
            string playerClass = classBox.SelectedItem?.ToString()?.Replace("_", " ");

            var classData = playerStatsPerClass.Classes
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
        public Dictionary<StatEnum, float> GetEssenceSocketValues(Dictionary<StatEnum, float> currentStats)
        {

            foreach (ComboBox comboBox in primaryBoxes)
            {
                Enum.TryParse(comboBox.SelectedItem?.ToString()?.Replace(" ", "_"), out PrimaryEssenceSlot stat);

                if (stat != PrimaryEssenceSlot.None)
                {
                    switch (stat)
                    {
                        case PrimaryEssenceSlot.Might:
                            if (statCalc.ContainsKey(StatEnum.Might)) statCalc[StatEnum.Might] += Utility.GetBaseEssenceValue(StatEnum.Might, Utility.GetEssenceItemLevel(settings), essenceFilePath);
                            else statCalc.Add(StatEnum.Might, Utility.GetBaseEssenceValue(StatEnum.Might, Utility.GetEssenceItemLevel(settings), essenceFilePath));
                            break;
                        case PrimaryEssenceSlot.Agility:
                            if (statCalc.ContainsKey(StatEnum.Agility)) statCalc[StatEnum.Agility] += Utility.GetBaseEssenceValue(StatEnum.Agility, Utility.GetEssenceItemLevel(settings), essenceFilePath);
                            else statCalc.Add(StatEnum.Agility, Utility.GetBaseEssenceValue(StatEnum.Agility, Utility.GetEssenceItemLevel(settings), essenceFilePath));
                            break;
                        case PrimaryEssenceSlot.Will:
                            if (statCalc.ContainsKey(StatEnum.Will)) statCalc[StatEnum.Will] += Utility.GetBaseEssenceValue(StatEnum.Will, Utility.GetEssenceItemLevel(settings), essenceFilePath);
                            else statCalc.Add(StatEnum.Will, Utility.GetBaseEssenceValue(StatEnum.Will, Utility.GetEssenceItemLevel(settings), essenceFilePath));
                            break;
                    }

                }
            }
            foreach (ComboBox comboBox in vitalBoxes)
            {
                Enum.TryParse(comboBox.SelectedItem?.ToString()?.Replace(" ", "_"), out VitalEssenceSlot stat);
                if (stat != VitalEssenceSlot.None)
                {
                    switch (stat)
                    {
                        case VitalEssenceSlot.Fate:
                            if (statCalc.ContainsKey(StatEnum.Fate)) statCalc[StatEnum.Fate] += Utility.GetBaseEssenceValue(StatEnum.Fate, Utility.GetEssenceItemLevel(settings), essenceFilePath);
                            else statCalc.Add(StatEnum.Fate, Utility.GetBaseEssenceValue(StatEnum.Fate, Utility.GetEssenceItemLevel(settings), essenceFilePath));
                            break;
                        case VitalEssenceSlot.Vitality:
                            if (statCalc.ContainsKey(StatEnum.Vitality)) statCalc[StatEnum.Vitality] += Utility.GetBaseEssenceValue(StatEnum.Vitality, Utility.GetEssenceItemLevel(settings), essenceFilePath);
                            else statCalc.Add(StatEnum.Vitality, Utility.GetBaseEssenceValue(StatEnum.Vitality, Utility.GetEssenceItemLevel(settings), essenceFilePath));
                            break;

                    }

                }
            }
            return currentStats;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (dynamicControls.Count >= 6)
            {
                MessageBox.Show("Maximal 7 Stats pro Item erlaubt.");
                return;
            }
            ComboBox newComboBox = new ComboBox();
            Utility.PopulateStats(newComboBox);
            newComboBox.Location = new Point(comboBoxStats.Location.X, comboBoxStats.Location.Y + currentYOffset + 30);
            newComboBox.Size = comboBoxStats.Size;
            newComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            TextBox newTextBox = new TextBox();
            newTextBox.Location = new Point(inputField.Location.X, inputField.Location.Y + currentYOffset + 30);
            newTextBox.Size = inputField.Size;

            Controls.Add(newComboBox);
            Controls.Add(newTextBox);

            dynamicControls.Add(newComboBox, newTextBox);
            currentYOffset += 30;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            foreach (var kvp in dynamicControls)
            {
                if (kvp.Key == comboBoxStats) continue; 
                
                dynamicControls.Remove(kvp.Key);

                Controls.Remove(kvp.Key);
                Controls.Remove(kvp.Value);

                kvp.Key.Dispose();
                kvp.Value.Dispose();
            }

            foreach (ComboBox comboBox in primaryBoxes)
            {
                comboBox.SelectedIndex = 0;
            }

            foreach (ComboBox comboBox in vitalBoxes)
            {
                comboBox.SelectedIndex = 0;
            }

            currentYOffset = 0;

            comboBoxStats.SelectedIndex = 0;
            inputField.Clear();

        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (dynamicControls.Keys.Count <= 1) return;
            var lastPair = dynamicControls.Last();
            Controls.Remove(lastPair.Key);
            Controls.Remove(lastPair.Value);

            lastPair.Key.Dispose();
            lastPair.Value.Dispose();

            dynamicControls.Remove(lastPair.Key);
            currentYOffset -= 30;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (form2 == null)
            {
                form2 = new Form2();
            }

            form2.ShowDialog();
        }

        
    }
   
    [XmlRoot("EssenceValues")]
    public class EssenceValues
    {
        [XmlElement("Essence")]
        public List<Essence> Essences { get; set; }

        public EssenceValues()
        {
            Essences = new List<Essence>();
        }
    }

    public class Essence
    {
        public Essences essence { get; set; }
        [XmlElement("Stat")]
        public StatEnum Stat { get; set; }

        [XmlElement("ItemLevel")]
        public int ItemLevel { get; set; }

        [XmlElement("Value")]
        public float Value { get; set; }
    }


    [XmlRoot("Stats")]
    public class Stats
    {
        [XmlElement("Class")]
        public List<Class> Classes { get; set; }

        public Stats()
        {
            Classes = new List<Class>();
        }
    }

    public class Class
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Mainstat")]
        public List<Mainstat> Mainstats { get; set; }

        public Class()
        {
            Name = string.Empty;
            Mainstats = new List<Mainstat>();
        }
    }

    public class Mainstat
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }
        [XmlElement("Stat")]
        public List<Stat>? Stats { get; set; }
    }

    public class Stat
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("value")]
        public float Value { get; set; }
    }
   

}
