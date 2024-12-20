using Godot;
using System;

public partial class updownbar : Control
{
	// Called when the node enters the scene tree for the first time.
	Node parent;
	
	public void Up()
	{
		var i = GetParent().GetIndex();
		if (i != 0)
			GetParent().GetParent().MoveChild(GetParent(), i-1);
	}
	public void Down()
	{
        var i = GetParent().GetIndex();
        if (i != GetParent().GetParent().GetChildCount()-1)
            GetParent().GetParent().MoveChild(GetParent(), i+1);
    }
}
