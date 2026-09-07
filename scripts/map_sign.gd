@tool
extends Label3D
const Copy = preload("res://scripts/copy.gd")
@export_multiline var english := "":
	set(value):
		english = value
		_refresh()
@export_multiline var korean := "":
	set(value):
		korean = value
		_refresh()
func _ready() -> void: _refresh()
func _process(_delta: float) -> void: _refresh()
func _refresh() -> void:
	var wanted := korean if Engine.is_editor_hint() or Copy.language == "ko" else english
	if text != wanted: text = wanted
