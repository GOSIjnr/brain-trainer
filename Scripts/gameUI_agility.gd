extends CanvasLayer

@onready var word: Label = %word
@onready var option1: Button = %option1
@onready var option2: Button = %option2
@onready var question_timer: Timer = %"Question Timer"
@onready var animationPlayer: AnimationPlayer = %"gameUI animator"
@onready var score_board: HBoxContainer = %ScoreBoard
@onready var correction_panel: Control = %"Correction Panel"
@onready var pause_menu: Control = %"Pause Menu"

var question: Array = Helper.loadAllResources("res://Resources/Games/agility questions/")
var questionResource: agiltyQuestion

signal questionDone(buttonClicked, point: int)
signal answerAnimationDone
signal boostSpaceShip

func _ready() -> void:
	correction_panel.hide()
	setQuestion()

func _process(_delta):
	var time = round(question_timer.time_left)
	score_board.timeleft = time

func setQuestion():
	question.shuffle()
	
	questionResource = question.pick_random()
	question.erase(questionResource)
	
	word.text = questionResource.question
	option1.text = ""
	option2.text = ""
	animationPlayer.play("Intro")
	await animationPlayer.animation_finished
	
	option1.text = questionResource.options[0]
	option2.text = questionResource.options[1]

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
			correction_panel.correction = questionResource.correction
			get_tree().paused = true
			correction_panel.show()

func updateScore(score: int):
	score_board.target_score = score

func _on_correction_panel_correction_done() -> void:
	answerAnimationDone.emit()
