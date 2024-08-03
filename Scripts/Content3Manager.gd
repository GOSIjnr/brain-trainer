extends Control

var gameLocation := "res://Resources/Games/"
var gameLibary :Array
var gamePlaceHolder = load("res://Scenes/UI/gameList.tscn") as PackedScene

@onready var gameBar = %GameBar
@onready var filterGroup = %Filter
@onready var selectedGameTab = %SelectedGame

func _ready():
	gameLibary = Helper.loadAllResources(gameLocation)
	
	for button in filterGroup.get_children():
		if button is Button:
			button.pressed.connect(_on_button_pressed.bind(button, _positions(button)))
	
	var startTab := filterGroup.get_child(0)
	_on_button_pressed(startTab, 0)

func _resetLibary():
	for content in gameBar.get_children():
		content.queue_free()

func _on_button_pressed(button :Button, index :int):
	_resetLibary()
	
	if not button.button_pressed:
		button.button_pressed = true
	
	for game in gameLibary.size():
		if index == 0:
			var gameScene = gamePlaceHolder.instantiate()
			gameBar.add_child(gameScene)
			gameScene.Game = gameLibary[game]
			gameScene.parent = self
		else:
			if gameLibary[game].gameType == index - 1:
				var gameScene = gamePlaceHolder.instantiate()
				gameBar.add_child(gameScene)
				gameScene.Game = gameLibary[game]
				gameScene.parent = self

func _positions(button :Button):
	return button.get_index()

func showSelectedTab(Game :Games):
	selectedGameTab.visible = true
	selectedGameTab.selected = Game
