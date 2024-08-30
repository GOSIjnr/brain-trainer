extends MarginContainer

var libary: Array
var libaryPath := "res://Resources/Vocabulary/"

@onready var timer = $Timer
@onready var word = $Panel/VBoxContainer/MarginContainer/VBoxContainer/Label
@onready var meaning = $Panel/VBoxContainer/MarginContainer/VBoxContainer/Label2

func _ready():
	libary = Utils.loadAllResources(libaryPath)
	randomLibary()

func _on_timer_timeout():
	randomLibary()

func randomLibary():
	var selected = libary.pick_random() as Vocabulary
	word.text = selected.word
	meaning.text = selected.meaning
