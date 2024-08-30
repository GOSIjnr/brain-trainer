extends Control

func _on_panel_gui_input(event: InputEvent) -> void:
	if event is InputEventScreenTouch and event.is_released():
		self.queue_free()

func _notification(what):
	match what:
		NOTIFICATION_WM_GO_BACK_REQUEST:
			self.queue_free()
