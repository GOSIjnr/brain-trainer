extends Control

@export var help: PackedScene

func _on_help_gui_input(event):
	if event is InputEventScreenTouch and event.is_released():
		var rect = Rect2(Vector2(0, 0), Vector2(100, 100))
		
		if rect.has_point(event.position) and not help == null:
			var scene = help.instantiate()
			get_tree().root.add_child(scene)
