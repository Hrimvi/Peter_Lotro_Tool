using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EssenceValueCalculator
{
    #region progressions
    [XmlRoot("progressions")]
    public class Progressions
    {
        [XmlElement("linearInterpolationProgression")]
        public List<LinearInterpolationProgression> LinearInterpolationProgressions { get; set; }

        [XmlElement("arrayProgression")]
        public List<ArrayProgression> ArrayProgressions { get; set; }
    }

    public class LinearInterpolationProgression
    {
        [XmlAttribute("identifier")]
        public long Identifier { get; set; }

        [XmlAttribute("nbPoints")]
        public int NbPoints { get; set; }

        [XmlElement("point")]
        public List<Point1> Points { get; set; }
    }

    public class ArrayProgression
    {
        [XmlAttribute("identifier")]
        public long Identifier { get; set; }

        [XmlAttribute("nbPoints")]
        public int NbPoints { get; set; }

        [XmlElement("point")]
        public List<ArrayPoint> Points { get; set; }
    }

    public class Point1
    {
        [XmlAttribute("x")]
        public int X { get; set; } 

        [XmlAttribute("y")]
        public double Y { get; set; }
    }

    public class ArrayPoint
    {
        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlAttribute("y")]
        public double Y { get; set; }
    }
    #endregion

    #region settings
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
    #endregion

    #region Stat configs
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
    #endregion

    #region Items
    [XmlRoot("items")]
    public class Items
    {
        [XmlElement("item")]
        public List<Item> ItemList { get; set; }
    }

    public class Item
    {
        [XmlAttribute("key")]
        public long Key { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("icon")]
        public string Icon { get; set; }

        [XmlAttribute("level")]
        public int itemLevel { get; set; }

        [XmlAttribute("slot")]
        public string equipSlot { get; set; }

        [XmlAttribute("category")]
        public string Category { get; set; }

        [XmlAttribute("class")]
        public int Class { get; set; }

        [XmlAttribute("equipmentCategory")]
        public int EquipmentCategory { get; set; }

        [XmlAttribute("binding")]
        public string Binding { get; set; }

        [XmlAttribute("durability")]
        public int Durability { get; set; }

        [XmlAttribute("sturdiness")]
        public string Sturdiness { get; set; }

        [XmlAttribute("quality")]
        public string Quality { get; set; }

        [XmlAttribute("valueTableId")]
        public long ValueTableId { get; set; }

        [XmlAttribute("armourType")]
        public string ArmourType { get; set; }

        [XmlAttribute("dps")]
        public double Dps { get; set; }

        [XmlAttribute("dpsTableId")]
        public long DpsTableId { get; set; }

        [XmlAttribute("minDamage")]
        public int MinDamage { get; set; }

        [XmlAttribute("maxDamage")]
        public int MaxDamage { get; set; }

        [XmlAttribute("damageType")]
        public string DamageType { get; set; }

        [XmlAttribute("weaponType")]
        public string WeaponType { get; set; }

        [XmlAttribute("unique")]
        public bool Unique { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlAttribute("stackMax")]
        public int StackMax { get; set; }

        [XmlAttribute("minLevel")]
        public int minLevel { get; set; }

        [XmlElement("stats")]
        public ItemStats Stats { get; set; }

    }

    public class ItemStats
    {
        [XmlElement("stat")]
        public List<ItemStat> StatList { get; set; }
    }

    public class ItemStat
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("scaling")]
        public long Scaling { get; set; }

        [XmlAttribute("ranged")]
        public string rangedScaling { get; set; }
    }
    #endregion

}
