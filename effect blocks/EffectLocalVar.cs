using Godot;
using System;

public partial class EffectLocalVar : Control, EffectInterface
{
	// Called when the node enters the scene tree for the first time.
	
	public string GetSkript()
	{
		return $"set {GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer/Var").Text} to {GetNode<LineEdit>("Panel/VBoxContainer/HBoxContainer/Value").Text}";
	}
}
