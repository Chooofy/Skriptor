using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class RecipeButton : Control
{
    // Called when the node enters the scene tree for the first time.


    public string ReturnName()
    {
        return GetNode<OptionButton>("Panel/VBoxContainer/OptionButton").Text;
    }
    public void SetPicture(int n)
    {
        string name = GetNode<OptionButton>("Panel/VBoxContainer/OptionButton").Text;
        TextureRect rect = GetNode<TextureRect>("Panel/VBoxContainer/TextureRect2");
        rect.Texture = Main.ImportTexture(Path.Combine(Main.resourcePackDir, Item.itemPath, name.Replace(" ", "_") + ".png"));
        foreach (string file in Directory.EnumerateFiles(Path.Combine(Main.serverDir, @"Skriptor\items")))
        {
            string[] lines = File.ReadAllLines(file);
            if (Path.GetFileNameWithoutExtension(file) == name)
            {
                rect.Texture = Main.ImportTexture(Path.Combine(Main.resourcePackDir, Item.itemPath, lines[0].Replace(" ", "_") + ".png"));
                return;
            }
        }


    }
    public void UpdateItemList(Control obj)
    {
        var dirPath = Path.Combine(Main.resourcePackDir, @"minecraft\models\item\");
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
        OptionButton list = obj.GetNode<OptionButton>("Panel/VBoxContainer/OptionButton");
        list.Clear();
        foreach (var x in names)
        {
            list.AddItem(x);
            list.AddSeparator();
        }
        foreach (string file in Directory.EnumerateFiles(Path.Combine(Main.serverDir, @"Skriptor\items")))
        {
            if (Path.GetExtension(file) != ".sk")
            {
                var n = Path.GetFileNameWithoutExtension(file);
                list.AddItem(n);
                list.AddSeparator();
            }
        }
    }
}
