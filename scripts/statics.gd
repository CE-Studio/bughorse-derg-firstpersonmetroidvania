extends Object
class_name Statics


static var multiplayer:MultiplayerAPI


static func get_node(path:NodePath) -> Node:
    #This feels cheaty
    #why isn't get_node() available to static functions????????
    return Engine.get_main_loop().current_scene.get_node(path)
