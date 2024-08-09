extends Control

const PADDING = Vector2(100, 50)

@onready var panel = %Panel as Panel
@onready var toastMessage = %Label as Label

func showMessage(message: String, duration: float):
	if GlobalRef.isToastActive == false:
		GlobalRef.isToastActive = true
		toastMessage.text = message
		toastMessage.update_minimum_size()
		panel.custom_minimum_size = toastMessage.get_minimum_size() + PADDING
		await get_tree().create_timer(duration).timeout
		hideMessage()

func hideMessage():
	GlobalRef.isToastActive = false
	queue_free()
