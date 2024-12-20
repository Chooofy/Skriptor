using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class BlockInteraction : Control, ExtractNBT, Data
{
    // Called when the node enters the scene tree for the first time.
    public void LoadData()
    {
        string vr;
        Item.data.TryGetValue("blockint", out vr);

        List<string> list = vr.ToString().Split('|').ToList();
        List<string> brk = list[2].Split("::").ToList();
        List<string> plc = list[3].Split("::").ToList();

        foreach (string s in list) {
            if (list.IndexOf(s) == 0)
            {
                GetNode<OptionButton>("Panel/VBoxContainer/CanPlace/HBoxContainer/OptionButton").Text = s;
            }
            else if (list.IndexOf(s) == 1) {
                GetNode<OptionButton>("Panel/VBoxContainer/CanBreak/HBoxContainer/OptionButton").Text = s;
            }
        }
        foreach (string i in brk)
        {
            AddBreak(i);
        }
        foreach (string s in plc)
        {
            AddPlace(s);
        }
        
    }

    public void SaveData()
    {
        List<object> data1 = new List<object>();
        List<object> data2 = new List<object>();
        string sub1 = "";
        string sub2 = "";
        if (GetNode<VBoxContainer>("Panel/VBoxContainer/CanBreak/Blocks").GetChildCount() != 0)
        {
            sub1 = "";
            int j = 0;
            foreach (var child in GetNode<VBoxContainer>("Panel/VBoxContainer/CanBreak/Blocks").GetChildren())
            {
                if (j == 0)
                    sub1 += child.GetChild<OptionButton>(1).Text.Replace(" ", "_");
                else
                    sub1 += "::" + child.GetChild<OptionButton>(1).Text.Replace(" ", "_");
                j++;
            }
        }

        if (GetNode<VBoxContainer>("Panel/VBoxContainer/CanPlace/Blocks").GetChildCount() != 0)
        {
            sub2 = "";
            int j = 0;
            foreach (var child in GetNode<VBoxContainer>("Panel/VBoxContainer/CanPlace/Blocks").GetChildren())
            {
                if (j == 0)
                    sub2 += child.GetChild<OptionButton>(1).Text.Replace(" ", "_");
                else
                    sub2 += "::" + child.GetChild<OptionButton>(1).Text.Replace(" ", "_");
                j++;
            }
        }

        List<string> temp = new List<string>
        {
            GetNode<OptionButton>("Panel/VBoxContainer/CanPlace/HBoxContainer/OptionButton").Text,
            GetNode<OptionButton>("Panel/VBoxContainer/CanBreak/HBoxContainer/OptionButton").Text,
            sub1,
            sub2
        };

        string r = "";
        int i = 0;

        foreach (var item in temp)
        {
            if (i == 0)
                r += item.ToString();
            else
                r += "|" + item.ToString();
            i++;
        }
        Item.data.Add("blockint", r);
    }

    public void CallAddB()
    {
        AddBreak();
    }
    public void CallAddP()
    {
        AddPlace();
    }
    public void AddBreak(string na = null)
	{
        HBoxContainer cont = new HBoxContainer();
        cont.CustomMinimumSize = new Vector2(0, 56);
        GetNode<VBoxContainer>("Panel/VBoxContainer/CanBreak/Blocks").AddChild(cont);

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

        VBoxContainer v = GetChild(0).GetChild<VBoxContainer>(0);
        CustomMinimumSize = new Vector2(v.Size.X, v.Size.Y + 60);
        v.Size = new Vector2(v.Size.X, v.Size.Y + 60);
    }

    public void BlockRemove(Node node)
    {
        VBoxContainer v = GetChild(0).GetChild<VBoxContainer>(0);
        v.Size = new Vector2(v.Size.X, v.Size.Y - 60);
        CustomMinimumSize = new Vector2(v.Size.X, v.Size.Y - 60);
    }

	public void AddPlace(string na = null)
	{
        HBoxContainer cont = new HBoxContainer();
        cont.CustomMinimumSize = new Vector2(0, 56);
        GetNode<VBoxContainer>("Panel/VBoxContainer/CanPlace/Blocks").AddChild(cont);

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

        VBoxContainer v = GetChild(0).GetChild<VBoxContainer>(0);
        CustomMinimumSize = new Vector2(v.Size.X, v.Size.Y + 60);
        v.Size = new Vector2(v.Size.X, v.Size.Y + 60);
    }

    public string ExtractNBT()
    {
        var nbt1 = "";
        var nbt2 = "";
        if (GetNode<VBoxContainer>("Panel/VBoxContainer/CanBreak/Blocks").GetChildCount() != 0)
        {
            nbt1 = "can_break={predicates:[";

            foreach (var child in GetNode<VBoxContainer>("Panel/VBoxContainer/CanBreak/Blocks").GetChildren())
            {
                if (child.GetIndex() == 0)
                    nbt1 += "{blocks:" + $"\"{child.GetChild<OptionButton>(1).Text.Replace(" ", "_")}\"" + "}";
                else
                    nbt1 += ",{blocks:" + $"\"{child.GetChild<OptionButton>(1).Text.Replace(" ", "_")}\"" + "}";
            }
            nbt1 += $"],show_in_tooltip:{GetNode<OptionButton>("Panel/VBoxContainer/CanBreak/HBoxContainer/OptionButton").Text}" + "}";
        }

        if (GetNode<VBoxContainer>("Panel/VBoxContainer/CanPlace/Blocks").GetChildCount() != 0)
        {
            nbt2 = "can_place={predicates:[";

            foreach (var child in GetNode<VBoxContainer>("Panel/VBoxContainer/CanPlace/Blocks").GetChildren())
            {
                if (child.GetIndex() == 0)
                    nbt2 += "{blocks:" + $"\"{child.GetChild<OptionButton>(1).Text.Replace(" ", "_")}\"" + "}";
                else
                    nbt2 += ",{blocks:" + $"\"{child.GetChild<OptionButton>(1).Text.Replace(" ", "_")}\"" + "}";
            }
            nbt2 += $"],show_in_tooltip:{GetNode<OptionButton>("Panel/VBoxContainer/CanPlace/HBoxContainer/OptionButton").Text}" + "}";
        }

        if (nbt1 != "" && nbt2 != "")
            return nbt1 + "," + nbt2;
        if (nbt2 == "" && nbt1 != "")
            return nbt1;
        if (nbt1 == "" && nbt2 != "")
            return nbt2;
        else
            return "";
    }

}
