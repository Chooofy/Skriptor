using Godot;
using System;
using System.Collections.Generic;
using static potion;

public partial class Enchant : Control, Data
{
    // Called when the node enters the scene tree for the first time.

    public void LoadData()
    {
        foreach (var child in GetNode<VBoxContainer>("ScrollContainer/VBoxContainer").GetChildren())
            child.QueueFree();
        foreach (var item in Item.data)
        {
            if (item.Key.StartsWith("enchants-:"))
            {
                var i = item.Key.Split("enchants-:")[1];
                CreateEnchant(i, item.Value);
            }
        }
    }

    public void SaveData()
    {
        var dic = RetrieveEnchants();
        if (dic != null)
        {
            foreach (KeyValuePair<Variant, Variant> item in dic)
            {
                Item.data.Add("enchants-:" + item.Key.ToString(), item.Value.ToString());
            }
        }
        else
        {
            return;
        }
    }



    public void CreateEnchant(string e = null, string level = null)
    {
        var panel = GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");
        var temp = ResourceLoader.Load<PackedScene>("itemui/ench.tscn");
        var butt = temp.Instantiate();

        var container = new HBoxContainer();

        var xbutton = ResourceLoader.Load<PackedScene>("extra/xbutton.tscn");
        var butt2 = xbutton.Instantiate();

        butt2.GetChild(0).Connect(Button.SignalName.Pressed, new Callable(container, Node.MethodName.QueueFree));
        container.AddChild(butt2);
        container.AddChild(butt);

        foreach (var item in NBT.enchants)
        {
            string s = item.ToString().Replace("_", " ");
            s.ToCamelCase();
            butt.GetNode<OptionButton>("Element/OptionButton").AddItem(s);
            butt.GetNode<OptionButton>("Element/OptionButton").AddSeparator();
        }
        if (e != null && level != null)
        {
            butt.GetNode<OptionButton>("Element/OptionButton").Text = e;
            butt.GetNode<LineEdit>("Element/LineEdit").Text = level;
        }

        panel.AddChild(container);
    }

    public Godot.Collections.Dictionary RetrieveEnchants()
    {
        Godot.Collections.Dictionary dic = new Godot.Collections.Dictionary();
        foreach (var child in GetNode<VBoxContainer>("ScrollContainer/VBoxContainer").GetChildren())
        {
            var n1 = child.GetNode<OptionButton>("Control/Element/OptionButton").Text.Replace(" ","_");
            var n2 = child.GetNode<LineEdit>("Control/Element/LineEdit").Text;
            dic.Add(n1, n2);
        }
        if (dic.Count > 0)
            return dic;
        return null;
    }
}
