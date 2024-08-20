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
}
