extends Control

func _on_recommended_button_pressed():
	Helper.showToast(get_tree().root, "No recommendation found", 1.5)
