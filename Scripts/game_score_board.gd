extends HBoxContainer

@onready var pause: Button = %Pause
@onready var score_text: Label = %"Score Text"
@onready var timer_text: Label = %"Timer Text"

var score: int = 0
var target_score: int = 0:
	set(new_value):
		target_score = new_value
		var tween = get_tree().create_tween()
		tween.tween_property(self, "score", target_score, 0.5).set_trans(Tween.TRANS_LINEAR)
		tween.finished.connect(_on_tween_completed)

var timeleft: int = 0:
	set(new_value):
		timeleft = new_value
		timer_text.text = ":" + str(new_value)

signal pausedButtonPressed

func _process(_delta):
	score_text.text = str(score)

func _on_tween_completed():
	score = target_score
	score_text.text = str(score)

func _on_pause_pressed() -> void:
	pausedButtonPressed.emit()
