using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

public partial class Catalogue : GridContainer
{
    // Called when the node enters the scene tree for the first time.







    public void LoadItems()
	{
		foreach (var child in GetChildren())
		{
			child.QueueFree();
		}
		foreach (string file in Directory.EnumerateFiles(Path.Combine(Main.serverDir, @"Skriptor\items")))
		{
			GD.Print(file);
			if (Path.GetExtension(file) == ".txt")
			{
				string[] lines = File.ReadAllLines(file);
				LoadItem(lines[1], lines[0], file);
			}
		}
	}


	void LoadItem(string name, string type, string filePath)
	{

		GD.Print(name, type, filePath);

		var item = ResourceLoader.Load<PackedScene>("extra/item-template.tscn").Instantiate();
		var temp = item as ItemTemplateTemplate;
		temp.SetNameItem(Path.GetFileNameWithoutExtension(filePath));

		Button butt = temp.ReturnButton();

		TextureRect texture = temp.ReturnPic();
		texture.Texture = Main.ImportTexture(Path.Combine(Main.resourcePackDir, Item.itemPath, type.Replace(" ", "_") + ".png"));
		temp.buttonPath = filePath;
		AddChild(item);
    }
}
