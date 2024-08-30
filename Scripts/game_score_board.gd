extends HBoxContainer

@onready var score_text: Label = %"Score Text"
@onready var timer_text: Label = %"Timer Text"
@onready var audioPlayer: AudioStreamPlayer = $AudioStreamPlayer

var elaspedTime: float = 0.0
var score: int = 0
var target_score: int = 0:
	set(new_value):
		target_score = new_value
		var tween = get_tree().create_tween()
		tween.tween_property(self, "score", target_score, 0.5).set_trans(Tween.TRANS_LINEAR)
		tween.finished.connect(_on_tween_completed)

var timeleft: float = 0:
	set(new_value):
		var time = round(new_value)
		var formattedTime = "%02d" % time
		
		timer_text.text = ":" + formattedTime
		timeleft = new_value

func _process(delta):
	score_text.text = str(score)
	
	if round(timeleft) <= 11:
		if round(timeleft) <= 10:
			timer_text.self_modulate = Color.RED
		
		elaspedTime += delta
		if elaspedTime >= 1.0:
			if not audioPlayer.playing and not timeleft == 0:
				elaspedTime = 0
				audioPlayer.play()
	else:
		timer_text.self_modulate = Color.WHITE

func _on_tween_completed():
	score = target_score
	score_text.text = str(score)
