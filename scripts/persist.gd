extends Node


enum Char {
    Clarence,
    Epsilon,
    Request,
}


var character:Char


var _charsel:OptionButton


@rpc("authority", "call_remote", "reliable")
func setchar(char:Char):
    MultiManager.dlog(Char.find_key(char))


@rpc("authority", "call_local", "reliable")
func startgame():
    get_child(0).hide()
    get_tree().change_scene_to_file("res://assets/world.tscn")


@rpc("any_peer", "call_remote", "reliable")
func negotiate_char(char:Char):
    if Statics.multiplayer.is_server():
        assert(char != Char.Request, "Client is requesting character!")
        if char == Char.Clarence:
            setchar(Char.Epsilon)
            setchar.rpc(Char.Clarence)
        else:
            setchar.rpc(Char.Epsilon)
            setchar(Char.Clarence)
    else:
        assert(char == Char.Request, "Server already knows character but is trying to negotiate!")
        var tosel:Char = _charsel.selected  as Char
        if tosel == Char.Request:
            tosel = randi_range(0, 1) as Char
        negotiate_char.rpc(tosel)
