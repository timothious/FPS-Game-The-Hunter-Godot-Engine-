using Godot;
using System;

public partial class LevelComplete : Area3D
{
	private PackedScene _nextScene;

	public override void _Ready()
	{
		// Preload the next scene when the current scene is ready
		_nextScene = (PackedScene)ResourceLoader.Load("res://scenes/level2.tscn");
	}

	public void _on_body_entered(Node3D body)
	{
		// Change to the preloaded scene on collision
		ChangeScene();
	}

	private void ChangeScene()
	{
		if (_nextScene != null)
		{
			// Instantiate the preloaded scene and replace the current scene
			GetTree().ChangeSceneToPacked(_nextScene);
		}
	}
}
