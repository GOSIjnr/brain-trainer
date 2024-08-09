extends Control

func _ready():
	get_tree().quit_on_go_back = false

func _notification(what):
	if what == NOTIFICATION_WM_GO_BACK_REQUEST:
		go_back_request()

func go_back_request():
	if %DeleteAccount.visible == true:
		%DeleteAccount.visible = false
		return
	
	if %PQhelp.visible == true:
		%PQhelp.visible = false
		return
	
	if %SelectedGame.visible == true:
		if %SelectedGame.get_node("Help").visible == false:
			%SelectedGame.visible = false
		else:
			%SelectedGame.get_node("Help").visible = false
		return
	
	if $VBoxContainer/Contents.get_child(0).visible == true:
		get_tree().quit()
		return
	elif %PQhelp.visible == false:
		var TabGroup = $VBoxContainer/Tabs/MarginContainer/HBoxContainer
		var startTab := TabGroup.get_child(0).get_child(0)
		$VBoxContainer/Tabs._on_button_pressed(startTab, 0)
		return
	
