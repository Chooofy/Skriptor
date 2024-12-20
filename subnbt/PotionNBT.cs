using Godot;
using System;
using System.Collections.Generic;

public partial class PotionNBT : Control, ExtractNBT, Data
{
    // Called when the node enters the scene tree for the first time.
    public void LoadData()
    {
        string r;
        Item.data.TryGetValue("potion", out r);
        GetNode<OptionButton>("Panel/VBoxContainer/HBoxContainer/OptionButton").Text = r.ToString().Capitalize();
    }

    public void SaveData()
    {
        string s = GetNode<OptionButton>("Panel/VBoxContainer/HBoxContainer/OptionButton").Text.ToSnakeCase();
        Item.data.Add("potion", s);
    }


    public override void _Ready()
    {
        var n = GetNode<OptionButton>("Panel/VBoxContainer/HBoxContainer/OptionButton");

        foreach (var effect in NBT.PotionTypes)
        {
            var name = effect.Capitalize();
            n.AddItem(name);
            n.AddSeparator();
        }
    }

    public void ColorChange(Color color)
    {
        GD.Print(color.ToRgba32()/255);
    }
    public string ExtractNBT()
	{
        var nbt = "";

        if (GetNode<OptionButton>("Panel/VBoxContainer/HBoxContainer/OptionButton").Text != "" && Visible == true)
        {
            var color = GetNode<ColorPickerButton>("Panel/VBoxContainer/HBoxContainer2/ColorPickerButton").Color;
            nbt += "potion_contents={potion:";
            nbt += $"\"minecraft:{GetNode<OptionButton>("Panel/VBoxContainer/HBoxContainer/OptionButton").Text.ToSnakeCase()}\"";
            //nbt += $",custom_color:" + "}";
            nbt += "}";
            return nbt;
        }
        return "";
	}
}
