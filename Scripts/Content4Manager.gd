extends Control

@onready var user_name = %UserName
@onready var workout_counter = %WorkoutCounter
@onready var writing_cb = %WritingCB
@onready var speaking_cb = %SpeakingCB
@onready var reading_cb = %ReadingCB
@onready var maths_cb = %MathsCB
@onready var memory_cb = %MemoryCB

@export var deleteAccountScene: PackedScene

func _ready():
	SaveManager.loadData()
	
	user_name.text = SaveManager.fileData.UserName
	workout_counter.text = str(SaveManager.fileData.Workouts)
	
	writing_cb.button_pressed = SaveManager.fileData.Writing
	speaking_cb.button_pressed = SaveManager.fileData.Speaking
	reading_cb.button_pressed = SaveManager.fileData.Reading
	maths_cb.button_pressed = SaveManager.fileData.Maths
	memory_cb.button_pressed = SaveManager.fileData.Memory

func _on_check_button_1_toggled(toggled_on):
	SaveManager.fileData.Writing = toggled_on
	SaveManager.saveData()

func _on_check_button_2_toggled(toggled_on):
	SaveManager.fileData.Speaking = toggled_on
	SaveManager.saveData()

func _on_check_button_3_toggled(toggled_on):
	SaveManager.fileData.Reading = toggled_on
	SaveManager.saveData()

func _on_check_button_4_toggled(toggled_on):
	SaveManager.fileData.Maths = toggled_on
	SaveManager.saveData()

func _on_check_button_5_toggled(toggled_on):
	SaveManager.fileData.Memory = toggled_on
	SaveManager.saveData()

func _on_delete_account_pressed():
	var scene = deleteAccountScene.instantiate()
	get_tree().root.add_child(scene)
