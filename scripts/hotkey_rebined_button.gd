class_name HotkeyRebindButton
extends Control

@onready var label = $HBoxContainer/Label as Label
@onready var button = $HBoxContainer/Button as Button

@export var action_name : String = "move_forward"

# Called when the node enters the scene tree for the first time.
func _ready():
	set_process_unhandled_key_input(false)
	set_action_name()
	set_text_for_key()

func set_action_name() -> void:
	label.text = "Unassigned"
	match action_name:
		"move_forward":
			label.text = "Move Forward"
		"move_back":
			label.text = "Move Back"
		"jump":
			label.text = "Jump"
		"move_left":
			label.text = "Move Left"
		"move_right":
			label.text = "Move Right"
		"gun_shoot":
			label.text = "Gun Fire"
		"use":
			label.text = "Pull out & Pull away Gun"

func set_text_for_key() -> void:
	var actions_events = InputMap.action_get_events(action_name)
	if actions_events.size() > 0:
		var action_event = actions_events[0]
		if action_event is InputEventKey:
			var action_keycode = OS.get_keycode_string(action_event.physical_keycode)
			button.text = "%s" % action_keycode
		elif action_event is InputEventMouseButton:
			var button_index = action_event.button_index
			if button_index == 1:
				var button_name = "Left Mouse Button"
				button.text = button_name
		else:
			button.text = "Unknown Input"
	else:
		button.text = "No events assigned to the action."

