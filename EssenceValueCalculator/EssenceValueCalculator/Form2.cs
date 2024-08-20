using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EssenceValueCalculator
{
   
    public partial class Form2 : Form
    {
        private const string settingsFilePath = "settings.xml";
        private Settings settings;
        public Form2()
        {
            InitializeComponent();
            settings = LoadSettings(settingsFilePath);
            itemLevelDropBox.DropDownStyle = ComboBoxStyle.DropDownList;
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
                // Wähle den gespeicherten Item-Level aus
               
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

            if (Enum.TryParse(itemLevelDropBox.SelectedItem.ToString().Replace(" ", "_"), out EssenceItemLevel level))
            {
                settings.setting.SetEssenceItemLevel(level);
            }

            SaveSettings(settings, settingsFilePath);
        }
        public void SaveSettings(Settings settings, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, settings);
            }
        }

        public Settings LoadSettings(string filePath)
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
        public string essenceItemLevel { get; set; }

        [XmlElement("supValuesUsed")]
        public bool supValuesUsed { get; set; }
        public void SetEssenceItemLevel(EssenceItemLevel level)
        {
            essenceItemLevel = level.ToString();
        }
    }
}
