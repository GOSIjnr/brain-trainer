extends Control

func _on_recommended_button_pressed():
	Utils.showToast(get_tree().root, "No recommendation found", 1.5)
