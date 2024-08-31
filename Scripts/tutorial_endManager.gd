extends Control

@onready var page1ProgressBar: TextureProgressBar = %ProgressBar
@onready var page1Timer: Timer = %page1Timer

var userName: String

func _ready():
	get_tree().quit_on_go_back = false
	page1ProgressBar.max_value = page1Timer.wait_time
	SaveManager.loadData()

func _process(_delta):
	if $page1.visible == true:
		updateBar()
	
	if $page4.visible == true:
		if userName.is_empty():
			$page4/MarginContainer2/Button.disabled = true
		else:
			$page4/MarginContainer2/Button.disabled = false

#page1
func updateBar():
	page1ProgressBar.value = page1Timer.wait_time - page1Timer.time_left

func _on_page1Timer_timeout():
	if $page1.visible == true:
		$page1.hide()
		$page2.show()
		$page1/sfx_whoosh.play()

#page2
func _on_page2Button_pressed():
	$page2.hide()
	$page3.show()

#page3
func _on_page3Button_pressed():
	$page3.hide()
	$page4.show()

#page4
func _on_page4Button_pressed():
	SaveManager.fileData.UserName = userName
	SaveManager.fileData.isTutorialDone = true
	
	SaveManager.saveData()
	get_tree().change_scene_to_packed(SceneLoader.get_resource("main_menu"))

func _on_line_edit_text_changed(new_text):
	userName = new_text.strip_edges()

func _notification(what):
	if what == NOTIFICATION_WM_GO_BACK_REQUEST:
		go_back_request()

func go_back_request():
	if $page1.visible == true or $page2.visible == true:
		Utils.showToast(get_tree().root, "Can't go back at this stage", 1.5)
		return
	
	if $page3.visible == true:
		$page3.hide()
		$page2.show()
		return
	
	if $page4.visible == true:
		$page4.hide()
		$page3.show()
		return
