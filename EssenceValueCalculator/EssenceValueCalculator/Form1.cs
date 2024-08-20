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



        private Dictionary<StatEnum, float> statCalc;

        private Dictionary<StatEnum, float> mainStatCalc = new Dictionary<StatEnum, float>();

        public List<ComboBox> primaryBoxes = new List<ComboBox>();
        public List<ComboBox> vitalBoxes = new List<ComboBox>()



        public Peter_Tool()
        {
            InitializeComponent();
            settings = Utility.LoadSettings(settingsFilePath);
            Utility.PopulateStats(comboBoxStats);
            Utility.PopulateClasses(classBox);
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
                if (!Enum.TryParse(comboBox.SelectedItem.ToString().Replace(" ", "_"), out PrimaryEssenceSlot stat))
                {
                    // Handle error
                }

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
                if (!Enum.TryParse(comboBox.SelectedItem.ToString().Replace(" ", "_"), out VitalEssenceSlot stat))
                {
                    // Handle error
                }

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
            essenceValue = 0;
            ComboBox statComboBox1 = comboBoxStats;
            TextBox valueBox1 = inputField;

            if (!Enum.TryParse(statComboBox1.SelectedItem.ToString().Replace(" ", "_"), out StatEnum stat))
            {
                // Handle error
            }
            if (!int.TryParse(valueBox1.Text, out int stats))
            {
                // Handle error
            }

            int essenceItemlevel = GetItemLevel();
            if (!Enum.TryParse(classBox.SelectedItem.ToString().Replace(" ", "_"), out Classes classe))
            {
                // Handle error
            }

            if (stat == StatEnum.Max_Morale || stat == StatEnum.Max_Power)
            {
                if (stat == StatEnum.Max_Power)
                {
                    if (classe == Classes.Beorning) essenceValue += 0;
                    else essenceValue += stats / GetEssenceStatValue(StatEnum.Fate, essenceItemlevel, essenceFilePath);
                }
                if (stat == StatEnum.Max_Morale)
                {
                    essenceValue += (stats / 4.5f) / GetEssenceStatValue(StatEnum.Vitality, essenceItemlevel, essenceFilePath);
                }
            }

            if (stat == StatEnum.Armour)
            {
                essenceValue += stats / GetEssenceStatValue(StatEnum.Physical_Mitigation, essenceItemlevel, essenceFilePath);
                essenceValue += stats / GetEssenceStatValue(StatEnum.Tactical_Mitigation, essenceItemlevel, essenceFilePath);
            }
            else if (stat != StatEnum.Basic_EssenceSlot && stat != StatEnum.Armour)
            {
                if (!Utility.isMainStat(stat)) essenceValue += stats / GetEssenceStatValue(stat, essenceItemlevel, essenceFilePath);
                else essenceValue += CalculateMainstat(stat, essenceItemlevel, stats);
            }
            else
            {
                if (stat == StatEnum.Basic_EssenceSlot)
                {
                    essenceValue += stats;
                }
            }

            foreach (var kvp in dynamicControls)
            {
                ComboBox statComboBox = kvp.Key;
                TextBox valueBox = kvp.Value;

                if (!Enum.TryParse(statComboBox.SelectedItem.ToString().Replace(" ", "_"), out StatEnum stat1))
                {
                    // Handle error
                }
                if (!int.TryParse(valueBox.Text, out int stats1))
                {
                    // Handle error
                }

                if (stat1 == StatEnum.Max_Morale || stat1 == StatEnum.Max_Power)
                {
                    if (stat == StatEnum.Max_Power)
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
                else if (stat1 != StatEnum.Basic_EssenceSlot && stat != StatEnum.Armour)
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
            if (!Enum.TryParse(settings.setting.essenceItemLevel.Replace(" ", "_"), out EssenceItemLevel itemLevel))
            {
                // Handle error
            }
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
#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
            string playerClass = classBox.SelectedItem.ToString().Replace("_", " ");
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.

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
            dynamicControls.Clear();

            currentYOffset = 0;

            comboBoxStats.SelectedIndex = 0;
            inputField.Clear();

        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (dynamicControls.Keys.Count <= 0) return;
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

        private void comboBoxStats_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void inputField_TextChanged(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void primaryBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                Essence essence = essenceValues.Essences
                    .Find(e => e.Stat == stat && e.ItemLevel == itemlevel);

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
   

    public static class Utility
    {
        public static EssenceValues LoadEssenceValues(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EssenceValues));
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                return (EssenceValues)serializer.Deserialize(fs);
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
                return (Settings)serializer.Deserialize(fs);
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
                return (Stats)serializer.Deserialize(fs);
            }
        }

        public static bool isMainStat(StatEnum stat)
        {
            if (stat == StatEnum.Might || stat == StatEnum.Agility || stat == StatEnum.Will || stat == StatEnum.Max_Power || stat == StatEnum.Max_Morale) return true;
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
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public List<Class> Classes { get; set; }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
    }

    public class Class
    {
        [XmlAttribute("name")]
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.

        [XmlElement("Mainstat")]
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public List<Mainstat> Mainstats { get; set; }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
    }

    public class Mainstat
    {
        [XmlAttribute("name")]
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.

        [XmlElement("Stat")]
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public List<Stat> Stats { get; set; }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
    }

    public class Stat
    {
        [XmlAttribute("name")]
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.

        [XmlAttribute("value")]
        public float Value { get; set; }
    }
}
