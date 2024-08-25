extends CanvasLayer

@onready var word: Label = %word
@onready var option1: Button = %option1
@onready var option2: Button = %option2
@onready var question_timer: Timer = %"Question Timer"
@onready var animationPlayer: AnimationPlayer = %"gameUI animator"
@onready var score_board: HBoxContainer = %ScoreBoard
@onready var correction_panel: Control = %"Correction Panel"

var question: Array = Helper.loadAllResources("res://Resources/Games/agility questions/")
var questionResource: agiltyQuestion

signal questionDone(buttonClicked, point: int)
signal answerAnimationDone
signal boostSpaceShip

func _ready() -> void:
	correction_panel.hide()
	setQuestion()

func _process(_delta):
	score_board.timeleft = question_timer.time_left

func setQuestion():
	question.shuffle()
	
	questionResource = question.pick_random()
	question.erase(questionResource)
	
	word.text = questionResource.question.to_lower()
	option1.text = ""
	option2.text = ""
	animationPlayer.play("Intro")
	await animationPlayer.animation_finished
	
	option1.text = questionResource.options[0].to_lower()
	option2.text = questionResource.options[1].to_lower()

func _on_option_1_pressed() -> void:
	if not animationPlayer.is_playing():
		option2.text = ""
		animationPlayer.play("Option1Clicked")
		await animationPlayer.animation_finished
		
		var point = questionResource.pointsDistribution[0]
		questionDone.emit(option1, point)

func _on_option_2_pressed() -> void:
	if not animationPlayer.is_playing():
		option1.text = ""
		animationPlayer.play("Option2Clicked")
		await animationPlayer.animation_finished
		
		var point = questionResource.pointsDistribution[1]
		questionDone.emit(option2, point)

func animate(button, option: String, score: int):
	updateScore(score)
	match option:
		"good":
			button.modulate = Color.GREEN
			answerAnimationDone.emit()
			boostSpaceShip.emit()
		"bad":
			button.modulate = Color.RED
			correction_panel.word = questionResource.question
			match button.name:
				"option1":
					correction_panel.option1 = questionResource.options[1]
					correction_panel.option2 = questionResource.options[0]
					correction_panel.option1Meaning = questionResource.meaning[1]
					correction_panel.option2Meaning = questionResource.meaning[0]
				"option2":
					correction_panel.option1 = questionResource.options[0]
					correction_panel.option2 = questionResource.options[1]
					correction_panel.option1Meaning = questionResource.meaning[0]
					correction_panel.option2Meaning = questionResource.meaning[1]
			
			get_tree().paused = true
			correction_panel.show()

func updateScore(score: int):
	score_board.target_score = score

func _on_correction_panel_correction_done() -> void:
	answerAnimationDone.emit()
