using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class ToolRules : Control, ExtractNBT
{
    // Called when the node enters the scene tree for the first time.
    public void LoadData(string rule)
    {
        string s = rule;

        s = s.TrimStart('[');
        s = s.TrimEnd(']');

        List<string> t = s.Split(":-:-:").ToList();
        GD.Print(t);

        GetNode<LineEdit>("VBoxContainer/HBoxContainer/LineEdit").Text = t[0];
        GetNode<OptionButton>("VBoxContainer/HBoxContainer2/OptionButton").Text = t[1];
        int i = 0;
        foreach(var d in t)
        {
            if (i > 1)
                AddBlock(d);
            i++;
        }

    }
    public string SaveData()
    {
        var blocks = "[";
        var speed = GetNode<LineEdit>("VBoxContainer/HBoxContainer/LineEdit").Text;
        var correctForDrops = GetNode<OptionButton>("VBoxContainer/HBoxContainer2/OptionButton").Text;
        if (speed == "" || speed == " ")
        {
            speed = "0.8";
        }
        blocks += speed;
        blocks += ":-:-:" + correctForDrops;

        if (GetNode<VBoxContainer>("VBoxContainer/Blocks").GetChildCount() > 0)
        {
            var count = 0;
            foreach (var child in GetNode<VBoxContainer>("VBoxContainer/Blocks").GetChildren())
            {
                if (child.GetType() == typeof(HBoxContainer))
                {
                    var s = child.GetChild<OptionButton>(1).Text;

                    blocks += ":-:-:" + s;
                    count++;
                }
            }
        }
        blocks += "]";

        return blocks;
    }
    public void CallAddB()
    {
        AddBlock();
    }
	public void AddBlock(string na = null)
	{
		HBoxContainer cont = new HBoxContainer();
		cont.CustomMinimumSize = new Vector2(0, 56);
        GetNode<VBoxContainer>("VBoxContainer/Blocks").AddChild(cont);

		OptionButton button = new OptionButton();


        var dirPath = Path.Combine(Main.resourcePackDir, @"minecraft\blockstates\");
        List<string> names = new List<string>();
        foreach (string file in Directory.EnumerateFiles(dirPath))
        {
            var name = Path.GetFileName(file);
            var tName = "";
            if (name.Contains('_'))
                tName = name.Split('.')[0].Replace("_", " ");
            else
                tName = name.Split('.')[0];
            names.Add(tName);
        }
        foreach (var x in names)
        {
            button.AddItem(x);
            button.AddSeparator();
        }

        if (na != null)
            button.Text = na;
        var butt = ResourceLoader.Load<PackedScene>("extra/xbutton.tscn").Instantiate();
        butt.GetChild(0).Connect(Button.SignalName.Pressed, new Callable(cont, Node.MethodName.QueueFree));

        cont.AddChild(butt);
        cont.AddChild(button);

        VBoxContainer v = GetChild<VBoxContainer>(0);
        CustomMinimumSize = new Vector2(v.Size.X, v.Size.Y + 60);
        v.Size = new Vector2(v.Size.X, v.Size.Y + 60);
    }

    public void BlockRemove(Node node)
    {
        VBoxContainer v = GetChild<VBoxContainer>(0);
        v.Size = new Vector2(v.Size.X, v.Size.Y - 60);
        CustomMinimumSize = new Vector2(v.Size.X, v.Size.Y - 60);
    }

    public string ExtractNBT()
    {
        var tNBT = "";
        string nbt = "{";
        var speed = GetNode<LineEdit>("VBoxContainer/HBoxContainer/LineEdit").Text;
        if (speed != "")
            nbt += $"speed:{speed},";
        var correctForDrops = GetNode<OptionButton>("VBoxContainer/HBoxContainer2/OptionButton").Text;
        if (correctForDrops != "")
            nbt += $"correct_for_drops:{correctForDrops}";
        if (GetNode<VBoxContainer>("VBoxContainer/Blocks").GetChildCount() > 1)
        {
            tNBT = ",blocks:[";
            foreach (var child in GetNode<VBoxContainer>("VBoxContainer/Blocks").GetChildren())
            {
                if (child.GetType() == typeof(HBoxContainer))
                {
                    var s = child.GetChild<OptionButton>(1).Text;
                    if (child.GetIndex() != 0)
                        tNBT += ",\"" + s.Replace(" ", "_") + "\"";
                    else
                        tNBT += "\"" + s.Replace(" ", "_") + "\"";

                }
            }
            tNBT += "]";
        }
        else if (GetNode<VBoxContainer>("VBoxContainer/Blocks").GetChildCount() == 1)
        {
            tNBT = $",blocks:\"{GetNode<VBoxContainer>("VBoxContainer/Blocks").GetChild(0).GetChild<OptionButton>(1).Text.Replace(" ", "_")}\"";
        }
        nbt += tNBT + "}";
        return nbt;
    }

}
