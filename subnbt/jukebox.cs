using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class jukebox : Control, ExtractNBT, Data
{
    // Called when the node enters the scene tree for the first time.


    public void LoadData()
    {
        string r;
        Item.data.TryGetValue("jukebox", out r);
        var data = r.Split("`").ToList();

        GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer2/LineEdit").Text = data[0].ToString();
        GetNode<OptionButton>("Panel/VBoxContainer/HBoxContainer/OptionButton").Text = data[1].ToString();
    }

    public void SaveData()
    {
        var a = GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer2/LineEdit").Text;
        var b = GetNode<OptionButton>("Panel/VBoxContainer/HBoxContainer/OptionButton").Text;
        string r;
        r = a + "`" + b;

        Item.data.Add("jukebox", r);
    }



    public string ExtractNBT()
	{

		var nbt = "jukebox_playable={";
		if (GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer2/LineEdit").Text != "") 
		{
			nbt += $"song:\"{GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer2/LineEdit").Text}\"";
			nbt += ",show_in_tooltip:" + GetNode<OptionButton>("Panel/VBoxContainer/HBoxContainer/OptionButton").Text;
			nbt += "}";
            return nbt;
        }
		return "";
    }
}
