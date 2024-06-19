using Godot;
using System;
using System.Threading.Tasks;

public partial class World : Node3D
{
	private ColorRect hitRect;
	// private Node spawns;
	private NavigationRegion3D navigationRegion;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hitRect = GetNode<ColorRect>("UI/HitRect");
		// spawns = GetNode("Map/Spawns");
		navigationRegion = GetNode<NavigationRegion3D>("Map/NavigationRegion3D");

		//Randomize();

		CharacterBody3D player = GetNode<CharacterBody3D>("Player");  // Replace with the correct path to your player node
		if (player != null)
		{
			player.Connect("PlayerHit", new Callable(this, nameof(_on_player_player_hit)));
		}
		else
		{
			GD.PrintErr("Player node is not assigned or path is incorrect.");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async void _on_player_player_hit()
	{
		if (hitRect != null)
		{
			hitRect.Visible = true;
			await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
			hitRect.Visible = false;
		}
		else
		{
			GD.PrintErr("HitRect node is not assigned or path is incorrect.");
		}
	}

	//private Node GetRandomChild(Node parentNode)
	//{
	//    var randomId = GD.Randi() % (uint)parentNode.GetChildCount();
	//    return parentNode.GetChild((int)randomId);
	//}

	//private void OnZombieSpawnTimerTimeout()
	//{
	//    var spawnPoint = GetRandomChild(spawns).GlobalPosition;
	//    var instance = (Node)zombie.Instance();
	//    instance.Position = spawnPoint;
	//    navigationRegion.AddChild(instance);
	//}
}
