using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public partial class Tool : Control, ExtractNBT, Data
{
    // Called when the node enters the scene tree for the first time.

    VBoxContainer reference;


    public void LoadData()
    {
        GD.Print("err");

        string a;
        Item.data.TryGetValue("tool-:", out a);
        List<string> list = a.Split("`").ToList<string>();
        GD.Print(list[0], list[1]);
        GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer/LineEdit").Text = list[0];
        GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer2/LineEdit").Text = list[1];

        foreach (var item2 in list)
        {
            if (list.IndexOf(item2) > 1)
            {
                var rule = AddRule();
                rule.LoadData(item2);
            }
        }
    }

    public void SaveData()
    {
        var b = GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer/LineEdit").Text;
        var a = GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer2/LineEdit").Text;
        string data = b + "`" + a;

        if (GetNode<VBoxContainer>("Panel/VBoxContainer/ScrollContainer/VBoxContainer").GetChildCount() > 0)
        {
            foreach (var child in GetNode<VBoxContainer>("Panel/VBoxContainer/ScrollContainer/VBoxContainer").GetChildren())
            {
                if (GetNode<VBoxContainer>("Panel/VBoxContainer/ScrollContainer/VBoxContainer").GetChildCount() > 0)
                {
                    var child2 = child.GetChild<ToolRules>(1);
                    data += "`" + child2.SaveData();
                }
            }
        }
        Item.data.Add("tool-:", data);
    }

    public override void _Ready()
    {
        reference = GetNode<VBoxContainer>("Panel/VBoxContainer/ScrollContainer/VBoxContainer");
    }
    public ToolRules AddRule()
	{

        HBoxContainer cont = new HBoxContainer();
        cont.CustomMinimumSize = new Vector2(0, 166);

        reference.AddChild(cont);

        var butt = ResourceLoader.Load<PackedScene>("extra/xbutton.tscn").Instantiate();
        var scene = ResourceLoader.Load<PackedScene>("subnbt/ToolRules.tscn").Instantiate();

        //Singals
        butt.GetChild(0).Connect(Button.SignalName.Pressed, new Callable(cont, Node.MethodName.QueueFree));

        cont.AddChild(butt);
        cont.AddChild(scene);

        return cont.GetChild<ToolRules>(1);
	}


    public string ExtractNBT()
    {
        var nbt = "tool={";

        nbt += $"default_mining_speed:{GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer/LineEdit").Text}";
        nbt += $",damage_per_block:{GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer2/LineEdit").Text}";

        if (GetNode<VBoxContainer>("Panel/VBoxContainer/ScrollContainer/VBoxContainer").GetChildCount() > 0)
        {
            nbt += ",rules:[";
            foreach (var child in GetNode<VBoxContainer>("Panel/VBoxContainer/ScrollContainer/VBoxContainer").GetChildren())
            {
                if (GetNode<VBoxContainer>("Panel/VBoxContainer/ScrollContainer/VBoxContainer").GetChildCount() > 0)
                {
                    var child2 = child.GetChild<ToolRules>(1);
                    if (child.GetIndex() > 0)
                        nbt += "," + child2.ExtractNBT();
                    else
                        nbt += child2.ExtractNBT();
                }
            }
            nbt += "]";
        }
        nbt += "}";
        if (GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer/LineEdit").Text != "" && GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer2/LineEdit").Text != "")
            return nbt;
        return "";
    }   
}
