using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace EssenceValueCalculator
{

    public partial class Form2 : Form
    {
        private const string statConfigFilePath = "statConfigs.xml";
        private const string settingsFilePath = "settings.xml";
        private Settings? settings;
        private StatConfigs? statConfig;

        public Form2()
        {
            InitializeComponent();
            settings = Utility.LoadSettings(settingsFilePath);

            itemLevelDropBox.DropDownStyle = ComboBoxStyle.DropDownList;
            configEditorSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            activeStatConfigSelection.DropDownStyle = ComboBoxStyle.DropDownList;


            // Setze den Status der Checkbox
            if (settings != null && settings.setting != null)
            {
                subEssencesCheckbox.Checked = settings.setting.supValuesUsed;

                // Fülle die ComboBox mit den Item-Level-Werten
                foreach (EssenceItemLevel itemLevel in Enum.GetValues(typeof(EssenceItemLevel)))
                {
                    itemLevelDropBox.Items.Add(itemLevel.ToString().Replace("_", " "));
                }
                itemLevelDropBox.SelectedIndex = 0;
                if (Enum.TryParse(settings.setting.essenceItemLevel, out EssenceItemLevel level))
                {
                    itemLevelDropBox.SelectedIndex = (int)level;
                }

            }
            else
            {
                // Füge Item-Level-Werte hinzu und setze den Standardwert
                foreach (EssenceItemLevel itemLevel in Enum.GetValues(typeof(EssenceItemLevel)))
                {
                    itemLevelDropBox.Items.Add(itemLevel.ToString().Replace("_", " "));
                }
                itemLevelDropBox.SelectedIndex = 0; // Standardwert
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            SaveSettings();
        }
        private void backToMainWindow_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Hide();
        }

        private void SaveSettings()
        {
            if (settings == null)
                settings = new Settings();

            if (settings.setting == null)
                settings.setting = new Setting();

            settings.setting.supValuesUsed = subEssencesCheckbox.Checked;

            if (Enum.TryParse(itemLevelDropBox.SelectedItem?.ToString()?.Replace(" ", "_"), out EssenceItemLevel level))
            {
                settings.setting.SetEssenceItemLevel(level);
            }

            var selectedConfigName = activeStatConfigSelection.SelectedItem as string;
            settings.setting.usedConfigName = selectedConfigName ?? string.Empty;

            Utility.SaveSettings(settings, settingsFilePath);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            statConfig = Utility.LoadStatConfigs(statConfigFilePath);
            UpdateConfigComboBox(activeStatConfigSelection);
            UpdateConfigComboBox(configEditorSelection);

            if (settings != null && !string.IsNullOrEmpty(settings.setting.usedConfigName))
            {
                var selectedConfigIndex = activeStatConfigSelection.Items.IndexOf(settings.setting.usedConfigName);
                if (selectedConfigIndex != -1)
                {
                    activeStatConfigSelection.SelectedIndex = selectedConfigIndex;
                    configEditorSelection.SelectedIndex = selectedConfigIndex;
                }
            }
            var selectedConfig = statConfig?.Configs.FirstOrDefault(c => c.Name.Equals(settings?.setting.usedConfigName, StringComparison.OrdinalIgnoreCase));

            if (selectedConfig != null)
            {
                CreateConfigEditorControls(selectedConfig.Stats);
            }
        }

        private void createConfig_Click(object sender, EventArgs e)
        {
            var defaultStats = GetDefaultStats();
            string newName = ShowInputDialog("Enter new config name:");
            if (!string.IsNullOrEmpty(newName))
            {
                var newConfig = new StatConfig
                {
                    Name = newName,
                    Stats = defaultStats
                };

                statConfig?.Configs.Add(newConfig);
                Utility.SaveStatConfigs(statConfigFilePath, statConfig);

                UpdateConfigComboBox(activeStatConfigSelection);
                UpdateConfigComboBox(configEditorSelection);

                var selectedIndex = configEditorSelection.Items.IndexOf(newName);
                if (selectedIndex != -1)
                {
                    configEditorSelection.SelectedIndex = selectedIndex;
                }
                if (settings != null && !string.IsNullOrEmpty(settings.setting.usedConfigName))
                {
                    var selectedConfigIndex = activeStatConfigSelection.Items.IndexOf(settings.setting.usedConfigName);
                    if (selectedConfigIndex != -1)
                    {
                        activeStatConfigSelection.SelectedIndex = selectedConfigIndex;
                    }
                }
            }
        }
        private void UpdateConfigComboBox(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(statConfig?.Configs?.Select(c => c.Name).ToArray());

        }
        private List<StatElement> GetDefaultStats()
        {
            var excludedStats = new HashSet<StatEnum>
    {
        StatEnum.Might,
        StatEnum.Agility,
        StatEnum.Vitality,
        StatEnum.Will,
        StatEnum.Fate,
        StatEnum.Armour,
        StatEnum.Basic_EssenceSlot
    };

            return Enum.GetValues(typeof(StatEnum))
                        .Cast<StatEnum>()
                        .Where(statEnum => !excludedStats.Contains(statEnum))
                        .Select(statEnum => new StatElement
                        {
                            Name = statEnum.ToString().Replace("_", " "),
                            Value = 1.00f,
                            Active = true
                        })
                        .ToList();
        }
        public static string ShowInputDialog(string prompt)
        {
            Form inputBox = new Form();
            Label lblPrompt = new Label() { Left = 20, Top = 20, Text = prompt };
            TextBox txtInput = new TextBox() { Left = 20, Top = 50, Width = 200 };
            Button btnOk = new Button() { Text = "OK", Left = 150, Width = 70, Top = 80, DialogResult = DialogResult.OK };
            Button btnCancel = new Button() { Text = "Cancel", Left = 230, Width = 70, Top = 80, DialogResult = DialogResult.Cancel };

            btnOk.Click += (sender, e) => { inputBox.Close(); };
            btnCancel.Click += (sender, e) => { inputBox.DialogResult = DialogResult.Cancel; inputBox.Close(); };

            inputBox.Controls.Add(lblPrompt);
            inputBox.Controls.Add(txtInput);
            inputBox.Controls.Add(btnOk);
            inputBox.Controls.Add(btnCancel);
            inputBox.AcceptButton = btnOk;
            inputBox.CancelButton = btnCancel;

            return inputBox.ShowDialog() == DialogResult.OK ? txtInput.Text : null;
        }

        private void deleteConfig_Click(object sender, EventArgs e)
        {
            var selectedConfigName = configEditorSelection.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedConfigName))
            {
                MessageBox.Show("No configuration selected for deletion.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedConfig = statConfig?.Configs
                .FirstOrDefault(c => c.Name.Equals(selectedConfigName, StringComparison.OrdinalIgnoreCase));
            if (selectedConfig == null)
            {
                MessageBox.Show("The selected configuration could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var result = MessageBox.Show(
                $"Are you sure you want to delete the configuration '{selectedConfig.Name}'?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                statConfig?.Configs.Remove(selectedConfig);
                Utility.SaveStatConfigs(statConfigFilePath, statConfig);

                // Aktualisieren der ComboBoxen
                UpdateConfigComboBox(activeStatConfigSelection);
                UpdateConfigComboBox(configEditorSelection);

                // Wenn die gelöschte Konfiguration ausgewählt war, setze die Auswahl auf das erste Element
                if (configEditorSelection.Items.Count > 0)
                {
                    if (configEditorSelection.Items.Contains(selectedConfigName))
                    {
                        // Die Konfiguration, die gerade gelöscht wurde, ist immer noch in der ComboBox
                        // Das bedeutet, dass sie nur aus der Konfigurationliste entfernt wurde,
                        // nicht aus der ComboBox
                        configEditorSelection.SelectedIndex = 0; // Setzt die Auswahl auf das erste Element
                    }
                    else
                    {
                        // Falls die gelöschte Konfiguration nicht mehr in der ComboBox ist
                        // Setze die Auswahl auf das erste verfügbare Element
                        configEditorSelection.SelectedIndex = 0;
                    }
                }

                // Leeren des Konfigurations-Editors
                configPanel.Controls.Clear();

                // Wiederherstellen der Auswahl in der aktiven Konfigurations-ComboBox
                if (settings != null && !string.IsNullOrEmpty(settings.setting.usedConfigName))
                {
                    var selectedConfigIndex = activeStatConfigSelection.Items.IndexOf(settings.setting.usedConfigName);
                    if (selectedConfigIndex != -1)
                    {
                        activeStatConfigSelection.SelectedIndex = selectedConfigIndex;
                    }
                }
                var newSelectedConfig = statConfig?.Configs.FirstOrDefault(c => c.Name.Equals(settings?.setting.usedConfigName, StringComparison.OrdinalIgnoreCase));

                if (newSelectedConfig != null)
                {
                    CreateConfigEditorControls(newSelectedConfig.Stats);
                }
            }
        }
        private void StatValueCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox?.Tag is StatElement statElement)
            {
                statElement.Active = checkBox.Checked;
                SaveCurrentConfig();
            }
        }

        private void CreateConfigEditorControls(List<StatElement> statElements)
        {
            configPanel.Controls.Clear();

            int yOffset = 10;

            foreach (var stat in statElements)
            {
                // Checkbox
                CheckBox checkBox = new CheckBox
                {
                    Text = "",
                    Checked = stat.Active,
                    Width = 15,
                    Tag = stat
                };
                checkBox.Location = new Point(5, yOffset);
                checkBox.CheckedChanged += StatValueCheckbox_CheckedChanged;

                // Label
                Label label = new Label
                {
                    Text = stat.Name,
                    Location = new Point(25, yOffset),
                    Width = 150,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                // TextBox
                TextBox textBox = new TextBox
                {
                    Text = stat.Value.ToString("0.00"),
                    Location = new Point(260, yOffset),
                    Width = 50
                };
                textBox.TextChanged += StatValueTextBox_TextChanged;
                textBox.Tag = stat;

                configPanel.Controls.Add(checkBox);
                configPanel.Controls.Add(label);
                configPanel.Controls.Add(textBox);

                yOffset += 30;
            }
        }
        private void StatValueTextBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Tag is StatElement statElement)
            {
                if (float.TryParse(textBox.Text, out float value))
                {
                    statElement.Value = value;
                    SaveCurrentConfig();
                }
                else
                {
                    MessageBox.Show("Please enter a valid number.");
                    textBox.Text = statElement.Value.ToString("0.00"); // Wiederherstellen des alten Wertes
                }
            }
        }
        private void SaveCurrentConfig()
        {
            var selectedConfigName = configEditorSelection.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedConfigName)) return;

            var selectedConfig = statConfig?.Configs
                .FirstOrDefault(c => c.Name.Equals(selectedConfigName, StringComparison.OrdinalIgnoreCase));

            if (selectedConfig != null)
            {
                selectedConfig.Stats = configPanel.Controls
                    .OfType<Control>()
                    .Where(c => c is CheckBox || c is TextBox)
                    .GroupBy(c => c.Tag)
                    .Select(g =>
                    {
                        var statElement = g.Key as StatElement;
                        if (statElement == null) return null;

                        var checkBox = g.OfType<CheckBox>().FirstOrDefault();
                        if (checkBox != null)
                        {
                            statElement.Active = checkBox.Checked;
                        }

                        var textBox = g.OfType<TextBox>().FirstOrDefault();
                        if (textBox != null && float.TryParse(textBox.Text, out float value))
                        {
                            statElement.Value = value;
                        }

                        return statElement;
                    })
                    .Where(s => s != null)
                    .ToList();

                Utility.SaveStatConfigs(statConfigFilePath, statConfig);
            }
        }
        private void LoadSelectedConfig()
        {
            var selectedConfigName = configEditorSelection.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedConfigName)) return;

            var selectedConfig = statConfig?.Configs
                .FirstOrDefault(c => c.Name.Equals(selectedConfigName, StringComparison.OrdinalIgnoreCase));

            if (selectedConfig != null)
            {
                CreateConfigEditorControls(selectedConfig.Stats);
            }
        }

        private void configEditorSelection_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            LoadSelectedConfig();
        }
    }


    [XmlRoot("Settings")]
    public class Settings
    {
        [XmlElement("Setting")]
        public Setting setting { get; set; }

        public Settings()
        {
            setting = new Setting();
        }
    }
    public class Setting
    {
        [XmlElement("essenceItemLevel")]
        public string? essenceItemLevel { get; set; }

        [XmlElement("supValuesUsed")]
        public bool supValuesUsed { get; set; }

        [XmlElement("usedConfigName")]
        public string usedConfigName { get; set; }

        public void SetEssenceItemLevel(EssenceItemLevel level)
        {
            essenceItemLevel = level.ToString();
        }
    }
    [XmlRoot("StatConfigs")]
    public class StatConfigs
    {
        [XmlElement("StatConfig")]
        public List<StatConfig> Configs { get; set; } = new List<StatConfig>();
    }

    public class StatConfig
    {
        [XmlAttribute("Name")]
        public string? Name { get; set; }

        [XmlElement("Stat")]
        public List<StatElement> Stats { get; set; } = new List<StatElement>();
    }

    public class StatElement
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("value")]
        public float Value { get; set; }

        [XmlAttribute("active")]
        public bool Active { get; set; }
    }
}
