extends Control

@onready var gameName = %game_name as Label
@onready var gameType = %game_type as Label
@onready var gameIcon = %game_icon as TextureRect
@onready var gameBackground = %Background as TextureRect
@onready var hex = %hex as TextureRect
@onready var benefits = %Benefits as RichTextLabel
@onready var startButton = %startGame as Button
@onready var highScore = %HighScore as Label
@onready var help = %Help
@onready var helpText = %HelpText

var selected :Games:
	set = setSelected

func setSelected(resource :Games):
	selected = resource
	gameName.text = resource.gameName
	gameIcon.texture = resource.gameIcon
	gameBackground.texture = resource.gameFullBackground
	hex.self_modulate = resource.gameColor
	highScore.text = str(GlobalRef.fileData.HighScores[resource.gameName.to_lower()])
	updateStartButton(resource.gameColor)
	updateText(benefits, "[color=#cccccc][b]BENEFITS:[/b][/color]", resource.gameBenefits, "")
	updateText(helpText, "[b]INSTRUCTIONS[/b]", resource.howToPlay, "[center][b]Tap to close[/b][/center]")
	
	if resource.gameScene == null:
		startButton.disabled = true
	
	var gameRef = ["Writing", "Speaking", "Reading", "Maths", "Memory"]
	gameType.text = gameRef[resource.gameType]

func updateStartButton(color :Color):
	var darken := 0.15
	
	var styleBoxflat = StyleBoxFlat.new()
	styleBoxflat.bg_color = color
	styleBoxflat.corner_detail = 12
	styleBoxflat.border_width_bottom = 15
	styleBoxflat.border_color = color.darkened(darken)
	styleBoxflat.set_corner_radius_all(20)
	
	var styleBoxclicked = StyleBoxFlat.new()
	styleBoxclicked.bg_color = color.darkened(darken)
	styleBoxclicked.corner_detail = 12
	styleBoxclicked.set_corner_radius_all(20)
	
	startButton.add_theme_stylebox_override("hover", styleBoxflat)
	startButton.add_theme_stylebox_override("normal", styleBoxflat)
	startButton.add_theme_stylebox_override("pressed", styleBoxclicked)

func updateText(text :RichTextLabel, suffix :String, textToAdd :String, prefix :String):
	text.clear()
	text.append_text(suffix)
	text.add_text("\n\n")
	text.append_text(textToAdd)
	
	if not prefix.is_empty():
		text.add_text("\n\n")
		text.append_text(prefix)

func _on_close_button_gui_input(event: InputEvent):
	if event is InputEventScreenTouch and event.is_released():
		var rect = Rect2(Vector2(0, 0), Vector2(100, 100))
		
		if rect.has_point(event.position):
			self.visible = false

func _on_help_button_gui_input(event):
	if event is InputEventScreenTouch and event.is_released():
		var rect = Rect2(Vector2(0, 0), Vector2(100, 100))
		
		if rect.has_point(event.position):
			help.visible = true
			help.modulate = Color(1, 1, 1, 0)
		
			var tween = create_tween()
			tween.tween_property(help, "modulate", Color(1, 1, 1, 1), 0.05)

func _on_start_game_pressed():
	get_tree().change_scene_to_packed(selected.gameScene)

func _on_help_gui_input(event: InputEvent):
	if event is InputEventScreenTouch and event.is_released():
		help.visible = false
