extends Node3D

@onready var hit_rect = $UI/HitRect
#@onready var spawns = $Map/Spawns
@onready var navigation_region = $Map/NavigationRegion3D

@onready var health_bar = $UI/ProgressBar

@onready var menu_scene = preload("res://scenes/menu.tscn") as PackedScene
var health = 100

# Called when the node enters the scene tree for the first time.
func _ready():
	randomize()
	health_bar.value = 100

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func _on_player_player_hit():
	hit_rect.visible = true
	await get_tree().create_timer(0.2).timeout
	hit_rect.visible = false
	
	# Decrease health by 10 on hit
	health -= 5
	update_health_bar()

func _get_random_child(parent_node):
	var random_id = randi() % parent_node.get_child_count()
	return parent_node.get_child(random_id)

func update_health_bar():
	# Ensure health doesn't go below 0
	health = max(0, health)
	health_bar.value = health

	if health <= 0:
		game_over()

func game_over():
	get_tree().change_scene_to_packed(menu_scene)
