extends Control

@onready var user_name = %UserName
@onready var workout_counter = %WorkoutCounter
@onready var writing_cb = %WritingCB
@onready var speaking_cb = %SpeakingCB
@onready var reading_cb = %ReadingCB
@onready var maths_cb = %MathsCB
@onready var memory_cb = %MemoryCB

func _ready():
	GlobalRef.fileData = load(GlobalRef.gamefilePath) as UserData
	
	user_name.text = GlobalRef.fileData.UserName
	workout_counter.text = str(GlobalRef.fileData.Workouts)
	
	writing_cb.button_pressed = GlobalRef.fileData.Writing
	speaking_cb.button_pressed = GlobalRef.fileData.Speaking
	reading_cb.button_pressed = GlobalRef.fileData.Reading
	maths_cb.button_pressed = GlobalRef.fileData.Maths
	memory_cb.button_pressed = GlobalRef.fileData.Memory

func _on_check_button_1_toggled(toggled_on):
	GlobalRef.fileData.Writing = toggled_on
	ResourceSaver.save(GlobalRef.fileData, GlobalRef.gamefilePath)

func _on_check_button_2_toggled(toggled_on):
	GlobalRef.fileData.Speaking = toggled_on
	ResourceSaver.save(GlobalRef.fileData, GlobalRef.gamefilePath)

func _on_check_button_3_toggled(toggled_on):
	GlobalRef.fileData.Reading = toggled_on
	ResourceSaver.save(GlobalRef.fileData, GlobalRef.gamefilePath)

func _on_check_button_4_toggled(toggled_on):
	GlobalRef.fileData.Maths = toggled_on
	ResourceSaver.save(GlobalRef.fileData, GlobalRef.gamefilePath)

func _on_check_button_5_toggled(toggled_on):
	GlobalRef.fileData.Memory = toggled_on
	ResourceSaver.save(GlobalRef.fileData, GlobalRef.gamefilePath)

func _on_delete_account_pressed():
	%DeleteAccount.visible = true
