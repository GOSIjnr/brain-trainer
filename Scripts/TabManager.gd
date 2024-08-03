extends Panel

@onready var TabGroup = $MarginContainer/HBoxContainer
@onready var ContentGroup = $"../Contents"

@export var TabColor_Selected : Color
@export var TabColor_Deselected : Color

func _ready():
	for tab in TabGroup.get_children():
		for button in tab.get_children():
			if button is TextureButton:
				button.pressed.connect(_on_button_pressed.bind(button, _positions(button)))
	
	var startTab := TabGroup.get_child(0).get_child(0)
	_on_button_pressed(startTab, 0)

func _positions(button :TextureButton):
	return button.get_parent().get_index()

func _resetTabs():
	for content in ContentGroup.get_children():
		if content is Control:
			content.hide()
	
	for tab in TabGroup.get_children():
		tab.modulate = TabColor_Deselected

func _on_button_pressed(button :TextureButton, index :int):
	_resetTabs()
	
	if not button.button_pressed:
		button.button_pressed = true
	
	button.get_parent().modulate = TabColor_Selected
	ContentGroup.get_child(index).show()
