extends Node

@onready var gameUI: CanvasLayer = %gameUI
@onready var end_game: CanvasLayer = $EndGame

@export var questionSize: int = 5

var playerScore: int = 0:
	set(new_value):
		if new_value < 0:
			playerScore = 0
		else:
			playerScore = new_value

func _ready():
	get_tree().quit_on_go_back = false
	end_game.hide()

func _notification(what):
	if what == NOTIFICATION_WM_GO_BACK_REQUEST:
		Utils.showToast(gameUI, "Can't go back at this stage", 1)

func setQuestion():
	if questionSize == 0:
		gameUI.hide()
		endGame()
	else:
		gameUI.setQuestion()

func _on_game_ui_question_done(buttonClicked, point: int) -> void:
	questionSize -= 1
	playerScore += point
	
	if point > 0:
		%Sfx_Right.play()
		answerReview(buttonClicked, "good")
	else:
		%Sfx_Wrong.play()
		answerReview(buttonClicked, "bad")

func answerReview(button, option: String):
	gameUI.animate(button, option, playerScore)

func _on_game_ui_answer_animation_done() -> void:
	await get_tree().create_timer(1).timeout
	setQuestion()

func endGame():
	%"Question Timer".stop()
	end_game.score = playerScore
	end_game.show()

func _on_question_timer_timeout() -> void:
	questionSize = 0
	setQuestion()
