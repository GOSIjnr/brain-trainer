extends Node

var gamefilePath := "user://UserData.tres"
var fileData: UserData
var isToastActive := false
var selectedGameResource: Games

var scenes := {
	"mainmenu": "res://Scenes/UI/main_menu_ui.tscn",
	"tutorial": "res://Scenes/UI/tutorial.tscn",
	"startup": "res://Scenes/UI/StartUp.tscn",
	"tutorialgame": "res://Scenes/Games/tutorial_game.tscn",
	"tutorialend": "res://Scenes/UI/tutorial_end.tscn",
	"toast": "res://Scenes/UI/toast.tscn",
} 

func _ready() -> void:
	Engine.max_fps = 60
