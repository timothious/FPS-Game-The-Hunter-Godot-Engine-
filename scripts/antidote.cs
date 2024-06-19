using Godot;
using System;

public partial class antidote : Area3D
{
	 private PackedScene _completeScene;

	public override void _Ready()
	{
		_completeScene = (PackedScene)ResourceLoader.Load("res://scenes/win.tscn");
	}


	private void _on_body_entered(Node3D body)
	{
		ChangeScene();
	}
	
	private void ChangeScene()
	{
		if (_completeScene != null)
		{
			// Instantiate the preloaded scene and replace the current scene
			GetTree().ChangeSceneToPacked(_completeScene);
		}
	}
}









