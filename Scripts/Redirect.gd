extends Panel

var File = FileAccess

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
	get_tree().quit_on_go_back = false
	
	var check = userDataCheck()
	if check:
		GlobalRef.fileData = load(GlobalRef.gamefilePath) as UserData
		
		GlobalRef.fileData.HighScores = Helper.updateDictionary(GlobalRef.fileData.HighScores, newScores)
		
		ResourceSaver.save(GlobalRef.fileData, GlobalRef.gamefilePath)
	else:
		GlobalRef.fileData = UserData.new()
		
		GlobalRef.fileData.HighScores = Helper.updateDictionary(GlobalRef.fileData.HighScores, newScores)
		
		ResourceSaver.save(GlobalRef.fileData, GlobalRef.gamefilePath)
	
	GlobalRef.fileData = load(GlobalRef.gamefilePath) as UserData
	if GlobalRef.fileData.isTutorialDone:
		call_deferred("load_main_menu")
	else:
		call_deferred("load_tutorial")

func userDataCheck():
	if File.file_exists(GlobalRef.gamefilePath):
		return true
	else:
		return false

func load_main_menu():
	var target_scene = GlobalRef.scenes["mainmenu"]
	get_tree().change_scene_to_file(target_scene)

func load_tutorial():
	var target_scene = GlobalRef.scenes["tutorial"]
	get_tree().change_scene_to_file(target_scene)
