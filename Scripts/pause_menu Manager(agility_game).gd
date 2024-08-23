extends Control

func _ready() -> void:
	self.visibility_changed.connect(pauseScene)

func pauseScene():
	if self.visible == true:
		get_tree().paused = true
