extends Control

@onready var correction_panel: Control = %"Correction Panel"
@onready var correction_box: Panel = %"Correction Box"
@onready var correction_text: RichTextLabel = %"Correction Text"
@onready var animationPlayer: AnimationPlayer = $AnimationPlayer

var correction: String

signal correctionDone

func _ready() -> void:
	self.visibility_changed.connect(updateUI)

func _input(event: InputEvent) -> void:
	if event is InputEventScreenTouch and event.is_released():
		if not animationPlayer.is_playing():
			correction_text.hide()
			animationPlayer.play("Panel_Close")
			await animationPlayer.animation_finished
			get_tree().paused = false
			self.hide()
			correctionDone.emit()

func updateUI():
	if self.visible == true:
		correction_text.text = correction
		correction_text.hide()
		animationPlayer.play("Panel_Open")
		await animationPlayer.animation_finished
		correction_text.show()
