NetworkController = require("./NetworkController.js")
Firebase = require("./FirebaseDataModel.js")

/*
Data format:
{
    uid: string,
    fid: string
}
*/

class FriendController {
    static async processFriendRequest(socket, data) {
        network = NetworkController.getInstance();
        let fb = Firebase.getInstance();
        await fb.friendRequest(data.uid, data.fid);
        if (data.fid in network.clientList) {
            network.clientList[data.fid].emit('receivedFriendRequest', null);
        }
        socket.emit('friendRequestReply', data);
    }
    static async processFriendRequestAccept(socket, data) {
        network = NetworkController.getInstance();
        let fb = Firebase.getInstance();
        await fb.friendRequestAccept(data.uid, data.fid);
        if (data.fid in network.clientList) {
            network.clientList[data.fid].emit('friendRequestAcceptReply', data);
        }
        socket.emit('friendRequestAcceptReply', null);
    }
    static async processFriendRequestRefuse(socket, data) {
        network = NetworkController.getInstance();
        let fb = Firebase.getInstance();
        await fb.friendRequestRefuse(data.uid, data.fid);
        if (data.fid in network.clientList) {
            network.clientList[data.fid].emit('friendRequestRefuseReply', null);
        }
        socket.emit('friendRequestRefuseReply', null);
    }
    static processViewFriendList(socket, data) {
        let fb = Firebase.getInstance();
        fb.getFriendList(socket, data.uid);
    }
    static processViewFriendRequest(socket, data) {
        let fb = Firebase.getInstance();
        fb.getFriendRequest(socket, data.uid);
    }
    static async processRemoveFriend(socket, data) {
        let fb = Firebase.getInstance();
        await fb.friendRemove(data.uid, data.fid);
        if (data.fid in network.clientList) {
            network.clientList[data.fid].emit('friendRemove', null);
        }
        socket.emit('friendRemoveReply', null);
    }
}

module.exports = FriendController