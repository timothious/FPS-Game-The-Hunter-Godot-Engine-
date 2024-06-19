class_name OptionsMenu
extends Control

@onready var Exit_Button = $MarginContainer/VBoxContainer/Exit_Button as Button
signal exit_options_menu
func _ready():
	Exit_Button.button_down.connect(on_exit_pressed)
	set_process(false)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
func on_exit_pressed() -> void:
	exit_options_menu.emit()
	set_process(false)
	






