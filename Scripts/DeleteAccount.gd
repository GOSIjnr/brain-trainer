extends Control

func _on_yes_pressed():
	GlobalRef.fileData = UserData.new()
	ResourceSaver.save(GlobalRef.fileData, GlobalRef.gamefilePath)
	
	get_tree().change_scene_to_file(GlobalRef.scenes["startup"])

func _on_no_pressed():
	self.visible = false
