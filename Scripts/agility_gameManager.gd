extends Node

@onready var gameUI: CanvasLayer = %gameUI

@export var questionSize: int = 5

var playerScore: int = 0:
	set(new_value):
		if new_value < 0:
			playerScore = 0
		else:
			playerScore = new_value

func _ready():
	get_tree().quit_on_go_back = false
	setQuestion()

func _notification(what):
	if what == NOTIFICATION_WM_GO_BACK_REQUEST:
		go_back_request()

func go_back_request():
	Helper.showToast(get_node("gameUI/Control"), "Can't go back at this stage", 1.5)

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
		answerReview(buttonClicked, "good")
	else:
		answerReview(buttonClicked, "bad")

func answerReview(button, option: String):
	gameUI.animate(button, option, playerScore)

func _on_game_ui_answer_animation_done() -> void:
	await get_tree().create_timer(2).timeout
	setQuestion()

func endGame():
	print(playerScore)
