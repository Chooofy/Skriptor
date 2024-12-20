using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

public partial class NBT : Node
{
    // Called when the node enters the scene tree for the first time.

    //Add Section for place on, can break, rarity, jukebox, food, tool, potions

    public static string GetGuid()
    {
        return Guid.NewGuid().ToString();
    }
    public static string ReturnFormattedNBT(string nbtName, string value, Type type)
    {
        nbtName = nbtName.ToLower();
        nbtName = nbtName.Replace(" ", "_");
        value = value.ToLower();
        if (type == typeof(string))
            return $"{nbtName}=\"{value}\"";
        else if (type == typeof(bool))
        {
            if (Convert.ToBoolean(value) == true)
                return $"{nbtName}=" + "{show_in_tooltip:false}";
            return "";
        }
        else
            return $"{nbtName}:{value}";
    }
    public static string GetEnchantNBT(Godot.Collections.Dictionary enchants)
    {
        if (enchants == null)
            return "";
        string s = "enchantments={levels:{";
        var kvpNum = 0;
        foreach (KeyValuePair<Godot.Variant, Godot.Variant> pair in enchants)
        {
            if (kvpNum != 0)
                s += ",";
            kvpNum++;
            s += $"\"minecraft:{pair.Key}\":";
            s += $"{pair.Value}";
        }
        s += "},show_in_tooltip:false}";
        return s;
    }
    public static string GetAttributeNBT(List<string[]> attributes)
    {
        if (attributes == null)
            return "";
        string s = "attribute_modifiers={modifiers:[";
        var kvpNum = 0;
        foreach (string[] subArray in attributes)
        {
            if (kvpNum != 0)
                s += ",";
            kvpNum++;
            s += "{id:\"" + GetGuid() + "\"";
            s += ",type:\"generic." + subArray[0] + "\"";
            s += ",amount:" + subArray[1];
            s += ",operation:\"add_value\"";
            s += $",slot:\"{subArray[2]}\"" + "}";
        }
        s += "],show_in_tooltip:false}";

        return s.ToString();
    }
    public static List<string> Attributes = new List<string>
    {
        "armor",
        "armor_toughness",
        "attack_damage",
        "attack_knockback",
        "attack_speed",
        "block_break_speed",
        "block_interaction_range",
        "entity_interaction_range",
        "fall_damage_multiplier",
        "follow_range",
        "gravity",
        "jump_strength",
        "knockback_resistance",
        "luck",
        "max_absorption",
        "max_health",
        "max_health",
        "movement_speed",
        "safe_fall_distance",
        "scale",
        "step_height",
        "burning_time",
        "explosion_knockback_resistance",
        "mining_efficiency",
        "movement_efficiency",
        "oxygen_bonus",
        "sneaking_speed",
        "submerged_mining_speed",
        "sweeping_damage_ratio",
        "water_movement_efficiency"
    };

    public static List<string> slots = new List<string>
    {
        "any",
        "hand",
        "mainhand",
        "offhand",
        "armor",
        "head",
        "chest",
        "legs",
        "feet"
    };

    public static List<string> enchants = new List<string>
    {
        "blast_protection",
        "feather_falling",
        "fire_protection",
        "projectile_protection",
        "protection",
        "thorns",
        "aqua_affinity",
        "depth_strider",
        "frost_walker",
        "respiration",
        "soul_speed",
        "swift_sneak",
        "bane_of_arthropods",
        "breach",
        "density",
        "fire_aspect",
        "knockback",
        "looting",
        "sharpness",
        "smite",
        "sweeping_edge",
        "wind_burst",
        "flame",
        "power",
        "punch",
        "quick_charge",
        "multishot",
        "piercing",
        "infinity",
        "channeling",
        "impaling",
        "loyalty",
        "riptide",
        "efficiency",
        "fortune",
        "silk_touch",
        "mending",
        "unbreaking",
        "luck_of_the_sea",
        "lure",
        "binding_curse",
        "vanishing_curse"
    };

    public static System.Collections.Generic.Dictionary<string, List<Type>> NBTTags = new System.Collections.Generic.Dictionary<string, List<Type>>
    {
        { "max_stack_size", new List<Type> {typeof(float), typeof(LineEdit)} },
        { "max_damage", new List<Type> {typeof(float), typeof(LineEdit)} },
        { "damage", new List<Type> { typeof(float), typeof(LineEdit)} },
        { "repair_cost", new List<Type> { typeof(float), typeof(LineEdit)} },
        { "unbreakable", new List<Type> {typeof(bool), typeof(CheckButton)} },
        { "fire_resistant", new List<Type> { typeof(bool), typeof(CheckButton)} },
        { "hide_tooltip", new List<Type> { typeof(bool), typeof(CheckButton)} },
        { "hide_additional_tooltip", new List<Type> { typeof(bool), typeof(CheckButton)} }
    };

    public static List<string> CanPlaceOn = new List<string>
    {
        "can_place_on", //Blocks
        "show_in_tooltip" //TF
    };
    public static List<string> CanBreak = new List<string>
    {
        "can_break", //Blocks
        "show_in_tooltip" //TF
    };
    public static List<string> Food = new List<string>
    {
        "nutrition", //Int
        "saturation", //Int
        "can_always_eat", //TF
        "eat_seconds", //Int
        "effects", //Effects
        "using_comverts_to" //Item
    };

    public static List<string> jukeboxPlayable = new List<string>
    {
        "song", //path
        "show_in_tooltip" //TF
    };
    public static List<string> tool = new List<string>
    {
        "default_mining_speed", //Int
        "damage_per_block", //Int

    };
    public static List<string> toolRules = new List<string>
    {
        "blocks", //blocks
        "speed", //int
        "correct_for_drops" //TF
    };

    public static List<string> Potion = new List<string>
    {
        "Potion", //Item
        "CustomPotionColor", //Hex
        "CustomPotionEffects" //Effect
    };

    public static List<string> PotionTypes = new List<string>
    {
        "uncraftable",
        "mundane",
        "water",
        "awkward",
        "thick",
        "strong_swiftness",
        "long_swiftness",
        "swiftness",
        "slowness",
        "strong_slowness",
        "long_slowness",
        "strength",
        "strong_strength",
        "long_strength",
        "weakness",
        "strong_weakness",
        "long_weakness",
        "healing",
        "strong_healing",
        "harming",
        "strong_harming",
        "leaping",
        "strong_leaping",
        "long_leaping",
        "luck",
        "strong_regeneration",
        "long_regeneration",
        "regeneration",
        "strong_poison",
        "long_poison",
        "poison",
        "strong_fire_resistance",
        "long_fire_resistance",
        "fire_resistance",
        "long_water_breathing",
        "water_breathing",
        "long_night_vision",
        "night_vision",
        "long_invisibility",
        "invisibility",
        "strong_turtle_master",
        "long_turtle_master",
        "turtle_master",
        "long_slow_falling",
        "slow_falling",
        "wind_charging",
        "oozing",
        "infestation",
        "weaving"
    };
}
