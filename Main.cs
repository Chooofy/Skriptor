using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

public partial class Main : Node
{

    //Data


    void RefreshCatalogue(int n)
    {
        Reload();
    }

    public static Godot.Collections.Dictionary data;


    //Signal Methods

    //Properties
	public static string resourcePackDir;
	public static string serverDir;
    public static string recipeDir;

    public bool TextureSet = false;
    public static Main instance;

    //Ref nodes
    public HBoxContainer blockRefNode;
    public VBoxContainer settingsRefNode;
    public Control itemRefNode;
    public VBoxContainer craftRefNode;
    public Control catalogueRefNode;
 

    void SetRefNodes()
    {
        blockRefNode = GetNode<HBoxContainer>("TabContainer/Block/HBoxContainer");
        settingsRefNode = GetNode<VBoxContainer>("TabContainer/Settings/VBoxContainer");
        itemRefNode = GetNode<Item>("TabContainer/Item/Item");
        craftRefNode = GetNode<VBoxContainer>("TabContainer/Crafting/VBoxContainer");
        catalogueRefNode = GetNode<Control>("TabContainer/Catalogue/Catalogue");
        ScriptUI.instance = itemRefNode.GetNode<ScriptUI>("TabContainer/Scripts/ScriptUI");
    }

    public override void _Process(double delta)
    {
         UpdatePreview();
    }
    public override void _Ready()
    {
        instance = this;
        Reload();
    }
    public void Reload()
    {
        LoadGame();
        SetRefNodes();
        UpdateSavedDirectories();
        UpdateBlockList(resourcePackDir);
        UpdateItemList(resourcePackDir);
        GetNode<ScriptUI>("TabContainer/Block/HBoxContainer/VBoxContainer/ScriptUI").AddTabBars();
        itemRefNode.GetNode<ScriptUI>("TabContainer/Scripts/ScriptUI").AddTabBars();
        catalogueRefNode.GetNode<Catalogue>("Panel/ScrollContainer/GridContainer").LoadItems();
        ScriptUI.BlockCodes();
        Crafting.instance.LoadNames();

    }
    /*
     {"ResourceDir":"C:\\Users\\kuijp\\AppData\\Roaming\\.minecraft\\resourcepacks\\TestPack\\assets","ServerDir":"C:\\Users\\kuijp\\OneDrive\\Documents\\Code\\Test"} 
     */


    //Block-UI Methods

    string name;
    string itemPath = @"minecraft\textures\block\";

    public void UpdateItemList(string dir)
    {
        var dirPath = Path.Combine(dir, @"minecraft\models\item");
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
        Item.UpdateItemList(itemRefNode, names);
    }
    public void UpdateBlockList(string dir)
    {

        var dirPath = Path.Combine(dir, @"minecraft\blockstates\");
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


        OptionButton list = blockRefNode.GetNode<OptionButton>("VBoxContainer/Block List");
        list.Clear();
        foreach (var x in names)
        {
            list.AddItem(x);
            list.AddSeparator();
        }
    }
    public void Export()
    {
        var fname = GetNode<LineEdit>("TabContainer/Block/HBoxContainer/Preview/LineEdit").Text;
        var ui = GetNode<ScriptUI>("TabContainer/Block/HBoxContainer/VBoxContainer/ScriptUI");

        string s = "";
        foreach (string item in ui.GetCode())
        {
            s += item + "\n";
        }
        string dir = Path.Combine(serverDir, @"blocks\");
        string path = Path.Combine(serverDir, @"blocks\", fname);

        if (Directory.Exists(dir))
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(s);
            }
        }
        else if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(s);
            }
        }
        blockRefNode.GetTree().ReloadCurrentScene();
        Reload();
    }
    public Godot.Image ImportImage(string path)
    {
        return Godot.Image.LoadFromFile(path);
    }

    //Sub-UI Components 

    void OnBlockSelect(int n)
    {
        name = blockRefNode.GetNode<OptionButton>("VBoxContainer/Block List").Text;
        GetNode<ScriptUI>("TabContainer/Block/HBoxContainer/VBoxContainer/ScriptUI").block = blockRefNode.GetNode<OptionButton>("VBoxContainer/Block List").Text;
        TrySetTexture();
    }

    void TrySetTexture()
    {
        var fullPath = Path.Combine(resourcePackDir, itemPath, name.Replace(" ", "_") + ".png");
        TextureRect rect = blockRefNode.GetNode<TextureRect>("Preview/TextureRect");
        rect.Texture = ImportTexture(fullPath);
    }

    void UpdatePreview()
    {
        var preview = GetNode<CodeEdit>("TabContainer/Block/HBoxContainer/Preview/Preview/CodeEdit");
        var ui = GetNode<ScriptUI>("TabContainer/Block/HBoxContainer/VBoxContainer/ScriptUI");

        string s = "";
        foreach (string item in ui.GetCode())
        {
            s += item + "\n";
        }
        preview.Text = s;
    }

    public static Texture2D ImportTexture(string path)
    {
        try
        {
            var ret = ImageTexture.CreateFromImage(Image.LoadFromFile(path));
            if (ret == null)
                return ImageTexture.CreateFromImage(Image.LoadFromFile(Path.Combine(resourcePackDir, @"minecraft\textures\item\", "barrier.png")));
            return ret;
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    //Setting Resource Pack Directories
    void UpdateSavedDirectories()
    {
        if (serverDir != null)
            GetNode<TabBar>("TabContainer/Settings").GetNode<Button>("VBoxContainer/SLB").Text = serverDir;
        if (resourcePackDir != null)
            GetNode<TabBar>("TabContainer/Settings").GetNode<Button>("VBoxContainer/RPLB").Text = resourcePackDir;
        if (recipeDir != null)
            GetNode<TabBar>("TabContainer/Settings").GetNode<Button>("VBoxContainer/RCPB").Text = recipeDir;
    }
    public void ReceiveServerDir(string x)
    {
        serverDir = x;
        SaveGame();
        UpdateSavedDirectories();
    }
    public void ReceiveResourceDir(string x)
    {
        resourcePackDir = x;
        SaveGame();
        UpdateSavedDirectories();
    }
    public void ReceiveRecipeDir(string x)
    {
        recipeDir = x;
        SaveGame();
        UpdateSavedDirectories();
    }
    public void ChooseServerDir()
    {
        var dialog = GetNode<TabBar>("TabContainer/Settings").GetNode<FileDialog>("VBoxContainer/SL/FileDialog");
		dialog.Popup();
    }
	public void ChooseResourceDir()
	{
        var dialog = GetNode<TabBar>("TabContainer/Settings").GetNode<FileDialog>("VBoxContainer/RPL/FileDialog");
        dialog.Popup();
    }
    public void ChooseRecipeDir()
    {
        var dialog = GetNode<TabBar>("TabContainer/Settings").GetNode<FileDialog>("VBoxContainer/RCP/FileDialog");
        dialog.Popup();
    }
    //Saving and Loading User Settings
    void Load(Godot.Collections.Dictionary dic)
    {
        serverDir = DicFuc(dic, "ServerDir");
        resourcePackDir = DicFuc(dic, "ResourceDir");
        recipeDir = DicFuc(dic, "RecipeDir");

    }
    Variant Save()
    {
        var sD = new Godot.Collections.Dictionary();
        sD.Add("ResourceDir", resourcePackDir);
        sD.Add("ServerDir", serverDir);
        sD.Add("RecipeDir", recipeDir);

        return sD;
    }
    void SaveGame()
    {
        var saveGame = Godot.FileAccess.Open("user://settings.save", Godot.FileAccess.ModeFlags.Write);
        var jsonString = Json.Stringify(Save());
        saveGame.StoreLine(jsonString);
        saveGame.Close();
    }
    void LoadGame()
    {
        if (!Godot.FileAccess.FileExists("user://settings.save"))
        {
            return;
        }
        var saveGame = Godot.FileAccess.Open("user://settings.save", Godot.FileAccess.ModeFlags.Read);

        while (saveGame.GetPosition() < saveGame.GetLength())
        {
            var jsonString = saveGame.GetLine();
            var json = new Json();
            var parseResult = json.Parse(jsonString);

            var nodeData = json.Data;
            var dic = nodeData.AsGodotDictionary();
            Load(dic);
        }
        saveGame.Close();
    }
    string DicFuc(Godot.Collections.Dictionary dic, string key)
    {
        Variant temp;
        dic.TryGetValue(key, out temp);
        return temp.ToString();
    }


    public void GenerateFiles()
    {
        List<string> baseSkript = new List<string>
        {
            "function getItem(s: text) :: item:",
            " if file \"plugins/Skript/scripts/items/%{{_s}}%.txt\" exists:",
            "  set {_type} to line 1 in file \"plugins/Skript/scripts/Skriptor/items/%{_s}%.txt\"",
            "  set {_name} to line 2 in file \"plugins/Skript/scripts/Skriptor/items/%{_s}%.txt\"",
            "  set {_lore} to line 3 in file \"plugins/Skript/scripts/Skriptor/items/%{_s}%.txt\"",
            "  set {_nbt} to line 4 in file \"plugins/Skript/scripts/Skriptor/items/%{_s}%.txt\"",
            "  set {_i} to (\"%{_type}%\" parsed as an item)",
            "  set name of {_i} to (colored {_name})",
            "  set {_s::*} to {_lore} split at \",\"",
            "  set lore of {_i} to colored { _s::*}",
            "  replace every \"=\" in {_nbt} with \":\"",
            "  add nbt compound from \"%{_nbt}%\" to nbt of {_i}",
            "  return {_i}"
        };


        var pathSk = Path.Combine(serverDir, "Skriptor.sk");
        if (!Directory.Exists(Path.Combine(Main.serverDir, @"Skriptor\")))
            Directory.CreateDirectory(Path.Combine(Main.serverDir, @"Skriptor\"));
        using (StreamWriter sw = File.CreateText(pathSk))
        {
            foreach (var line in baseSkript)
                sw.WriteLine(line);
        }
    }
}
