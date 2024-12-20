using Godot;
using System;
using System.Collections.Generic;

public partial class ScriptUI : TabContainer
{
    // Called when the node enters the scene tree for the first time.

    public static ScriptUI instance;
    static List<string> effects = new List<string>
    {
        "Potion Effect",
        "Teleport",
        "Function",
        "EffectLocalVar",
        "Title"
    };
    public string block = "acacia button";
    public int type = 1;

    public static void BlockCodes()
    {
        
    }


    public void OnItemSelect(int n)
    {
        List<string> eventListO = eventListItem;
        if (type == 1)
            eventListO = eventList;
        foreach (var item in eventListO)
        {
            TabContainer cont = this;
            if (cont.CurrentTab == eventListO.IndexOf(item))
            {
                var Ref = GetNode<TabBar>($"{item}");
                OptionButton options = Ref.GetNode<OptionButton>("Panel/OptionButton");
                VBoxContainer panel = Ref.GetNode<VBoxContainer>("Panel/ScrollContainer/VBoxContainer");
                Node butt = null;
                PackedScene temp;

                if (options.GetItemText(n) == optionList[0])
                {
                    temp = ResourceLoader.Load<PackedScene>("effect blocks/potion.tscn");
                    butt = temp.Instantiate();
                }
                else if (options.GetItemText(n) == optionList[1])
                {
                    temp = ResourceLoader.Load<PackedScene>("effect blocks/teleport.tscn");
                    butt = temp.Instantiate();
                }
                else if (options.GetItemText(n) == optionList[2])
                {
                    temp = ResourceLoader.Load<PackedScene>("effect blocks/function.tscn");
                    butt = temp.Instantiate();
                    if (type == 2)
                    {
                        butt.Set(EffectFunction.PropertyName.type, 2);
                    }
                }
                else if (options.GetItemText(n) == optionList[3])
                {
                    temp = ResourceLoader.Load<PackedScene>("effect blocks/title.tscn");
                    butt = temp.Instantiate();
                }
                else if (options.GetItemText(n) == optionList[4])
                {
                    temp = ResourceLoader.Load<PackedScene>("effect blocks/EffectLocalVar.tscn");
                    butt = temp.Instantiate();
                }



                var container = new HBoxContainer();
                panel.AddChild(container);

                var scrollbutton = ResourceLoader.Load<PackedScene>("extra/updownbar.tscn");
                var boombayah = scrollbutton.Instantiate();

                var xbutton = ResourceLoader.Load<PackedScene>("extra/xbutton.tscn");
                var butt2 = xbutton.Instantiate();

                butt2.GetChild(0).Connect(Button.SignalName.Pressed, new Callable(butt, Node.MethodName.QueueFree));
                butt2.GetChild(0).Connect(Button.SignalName.Pressed, new Callable(boombayah, Node.MethodName.QueueFree));
                butt2.GetChild(0).Connect(Button.SignalName.Pressed, new Callable(butt2, Node.MethodName.QueueFree));
                container.AddChild(butt2);
                container.AddChild(boombayah);
                container.AddChild(butt);

                    // Figure out how to delete the node.
            }
        }
    }

    public static List<string> eventList = new List<string>
    {
        "On Break",
        "On Place",
        "On Walk On",
        "Right Click",
        "Left Click"
    };

    public static List<string> eventListItem = new List<string>
    {
        "On Drop",
        "Right Click",
        "Left Click"
    };
    public static List<string> optionList = new List<string>
    {
        "Add Potion Effect",
        "Add Teleport Effect",
        "Add Function Call",
        "Add Title Message",
        "Add Local Variable"
    };

    public void AddTabBars()
    {
        List<string> eventListO = eventListItem;
        if (type == 1)
            eventListO = eventList;
        foreach (var x in eventListO)
        {
            var Ref = GetNode<TabBar>($"{x}"); //***
            OptionButton options = Ref.GetNode<OptionButton>("Panel/OptionButton");
            options.Clear();

            foreach (var option in optionList)
            {
                options.AddItem(option);
                options.AddSeparator();
            }
        }
    }

    public List<string> GetCode()
    {
        List<string> scripts = new List<string>();
        List<string> eventListO = eventListItem;
        var itemName = Item.instance.GetNode<LineEdit>("Label3/LineEdit").Text;
        if (type == 1)
            eventListO = eventList;
        foreach (var x in eventListO)
        {
            List<string> subScripts = new List<string>();
            var Ref = GetNode<TabBar>($"{x}"); //***
            var Ref2 = Ref.GetNode<VBoxContainer>("Panel/ScrollContainer/VBoxContainer");
            if (type == 1)
            {
                if (eventListO.IndexOf(x) < 3)
                    if (eventListO.IndexOf(x) < 2)
                        subScripts.Add($"{x.ToLower()} of {block}:");
                    else
                        subScripts.Add($"{x.ToLower()} {block}:");
                else
                    subScripts.Add($"on {x.ToLower()} on {block}:");
            }
            else
            {
                if (eventListO.IndexOf(x) < 1)
                    subScripts.Add($"{x.ToLower()} of {block}:");
                else
                    subScripts.Add($"on {x.ToLower()} on {block}:");
                subScripts.Add(" set {_n}" + $" to getItem(\"{itemName}\")"); //**
                subScripts.Add(" if event-item = {_n}:");
            }
            foreach (var child in Ref2.GetChildren())
            {
                string script = "";
                foreach (var s in effects)
                {
                    try
                    {
                        script = child.GetNodeOrNull<EffectInterface>(s).GetSkript();
                    }
                    catch
                    {
                        continue;
                    }
                }
                if (script != null && script != "")
                {
                    subScripts.Add("  " + script);
                }
            }
            if (type == 1)
            {
                if (subScripts.Count > 1)
                {
                    foreach (var line in subScripts)
                    {
                        scripts.Add(line);
                    }
                    scripts.Add(" ");
                }
                else
                {
                    subScripts.Clear();
                }
            } else
            {
                if (subScripts.Count > 3)
                {
                    foreach (var line in subScripts)
                    {
                        scripts.Add(line);
                    }
                    scripts.Add(" ");
                }
                else
                {
                    subScripts.Clear();
                }
            }
        }
        return scripts;
    }
}
