extends Control


# Called when the node enters the scene tree for the first time.
@onready var Start_button = $VBoxContainer/StartButton as Button
@onready var quit_button  = $VBoxContainer/QuitButton as Button
@onready var options_button  = $VBoxContainer/OptionsButton as Button
@onready var option_menu = $Options_Menu as OptionsMenu
@onready var vbox = $VBoxContainer as VBoxContainer
@onready var hit_sound = $HitSound as AudioStreamPlayer
@onready var button_pressed_sound = $ButtonPressedStreamPlayer as AudioStreamPlayer
@onready var game_Scene = preload("res://scenes/level1.tscn") as PackedScene



func _ready():
	$VBoxContainer/StartButton.grab_focus()
	handle_signals()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_start_button_pressed():
	get_tree().change_scene_to_packed(game_Scene)
	
func on_exit_pressed() -> void:
	button_pressed_sound.play()
	get_tree().quit()

func on_option_pressed()-> void:
	button_pressed_sound.play()
	vbox.visible = false
	option_menu.set_process(true)
	option_menu.visible = true

func on_exit_options_menu() -> void:
	vbox.visible = true
	option_menu.visible = false
	

func handle_signals() -> void:
	
	quit_button.button_down.connect(on_exit_pressed)
	options_button.button_down.connect(on_option_pressed)
	option_menu.exit_options_menu.connect(on_exit_options_menu)
	


func _on_start_button_mouse_entered():
	hit_sound.play()

func _on_options_button_mouse_entered():
	hit_sound.play()
	
func _on_quit_button_mouse_entered():
	hit_sound.play()
