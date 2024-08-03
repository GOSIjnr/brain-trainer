extends Control

func _on_help_gui_input(event):
	if event is InputEventScreenTouch and event.pressed:
		%PQhelp.visible = true

func _on_pqhelp_gui_input(event):
	if event is InputEventScreenTouch and event.pressed:
		%PQhelp.visible = false
