NetworkController = require("./NetworkController.js")
Firebase = require("./FirebaseDataModel.js")

class Friend {
    static async processFriendRequest(socket, data) {
        network = NetworkController.getInstance();
        let fb = Firebase.getInstance();
        await fb.databaseService.ref(`Account/${data.fid}/FriendRequest`).set({
            [data.uid]: data.uid
        });
        if (data.fid in network.clientList) {
            network.clientList[data.fid].emit('receivedFriendRequest', null);
        }
        socket.emit('friendRequestReply', data);
    }

    static async processFriendRequestAccept(socket, data) {
        network = NetworkController.getInstance();
        let fb = Firebase.getInstance();
        let userId = data.uid;
        let targetId = data.fid;
        await fb.databaseService.ref(`Account/${userId}/FriendRequest/${targetId}`).remove();
        let consId = (await this.databaseService.ref(`DMessage`).push({
            [`${userId}_has_new`]: false,
            [`${targetId}_has_new`]: false
        })).key;
        await fb.databaseService.ref(`Account/${userId}/Friend/`).set({
            [targetId]: consId
        })
        await fb.databaseService.ref(`Account/${targetId}/Friend/`).set({
            [userId]: consId
        })
        await fb.databaseService.ref(`DMessage/${consId}/${targetId}_has_new`).set(true);
        await fb.databaseService.ref(`DMessage/${consId}/${userId}_has_new`).set(true);

        if (data.fid in network.clientList) {
            network.clientList[data.fid].emit('friendRequestAcceptReply', data);
        }
        socket.emit('friendRequestAcceptReply', null);
    }

    static async processFriendRequestRefuse(socket, data) {
        network = NetworkController.getInstance();
        let fb = Firebase.getInstance();
        await fb.databaseService.ref(`Account/${data.uid}/FriendRequest/${data.fid}`).remove();
        if (data.fid in network.clientList) {
            network.clientList[data.fid].emit('friendRequestRefuseReply', null);
        }
        socket.emit('friendRequestRefuseReply', null);
    }

    static async processViewFriendList(socket, data) {
        let fb = Firebase.getInstance();
        fb.databaseService.ref(`Account/${data.uid}/Friend`)
            .once('value', async (data) => {
                if (data.val() == null) {
                    console.log('testing 2');
                    return [];
                }
                let res = Object.entries(data.val()).map(([fid, id_cons]) => ({
                    uid: fid,
                    id_cons: id_cons,
                }));
                let promises = []
                for (let item in res) {
                    promises.push(
                        await this.databaseService.ref(`Account/${res[item]['uid']}`)
                            .once('value', (friend) => {
                                let val = friend.val();
                                res[item]['username'] = val['username'];
                                res[item]['birthday'] = val['birthday'];
                                res[item]['description'] = val['description'];
                            })
                    )

                }
                socket.emit('viewFriendReply', res);
            })
    }

    static async processViewFriendRequest(socket, data) {
        let fb = Firebase.getInstance();
        fb.databaseService.ref(`Account/${data.uid}/FriendRequest`)
            .once('value', async (data) => {
                if (data.val() == null) {
                    return [];
                }
                let res = Object.values(data.val()).map((fid) => ({
                    uid: fid
                }));
                let promises = []
                for (let item of res) {
                    promises.push(
                        await this.databaseService.ref(`Account/${item['uid']}`)
                            .once('value', (friend) => {
                                let val = friend.val();
                                item['username'] = val['username'];
                                item['birthday'] = val['birthday'];
                                item['description'] = val['description'];
                            })
                    )
                }
                socket.emit('viewFriendRequestReply', res);
            })
    }

    static async processRemoveFriend(socket, data) {
        let fb = Firebase.getInstance();
        await fb.databaseService.ref(`Account/${data.uid}/Friend/${data.fid}`)
            .once('value', (cons_id) => {
                console.log(cons_id.val());
                this.databaseService.ref(`DMessage/${cons_id.val()}`).remove();
            });
        await fb.databaseService.ref(`Account/${data.uid}/Friend/${data.fid}`).remove();
        await fb.databaseService.ref(`Account/${data.fid}/Friend/${data.uid}`).remove();
        if (data.fid in network.clientList) {
            network.clientList[data.fid].emit('friendRemove', null);
        }
        socket.emit('friendRemoveReply', null);
    }
}

module.exports = Friend