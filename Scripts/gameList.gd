extends Control

var Game :Games:
	set = setGame

var parent :Node

@onready var backGround = %BackGround as TextureRect
@onready var gameIcon = %GameIcon as TextureRect
@onready var gameName = %GameName as Label
@onready var gameType = %GameType as Label
@onready var disabledBG = %Disabled as Panel

func setGame(resource :Games):
	Game = resource
	self.name = resource.gameName
	backGround.texture = resource.gameBackground
	gameIcon.texture = resource.gameIcon
	gameName.text = resource.gameName
	
	var gameRef = ["writing", "speaking", "reading", "maths", "memory"]
	gameType.text = gameRef[resource.gameType]
	
	if not Game.gameScene == null:
		disabledBG.visible = false

func _on_pressed():
	if not Game.gameScene == null:
		parent.showSelectedTab(Game)
