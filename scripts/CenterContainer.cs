using Godot;
using System;

public partial class CenterContainer : Godot.CenterContainer
{
	
	public float DOT_RADIUS = 1.0f;
	public Color DOT_COLOR = new Color(1, 1, 1, 1); // Assuming you want white color with full opacity

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		QueueRedraw();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Draw() // Correct method name
	{
		DrawCircle(new Vector2(0, 0), DOT_RADIUS, DOT_COLOR);
	}
}
