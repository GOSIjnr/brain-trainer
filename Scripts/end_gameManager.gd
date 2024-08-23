extends CanvasLayer

@onready var iconBackground: TextureRect = %"Icon Background"
@onready var gameIcon: TextureRect = %"Game Icon"
@onready var scoreText: Label = %Score
@onready var remarks: Label = %Remarks
@onready var proficiency: Label = %Proficiency
@onready var continueText: Label = %Continue

var score: int = 0
var gameResource: Games = GlobalRef.selectedGameResource

func _ready() -> void:
	self.visibility_changed.connect(updateUI)

func updateUI():
	if not gameResource == null and self.visible == true:
		iconBackground.self_modulate = gameResource.gameColor
		gameIcon.texture = gameResource.gameIcon
		scoreText.text = str(score)
		scoreText.self_modulate = gameResource.gameColor
		remarks.text = getRemark()
		proficiency.text = getProficiency()

func _on_panel_gui_input(event: InputEvent) -> void:
	if event is InputEventScreenTouch and event.is_released():
		get_tree().change_scene_to_file(GlobalRef.scenes["mainmenu"])

func getRemark() -> String:
	var remark: String = ""
	
	if score == 6000:
		remark += "excellent!"
	elif score >= 4000:
		remark += "goodjob."
	elif score >= 2000:
		remark += "pass."
	else:
		remark += "fail!"
	
	if score > GlobalRef.fileData.HighScores[gameResource.gameName.to_lower()]:
		remark += " highscore!"
		GlobalRef.fileData.HighScores[gameResource.gameName.to_lower()] = score
		ResourceSaver.save(GlobalRef.fileData, GlobalRef.gamefilePath)
	
	return remark

func getProficiency() -> String:
	var text: String = ""
	
	match gameResource.gameType:
		gameResource.Type.writing:
			text += str(abs(proficiencyScore())) + " Writing"
			GlobalRef.fileData.WritingEPQ += proficiencyScore()
		gameResource.Type.speaking:
			text += str(abs(proficiencyScore())) + " Speaking"
			GlobalRef.fileData.SpeakingEPQ += proficiencyScore()
		gameResource.Type.reading:
			text += str(abs(proficiencyScore())) + " Reading"
			GlobalRef.fileData.ReadingEPQ += proficiencyScore()
		gameResource.Type.maths:
			text += str(abs(proficiencyScore())) + " Maths"
			GlobalRef.fileData.MathsEPQ += proficiencyScore()
		gameResource.Type.memory:
			text += str(abs(proficiencyScore())) + " Memory"
			GlobalRef.fileData.MemoryEPQ += proficiencyScore()
	
	if proficiencyScore() > 0:
		text += " Proficiency Gained"
	else:
		text += " Proficiency Deducted"
	
	ResourceSaver.save(GlobalRef.fileData, GlobalRef.gamefilePath)
	
	return text

func proficiencyScore() -> int:
	if score == 6000:
		return 250
	elif score >= 4000:
		return 150
	elif score >= 2000:
		return 100
	else:
		return -250
