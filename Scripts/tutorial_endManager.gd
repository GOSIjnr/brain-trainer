extends Control

@onready var page1ProgressBar = $page1/MarginContainer/VBoxContainer/TextureProgressBar
@onready var page1Timer = $page1/page1Timer
var userName :String

func _ready():
	page1ProgressBar.max_value = page1Timer.wait_time

func _process(_delta):
	if $page1.visible == true:
		updateBar()
	
	if userName.is_empty():
		$page4/MarginContainer2/Button.disabled = true
	else:
		$page4/MarginContainer2/Button.disabled = false

#page1
func updateBar():
	page1ProgressBar.value = page1Timer.wait_time - page1Timer.time_left

func _on_page1Timer_timeout():
	if $page1.visible == true:
		$page1.visible = false
		$page2.visible = true
		$page1/sfx_whoosh.play()

#page2
func _on_page2Button_pressed():
	$page2.visible = false
	$page3.visible = true

#page3
func _on_page3Button_pressed():
	$page3.visible = false
	$page4.visible = true

#page4
func _on_page4Button_pressed():
	var filePath = "user://UserData.tres"
	var fileData: UserData
	
	fileData = load(filePath) as UserData
	
	fileData.UserName = userName
	fileData.isTutorialDone = true
	
	ResourceSaver.save(fileData, filePath)
	
	var target_scene = "res://Scenes/main_menu_ui.tscn"
	get_tree().change_scene_to_file(target_scene)

func _on_line_edit_text_changed(new_text):
	userName = new_text.strip_edges()
