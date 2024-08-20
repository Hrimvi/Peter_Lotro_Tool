using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
namespace EssenceValueCalculator
{
    public enum EssenceItemLevel
    {
        Level_508,
        Level_514,
        Level_520,
        Level_526,
        Level_532,
        Level_538
    }
    public enum StatEnum
    {

        Might,
        Agility,
        Vitality,
        Will,
        Fate,
        Critical_Rating,
        Physical_Mastery,
        Tactical_Mastery,
        Physical_Mitigation,
        Tactical_Mitigation,
        Critical_Defense,
        Finesse,
        Block,
        Parry,
        Evade,
        Outgoing_Healing,
        Incoming_Healing,
        Resistance,
        Max_Morale,
        Max_Power,
        Armour,
        Basic_EssenceSlot
    }
    public enum Essences
    {
        Might,
        Agility,
        Vitality,
        Will,
        Fate,
        Critical_Rating,
        Physical_Mastery,
        Tactical_Mastery,
        Physical_Mitigation,
        Tactical_Mitigation,
        Critical_Defense,
        Finesse,
        Block,
        Parry,
        Evade,
        Outgoing_Healing,
        Incoming_Healing,
        Resistance,
        Sub_Vit,
        Sub_Fate
    }
    public enum PrimaryEssenceSlot
    {
        None,
        Might,
        Agility,
        Will
    }

    public enum VitalEssenceSlot
    {
        None,
        Fate,
        Vitality
    }
    public enum Classes
    {
        Beorning,
        Brawler,
        Burglar,
        Captain,
        Champion,
        Guardian,
        Hunter,
        Lore_Master,
        Mariner,
        Minstrel,
        Rune_Keeper,
        Warden
    }
    public partial class Peter_Tool : Form
    {
        private Dictionary<ComboBox, TextBox> dynamicControls = new Dictionary<ComboBox, TextBox>();

        private const string essenceFilePath = "essence_values.xml";
        private const string characterStatDerivationFilePath = "classStatDerivations.xml";
        private const string settingsFilePath = "settings.xml";

        private int currentYOffset = 0;

        private System.Windows.Forms.Timer updateTimer;
        private Form2 form2;

        private Settings settings;
        private Stats playerStatsPerClass;
        private float essenceValue;



        private Dictionary<StatEnum, float> statCalc = new Dictionary<StatEnum, float>();

        private Dictionary<StatEnum, float> mainStatCalc = new Dictionary<StatEnum, float>();

        public List<ComboBox> primaryBoxes = new List<ComboBox>();
        public List<ComboBox> vitalBoxes = new List<ComboBox>();



        public Peter_Tool()
        {
            InitializeComponent();
            settings = Utility.LoadSettings(settingsFilePath);
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

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            settings = Utility.LoadSettings(settingsFilePath);
            playerStatsPerClass = Utility.LoadClass(characterStatDerivationFilePath);
            float essenceValue = GetEssenceValue();
            essenceValueText.Text = $"Essence-Value: {essenceValue:F2}";
        }
        public float GetEssenceSocketValues()
        {
            






















            float essenceValue = 0;
            foreach (ComboBox comboBox in primaryBoxes)
            {
                Enum.TryParse(comboBox.SelectedItem.ToString().Replace(" ", "_"), out PrimaryEssenceSlot stat);

                if (stat != PrimaryEssenceSlot.None)
                {
                    switch (stat)
                    {
                        case PrimaryEssenceSlot.Might:
                            essenceValue += CalculateMainstat(StatEnum.Might, GetItemLevel(), GetEssenceStatValue(StatEnum.Might, GetItemLevel(), essenceFilePath));
                            break;
                        case PrimaryEssenceSlot.Agility:
                            essenceValue += CalculateMainstat(StatEnum.Agility, GetItemLevel(), GetEssenceStatValue(StatEnum.Agility, GetItemLevel(), essenceFilePath));
                            break;
                        case PrimaryEssenceSlot.Will:
                            essenceValue += CalculateMainstat(StatEnum.Will, GetItemLevel(), GetEssenceStatValue(StatEnum.Will, GetItemLevel(), essenceFilePath));
                            break;
                    }
                    
                }
            }
            foreach (ComboBox comboBox in vitalBoxes)
            {
                Enum.TryParse(comboBox.SelectedItem.ToString().Replace(" ", "_"), out VitalEssenceSlot stat);
                if (stat != VitalEssenceSlot.None)
                {
                    switch (stat)
                    {
                        case VitalEssenceSlot.Fate:
                            essenceValue += GetBaseEssenceValue(StatEnum.Fate, GetItemLevel(), essenceFilePath) / GetEssenceStatValue(StatEnum.Fate, GetItemLevel(), essenceFilePath);
                            break;
                        case VitalEssenceSlot.Vitality:
                            essenceValue += GetBaseEssenceValue(StatEnum.Vitality, GetItemLevel(), essenceFilePath) / GetEssenceStatValue(StatEnum.Vitality, GetItemLevel(), essenceFilePath); ; 
                            break;
                           
                    }
                    
                }
            }
            return essenceValue;
        }
        private float GetEssenceValue()
        {
            statCalc.Clear();


            essenceValue = 0;

            int essenceItemlevel = GetItemLevel();


            Enum.TryParse(classBox.SelectedItem.ToString().Replace(" ", "_"), out Classes classe);


            foreach (var kvp in dynamicControls)
            {
                ComboBox statComboBox = kvp.Key;
                TextBox valueBox = kvp.Value;

                Enum.TryParse(statComboBox.SelectedItem.ToString().Replace(" ", "_"), out StatEnum stat1);

                int.TryParse(valueBox.Text, out int stats1);

                if (stat1 == StatEnum.Max_Morale || stat1 == StatEnum.Max_Power)
                {
                    if (stat1 == StatEnum.Max_Power)
                    {
                        if (classe == Classes.Beorning) essenceValue += 0;
                        else essenceValue += stats1 / GetEssenceStatValue(StatEnum.Fate, essenceItemlevel, essenceFilePath);
                    }
                    if (stat1 == StatEnum.Max_Morale)
                    {
                        essenceValue += (stats1 / 4.5f) / GetEssenceStatValue(StatEnum.Vitality, essenceItemlevel, essenceFilePath);
                    }
                }

                if (stat1 == StatEnum.Armour)
                {
                    essenceValue += stats1 / GetEssenceStatValue(StatEnum.Physical_Mitigation, essenceItemlevel, essenceFilePath);
                    essenceValue += stats1 / GetEssenceStatValue(StatEnum.Tactical_Mitigation, essenceItemlevel, essenceFilePath);
                }
                else if (stat1 != StatEnum.Basic_EssenceSlot && stat1 != StatEnum.Armour)
                {
                    if (!Utility.isMainStat(stat1)) essenceValue += stats1 / GetEssenceStatValue(stat1, essenceItemlevel, essenceFilePath);
                    else essenceValue += CalculateMainstat(stat1, essenceItemlevel, stats1);
                }
                else
                {
                    if (stat1 == StatEnum.Basic_EssenceSlot)
                    {
                        essenceValue += stats1;
                    }
                }
            }
            essenceValue += GetEssenceSocketValues();
            return essenceValue;
        }
        public int GetItemLevel()
        {
            Enum.TryParse(settings.setting.essenceItemLevel.Replace(" ", "_"), out EssenceItemLevel itemLevel);
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
        public float CalculateMainstat(StatEnum statEnum, int essenceItemLevel, float statValue)
        {
            if (statEnum == StatEnum.Max_Power || statEnum == StatEnum.Max_Morale) return 0;
            float essenceValue = 0;
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
            string playerClass = classBox.SelectedItem.ToString().Replace("_", " ");

            var classData = playerStatsPerClass.Classes
                .FirstOrDefault(c => c.Name.Equals(playerClass, StringComparison.OrdinalIgnoreCase));

            if (classData == null)
            {
                MessageBox.Show($"Keine Daten für die Klasse '{playerClass}' gefunden.");
                return 0;
            }

            var mainstat = classData.Mainstats
                .FirstOrDefault(m => m.Name.Equals(statEnum.ToString().Replace("_", " "), StringComparison.OrdinalIgnoreCase));

            if (mainstat == null)
            {
                MessageBox.Show($"Kein Mainstat für '{statEnum}' in der Klasse '{playerClass}' gefunden.");
                return 0;
            }

            foreach (var stat in mainstat.Stats)
            {
                if (Enum.TryParse(stat.Name.Replace(" ", "_"), out StatEnum statEnumValue))
                {
                    mainStatCalc[statEnumValue] = stat.Value * statValue;
                }
            }

            foreach (var kvp in mainStatCalc)
            {
                essenceValue += kvp.Value / GetEssenceStatValue(kvp.Key, essenceItemLevel, essenceFilePath);
            }
            return essenceValue;
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

        public float GetEssenceStatValue(StatEnum stat, int itemlevel, string essenceFilePath)
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
        public float GetBaseEssenceValue(StatEnum stat, int itemlevel, string essenceFilePath)
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
    }

    public class Class
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Mainstat")]
        public List<Mainstat> Mainstats { get; set; }
    }

    public class Mainstat
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("Stat")]
        public List<Stat> Stats { get; set; }
    }

    public class Stat
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public float Value { get; set; }
    }
}
