extends Node
class_name MultiManager


static var address := "127.0.0.1"
static var port:int = 8989
static var peer:ENetMultiplayerPeer
static var loadscreen:Control


static func dlog(content:String):
    if Statics.multiplayer.is_server():
        print("[server] " + content)
    else:
        print("[client] " + content)


static func mjoin():
    peer = ENetMultiplayerPeer.new()
    var error = peer.create_client(address, port)
    if error != OK:
        dlog(error)
        return error
    peer.get_host().compress(ENetConnection.COMPRESS_FASTLZ)
    Statics.multiplayer.set_multiplayer_peer(peer)
    
    
static func mhost() -> Error:
    peer = ENetMultiplayerPeer.new()
    var error = peer.create_server(port, 2)
    if error != OK:
        dlog(error)
        return error
    peer.get_host().compress(ENetConnection.COMPRESS_FASTLZ)
    Statics.multiplayer.set_multiplayer_peer(peer)
    dlog("waiting for player")
    loadscreen.show()
    return error


func _ready():
    loadscreen = preload("res://assets/prefabs/connect.tscn").instantiate()
    Persist.add_child(loadscreen)
    Statics.multiplayer = multiplayer
    multiplayer.connected_to_server.connect(MultiManager.connected_to_server)
    multiplayer.connection_failed.connect(MultiManager.connection_failed)
    multiplayer.server_disconnected.connect(MultiManager.server_disconnected)
    multiplayer.peer_connected.connect(MultiManager.peer_connected)
    multiplayer.peer_disconnected.connect(MultiManager.peer_disconnected)
    
    
static func connected_to_server():
    dlog("connected_to_server")


static func connection_failed():
    dlog("connection_failed")


static func server_disconnected():
    dlog("server_disconnected")
    

static func peer_connected(id:int):
    dlog("connected:" + str(id))
    if Statics.multiplayer.is_server():
        Persist.setid.rpc(id)
        Persist.negotiate_char.rpc(Persist.Char.Request)
    

static func peer_disconnected(id:int):
    dlog("disconnected:" + str(id))
