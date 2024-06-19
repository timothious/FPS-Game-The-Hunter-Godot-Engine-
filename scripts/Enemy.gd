extends CharacterBody3D

const SPEED = 4.0
const Attack_Range = 2.5
const MAX_HITS = 2


var current_hits = 0
var anim_state_mashine
var player = null
@export var player_path : NodePath


@onready var anim_tree  = $AnimationTree
@onready var nav_agent = $NavigationAgent3D

func _ready():
	# Ensure player node is correctly assigned
	
	player = get_node(player_path)
	if player == null:
		print("Player node is not assigned or path is incorrect.")
		return
	anim_state_mashine = anim_tree.get("parameters/playback")

func _process(delta):
	# Ensure player is not null before accessing its properties
	if player == null:
		return
	
	velocity = Vector3.ZERO
	
	match anim_state_mashine.get_current_node():
		"Run":
			#navigation
			nav_agent.set_target_position(player.global_transform.origin)
			var next_nav_point = nav_agent.get_next_path_position()
			velocity = (next_nav_point - global_transform.origin).normalized() * SPEED
			look_at(Vector3(global_position.x + velocity.x, global_position.y, global_position.z + velocity.z), Vector3.UP)
		
		"Attack":
			look_at(Vector3(player.global_position.x, global_position.y, player.global_position.z), Vector3.UP)
		



	#condition
	anim_tree.set("parameters/conditions/attack", _target_in_range())
	anim_tree.set("parameters/conditions/run", !_target_in_range())
	anim_tree.get("parameters/playback")
	move_and_slide()
	
func _target_in_range():
	return global_position.distance_to(player.global_position) < Attack_Range	
	
func _hit_finished():
	player.hit()
func hit():
	current_hits += 1
	print("Zombie hit count: ", current_hits)
	if current_hits >= MAX_HITS:
		queue_free() # Remove the zombie from the scene tree
