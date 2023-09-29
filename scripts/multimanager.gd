extends Node
class_name MultiManager


static var address := "127.0.0.1"
static var port:int = 8989
static var peer:ENetMultiplayerPeer


static func mjoin():
    peer = ENetMultiplayerPeer.new()
    var error = peer.create_client(address, port)
    if error != OK:
        print(error)
        return error
    peer.get_host().compress(ENetConnection.COMPRESS_FASTLZ)
    Statics.multiplayer.set_multiplayer_peer(peer)
    
    
static func mhost():
    peer = ENetMultiplayerPeer.new()
    var error = peer.create_server(port, 2)
    if error != OK:
        print(error)
        return error
    peer.get_host().compress(ENetConnection.COMPRESS_FASTLZ)
    Statics.multiplayer.set_multiplayer_peer(peer)
    print("waiting for player")


func _ready():
    Statics.multiplayer = multiplayer
    multiplayer.connected_to_server.connect(MultiManager.connected_to_server)
    multiplayer.connection_failed.connect(MultiManager.connection_failed)
    multiplayer.server_disconnected.connect(MultiManager.server_disconnected)
    multiplayer.peer_connected.connect(MultiManager.peer_connected)
    multiplayer.peer_disconnected.connect(MultiManager.peer_disconnected)
    
    
static func connected_to_server():
    pass


static func connection_failed():
    pass


static func server_disconnected():
    pass
    

static func peer_connected(id:int):
    print("connected:" + str(id))
    

static func peer_disconnected(id:int):
    print("disconnected:" + str(id))
