using Godot;
using System;

public partial class Teleport : Control, EffectInterface
{
    // Called when the node enters the scene tree for the first time.
    public Godot.Collections.Dictionary GetInfo()
    {
        Godot.Collections.Dictionary data = new Godot.Collections.Dictionary();
        if (GetNode<LineEdit>("Panel/HBoxContainer/Target/LineEdit").Text != null)
            data.Add("Target", GetNode<LineEdit>("Panel/HBoxContainer/Target/LineEdit").Text);
        else
            data.Add("Target", GetNode<LineEdit>("Panel/HBoxContainer/Target/LineEdit").PlaceholderText);
        if (GetNode<OptionButton>("Panel/HBoxContainer/Location/OptionButton").Selected == 1)
            data.Add("Location", "location(" + GetNode<LineEdit>("Panel/HBoxContainer/Location/LineEdit").Text + ")");
        else
            data.Add("Location", GetNode<LineEdit>("Panel/HBoxContainer/Location/LineEdit").Text);
        return data;
    }

    public string GetSkript()
    {
        Godot.Collections.Dictionary data = GetInfo();

        Variant location;
        Variant target;

        data.TryGetValue("Location", out location);
        data.TryGetValue("Target", out target);

        return $"teleport {target} to {location}";
    }
    public void OnItemSelect(int n)
    {
        var optionbutton = GetNode<OptionButton>("Panel/HBoxContainer/Location/OptionButton");
        if (n == 0)
        {
            GetNode<LineEdit>("Panel/HBoxContainer/Location/LineEdit").PlaceholderText = "{example}";
        }
        else
        {
            GetNode<LineEdit>("Panel/HBoxContainer/Location/LineEdit").PlaceholderText = "x, y, z, [world]";
        }
    }
}
