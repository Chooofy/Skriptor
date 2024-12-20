using Godot;
using System;
using System.ComponentModel;

public partial class EffectFunction : Control, EffectInterface
{
	// Called when the node enters the scene tree for the first time.
	[Export] public int type { get; set; } = 1;

	Godot.Collections.Dictionary GetInfo()
	{
		Godot.Collections.Dictionary data = new Godot.Collections.Dictionary();
		data.Add("Function", GetNode<CodeEdit>("Panel/VBoxContainer/CodeEdit").Text);
		return data;
	}

	public string GetSkript()
	{
        Godot.Collections.Dictionary data = GetInfo();
		Variant temp;
		data.TryGetValue("Function", out temp);
		if (type == 2)
		{
			var newTemp = Convert.ToString(temp);
			var spliced = newTemp.Split("\n");
			string newString = "";
			foreach (var s in spliced)
			{
				newString += $"{s}\n  ";
			}

			return newString;
		}
        else
        {
            var newTemp = Convert.ToString(temp);
            var spliced = newTemp.Split("\n");
            string newString = "";
            foreach (var s in spliced)
            {
                newString += $"{s}\n  ";
            }

            return newString;
        }
    }
}
