extends Control

var filePath = "user://UserData.tres"
var fileData: UserData

func _on_yes_pressed():
	fileData = UserData.new()
	ResourceSaver.save(fileData, filePath)
	
	var target_scene = "res://Scenes/StartUp.tscn"
	get_tree().change_scene_to_file(target_scene)

func _on_no_pressed():
	self.visible = false
