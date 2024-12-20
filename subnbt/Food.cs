using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Food : Control, ExtractNBT, Data
{
    // Called when the node enters the scene tree for the first time.
    public void LoadData()
    {
        string str;
        Item.data.TryGetValue("food", out str);
        List<string> r = str.ToString().Split("|").ToList();
        GetNode<LineEdit>("Panel/VBoxContainer/Element/LineEdit").Text = r[0];
        GetNode<LineEdit>("Panel/VBoxContainer/Element2/LineEdit").Text = r[1];
        GetNode<OptionButton>("Panel/VBoxContainer/Element3/OptionButton").Text = r[2];
        GetNode<LineEdit>("Panel/VBoxContainer/Element4/LineEdit").Text = r[3];
    }

    public void SaveData()
    {

        string r = "";
        r += GetNode<LineEdit>("Panel/VBoxContainer/Element/LineEdit").Text;
        r += "|" + GetNode<LineEdit>("Panel/VBoxContainer/Element2/LineEdit").Text;
        r += "|" + GetNode<OptionButton>("Panel/VBoxContainer/Element3/OptionButton").Text;
        r += "|" + GetNode<LineEdit>("Panel/VBoxContainer/Element4/LineEdit").Text;
        Item.data.Add("food", r);      
    }



    public string ExtractNBT()
	{
		var nbt = "food={";
		if (GetNode<LineEdit>("Panel/VBoxContainer/Element/LineEdit").Text != "")
		{
			nbt += "nutrition:" + GetNode<LineEdit>("Panel/VBoxContainer/Element/LineEdit").Text;
			nbt += ",saturation:" + GetNode<LineEdit>("Panel/VBoxContainer/Element2/LineEdit").Text;
			nbt += ",can_always_eat:" + GetNode<OptionButton>("Panel/VBoxContainer/Element3/OptionButton").Text;
			nbt += ",eat_seconds:" + GetNode<LineEdit>("Panel/VBoxContainer/Element4/LineEdit").Text;
			nbt += "}";

			return nbt;
		}
		return "";
    }
}
