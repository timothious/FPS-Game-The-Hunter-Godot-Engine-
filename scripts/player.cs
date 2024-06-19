using Godot;
using System;

public partial class player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	// Mouse look variables
	private const float MouseSensitivity = 0.01f;
	private float _rotationX;
	private float _rotationY;

	private AnimationPlayer animat_player;

	// Track the state of the animations
	private bool isPutAway = true;
	private bool canToggleAnimation = true;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	// Bullet scene
	[Export] public PackedScene bulletScene;

	private AudioStreamPlayer shoot_audio;

	private Node3D muzzle_flash;

	// Gun RayCast
	private RayCast3D gun_ray;
	
	
	
	//Playe Hit
	// Player Hit
	[Signal] public delegate void PlayerHitEventHandler();

	

	// Health Bar
	private ProgressBar health_label_bar;
	
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Hidden;
		animat_player = GetNode<AnimationPlayer>("CameraComponent/Camera3D/Rifile/AnimationPlayer");
		animat_player.Play("put_away");

		// Load the bullet scene
		bulletScene = (PackedScene)ResourceLoader.Load("res://scenes/bullet.tscn");

		gun_ray = GetNode<RayCast3D>("CameraComponent/Camera3D/RayCast3D");

		shoot_audio = GetNode<AudioStreamPlayer>("CameraComponent/Camera3D/Rifile/Shoot");

		muzzle_flash = GetNode<Node3D>("CameraComponent/Camera3D/Rifile/MuzzleFlash");
		muzzle_flash.Visible = false; // Ensure the muzzle flash is hidden initially
		

		// Connect the animation finished signal to a function
		animat_player.AnimationFinished += OnAnimationFinished;
		

	}


	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();

		// Prevent shooting while the put_away animation is playing
		if (Input.IsActionPressed("gun_shoot") && (!animat_player.IsPlaying() || animat_player.CurrentAnimation != "put_away"))
		{
			shoot_audio.Play();
			Shoot();
		}

		if (Input.IsActionJustPressed("use") && canToggleAnimation)
		{
			if (isPutAway)
			{
				animat_player.Play("pull_up");
			}
			else
			{
				animat_player.Play("put_away");
			}
			isPutAway = !isPutAway;
			canToggleAnimation = false; // Disable toggling until the current animation is finished
		}
	}

	private void OnAnimationFinished(StringName animName)
	{
		// Re-enable toggling when the animation is finished
		canToggleAnimation = true;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// Handle mouse look
		if (@event is InputEventMouseMotion mouseMotion)
		{
			_rotationX -= mouseMotion.Relative.Y * MouseSensitivity;
			_rotationY -= mouseMotion.Relative.X * MouseSensitivity;
			Rotation = new Vector3(_rotationX, _rotationY, 0);
		}

		// Close the window if the "Esc" key is pressed
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.Escape)
		{
			GetTree().Quit();
		}
	}

	private void Shoot()
	{
		if (bulletScene != null)
		{
			// Instantiate the bullet scene
			Bullet bulletInstance = (Bullet)bulletScene.Instantiate();

			// Set the bullet's transform to match the RayCast's global transform
			bulletInstance.GlobalTransform = gun_ray.GlobalTransform;

			// Add the bullet to the scene
			GetParent().AddChild(bulletInstance);
			GD.Print("Gun Shoot");

			// Play shoot animation
			animat_player.Play("shoot");

			// Show the muzzle flash
			ShowMuzzleFlash();
		}
		else
		{
			GD.PrintErr("Bullet scene is not loaded!");
		}
	}


	private void ShowMuzzleFlash()
	{
		if (muzzle_flash != null)
		{
			muzzle_flash.Visible = true;

			// Optionally, you can hide the muzzle flash after some time to stop the effect.
			// You might want to use a Timer node or an animation callback for this.
			// Here's an example using a Timer:
			Timer timer = new Timer();
			timer.WaitTime = 0.1f; // Adjust this time as necessary
			timer.OneShot = true;
			timer.Timeout += () => muzzle_flash.Visible = false;
			muzzle_flash.AddChild(timer);
			timer.Start();
		}
		else
		{
			GD.PrintErr("Muzzle flash node is not found!");
		}
	}
	private void hit()
	{
		EmitSignal("PlayerHit");

	}
}



