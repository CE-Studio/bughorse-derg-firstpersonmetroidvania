extends Node


@rpc("authority", "call_local", "reliable")
func startgame():
    get_child(0).hide()
    get_tree().change_scene_to_file("res://assets/world.tscn")
