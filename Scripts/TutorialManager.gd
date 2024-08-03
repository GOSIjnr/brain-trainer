extends Control

var CurrentPage
var NextPage
var currentPosition = 0
var scrollView
var scrollPositions
var previousButton
var nextButton

func _ready():
	previousButton = $"Page 2/MarginContainer/UI/HBoxContainer/Button1"
	nextButton = $"Page 2/MarginContainer/UI/HBoxContainer/Button2"
	scrollView = $"Page 2/Content"
	
	var scrollSection = scrollView.size.x / 3
	scrollPositions = [0, -scrollSection, -scrollSection * 2]

func _process(_delta):
	page4UpdateUI()

# page1
func _on_page1NextButton_pressed():
	CurrentPage = $"Page 1"
	NextPage = $"Page 2"
	CurrentPage.hide()
	NextPage.show()

# page2
func _on_buttonPrevious_pressed():
	currentPosition -= 1
	
	if currentPosition < 0:
		currentPosition = 0
	
	updateUI(currentPosition)

func _on_buttonNext_pressed():
	currentPosition += 1
	
	if currentPosition > 2:
		currentPosition = 2
		CurrentPage = $"Page 2"
		NextPage = $"Page 3"
		CurrentPage.hide()
		NextPage.show()
	else:
		updateUI(currentPosition)

func updateUI(new_position):
	var tween = create_tween()
	tween.tween_property(scrollView, "position", Vector2(scrollPositions[new_position], 0), .15)
	
	match new_position:
		0:
			previousButton.disabled = true
		1:
			previousButton.disabled = false
			nextButton.text = "Next"
		2:
			nextButton.text = "Continue"

# page3
func _on_Page3NextButton_pressed():
	CurrentPage = $"Page 3"
	NextPage = $"Page 4"
	CurrentPage.hide()
	NextPage.show()

# page4
func _on_Page4NextButton_pressed():
	CurrentPage = $"Page 4"
	NextPage = $"Page 5"
	CurrentPage.hide()
	NextPage.show()
	savePersonalization()

func page4UpdateUI():
	if checkCheckedCheckboxes():
		$"Page 4/MarginContainer/Button".disabled = false
	else:
		$"Page 4/MarginContainer/Button".disabled = true

func checkCheckedCheckboxes():
	var checkBoxContainer = $"Page 4/MarginContainer/VBoxContainer"
	var num_checkboxes = checkBoxContainer.get_child_count()
	var any_checked = false
	
	for i in range(num_checkboxes):
		var checkbox = checkBoxContainer.get_child(i)
		if checkbox is CheckButton and checkbox.button_pressed:
			any_checked = true
			break
	
	return any_checked

func savePersonalization():
	var data: UserData = load(GlobalRef.gamefilePath) as UserData
	data.Writing = $"Page 4/MarginContainer/VBoxContainer/CheckButton1".button_pressed
	data.Speaking = $"Page 4/MarginContainer/VBoxContainer/CheckButton2".button_pressed
	data.Reading = $"Page 4/MarginContainer/VBoxContainer/CheckButton3".button_pressed
	data.Maths = $"Page 4/MarginContainer/VBoxContainer/CheckButton4".button_pressed
	data.Memory = $"Page 4/MarginContainer/VBoxContainer/CheckButton5".button_pressed
	
	ResourceSaver.save(data, GlobalRef.gamefilePath)

# page5
func _on_Page5NextButton_pressed():
	get_tree().change_scene_to_file(GlobalRef.scenes["tutorialgame"])
