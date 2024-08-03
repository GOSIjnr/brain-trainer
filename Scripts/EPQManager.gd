extends VBoxContainer

@export var progressBarColor :Color

enum Type {writing, speaking, reading, maths, memory, average}
@export var barType :Type

@onready var score = $HBoxContainer1/Score as RichTextLabel
@onready var title = $HBoxContainer1/Title as Label
@onready var progress_bar = $HBoxContainer2/ProgressBar as TextureProgressBar

func _ready():
	progress_bar.tint_progress = progressBarColor
	
	GlobalRef.fileData = load(GlobalRef.gamefilePath) as UserData
	
	match barType:
		Type.writing:
			title.text = getTitle(GlobalRef.fileData.WritingEPQ)
			score.text = "[color=#999999]WRITING: [/color][b]" + str(GlobalRef.fileData.WritingEPQ) + "[/b]"
			progress_bar.value = GlobalRef.fileData.WritingEPQ
		Type.speaking:
			title.text = getTitle(GlobalRef.fileData.SpeakingEPQ)
			score.text = "[color=#999999]SPEAKING: [/color][b]" + str(GlobalRef.fileData.SpeakingEPQ) + "[/b]"
			progress_bar.value = GlobalRef.fileData.SpeakingEPQ
		Type.reading:
			title.text = getTitle(GlobalRef.fileData.ReadingEPQ)
			score.text = "[color=#999999]READING: [/color][b]" + str(GlobalRef.fileData.ReadingEPQ) + "[/b]"
			progress_bar.value = GlobalRef.fileData.ReadingEPQ
		Type.maths:
			title.text = getTitle(GlobalRef.fileData.MathsEPQ)
			score.text = "[color=#999999]MATHS: [/color][b]" + str(GlobalRef.fileData.MathsEPQ) + "[/b]"
			progress_bar.value = GlobalRef.fileData.MathsEPQ
		Type.memory:
			title.text = getTitle(GlobalRef.fileData.MemoryEPQ)
			score.text = "[color=#999999]MEMORY: [/color][b]" + str(GlobalRef.fileData.MemoryEPQ) + "[/b]"
			progress_bar.value = GlobalRef.fileData.MemoryEPQ
		Type.average:
			title.visible = false
			@warning_ignore("integer_division")
			var average = (GlobalRef.fileData.WritingEPQ + GlobalRef.fileData.SpeakingEPQ + GlobalRef.fileData.ReadingEPQ + GlobalRef.fileData.MathsEPQ + GlobalRef.fileData.MemoryEPQ) / 5
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
