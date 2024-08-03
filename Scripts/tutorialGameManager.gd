extends Control

@export var timerNormal :Color
@export var timerWarning :Color

var questionsPath := "res://Resources/TutorialGame/"
var questions = [] as Array[tutorialGame]
var questionIndex := 0

var writingScore := 0
var speakingScore := 0
var readingScore := 0
var mathsScore := 0
var memoryScore := 0

@onready var question = %Question as RichTextLabel
@onready var progress = %Progress as TextureProgressBar
@onready var option_1 = %Option1 as Button
@onready var option_2 = %Option2 as Button
@onready var option_3 = %Option3 as Button
@onready var option_4 = %Option4 as Button
@onready var option_5 = %Option5 as Button
@onready var question_timer = %QuestionTimer as Timer
@onready var wait_timer = %WaitTimer as Timer
@onready var timer_countdown = %TimerCountdown as Label
@onready var sfx_right = %Sfx_Right as AudioStreamPlayer
@onready var sfx_wrong = %Sfx_Wrong as AudioStreamPlayer
@onready var sfx_tick = %Sfx_Tick as AudioStreamPlayer

func _ready():
	questions = Helper.loadAllResources(questionsPath)
	questions.shuffle()
	progress.max_value = questions.size()
	displayQuestion(questionIndex)

func _process(_delta):
	var time = round(question_timer.time_left)
	var formattedTime = "%02d" % time
	timer_countdown.text = ":" + formattedTime
	
	if time <= 10:
		timer_countdown.self_modulate = timerWarning
		
		var timeDif = ceil(question_timer.time_left) - question_timer.time_left
		if not sfx_tick.playing and timeDif <= 0.1 and timeDif > 0:
			sfx_tick.play()
	else:
		timer_countdown.self_modulate = timerNormal

func resetAll():
	option_1.visible = false
	option_1.disabled = false
	option_1.theme_type_variation = ""
	
	option_2.visible = false
	option_2.disabled = false
	option_2.theme_type_variation = ""
	
	option_3.visible = false
	option_3.disabled = false
	option_3.theme_type_variation = ""
	
	option_4.visible = false
	option_4.disabled = false
	option_4.theme_type_variation = ""
	
	option_5.visible = false
	option_5.disabled = false
	
	question.visible = false

func displayQuestion(index :int):
	if index > questions.size() - 1:
		GlobalRef.fileData = load(GlobalRef.gamefilePath) as UserData
		
		GlobalRef.fileData.WritingEPQ = writingScore
		GlobalRef.fileData.SpeakingEPQ = speakingScore
		GlobalRef.fileData.ReadingEPQ = readingScore
		GlobalRef.fileData.MathsEPQ = mathsScore
		GlobalRef.fileData.MemoryEPQ = memoryScore
		
		GlobalRef.fileData.starting_WritingEPQ = writingScore
		GlobalRef.fileData.starting_SpeakingEPQ = speakingScore
		GlobalRef.fileData.starting_ReadingEPQ = readingScore
		GlobalRef.fileData.starting_MathsEPQ = mathsScore
		GlobalRef.fileData.starting_MemoryEPQ = memoryScore
		
		ResourceSaver.save(GlobalRef.fileData, GlobalRef.gamefilePath)
		get_tree().change_scene_to_file(GlobalRef.scenes["tutorialend"])
	else:
		resetAll()
		progress.value = index + 1
		question.visible = true
		var _resource = questions[index] as tutorialGame
		question.text = _resource.question
		displayOptions(_resource, _resource.options.size())
		question_timer.start()

func displayOptions(selectedRes :tutorialGame, lengthSize :int):
	option_5.visible = true
	
	match lengthSize:
		2:
			option_1.visible = true
			option_2.visible = true
			option_1.text = selectedRes.options[0]
			option_2.text = selectedRes.options[1]
		4:
			option_1.visible = true
			option_2.visible = true
			option_3.visible = true
			option_4.visible = true
			option_1.text = selectedRes.options[0]
			option_2.text = selectedRes.options[1]
			option_3.text = selectedRes.options[2]
			option_4.text = selectedRes.options[3]

func disableOptions():
	option_1.disabled = true
	option_2.disabled = true
	option_3.disabled = true
	option_4.disabled = true
	option_5.disabled = true

func getTheRightOption(_resource: Array[int]):
	var rightOption := 0
	
	for number in range(_resource.size()):
		if _resource[number] > 0:
			rightOption = number + 1
			break
	
	match rightOption:
		1:
			option_1.theme_type_variation = "Button_Right"
		2:
			option_2.theme_type_variation = "Button_Right"
		3:
			option_3.theme_type_variation = "Button_Right"
		4:
			option_4.theme_type_variation = "Button_Right"

func scorePlayer(_resource :tutorialGame, _points :int):
	match _resource.QuestionType:
		0:
			writingScore += _points
		1:
			speakingScore += _points
		2:
			readingScore += _points
		3:
			mathsScore += _points
		4:
			memoryScore += _points

func _on_option_1_pressed():
	disableOptions()
	var _clicked := 0
	var _resource = questions[questionIndex] as tutorialGame
	
	if _resource.pointsDistribution[_clicked] > 0:
		sfx_right.playing = true
		option_1.theme_type_variation = "Button_Right"
		scorePlayer(_resource, _resource.pointsDistribution[_clicked])
	else:
		sfx_wrong.playing = true
		option_1.theme_type_variation = "Button_Wrong"
		getTheRightOption(_resource.pointsDistribution)
	
	wait_timer.start()

func _on_option_2_pressed():
	disableOptions()
	var _clicked := 1
	var _resource = questions[questionIndex] as tutorialGame
	
	if _resource.pointsDistribution[_clicked] > 0:
		sfx_right.play()
		option_2.theme_type_variation = "Button_Right"
		scorePlayer(_resource, _resource.pointsDistribution[_clicked])
	else:
		sfx_wrong.play()
		option_2.theme_type_variation = "Button_Wrong"
		getTheRightOption(_resource.pointsDistribution)
	
	wait_timer.start()

func _on_option_3_pressed():
	disableOptions()
	var _clicked := 2
	var _resource = questions[questionIndex] as tutorialGame
	
	if _resource.pointsDistribution[_clicked] > 0:
		sfx_right.play()
		option_3.theme_type_variation = "Button_Right"
		scorePlayer(_resource, _resource.pointsDistribution[_clicked])
	else:
		sfx_wrong.play()
		option_3.theme_type_variation = "Button_Wrong"
		getTheRightOption(_resource.pointsDistribution)
	
	wait_timer.start()

func _on_option_4_pressed():
	disableOptions()
	var _clicked := 3
	var _resource = questions[questionIndex] as tutorialGame
	
	if _resource.pointsDistribution[_clicked] > 0:
		sfx_right.play()
		option_4.theme_type_variation = "Button_Right"
		scorePlayer(_resource, _resource.pointsDistribution[_clicked])
	else:
		sfx_wrong.play()
		option_4.theme_type_variation = "Button_Wrong"
		getTheRightOption(_resource.pointsDistribution)
	
	wait_timer.start()

func _on_option_5_pressed():
	disableOptions()
	sfx_wrong.play()
	var _resource = questions[questionIndex] as tutorialGame
	
	if _resource.QuestionType == _resource.pointType.speaking or _resource.QuestionType == _resource.pointType.memory:
		pass
	else:
		getTheRightOption(_resource.pointsDistribution)
	
	wait_timer.start()


func _on_wait_timer_timeout():
	questionIndex += 1
	displayQuestion(questionIndex)


func _on_question_timer_timeout():
	if !(questionIndex > questions.size() - 1):
		disableOptions()
		sfx_wrong.play()
		var _resource = questions[questionIndex] as tutorialGame
	
		if _resource.QuestionType == _resource.pointType.speaking or _resource.QuestionType == _resource.pointType.memory:
			pass
		else:
			getTheRightOption(_resource.pointsDistribution)
	
		wait_timer.start()
