using Godot;
using System;
using System.Collections.Generic;

public partial class customtag : Control, ExtractNBT, Data
{
    // Called when the node enters the scene tree for the first time.

    public void LoadData()
    {
        foreach (var child in GetNode<VBoxContainer>("ScrollContainer/VBoxContainer").GetChildren())
            child.QueueFree();
        foreach (var item in Item.data) 
        {
            if (item.Key.StartsWith("customtag-:"))
            {
                var i = item.Key.Split("customtag-:")[1];
                CreateTag(i, item.Value);
            }
        }
    }

    public void SaveData()
    {
        List<object> data = new List<object>();
        var refe = GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");
        if (refe.GetChildCount() > 0)
        {
            foreach (var child in refe.GetChildren())
            {
                var key = child.GetChild(1).GetNode<LineEdit>("Element/LineEdit2").Text;
                var value = child.GetChild(1).GetNode<LineEdit>("Element/LineEdit").Text;

                Item.data.Add("customtag-:" + key, value);
            }
        }
    }

    public void CreateTag(string a = null, string b = null)
	{
        var panel = GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");
        var temp = ResourceLoader.Load<PackedScene>("itemui/customtag.tscn");
        var butt = temp.Instantiate();

        if (a != null && b != null)
        {
            butt.GetNode<LineEdit>("Element/LineEdit2").Text = a;
            butt.GetNode<LineEdit>("Element/LineEdit").Text = b;
        }

        var container = new HBoxContainer();

        var xbutton = ResourceLoader.Load<PackedScene>("extra/xbutton.tscn");
        var butt2 = xbutton.Instantiate();

        butt2.GetChild(0).Connect(Button.SignalName.Pressed, new Callable(container, Node.MethodName.QueueFree));
        container.AddChild(butt2);
        container.AddChild(butt);

        panel.AddChild(container);
    }


    public string ExtractNBT()
    {
        var panel = GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");
        var nbt = "";
        if (panel.GetChildCount() > 0)
        {
            nbt = "custom_data={";
            foreach (var child in panel.GetChildren())
            {
                var key = child.GetChild(1).GetNode<LineEdit>("Element/LineEdit2").Text;
                var value = child.GetChild(1).GetNode<LineEdit>("Element/LineEdit").Text;
                if (child.GetIndex() == 0)
                    nbt += $"{key}:{value}";
                else
                    nbt += $",{key}:{value}";
            }
            nbt += "}";
        }
        if (nbt != "")
            return nbt;
        return "";
    }
}
