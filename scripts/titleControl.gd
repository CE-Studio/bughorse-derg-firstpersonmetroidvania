extends Control


# Called when the node enters the scene tree for the first time.
func _ready():
    Persist._charsel = $CenterContainer/TabContainer/Join/HBoxContainer/OptionButton


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
    pass


func _on_host_pressed():
    var error = MultiManager.mhost()
    if error == OK:
        hide()
    else:
        print(error)


func _on_join_pressed():
    MultiManager.mjoin()
