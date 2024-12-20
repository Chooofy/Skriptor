using Godot;
using System;
using static potion;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

public partial class Attribute : Control, Data
{
    // Called when the node enters the scene tree for the first time.
    public void LoadData()
    {
        foreach (var child in GetNode<VBoxContainer>("ScrollContainer/VBoxContainer").GetChildren())
            child.QueueFree();
        foreach (var item in Item.data)
        {
            if (item.Key.StartsWith("attributes-:"))
            {
                var a = item.Key.Split("attributes-:")[1];
                var b = item.Value.Split(":")[0];
                var c = item.Value.Split(":")[1];
                CreateAttribute(a, b, c);
            }
        }

    }

    public void SaveData()
    {
        List<string[]> list = RetrieveAttributes();
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                string[] item = list[i];
                Item.data.Add("attributes-:" + item[0], item[1] + ":" + item[2]);
            }
        }
        else
        {
            return;
        }
    }
    public void CreateAttribute(string a = null, string level = null, string b = null)
    {
        var panel = GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");
        var temp = ResourceLoader.Load<PackedScene>("itemui/attr.tscn");
        var butt = temp.Instantiate();

        var container = new HBoxContainer();

        var xbutton = ResourceLoader.Load<PackedScene>("extra/xbutton.tscn");
        var butt2 = xbutton.Instantiate();

        butt2.GetChild(0).Connect(Button.SignalName.Pressed, new Callable(container, Node.MethodName.QueueFree));
        container.AddChild(butt2);
        container.AddChild(butt);

        foreach (var item in NBT.Attributes)
        {
            string s = item.ToString().Replace("_", " ");
            s.ToCamelCase();
            butt.GetNode<OptionButton>("Element/OptionButton").AddItem(s);
            butt.GetNode<OptionButton>("Element/OptionButton").AddSeparator();
        }
        foreach (var item in NBT.slots)
        {
            string s = item.ToString().Replace("_", " ");
            s.ToCamelCase();
            butt.GetNode<OptionButton>("Element/Slots").AddItem(s);
            butt.GetNode<OptionButton>("Element/Slots").AddSeparator();
        }
        if (a != null && level != null && b != null)
        {
            butt.GetNode<OptionButton>("Element/Slots").Text = b;
            butt.GetNode<OptionButton>("Element/OptionButton").Text = a;
            butt.GetNode<LineEdit>("Element/LineEdit").Text = level.ToString();
        }
        panel.AddChild(container);
    }

    public List<string[]> RetrieveAttributes()
    {
        List<string[]> s = new List<string[]>();
        foreach (var child in GetNode<VBoxContainer>("ScrollContainer/VBoxContainer").GetChildren())
        {
            var n1 = child.GetNode<OptionButton>("Control/Element/OptionButton").Text.Replace(" ","_");
            var n2 = child.GetNode<LineEdit>("Control/Element/LineEdit").Text;
            var n3 = child.GetNode<OptionButton>("Control/Element/Slots").Text.Replace(" ","_");

            string[] subS = { n1, n2, n3 };
            s.Add(subS);
        }
        if (s.Count > 0)
            return s;
        return null;
    }
}
