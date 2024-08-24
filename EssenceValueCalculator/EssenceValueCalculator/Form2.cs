using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace EssenceValueCalculator
{

    public partial class SettingsForm : Form
    {
        private bool loading = true;
        public SettingsForm()
        {
            InitializeComponent();
            ApplicationData.Instance.Settings = Utility.LoadSettings();
            itemLevelDropBox.DropDownStyle = ComboBoxStyle.DropDownList;
            configEditorSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            activeStatConfigSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            configEditorSelection.SelectedIndexChanged += configEditorSelection_SelectedIndexChanged_12;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            if (loading) return;
            if (ApplicationData.Instance.Settings == null) return;

            ApplicationData.Instance.Settings.setting.supValuesUsed = subEssencesCheckbox.Checked;

            if (Enum.TryParse(itemLevelDropBox.SelectedItem?.ToString()?.Replace(" ", "_"), out EssenceItemLevel level))
            {
                ApplicationData.Instance.Settings.setting.SetEssenceItemLevel(level);
            }

            var selectedConfigName = activeStatConfigSelection.SelectedItem as string;
            ApplicationData.Instance.Settings.setting.usedConfigName = selectedConfigName ?? string.Empty;

            Utility.SaveSettings(ApplicationData.Instance.Settings);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            

            UpdateConfigComboBox(activeStatConfigSelection);
            UpdateConfigComboBox(configEditorSelection);

            var settings = ApplicationData.Instance.Settings;
            if (settings != null && !string.IsNullOrEmpty(settings.setting.usedConfigName))
            {
                var selectedConfigIndex = activeStatConfigSelection.Items.IndexOf(settings.setting.usedConfigName);
                if (selectedConfigIndex != -1)
                {
                    activeStatConfigSelection.SelectedIndex = selectedConfigIndex;
                    configEditorSelection.SelectedIndex = selectedConfigIndex;
                }
            }

            var selectedConfig = ApplicationData.Instance.StatConfig.Configs
                .FirstOrDefault(c => c.Name.Equals(settings.setting.usedConfigName, StringComparison.OrdinalIgnoreCase));

            if (selectedConfig != null)
            {
                CreateConfigEditorControls(selectedConfig.Stats);
            }

           
            subEssencesCheckbox.Checked = settings.setting.supValuesUsed;

            if (settings != null && settings.setting != null)
            { 


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
                 foreach (EssenceItemLevel itemLevel in Enum.GetValues(typeof(EssenceItemLevel)))
                 {
                     itemLevelDropBox.Items.Add(itemLevel.ToString().Replace("_", " "));
                 }
               itemLevelDropBox.SelectedIndex = 0;
            }
            Utility.Log($"Settings loaded with {settings.setting.supValuesUsed}");
            Utility.Log($"Settings loaded with {settings.setting.essenceItemLevel}");
            Utility.Log($"Settings loaded with {settings.setting.usedConfigName}");
            loading = false;
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

                ApplicationData.Instance.StatConfig.Configs.Add(newConfig);
                Utility.SaveStatConfigs(ApplicationData.Instance.StatConfig);

                UpdateConfigComboBox(activeStatConfigSelection);
                UpdateConfigComboBox(configEditorSelection);

                var selectedIndex = configEditorSelection.Items.IndexOf(newName);
                if (selectedIndex != -1)
                {
                    configEditorSelection.SelectedIndex = selectedIndex;
                }
                if (ApplicationData.Instance.Settings != null && !string.IsNullOrEmpty(ApplicationData.Instance.Settings.setting.usedConfigName))
                {
                    var selectedConfigIndex = activeStatConfigSelection.Items.IndexOf(ApplicationData.Instance.Settings.setting.usedConfigName);
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
            comboBox.Items.AddRange(ApplicationData.Instance.StatConfig.Configs?.Select(c => c.Name).ToArray());
            

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
            Form inputBox = new Form
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                ClientSize = new Size(300, 150),
                Text = "Input"
            };

            Label lblPrompt = new Label
            {
                Left = 20,
                Top = 20,
                AutoSize = true,
                Text = prompt
            };

            TextBox txtInput = new TextBox
            {
                Left = 20,
                Top = lblPrompt.Bottom + 10,
                Width = 240,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };

            Button btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Left = 130,
                Width = 70,
                Top = txtInput.Bottom + 10,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Left = 210,
                Width = 70,
                Top = txtInput.Bottom + 10,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

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

            var selectedConfig = ApplicationData.Instance.StatConfig.Configs
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
                ApplicationData.Instance.StatConfig.Configs.Remove(selectedConfig);
                Utility.SaveStatConfigs(ApplicationData.Instance.StatConfig);

                UpdateConfigComboBox(activeStatConfigSelection);
                UpdateConfigComboBox(configEditorSelection);

                if (configEditorSelection.Items.Count > 0)
                {
                    if (configEditorSelection.Items.Contains(selectedConfigName))
                    {
                        configEditorSelection.SelectedIndex = 0;
                    }
                    else
                    {
                        configEditorSelection.SelectedIndex = 0;
                    }
                }

                configPanel.Controls.Clear();

                if (ApplicationData.Instance.Settings != null && !string.IsNullOrEmpty(ApplicationData.Instance.Settings.setting.usedConfigName))
                {
                    var selectedConfigIndex = activeStatConfigSelection.Items.IndexOf(ApplicationData.Instance.Settings.setting.usedConfigName);
                    if (selectedConfigIndex != -1)
                    {
                        activeStatConfigSelection.SelectedIndex = selectedConfigIndex;
                    }
                }
                var newSelectedConfig = ApplicationData.Instance.StatConfig.Configs.FirstOrDefault(c => c.Name.Equals(ApplicationData.Instance.Settings?.setting.usedConfigName, StringComparison.OrdinalIgnoreCase));

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
                checkBox.Location = new System.Drawing.Point(5, yOffset);
                checkBox.CheckedChanged += StatValueCheckbox_CheckedChanged;

                // Label
                Label label = new Label
                {
                    Text = stat.Name,
                    Location = new System.Drawing.Point(25, yOffset),
                    Width = 150,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                // TextBox
                TextBox textBox = new TextBox
                {
                    Text = stat.Value.ToString("0.00"),
                    Location = new System.Drawing.Point(260, yOffset),
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

            var selectedConfig = ApplicationData.Instance.StatConfig.Configs
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

                Utility.SaveStatConfigs(ApplicationData.Instance.StatConfig);
            }
        }
        private void LoadSelectedConfig()
        {
            var selectedConfigName = configEditorSelection.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedConfigName)) return;

            var selectedConfig = ApplicationData.Instance.StatConfig.Configs
                .FirstOrDefault(c => c.Name.Equals(selectedConfigName, StringComparison.OrdinalIgnoreCase));

            if (selectedConfig != null)
            {
                CreateConfigEditorControls(selectedConfig.Stats);
            }
        }

        private void configEditorSelection_SelectedIndexChanged_12(object sender, EventArgs e)
        {
            LoadSelectedConfig();
        }

        private void activeStatConfigSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void configPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void itemLevelDropBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }



}
