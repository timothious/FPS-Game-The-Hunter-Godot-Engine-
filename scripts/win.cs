using Godot;
using System;

public partial class win : Control
{
	private Control OptionMenu;
	private VBoxContainer VBox;
	private AudioStreamPlayer HitSound;
	private AudioStreamPlayer buttonpressed;
	private Button StartButton;
	private Button OptionsButton;
	private Button QuitButton;
	private Label Win_label;
	private Label Wait_label;
	private Button[] buttons;
	private int currentButtonIndex = 0;

	public override void _Ready()
	{
		HitSound = GetNode<AudioStreamPlayer>("HitSound");
		buttonpressed = GetNode<AudioStreamPlayer>("ButtonPressedStreamPlayer");
		OptionMenu = GetNode<Control>("Options_Menu");
		VBox = GetNode<VBoxContainer>("VBoxContainer");
		Win_label = GetNode<Label>("Label");
		Wait_label = GetNode<Label>("wait");
		
		StartButton = GetNode<Button>("VBoxContainer/StartButton");
		OptionsButton = GetNode<Button>("VBoxContainer/OptionsButton");
		QuitButton = GetNode<Button>("VBoxContainer/QuitButton");

		buttons = new Button[] { StartButton, OptionsButton, QuitButton };

		StartButton.GrabFocus(); // Set initial focus
	}

	public override void _Input(InputEvent @event)
	{
		HitSound.Play();
		if (@event is InputEventKey eventKey && eventKey.Pressed)
		
		{
			if (eventKey.Keycode == Key.Up)
			{
				MoveFocus(-1);
			}
			else if (eventKey.Keycode == Key.Down)
			{
				MoveFocus(1);
			}
		}
	}

	private void MoveFocus(int direction)
	{
		currentButtonIndex = (currentButtonIndex + direction + buttons.Length) % buttons.Length;
		buttons[currentButtonIndex].GrabFocus();
	}

	private void _on_start_button_button_down()
	{
		ButtonPressed();
		GetTree().ChangeSceneToFile("res://scenes/level1.tscn");
	}
	private void _on_options_button_button_down()
	{
		ButtonPressed();
		VBox.Visible = false;
		Win_label.Visible = false;
		Wait_label.Visible = true;
		GetTree().ChangeSceneToFile("res://scenes/menu.tscn");
	}
	private void _on_quit_button_button_down()
	{
		ButtonPressed();
		GetTree().Quit();
	}
	

	private void mouse_entered()
	{
		HitSound.Play();
	}
	private void ButtonPressed()
	{
		buttonpressed.Play();
	}
}
