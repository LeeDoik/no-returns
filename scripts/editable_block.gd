@tool
extends StaticBody3D
const Bevel = preload("res://scripts/bevel_mesh.gd")
@export_range(0.0,0.15,0.005) var edge_bevel := 0.0:
	set(value):
		edge_bevel = maxf(0,value)
		_sync()
@export var dimensions := Vector3(2,2,0.5):
	set(value):
		dimensions = Vector3(maxf(0.05,value.x),maxf(0.05,value.y),maxf(0.05,value.z))
		_sync()
@export var color := Color("65516e"):
	set(value):
		color = value
		_sync()
var unique_resources := false
func _ready() -> void: _sync()
func _sync() -> void:
	if not is_inside_tree(): return
	var mesh: MeshInstance3D
	var collision: CollisionShape3D
	for child in get_children():
		if child is MeshInstance3D: mesh = child
		if child is CollisionShape3D: collision = child
	if not mesh or not collision: return
	if not unique_resources:
		mesh.mesh = mesh.mesh.duplicate()
		mesh.material_override = mesh.material_override.duplicate()
		collision.shape = collision.shape.duplicate()
		unique_resources = true
	if edge_bevel > 0:
		mesh.mesh = Bevel.make(dimensions,edge_bevel)
	else:
		if not mesh.mesh is BoxMesh: mesh.mesh = BoxMesh.new()
		mesh.mesh.size = dimensions
	collision.shape.size = dimensions
	mesh.material_override.albedo_color = color
