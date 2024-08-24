using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Critical_Defence,
        Finesse,
        Block,
        Parry,
        Evade,
        Outgoing_Healing,
        Incoming_Healing,
        Resistance,
        Morale,
        Power,
        Armour,
        Basic_EssenceSlot,
        TBD
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
}
