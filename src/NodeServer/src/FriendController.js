NetworkController = require("./NetworkController.js")
Firebase = require("./FirebaseDataModel.js")

class FriendController {
    static processFriendRequest(socket, data) {
        network = NetworkController.getInstance();
        data['timestamp'] = Date.now();
        socket.emit('friendRequestReply', data);
        if(data.fid in network.clientList) {
            network.clientList[data.fid].emit('receivedFriendRequest', data);
        }
        let fb = Firebase.getInstance();
        fb.friendRequestWrite(data);
    }
    static processFriendRequestAccept(socket, data) {
        network = NetworkController.getInstance();
        if (data.fid in network.clientList) {
            network.clientList[data.fid].emit('friendRequestAccept', data);
        }
        let fb = Firebase.getInstance();
        fb.friendRequestAccept(data.uid, data.fid);
    }
    static processFriendRequestRefuse(socket, data) {
        network = NetworkController.getInstance();
        if (data.fid in network.clientList) {
            network.clientList[data.fid].emit('friendRequestRefuse', data);
        }
        let fb = Firebase.getInstance();
        fb.friendRequestRefuse(data.uid, data.fid);
    }
    static processViewFriendList(socket, data) {
        let fb = Firebase.getInstance();
        fb.getFriendList(socket, data.uid);
    }
    static processRemoveFriend(socket, data) {
        let fb = Firebase.getInstance();
        if (data.fid in network.clientList) {
            network.clientList[data.fid].emit('friendRemove', data);
        }
        fb.friendRemove(data.uid, data.fid);
    }
}

module.exports = FriendController