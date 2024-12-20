using Godot;
using System;

public partial class ItemTemplateTemplate : Control
{
	// Called when the node enters the scene tree for the first time.
	public string buttonPath;

	public void SetNameItem(string name)
	{
		GetNode<Label>("VBoxContainer/Label").Text = name;
	}

	public Button ReturnButton()
	{
		Button butt = GetNode<Button>("VBoxContainer/Button");
        butt.Connect(Button.SignalName.Pressed, new Callable(this, MethodName.LoadData));
        return butt;
	}

	public TextureRect ReturnPic()
	{
		TextureRect rec = GetNode<TextureRect>("VBoxContainer/TextureRect");
		return rec;
	}

	public void LoadData()
	{
		GD.Print(1);
		Item.instance.LoadData(buttonPath);
		Main.instance.GetNode<TabContainer>("TabContainer").CurrentTab = 1;
	}
}
