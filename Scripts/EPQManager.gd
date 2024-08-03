extends VBoxContainer

@export var progressBarColor :Color

enum Type {writing, speaking, reading, maths, memory, average}
@export var barType :Type

@onready var score = $HBoxContainer1/Score as RichTextLabel
@onready var title = $HBoxContainer1/Title as Label
@onready var progress_bar = $HBoxContainer2/ProgressBar as TextureProgressBar

var File = FileAccess
var filePath = "user://UserData.tres"
var fileData: UserData

func _ready():
	progress_bar.tint_progress = progressBarColor
	
	fileData = load(filePath) as UserData
	
	match barType:
		Type.writing:
			title.text = getTitle(fileData.WritingEPQ)
			score.text = "[color=#999999]WRITING: [/color][b]" + str(fileData.WritingEPQ) + "[/b]"
			progress_bar.value = fileData.WritingEPQ
		Type.speaking:
			title.text = getTitle(fileData.SpeakingEPQ)
			score.text = "[color=#999999]SPEAKING: [/color][b]" + str(fileData.SpeakingEPQ) + "[/b]"
			progress_bar.value = fileData.SpeakingEPQ
		Type.reading:
			title.text = getTitle(fileData.ReadingEPQ)
			score.text = "[color=#999999]READING: [/color][b]" + str(fileData.ReadingEPQ) + "[/b]"
			progress_bar.value = fileData.ReadingEPQ
		Type.maths:
			title.text = getTitle(fileData.MathsEPQ)
			score.text = "[color=#999999]MATHS: [/color][b]" + str(fileData.MathsEPQ) + "[/b]"
			progress_bar.value = fileData.MathsEPQ
		Type.memory:
			title.text = getTitle(fileData.MemoryEPQ)
			score.text = "[color=#999999]MEMORY: [/color][b]" + str(fileData.MemoryEPQ) + "[/b]"
			progress_bar.value = fileData.MemoryEPQ
		Type.average:
			title.visible = false
			@warning_ignore("integer_division")
			var average = (fileData.WritingEPQ + fileData.SpeakingEPQ + fileData.ReadingEPQ + fileData.MathsEPQ + fileData.MemoryEPQ) / 5
			score.text = "[color=#999999]AVERAGE: [/color][b]" + str(average) + "[/b]"
			progress_bar.value = average

func getTitle(value: int):
	if value >= 4750:
		return "MASTER"
	elif value >= 4250:
		return "ELITE"
	elif value >= 3750:
		return "EXPERT"
	elif value >= 2500:
		return "ADVANCED"
	elif value >= 1250:
		return "INTERMEDIATE"
	else:
		return "NOVICE"
