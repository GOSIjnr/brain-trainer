extends MarginContainer

@onready var back_ground: Panel = %BackGround
@onready var progress_low: TextureProgressBar = %progressLow
@onready var progress_starting: TextureProgressBar = %progressStarting
@onready var progress_high: TextureProgressBar = %progressHigh
@onready var starting_pq: Label = %StartingPQ
@onready var current_pq: Label = %CurrentPQ
@onready var proficiency: Label = %Proficiency
@onready var content_label: Label = %ContentLabel
@onready var growth: Label = %Growth

@export var contentColor: Color

enum Type {writing, speaking, reading, maths, memory}
@export var contentType: Type

func _ready():
	SaveManager.loadData()
	var current_date: Dictionary = Time.get_datetime_dict_from_system()
	var date = "%d-%d-%d" % [current_date.year, current_date.month, current_date.day]
	print(date + " : " + str(SaveManager.fileData.WritingEPQ))
	
	updateColors()
	match contentType:
		Type.writing:
			content_label.text = "writing pq"
			proficiency.text = getTitle(SaveManager.fileData.WritingEPQ)
			current_pq.text = str(SaveManager.fileData.WritingEPQ)
			starting_pq.text = str(SaveManager.fileData.starting_WritingEPQ)
			updateBar(int(current_pq.text), int(starting_pq.text))
		Type.speaking:
			content_label.text = "speaking pq"
			proficiency.text = getTitle(SaveManager.fileData.SpeakingEPQ)
			current_pq.text = str(SaveManager.fileData.SpeakingEPQ)
			starting_pq.text = str(SaveManager.fileData.starting_SpeakingEPQ)
			updateBar(int(current_pq.text), int(starting_pq.text))
		Type.reading:
			content_label.text = "reading pq"
			proficiency.text = getTitle(SaveManager.fileData.ReadingEPQ)
			current_pq.text = str(SaveManager.fileData.ReadingEPQ)
			starting_pq.text = str(SaveManager.fileData.starting_ReadingEPQ)
			updateBar(int(current_pq.text), int(starting_pq.text))
		Type.maths:
			content_label.text = "maths pq"
			proficiency.text = getTitle(SaveManager.fileData.MathsEPQ)
			current_pq.text = str(SaveManager.fileData.MathsEPQ)
			starting_pq.text = str(SaveManager.fileData.starting_MathsEPQ)
			updateBar(int(current_pq.text), int(starting_pq.text))
		Type.memory:
			content_label.text = "memory pq"
			proficiency.text = getTitle(SaveManager.fileData.MemoryEPQ)
			current_pq.text = str(SaveManager.fileData.MemoryEPQ)
			starting_pq.text = str(SaveManager.fileData.starting_MemoryEPQ)
			updateBar(int(current_pq.text), int(starting_pq.text))
	
	growth.text = updateGrowth(int(current_pq.text), int(starting_pq.text))

func updateColors():
	back_ground.self_modulate = contentColor.lightened(0.4)
	progress_high.tint_progress = contentColor
	progress_starting.tint_progress = contentColor.darkened(0.2)
	progress_low.tint_progress = contentColor.darkened(0.4)

func getTitle(value: int):
	if value >= 4750:
		return "Master"
	elif value >= 4250:
		return "Elite"
	elif value >= 3750:
		return "Expert"
	elif value >= 2500:
		return "Advanced"
	elif value >= 1250:
		return "Intermediate"
	else:
		return "Novice"

func updateBar(current: int, starting: int):
	progress_starting.value = starting
	
	if current >= starting:
		progress_high.value = current
		progress_low.value = 0
	else:
		progress_high.value = 0
		progress_low.value = current

func updateGrowth(current: float, starting: float):
	var _decimalMultiplex = 10
	
	if current > starting:
		var _difference = current - starting
		var _number = (_difference / starting) * 100
		var _rounded = round(_number * _decimalMultiplex) / _decimalMultiplex
		var _percentage = str(_rounded) + "%"
		return _percentage
	else:
		return "0%"
