using Godot;
using System;

public partial class Bullet : Node3D
{
	[Export] public float speed = 400.0f; // Speed of the bullet
	private RayCast3D ray;
	private MeshInstance3D mesh;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ray = GetNode<RayCast3D>("RayCast3D");
		mesh = GetNode<MeshInstance3D>("MeshInstance3D");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Translate(Vector3.Forward * speed * (float)delta);
		if (ray.IsColliding())
		{
			// Handle collision logic here, such as applying damage
			GD.Print("Bullet hit: ", ray.GetCollider());
			

			// Check if the collider is a zombie and call its hit method
			var collider = ray.GetCollider() as Node;
			if (collider != null && collider.HasMethod("hit"))
			{
				collider.Call("hit");
			}

			// Optionally, you might want to remove the bullet after it hits something
			QueueFree();
		}
	}
	
	

}
