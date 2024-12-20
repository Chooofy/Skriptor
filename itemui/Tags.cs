using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public partial class Tags : Control, Data
{
    // Called when the node enters the scene tree for the first time.

    public void LoadData()
    {

        List<string> data = new List<string>();
        string vr;
        Item.data.TryGetValue("tags", out vr);
        data = vr.Split("|").ToList<string>();

        if (data != null)
        {
            CreateChild(data);
            var d = ResourceLoader.Load<PackedScene>("subnbt/Tool.tscn").Instantiate();
            var child = ResourceLoader.Load<PackedScene>("subnbt/PotionNBT.tscn").Instantiate();
            if (Item.item != "potion")
            {
                child.Set(Node2D.PropertyName.Visible, false);
                potion = child;
            }
            var a = ResourceLoader.Load<PackedScene>("subnbt/jukebox.tscn").Instantiate();
            var b = ResourceLoader.Load<PackedScene>("subnbt/food.tscn").Instantiate();
            var c = ResourceLoader.Load<PackedScene>("subnbt/BlockInteraction.tscn").Instantiate();

            referenceNode.AddChild(d);
            referenceNode.AddChild(potion);
            referenceNode.AddChild(a);
            referenceNode.AddChild(b);
            referenceNode.AddChild(c);

            if (referenceNode.GetChildCount() > 0)
            {
                int n = 0;
                foreach (var j in referenceNode.GetChildren())
                {
                    List<Type> types = new List<Type>
                    {
                        typeof(Tool),
                        typeof(BlockInteraction),
                        typeof(Food),
                        typeof(jukebox),
                        typeof(potion)
                    };

                    foreach (var k in types)
                    {
                        if (j.GetType() == k)
                        {
                            n++;
                            GD.Print(n);
                            var id = j.GetIndex();
                            referenceNode.GetChild<Data>(id).LoadData();
                            GD.Print(j.Name);
                        }
                    }
                }
            }
        }
    }
    public void SaveData()
    {
        try {
            List<string> data = new List<string>();
            if (GetNode<VBoxContainer>("ScrollContainer/VBoxContainer").GetChildCount() > 0)
            {
                foreach (var cont in GetNode<VBoxContainer>("ScrollContainer/VBoxContainer").GetChildren())
                {
                    if (cont.GetType() == typeof(HBoxContainer))
                    {
                        if (cont.GetChild(2).GetType() == typeof(OptionButton))
                            data.Add(cont.GetChild<OptionButton>(2).Text);
                        else
                        {
                            if (cont.GetChild<LineEdit>(2).Text == "")
                                data.Add("");
                            else
                                data.Add(cont.GetChild<LineEdit>(2).Text);
                        }
                    }
                }
            }
            if (referenceNode.GetChildCount() > 0)
            {
                int n = 0;
                foreach (var c in referenceNode.GetChildren())
                {
                    if (c.GetType() != typeof(HBoxContainer) && c.GetType() != typeof(HSeparator) && c.GetType() != typeof(HScrollBar) && c.GetType() != typeof(VScrollBar))
                    {
                        GD.Print(1);
                        var id = c.GetIndex();
                        referenceNode.GetChild<Data>(id).SaveData();
                    }
                }
            }
            string r = "";
            int i = 0;
            foreach (var s in data)
            {
                if (i == 0)
                    r += s;
                else
                    r += "|" + s;
                i++;
            }
            Item.data.Add("tags", r);
        }
        catch (Exception e)
        {
            GD.Print(e);
        }
    }
    VBoxContainer referenceNode;
	public static Node potion;
	public static Node armor;
    public static bool load = false;
    public override void _Ready()
    {
        referenceNode = GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");
        QReady();
    }
    public void QReady()
	{
        if (load == false)
        {
            CreateChild();
            AddOtherChildren();
        }
        else
            LoadData();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public void AddOtherChildren()
	{
		referenceNode.AddChild(ResourceLoader.Load<PackedScene>("subnbt/Tool.tscn").Instantiate());
		var child = ResourceLoader.Load<PackedScene>("subnbt/PotionNBT.tscn").Instantiate();
		referenceNode.AddChild(child);

        if (Item.item != "potion")
		{
			child.Set(Node2D.PropertyName.Visible, false);
			potion = child;
		}
        referenceNode.AddChild(ResourceLoader.Load<PackedScene>("subnbt/jukebox.tscn").Instantiate());
        referenceNode.AddChild(ResourceLoader.Load<PackedScene>("subnbt/food.tscn").Instantiate());
        referenceNode.AddChild(ResourceLoader.Load<PackedScene>("subnbt/BlockInteraction.tscn").Instantiate());
    }
    public void CreateChild(List<string> data = null)
	{
		var refe = GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");
        if (data != null)
            foreach (var child in refe.GetChildren())
                child.QueueFree();
        int n = 0;
		foreach (var tag in NBT.NBTTags)
		{
			HBoxContainer container = new HBoxContainer();
			Label label = new Label();
			label.Text = tag.Key.Capitalize();
			container.AddChild(label);
			container.AddChild(new VSeparator());
            if (tag.Value[0] == typeof(bool))
			{
				OptionButton s;
				s = new OptionButton();
				s.AddItem("true");
				s.AddSeparator();
				s.AddItem("false");
                container.AddChild(s);
                if (data != null)
                    s.Text = data[n].ToString();
            }
            else
			{
				LineEdit s;
				s = new LineEdit();
                s.CustomMinimumSize = new Vector2(128, 0);

                s.PlaceholderText = "Number";
                container.AddChild(s);
                if (data != null)
                    s.Text = data[n].ToString();
            }
            container.CustomMinimumSize = new Vector2(128, 64);
            refe.AddChild(container);
            refe.AddChild(new HSeparator());
            n++;
        }

	}

}
