using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

public partial class Item : Control
{
    // Called when the node enters the scene tree for the first time.

    public static Item instance;
    public static Dictionary<string, string> data = new Dictionary<string, string>();

    string overwriteTexturePath;
    Variant nameColor = "";
    Variant loreColor = "";
    public static string item;
    public static string itemNBT;

    public override void _Ready()
    {
		GetNode<ScriptUI>("TabContainer/Scripts/ScriptUI").type = 2;
        instance = this;
    }

    public override void _Process(double delta)
    {
		UpdateScriptPreview();
    }
    public static void UpdateItemList(Control obj, List<string> names)
	{
		OptionButton list = obj.GetNode<OptionButton>("TabContainer/Item/Information/OptionButton");
        list.Clear();
        foreach (var x in names)
        {
            list.AddItem(x);
            list.AddSeparator();
        }
    }

	public static string itemPath = @"minecraft\textures\item\";
    void TrySetTexture()
    {
        if (overwriteTexturePath == null)
        {
            var name = GetNode<OptionButton>("TabContainer/Item/Information/OptionButton").Text;
            var fullPath = Path.Combine(Main.resourcePackDir, itemPath, name.Replace(" ", "_") + ".png");
            TextureRect rect = GetNode<TextureRect>("TabContainer/Item/Information/TextureRect");
            rect.Texture = Main.ImportTexture(fullPath);
        } else
        {
            TextureRect rect = GetNode<TextureRect>("TabContainer/Item/Information/TextureRect");
            rect.Texture = Main.ImportTexture(overwriteTexturePath);
        }
    }
    public void OnItemSelect(int n)
	{
        item = GetNode<OptionButton>("TabContainer/Item/Information/OptionButton").GetItemText(n);
        if (item == "potion")
            Tags.potion.Set(Node2D.PropertyName.Visible, true);
        else if (item != "potion")
            Tags.potion.Set(Node2D.PropertyName.Visible, false);
        TrySetTexture();
	}

	public void UpdateScriptPreview()
	{
		var prev = GetNode<CodeEdit>("TabContainer/Preview/ScrollContainer/Script");
		List<string> code = GetNode<ScriptUI>("TabContainer/Scripts/ScriptUI").GetCode();
		var s = "";
        foreach (string item in code)
        {
            s += item + "\n";
        }
        prev.Text = s;
    }

    public void NameColorChange(Color color)
	{
        nameColor = color;
	}
	public void LoreColorChange(Color color)
	{
        loreColor = color;
	}

    public void AddCustomTag()
    {
        var ctags = GetNode<customtag>("TabContainer/Item/TabContainer/NBT/HBoxContainer/VBoxContainer/CustomTags/VBoxContainer/Ctags");
        ctags.CreateTag();
    }
    public void SelectTexture()
	{
        GetNode<FileDialog>("TabContainer/Item/TabContainer/NBT/HBoxContainer/VBoxContainer/Button/FileDialog").Popup();
	}
    public void ReceiveTexture(string path)
    {
        GetNode<Button>("TabContainer/Item/TabContainer/NBT/HBoxContainer/VBoxContainer/Button").Text = path;
        overwriteTexturePath = path;
        TrySetTexture();

    }
	public void AddAttribute()
	{
        GetNode<Attribute>("TabContainer/Item/TabContainer/Attributes & Enchantments/HBoxContainer/Attr/Attributes").CreateAttribute();
	}

    public void AddEnchantment()
    {
        GetNode<Enchant>("TabContainer/Item/TabContainer/Attributes & Enchantments/HBoxContainer/Ench/Enchants").CreateEnchant();
    }

    public void UpdateNBTPreview(int n)
    {
        GetNode<CodeEdit>("TabContainer/Preview/ScrollContainer2/NBT").Text = ExtractEntireNBT();
    }

    public string ExtractEntireNBT()
    {
        string s = "";
        if (ExtractBasicNBT() != "")
        {
            s += ExtractBasicNBT();
            s += ",";
        }
        if (ExtractTagNBT() != "")
            s += ExtractTagNBT();
        itemNBT = "[" + s + "]";
        return "{" + s + "}";

    }
    public string ExtractBasicNBT()
    {

        string customModelData = GetNode<LineEdit>("TabContainer/Item/TabContainer/NBT/HBoxContainer/VBoxContainer/LineEdit").Text;
        string cmdNBT = "custom_model_data:" + customModelData;

        if (customModelData != "")
            return cmdNBT;
        return "";
    }
    public string ExtractTagNBT()
    {
        var x = GetNode<Control>("TabContainer/Item/TabContainer/NBT/HBoxContainer/Tags/VBoxContainer/Tags");
        var y = x.GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");

        var s = "";
        foreach (var child in y.GetChildren())
        {
            if (child.GetType() == typeof(HBoxContainer))
            {
                Type type = null;
                var value = "";
                var key = child.GetChild<Label>(0).Text;
                if (child.GetChildOrNull<LineEdit>(2) != null)
                {
                    try
                    {
                        value = child.GetChild<LineEdit>(2).Text;
                        value.ToFloat();
                        type = typeof(float);
                    }
                    catch
                    {
                        type = typeof(string);
                    }
                }
                else
                {
                    value = child.GetChild<OptionButton>(2).Text;
                    type = typeof(bool);
                }
                if (value.ToString() != "")
                    if (child.GetIndex() != 0)
                    {
                        if (NBT.ReturnFormattedNBT(key, value, type) != "")
                            if (s != "")
                                s += "," + NBT.ReturnFormattedNBT(key, value, type);
                            else
                                s += NBT.ReturnFormattedNBT(key, value, type);

                    }
                    else
                        s += NBT.ReturnFormattedNBT(key, value, type);
            }
            else if (child.GetType() != typeof(HSeparator))
            {
                var reference2 = (ExtractNBT)child;
                if (reference2.ExtractNBT() != "") 
                    s += "," + reference2.ExtractNBT();
            }
        }
        var ref1 = GetNode<Attribute>("TabContainer/Item/TabContainer/Attributes & Enchantments/HBoxContainer/Attr/Attributes");
        var ref2 = GetNode<Enchant>("TabContainer/Item/TabContainer/Attributes & Enchantments/HBoxContainer/Ench/Enchants");
        var ref3 = GetNode<customtag>("TabContainer/Item/TabContainer/NBT/HBoxContainer/VBoxContainer/CustomTags/VBoxContainer/Ctags");

        if (s != "" && NBT.GetAttributeNBT(ref1.RetrieveAttributes()) != "")
            s += "," + NBT.GetAttributeNBT(ref1.RetrieveAttributes());
        else
            s += NBT.GetAttributeNBT(ref1.RetrieveAttributes());
        if (s != "" && NBT.GetEnchantNBT(ref2.RetrieveEnchants()) != "")
            s += "," + NBT.GetEnchantNBT(ref2.RetrieveEnchants());
        else
            s += NBT.GetEnchantNBT(ref2.RetrieveEnchants());
        if (s != "" && ref3.ExtractNBT() != "")
            s += "," + ref3.ExtractNBT();
        else
            s += ref3.ExtractNBT();

        return s;
    }
    public void AddLoreLine()
    {
        var reference = GetNode<VBoxContainer>("TabContainer/Item/TabContainer/Appearance/HBoxContainer/VBoxContainer/ScrollContainer/VBoxContainer");

        HBoxContainer cont = new HBoxContainer();
        cont.CustomMinimumSize = new Vector2(310, 32);

        var button = new Button();
        var line = new LineEdit();
        line.CustomMinimumSize = new Vector2(290, 32);
        button.Size = new Vector2(20, 32);
        button.Text = "X";

        button.Connect(Button.SignalName.Pressed, new Callable(cont, Node.MethodName.QueueFree));
        var scrollbutton = ResourceLoader.Load<PackedScene>("extra/updownbarminus.tscn");
        var boombayah = scrollbutton.Instantiate();

        cont.AddChild(boombayah);
        cont.AddChild(button);
        cont.AddChild(line);

        reference.AddChild(cont);
    }
    public void AddLoreLineAlt(string txt = "")
    {
        var reference = GetNode<VBoxContainer>("TabContainer/Item/TabContainer/Appearance/HBoxContainer/VBoxContainer/ScrollContainer/VBoxContainer");

        HBoxContainer cont = new HBoxContainer();
        cont.CustomMinimumSize = new Vector2(310, 32);

        var button = new Button();
        var line = new LineEdit();
        if (txt != "")
        {
            line.Text = txt;
        }
        line.CustomMinimumSize = new Vector2(290, 32);
        button.Size = new Vector2(20, 32);
        button.Text = "X";

        button.Connect(Button.SignalName.Pressed, new Callable(cont, Node.MethodName.QueueFree));

        var scrollbutton = ResourceLoader.Load<PackedScene>("extra/updownbarminus.tscn");
        var boombayah = scrollbutton.Instantiate();

        cont.AddChild(boombayah);
        cont.AddChild(button);
        cont.AddChild(line);

        reference.AddChild(cont);
    }

    public string GetNameS()
    {
        var refe = GetNode<LineEdit>("TabContainer/Item/TabContainer/Appearance/HBoxContainer/VBoxContainer/LineEdit");
        return refe.Text;
    }
    public void LoadLoreAndName()
    {
        try
        {
            var refe = GetNode<LineEdit>("TabContainer/Item/TabContainer/Appearance/HBoxContainer/VBoxContainer/LineEdit");

            refe.Text = "";
            var reference = GetNode<VBoxContainer>("TabContainer/Item/TabContainer/Appearance/HBoxContainer/VBoxContainer/ScrollContainer/VBoxContainer");

            foreach (var child in reference.GetChildren())
                child.QueueFree();

            string s;
            data.TryGetValue("Lore", out s);
            if (s != "")
            {
                List<string> lore = s.Split(',').ToList();

                foreach (string l in lore)
                {
                    AddLoreLineAlt((string)l);
                }
            }
            data.TryGetValue("Name", out s);
            if (s != "")
                refe.Text = s;
        }
        catch (Exception ex)
        {
            GD.Print(ex);
        }

    }
    public string GetLore(int a = 0)
    {
        List<string> lore = new List<string>();
        var reef = GetNode<VBoxContainer>("TabContainer/Item/TabContainer/Appearance/HBoxContainer/VBoxContainer/ScrollContainer/VBoxContainer");
        foreach (var child in GetNode<VBoxContainer>("TabContainer/Item/TabContainer/Appearance/HBoxContainer/VBoxContainer/ScrollContainer/VBoxContainer").GetChildren())
        {
            var f = child.GetChild<LineEdit>(2).Text;
            lore.Add(f);
        }
        //Format
        string s = "";
        var n = 0;
        if (a == 0)
        {
            foreach (var i in lore)
            {
                if (lore.Count == 1)
                {
                    s += i.ToString();
                }
                else
                {
                    s += i + ", ";
                    var b = n + 1;
                    if (b == lore.Count)
                    {
                        s += "and " + i;
                    }
                }
                n++;
            }
        }
        else if (a == 1)
        {
            s = "";
            foreach (var i in lore)
            {
                if (lore.IndexOf(i) == lore.Count - 1)
                    s += i;
                else
                    s += i + ",";
            }
        }
        return s;
    }

	public void Export()
	{
        var allow = false;
        var n = "";
        if (GetNode<LineEdit>("Label3/LineEdit").Text != "")
        {
            n = GetNode<LineEdit>("Label3/LineEdit").Text;
            allow = true;
        }
        else
        {
            GetNode<AcceptDialog>("AcceptDialog").Popup();
        }
        if (allow == true)
        {
            var tempDic = new Godot.Collections.Dictionary();
            SaveData();
            foreach (var item in data)
            {
                tempDic.Add(item.Key, item.Value);
            }
            string s = Json.Stringify(tempDic);

            //

            var saveGame = Godot.FileAccess.Open("user://items/" + n + ".save", Godot.FileAccess.ModeFlags.Write);
            saveGame.StoreLine(s);
            saveGame.Close();
        }

        var fname = n;

        //type
        var t = GetNode<OptionButton>("TabContainer/Item/Information/OptionButton").Text;
        //name
        var na = GetNode<LineEdit>("TabContainer/Item/TabContainer/Appearance/HBoxContainer/VBoxContainer/LineEdit").Text;
        //lore
        var lore = GetLore(1);
        //nbt
        var nbt = ExtractEntireNBT();


        var sat = $"{t}\n{na}\n{lore}\n{nbt}" ;

        if (!Directory.Exists(Path.Combine(Main.serverDir, @"Skriptor\items\")))
        {
            Directory.CreateDirectory(Path.Combine(Main.serverDir, @"Skripter\items"));
        }
        string dir = Path.Combine(Main.serverDir, @"Skriptor\items\");
        string path = Path.Combine(Main.serverDir, @"Skriptor\items\", fname + ".txt");
        string pathSk = Path.Combine(Main.serverDir, @"Skriptor\items\", fname + ".sk");
        List<string> baseSkript = new List<string>
        {
            "options:",
            $" item: getItem(\"{fname}\")",
        };

  
        if (Directory.Exists(dir))
        {
            if (!File.Exists(pathSk))
            {
                using (StreamWriter sw = File.CreateText(pathSk))
                {
                    GD.Print(1);
                    foreach (var line in baseSkript)
                        sw.WriteLine(line);
                    foreach (string k in ScriptUI.instance.GetCode())
                    {
                        sw.WriteLine(k);
                    }
                }
            }
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(sat);
            }

        }
        else if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            if (!File.Exists(pathSk))
            {
                using (StreamWriter sw = File.CreateText(pathSk))
                {
                    GD.Print(2);
                    foreach (var line in baseSkript)
                        sw.WriteLine(line);
                    foreach (var k in ScriptUI.instance.GetCode())
                        sw.WriteLine(k);
                }
            }
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(sat);
            }
        }


        GetTree().ReloadCurrentScene();
    }
    public void SaveData()
    {
        try
        {
            data.Clear();
            GetNode<Data>("TabContainer/Item/TabContainer/Attributes & Enchantments/HBoxContainer/Attr/Attributes").SaveData();
            GetNode<Data>("TabContainer/Item/TabContainer/Attributes & Enchantments/HBoxContainer/Ench/Enchants").SaveData();
            GetNode<Data>("TabContainer/Item/TabContainer/NBT/HBoxContainer/VBoxContainer/CustomTags/VBoxContainer/Ctags").SaveData();

            GetNode<Data>("TabContainer/Item/TabContainer/NBT/HBoxContainer/Tags/VBoxContainer/Tags").SaveData();
            data.Add("Name", GetNameS());
            data.Add("Lore", GetLore(1));
            data.Add("CMD", GetNode<LineEdit>("TabContainer/Item/TabContainer/NBT/HBoxContainer/VBoxContainer/LineEdit").Text);
            data.Add("pic", Path.Combine(Main.resourcePackDir, itemPath, GetNode<OptionButton>("TabContainer/Item/Information/OptionButton").Text.Replace(" ", "_") + ".png"));
        }
        catch (Exception ex)
        {
            GD.Print(ex);
        }
    }

    public void LoadItem()
    {
        GetNode<FileDialog>("TabContainer/Item/Information/FileDialog").Popup();
    }
    public void LoadData(string n)
    {
        GD.Print(1);

        GetNode<Button>("TabContainer/Item/Information/Button").Text = n;
        string name = Path.GetFileNameWithoutExtension(n);
        string s = "";
        GetNode<LineEdit>("Label3/LineEdit").Text = name; //File Name

        //Get File
        GD.Print(name);

        if (!Godot.FileAccess.FileExists("user://items/" + name + ".save"))
        {
            return;
        }
        GD.Print(3);

        var saveGame = Godot.FileAccess.Open("user://items/" + name + ".save", Godot.FileAccess.ModeFlags.Read);

        s = saveGame.GetLine();

        // Get File

        var json = new Json();
        var parseResult = json.Parse(s);
        var nodeData = json.Data;
        var dic = nodeData.AsGodotDictionary<string, string>();
        GD.Print(4);


        data.Clear();
        foreach (var t in dic)
            data.Add(t.Key, t.Value);
        string s231;
        data.TryGetValue("CMD", out s231);
        GetNode<LineEdit>("TabContainer/Item/TabContainer/NBT/HBoxContainer/VBoxContainer/LineEdit").Text = s231;

        LoadLoreAndName();
        GetNode<Data>("TabContainer/Item/TabContainer/Attributes & Enchantments/HBoxContainer/Attr/Attributes").LoadData();
        GetNode<Data>("TabContainer/Item/TabContainer/Attributes & Enchantments/HBoxContainer/Ench/Enchants").LoadData();
        GetNode<Data>("TabContainer/Item/TabContainer/NBT/HBoxContainer/VBoxContainer/CustomTags/VBoxContainer/Ctags").LoadData();
        GetNode<Data>("TabContainer/Item/TabContainer/NBT/HBoxContainer/Tags/VBoxContainer/Tags").LoadData();
        TrySetTexture();
    }



    void CreateItemFile()
    {

    }
}
