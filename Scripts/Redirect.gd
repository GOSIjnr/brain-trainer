extends Panel

var File = FileAccess

var filePath = "user://UserData.tres"
var fileData: UserData

var newScores = {
	"agility": 0,
	"average": 0,
	"aviodance": 0,
	"collaspe": 0,
	"memory": 0,
	"pronunciation": 0,
	"sound match": 0,
	"synatx": 0,
	"tipping": 0,
	"word part": 0,
}

func _ready():
	var check = userDataCheck()
	if check:
		fileData = load(filePath) as UserData
		
		fileData.HighScores = Helper.updateDictionary(fileData.HighScores, newScores)
		
		ResourceSaver.save(fileData, filePath)
	else:
		fileData = UserData.new()
		
		fileData.HighScores = Helper.updateDictionary(fileData.HighScores, newScores)
		
		ResourceSaver.save(fileData, filePath)
	
	fileData = load(filePath) as UserData
	if fileData.isTutorialDone:
		call_deferred("load_main_menu")
	else:
		call_deferred("load_tutorial")

func userDataCheck():
	if File.file_exists(filePath):
		return true
	else:
		return false

func load_main_menu():
	var target_scene = "res://Scenes/main_menu_ui.tscn"
	get_tree().change_scene_to_file(target_scene)

func load_tutorial():
	var target_scene = "res://Scenes/tutorial.tscn"
	get_tree().change_scene_to_file(target_scene)
