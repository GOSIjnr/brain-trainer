extends Control

@onready var user_name = %UserName
@onready var workout_counter = %WorkoutCounter
@onready var writing_cb = %WritingCB
@onready var speaking_cb = %SpeakingCB
@onready var reading_cb = %ReadingCB
@onready var maths_cb = %MathsCB
@onready var memory_cb = %MemoryCB

var filePath = "user://UserData.tres"
var fileData: UserData

func _ready():
	fileData = load(filePath) as UserData
	
	user_name.text = fileData.UserName
	workout_counter.text = str(fileData.Workouts)
	
	writing_cb.button_pressed = fileData.Writing
	speaking_cb.button_pressed = fileData.Speaking
	reading_cb.button_pressed = fileData.Reading
	maths_cb.button_pressed = fileData.Maths
	memory_cb.button_pressed = fileData.Memory

func _on_check_button_1_toggled(toggled_on):
	fileData.Writing = toggled_on
	ResourceSaver.save(fileData, filePath)

func _on_check_button_2_toggled(toggled_on):
	fileData.Speaking = toggled_on
	ResourceSaver.save(fileData, filePath)

func _on_check_button_3_toggled(toggled_on):
	fileData.Reading = toggled_on
	ResourceSaver.save(fileData, filePath)

func _on_check_button_4_toggled(toggled_on):
	fileData.Maths = toggled_on
	ResourceSaver.save(fileData, filePath)

func _on_check_button_5_toggled(toggled_on):
	fileData.Memory = toggled_on
	ResourceSaver.save(fileData, filePath)

func _on_delete_account_pressed():
	%DeleteAccount.visible = true
