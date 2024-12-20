using Godot;
using System;
using System.Collections.Generic;

public partial class potion : Control, EffectInterface
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Enum num = new PotionEffects();
        foreach (var item in Enum.GetValues(num.GetType()))
        {
            string s = item.ToString().Replace("_", " ");
            s.ToCamelCase();
            GetNode<OptionButton>("Panel/HBoxContainer/Effect/OptionButton").AddItem(s);
            GetNode<OptionButton>("Panel/HBoxContainer/Effect/OptionButton").AddSeparator();
        }
    }

    public Godot.Collections.Dictionary GetInfo()
	{
		Godot.Collections.Dictionary data = new Godot.Collections.Dictionary();

		data.Add("State", GetNode<CheckButton>("Panel/HBoxContainer/Particles/CheckButton").ButtonPressed);
        data.Add("Effect", GetNode<OptionButton>("Panel/HBoxContainer/Effect/OptionButton").Text.ToLower());
		if (GetNode<LineEdit>("Panel/HBoxContainer/Duration/LineEdit").Text != "")
			data.Add("Duration", GetNode<LineEdit>("Panel/HBoxContainer/Duration/LineEdit").Text);
		else
            data.Add("Duration", GetNode<LineEdit>("Panel/HBoxContainer/Duration/LineEdit").PlaceholderText);
        if (GetNode<LineEdit>("Panel/HBoxContainer/Target/LineEdit").Text != "")
        	data.Add("Target", GetNode<LineEdit>("Panel/HBoxContainer/Target/LineEdit").Text);
        else
            data.Add("Target", GetNode<LineEdit>("Panel/HBoxContainer/Target/LineEdit").PlaceholderText);
        if (GetNode<LineEdit>("Panel/HBoxContainer/Strength/LineEdit").Text != "")
            data.Add("Strength", GetNode<LineEdit>("Panel/HBoxContainer/Strength/LineEdit").Text);
        else
            data.Add("Strength", GetNode<LineEdit>("Panel/HBoxContainer/Strength/LineEdit").PlaceholderText);

        return data;
    }

    public string GetSkript()
    {
        Godot.Collections.Dictionary data = GetInfo();

        string script = "";
        Variant state;
        Variant effect;
        Variant duration;
        Variant target;
        Variant strength;

        data.TryGetValue("State", out state);
        data.TryGetValue("Target", out target);
        data.TryGetValue("Duration", out duration);
        data.TryGetValue("Strength", out strength);
        data.TryGetValue("Effect", out effect);

        if (state.AsBool() && duration.AsString() != "infinite")
            script = $"apply potion of {effect} of tier {strength} to {target} for {duration} seconds";
        else if (state.AsBool() && duration.AsString() == "infinite")
            script = $"apply infinite potion of {effect} of tier {strength} to {target}";
        else if (!state.AsBool() && duration.AsString() != "infinite")
            script = $"apply potion of {effect} of tier {strength} without particles to {target} for {duration} seconds";
        else if (!state.AsBool() && duration.AsString() == "infinite")
            script = $"apply infinite potion of {effect} of tier {strength} without particles to {target}";

        return script;
    }


    public static List<string> Effects = new List<string>
    {
        "absorption",
        "bad_omen",
        "blindness",
        "conduit_power",
        "darkness",
        "dolphins_grace",
        "fire_resistance",
        "glowing",
        "haste",
        "health_boost",
        "hero_of_the_village",
        "hunger",
        "infesed",
        "instant_damage",
        "instant_health",
        "invisibility",
        "jump_boost",
        "levitation",
        "luck",
        "mining_fatigue",
        "nausea",
        "night_vision",
        "oozing",
        "poison",
        "raid_omen",
        "regeneration",
        "resistance",
        "saturation",
        "slow_falling",
        "slowness",
        "speed",
        "strength",
        "trial_omen",
        "unluck",
        "water_breathing",
        "weakness",
        "weaving",
        "wind_charged",
        "wither"
    };
    public enum PotionEffects
    {
        speed,
        slowness,
        haste,
        mining_fatigue,
        strenth,
        instant_health,
        instant_damage,
        jump_boost,
        nausea,
        regeneration,
        resistance,
        fire_resistance,
        water_breathing,
        invisibility,
        blindness,
        night_vision,
        hunger,
        weakness,
        poison,
        wither,
        health_boost,
        absorption,
        saturation,
        glowing,
        levitation,
        luck,
        bad_luck,
        slow_falling,
        conduit_power,
        dolphins_grace,
        bad_omen,
        hero_of_the_village,
        darkness
    }
}
