extends VBoxContainer

@export var progressBarColor: Color

enum Type {writing, speaking, reading, maths, memory, average}
@export var barType: Type

@onready var score: RichTextLabel = %Score
@onready var title: Label = %Title
@onready var progress_bar: TextureProgressBar = %ProgressBar

func _ready():
	progress_bar.tint_progress = progressBarColor
	
	SaveManager.fileData = load(SaveManager.gamefilePath) as UserData
	
	match barType:
		Type.writing:
			title.text = getTitle(SaveManager.fileData.WritingEPQ)
			score.text = "[color=#999999]WRITING: [/color][b]" + str(SaveManager.fileData.WritingEPQ) + "[/b]"
			progress_bar.value = SaveManager.fileData.WritingEPQ
		Type.speaking:
			title.text = getTitle(SaveManager.fileData.SpeakingEPQ)
			score.text = "[color=#999999]SPEAKING: [/color][b]" + str(SaveManager.fileData.SpeakingEPQ) + "[/b]"
			progress_bar.value = SaveManager.fileData.SpeakingEPQ
		Type.reading:
			title.text = getTitle(SaveManager.fileData.ReadingEPQ)
			score.text = "[color=#999999]READING: [/color][b]" + str(SaveManager.fileData.ReadingEPQ) + "[/b]"
			progress_bar.value = SaveManager.fileData.ReadingEPQ
		Type.maths:
			title.text = getTitle(SaveManager.fileData.MathsEPQ)
			score.text = "[color=#999999]MATHS: [/color][b]" + str(SaveManager.fileData.MathsEPQ) + "[/b]"
			progress_bar.value = SaveManager.fileData.MathsEPQ
		Type.memory:
			title.text = getTitle(SaveManager.fileData.MemoryEPQ)
			score.text = "[color=#999999]MEMORY: [/color][b]" + str(SaveManager.fileData.MemoryEPQ) + "[/b]"
			progress_bar.value = SaveManager.fileData.MemoryEPQ
		Type.average:
			title.visible = false
			@warning_ignore("integer_division")
			var average = (SaveManager.fileData.WritingEPQ + SaveManager.fileData.SpeakingEPQ + SaveManager.fileData.ReadingEPQ + SaveManager.fileData.MathsEPQ + SaveManager.fileData.MemoryEPQ) / 5
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
