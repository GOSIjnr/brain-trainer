extends MarginContainer

var libary: Array
var libaryPath := "res://Resources/Vocabulary/"

@onready var timer = $Timer
@onready var word = $Panel/VBoxContainer/MarginContainer/VBoxContainer/Label
@onready var meaning = $Panel/VBoxContainer/MarginContainer/VBoxContainer/Label2

func _ready():
	libary = Helper.loadAllResources(libaryPath)
	randomLibary()

func _on_timer_timeout():
	randomLibary()

func randomLibary():
	var selected = libary[randi_range(0, libary.size()) - 1] as Vocabulary
	word.text = selected.word
	meaning.text = selected.meaning
