extends Control


@onready var l1 := $Polygon2D
@onready var l2 := $Polygon2D2
@onready var l3 := $Polygon2D3

var time := 0.0


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
    time += delta * 2
    l1.scale.y = 0.1 + abs(sin(time))
    l2.scale.y = 0.1 + abs(sin(time - 0.5))
    l3.scale.y = 0.1 + abs(sin(time - 1))
