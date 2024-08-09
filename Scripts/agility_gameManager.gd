extends Node

func _ready():
	get_tree().quit_on_go_back = false

func _notification(what):
	if what == NOTIFICATION_WM_GO_BACK_REQUEST:
		go_back_request()

func go_back_request():
	var toastScene = load(GlobalRef.scenes["toast"]) as PackedScene
	var toast = toastScene.instantiate()
	add_child(toast)
	toast.showMessage("Hello It Works", 1.5)
