extends Node

var isToastActive: bool = false
var selectedGameResource: Games

func _ready() -> void:
	Engine.max_fps = 60
