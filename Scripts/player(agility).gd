extends CharacterBody2D

var gravity: Vector2 = Vector2(0, 30)

const SPEED = 300.0
const JUMP_VELOCITY = -400

func _physics_process(delta: float) -> void:
	# Add the gravity.
	if not is_on_floor():
		velocity += gravity * delta
	
	move_and_slide()

func _on_game_ui_boost_space_ship() -> void:
	jump()

func jump():
	# Handle jump.
	velocity.y = JUMP_VELOCITY
	
	var tween1 = create_tween()
	tween1.tween_property($"../CPUParticles2D", "gravity", Vector2(0, 500), 0.5)
	await get_tree().create_timer(0.5).timeout
	var tween2 = create_tween()
	tween2.tween_property($"../CPUParticles2D", "gravity", Vector2(0, 90), 1)
