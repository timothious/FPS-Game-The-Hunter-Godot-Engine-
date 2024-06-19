using Godot;
using System;

public partial class Zombie : CharacterBody3D
{
	private const int MaxHits = 2; // Maximum number of hits the zombie can take
	private int currentHits = 0; // Current number of hits taken

	private AnimationTree anim_tree;
	private NavigationAgent3D nav_agent;
	private CharacterBody3D player;

	[Export] public NodePath playerPath;

	private AnimationNodeStateMachinePlayback anim_state_mashine;

	private const float Speed = 4.0f;
	private const float AttackRange = 2.5f;

	public override void _Ready()
	{
		player = GetNode<CharacterBody3D>(playerPath);
		if (player == null)
		{
			GD.Print("Player node is not assigned or path is incorrect.");
			return;
		}
		anim_tree = GetNode<AnimationTree>("AnimationTree");
		nav_agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		anim_state_mashine = (AnimationNodeStateMachinePlayback)anim_tree.Get("parameters/playback");
	}

	public override void _Process(double delta)
	{
		if (player == null) return;

		Vector3 velocity = Vector3.Zero;

		switch (anim_state_mashine.GetCurrentNode())
		{
			case "Run":
				nav_agent.TargetPosition = player.GlobalTransform.Origin;
				Vector3 next_nav_point = nav_agent.GetNextPathPosition();
				velocity = (next_nav_point - GlobalTransform.Origin).Normalized() * Speed;
				LookAt(new Vector3(GlobalPosition.X + velocity.X, GlobalPosition.Y, GlobalPosition.Z + velocity.Z), Vector3.Up);
				break;

			case "Attack":
				LookAt(new Vector3(player.GlobalPosition.X, GlobalPosition.Y, player.GlobalPosition.Z), Vector3.Up);
				break;
		}

		anim_tree.Set("parameters/conditions/attack", TargetInRange());
		anim_tree.Set("parameters/conditions/run", !TargetInRange());
		MoveAndSlide();
	}

	private bool TargetInRange()
	{
		return GlobalPosition.DistanceTo(player.GlobalPosition) < AttackRange;
	}

	private void HitFinished()
	{
		player.Call("hit");
	}

	// Method to be called when the zombie is hit by a bullet
	public void Hit()
	{
		currentHits++;
		GD.Print("Zombie hit count: ", currentHits);
		if (currentHits >= MaxHits)
		{
			Hide(); // Hide the zombie after max hits
			QueueFree(); // Optionally, you can also remove the node from the scene tree
		}
	}
}
