using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class Crafting : Control
{
    // Called when the node enters the scene tree for the first time.

    public static Crafting instance;

    public override void _Ready()
    {
        instance = this;
    }

    public void LoadNames()
    {
        foreach (RecipeButton child in GetNode<GridContainer>("Panel/HBoxContainer/GridContainer").GetChildren())
        {
            child.UpdateItemList(child);
            child.GetNode<OptionButton>("Panel/VBoxContainer/OptionButton").Text = "air";
        }
        foreach (RecipeButton child in GetNode<GridContainer>("Panel/GridContainer2").GetChildren())
        {
            child.UpdateItemList(child);
            child.GetNode<OptionButton>("Panel/VBoxContainer/OptionButton").Text = "air";
        }
    }


    string FormatItem(string name)
    {
        string type = name;
        foreach (string file in Directory.EnumerateFiles(Path.Combine(Main.serverDir, "items")))
        {
            string[] lines = File.ReadAllLines(file);
            if (Path.GetFileNameWithoutExtension(file) == name)
            {
                type = lines[0];
                string line = $"getItem(\"{name}\")";
                return line;
            }
        }
        if (name == "" || name == null)
            type = "air";
        return $"{type}";
    }

    public void Export()
    {
        List<string> shape = new List<string>();
        string name = GetNode<LineEdit>("Panel2/VBoxContainer/LineEdit").Text;
        string type = "shapeless";
        if (GetNode<CheckButton>("Panel2/VBoxContainer/HBoxContainer/CheckButton").ButtonPressed == true)
        {
            type = "shaped";
        }
        List<RecipeButton> buttons = new List<RecipeButton>();
        foreach (RecipeButton child in GetNode<GridContainer>("Panel/HBoxContainer/GridContainer").GetChildren())
        {
            buttons.Add(child);
        }

        var s = "";
        s += (FormatItem(buttons[0].ReturnName()) == "air") ? ' ' : 'a';
        s += (FormatItem(buttons[1].ReturnName()) == "air") ? ' ' : 'b';
        s += (FormatItem(buttons[2].ReturnName()) == "air") ? ' ' : 'c';
        shape.Add(s);
        s = "";

        s += (FormatItem(buttons[3].ReturnName()) == "air") ? ' ' : 'd';
        s += (FormatItem(buttons[4].ReturnName()) == "air") ? ' ' : 'e';
        s += (FormatItem(buttons[5].ReturnName()) == "air") ? ' ' : 'f';
        shape.Add(s);
        s = "";

        s += (FormatItem(buttons[6].ReturnName()) == "air") ? ' ' : 'g';
        s += (FormatItem(buttons[7].ReturnName()) == "air") ? ' ' : 'h';
        s += (FormatItem(buttons[8].ReturnName()) == "air") ? ' ' : 'i';
        shape.Add(s);

        GD.Print(shape[0], shape[1], shape[2]);
        foreach (RecipeButton child in GetNode<GridContainer>("Panel/GridContainer2").GetChildren())
        {
            buttons.Add(child);
        }
        /*on load:
	register shaped recipe:
		id: "custom:fancy_stone"
		result: stone named "&aFANCY STONE"
		shape: "aaa", "aba", "aaa"
		group: "bloop"
		category: "building"
		ingredients:
			set ingredient of "a" to stone
			set ingredient of "b" to diamond
 * register shapeless recipe:
id: "custom:totem_of_undying"
result: totem of undying
group: "custom tools"
category: "redstone"
ingredients:
    add diamond block to ingredients
    add material choice of every plank to ingredients
    add emerald block to ingredients
    add end rod to ingredients
    add wither skeleton skull to ingredients
*/
        List<string> script = new List<string>();


        


        if (type == "shaped")
        {
            script = new List<string>
            {
                "on load:",
                " register shaped recipe:",
                $"  id: \"custom:{GetNode<LineEdit>("Panel2/VBoxContainer/LineEdit2").Text}\"",
                $"  result: {FormatItem(buttons[9].ReturnName())}",
                $"  shape: \"{shape[0]}\", \"{shape[1]}\", \"{shape[2]}\"",
                $"  group: \"{GetNode<LineEdit>("Panel2/VBoxContainer/LineEdit3").Text}\"",
                $"  category: \"{GetNode<LineEdit>("Panel2/VBoxContainer/LineEdit4").Text}\"",
                "  ingredients:",
                //
            };
            if (shape[0][0] != ' ')
                script.Add($"   set ingredient of \"a\" to {FormatItem(buttons[0].ReturnName())}");
            if (shape[0][1] != ' ')
                script.Add($"   set ingredient of \"b\" to {FormatItem(buttons[1].ReturnName())}");
            if (shape[0][2] != ' ')
                script.Add($"   set ingredient of \"c\" to {FormatItem(buttons[2].ReturnName())}");
            if (shape[1][0] != ' ')
                script.Add($"   set ingredient of \"d\" to {FormatItem(buttons[3].ReturnName())}");
            if (shape[1][1] != ' ')
                script.Add($"   set ingredient of \"e\" to {FormatItem(buttons[4].ReturnName())}");
            if (shape[1][2] != ' ')
                script.Add($"   set ingredient of \"f\" to {FormatItem(buttons[5].ReturnName())}");
            if (shape[2][0] != ' ')
                script.Add($"   set ingredient of \"g\" to {FormatItem(buttons[6].ReturnName())}");
            if (shape[2][1] != ' ')
                script.Add($"   set ingredient of \"h\" to {FormatItem(buttons[7].ReturnName())}");
            if (shape[2][2] != ' ')
                script.Add($"   set ingredient of \"i\" to {FormatItem(buttons[8].ReturnName())}");
        }
        if (type == "shapeless")
        {
            script = new List<string>
            {
                "on load:",
                " register shapeless recipe:",
                $"  id: \"custom:{GetNode<LineEdit>("Panel2/VBoxContainer/LineEdit2").Text}\"",
                $"  result: {FormatItem(buttons[9].ReturnName())}",
                $"  group: \"{GetNode<LineEdit>("Panel2/VBoxContainer/LineEdit3").Text}\"",
                $"  category: \"{GetNode<LineEdit>("Panel2/VBoxContainer/LineEdit4").Text}\"",
                "  ingredients:"
            };
            for (int i = 0; i < 9; i++) 
            {
                var item = buttons[i];
                if (FormatItem(item.ReturnName()) != null && FormatItem(item.ReturnName()) != "air")
                {
                    
                    script.Add($"   add {FormatItem(item.ReturnName())} to ingredients");
                }
            }
        }
        foreach (var i in script)
            GD.Print(i);

        var path = Path.Combine(Main.recipeDir, $"{name}.sk");
        if (Directory.Exists(Main.recipeDir))
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                foreach (var line in script)
                    sw.WriteLine(line);
            }
        }
        else if (!Directory.Exists(Main.recipeDir))
        {
            Directory.CreateDirectory(Main.recipeDir);
            using (StreamWriter sw = File.CreateText(path))
            {
                foreach (var line in script)
                    sw.WriteLine(line);
            }
        }
        GetTree().ReloadCurrentScene();


    }






}
