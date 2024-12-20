using Godot;
using System.Collections.Generic;


public partial class NBTParser : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Process(double delta)
    {
        //var str = Item.itemNBT;
        //PrintDictionary(ReadNBT(str));
    }


    Dictionary<string, object> ReadNBT(string nbt)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        nbt = nbt.Remove(0, 1);
        nbt = nbt.Remove(nbt.Length - 1, 1);
        nbt.ReplaceN("minecraft:", "");
        nbt = nbt.Replace("[", "");
        nbt = nbt.Replace("]", "");
        nbt = nbt.Replace('=', ':');
        nbt = nbt.Replace(" ", "");
        nbt = nbt.Replace('"'.ToString(), "");

        List<string> items = NBTSplit(nbt);
        foreach (var i in items)
        {
            string[] a = i.Split(':', 2);
            var value = a[1];
            if (value[0] == '{')
            {
                if (value[1] == '}')
                {
                    data.Add(a[0], "true");
                }
                else if (value[1] != '}')
                    data.Add(a[0], ReadNBT(a[1]));
            }
            else
            {
                int n = 1;
                var boo = data.TryAdd(a[0], a[1]);
                while (boo == false)
                    n++;
                    boo = data.TryAdd(a[0]+n.ToString(), a[1]);

            }
        }
        return data;
    }

    void PrintDictionary(Dictionary<string, object> data)
    {
        foreach (KeyValuePair<string, object> kvp in data)
        {
            if (kvp.Value.GetType() != typeof(Dictionary<string, object>))
            {
                GD.Print(kvp.Key.ToString() + ":" + kvp.Value.ToString());
            }
            else if (kvp.Value.GetType() == typeof(Dictionary<string, object>))
            {
                GD.Print(kvp.Key.ToString() + ":" + "{Dictionary}");
                PrintDictionary((Dictionary<string, object>)kvp.Value);
            }
        }
    }

    List<string> NBTSplit(string nbt)
    {

        List<string> list = new List<string>();
        List<string> splitString = new List<string>();

        string temp = nbt;
        int indentation = 0;
        int characterCount = 0;

        foreach (char c in temp)
        {
            if (c == '{')
                indentation++;
            else if (c == '}')
                indentation--;
            else if (c == ',' && indentation == 0)
            {
                splitString = SplitAt(temp, temp.IndexOf(c));
                temp = splitString[1];
                list.Add(splitString[0]);
                characterCount = 0;

            }
            characterCount++;
        }
        list.Add(splitString[1]);

        GD.Print(" ");
        foreach (var i in list)
            GD.Print(i);
        GD.Print(" ");

        return list;
    }

    public static List<string> SplitAt(string str, int index)
    {

        string one = "";
        string two = "";
        int count = 0;

        foreach (char c in str)
        {
            if (count < index)
                one += c;
            else if (count > index)
                two += c;
            count++;
        }
        List<string> ret = new List<string>();
        ret.Add(one);
        ret.Add(two);
        return ret;
    }
}
