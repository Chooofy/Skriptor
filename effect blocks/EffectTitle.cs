using Godot;
using System;

public partial class EffectTitle : Control, EffectInterface
{
	// Called when the node enters the scene tree for the first time.

	public void ToggleSubtitle(string n)
	{
		if (string.IsNullOrEmpty(n))
			GetNode<CheckButton>("Panel/VBoxContainer/HBoxContainer2/VBoxContainer/CheckButton").ToggleMode = false;
		else
            GetNode<CheckButton>("Panel/VBoxContainer/HBoxContainer2/VBoxContainer/CheckButton").ToggleMode = true;
    }
    Godot.Collections.Dictionary GetInfo()
	{
		Godot.Collections.Dictionary data = new Godot.Collections.Dictionary();
		data.Add("TitleMessage", GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer/Message/LineEdit").Text);
		data.Add("SubtitleMessage", GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer2/Message/LineEdit").Text);
		data.Add("FadeIn", GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer2/FadeIn/LineEdit").Text);
        data.Add("FadeOut", GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer2/FadeOut/LineEdit").Text);
		data.Add("Subtitle", GetNode<CheckButton>("Panel/VBoxContainer/HBoxContainer2/VBoxContainer/CheckButton").ButtonPressed);
        data.Add("Duration", GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer/Duration/LineEdit").Text);
		data.Add("Target", GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer/Target/LineEdit").Text);
		return data;
    }

	public string GetSkript()
	{
		Godot.Collections.Dictionary data = GetInfo();
		string script;

		Variant TitleMessage;
		Variant SubtitleMessage;
		Variant Subtitle;
		Variant Fadein;
		Variant Fadeout;
		Variant Duration;
		Variant Target;

		data.TryGetValue("Target", out Target);
        data.TryGetValue("Subtitle", out Subtitle);
        data.TryGetValue("FadeIn", out Fadein);
        data.TryGetValue("FadeOut", out Fadeout);
        data.TryGetValue("Duration", out Duration);
        data.TryGetValue("TitleMessage", out TitleMessage);
        data.TryGetValue("SubtitleMessage", out SubtitleMessage);

		if (Subtitle.AsBool() == true)
		{
			//send title %string% [with subtitle %string%] [to %players%] [for %timespan%] [with fade[(-| )]in %timespan%] [[and] [with] fade[(-| )]out %timespan%] 
			script = $"send title \"{TitleMessage}\" with subtitle \"{SubtitleMessage}\" to {Target} for {Duration} seconds with fadein {Fadein} seconds and with fadeout {Fadeout} seconds";
        }
		else
		{
            script = $"send title \"{TitleMessage}\" to {Target} for {Duration} seconds with fadein {Fadein} seconds and with fadeout {Fadeout} seconds";
        }
		return script;
    }
}

