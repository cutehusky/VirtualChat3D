const NetworkController = require("./NetworkController.js")
const Firebase = require("./FirebaseDataModel.js")

class Message {
    static processSendMessage(socket, data) {
        network = NetworkController.getInstance();
        data['timestamp'] = Date.now();
        if (data.fid in network.clientList) {
            network.clientList[data.fid].emit('receivedMessage', data);
        }
        socket.emit('sendMessageReply', data);
        let fb = Firebase.getInstance();
        fb.databaseService.ref(`DMessage/${data.id_cons}/${data.fid}_has_new`).set(true);
        fb.databaseService.ref(`DMessage/${data.id_cons}/${data.timestamp}`)
            .set({
                uid: data.uid,
                msg: data.msg
            });
    }
    static async processViewMessage(socket, data) {
        let fb = Firebase.getInstance();
        let consId = data.id_cons;
        fb.databaseService.ref(`/DMessage/${consId}`)
            .once('value', (data) => {
                if (data.val() == null) {
                    result = [];
                }
                let result = Object.entries(data.val())
                    .filter(([_, { uid }]) => uid);
                if (result == null) {
                    result = [];
                }
                else {
                    result = result.map(([timestamp, { msg, uid }]) => ({
                        timestamp: parseInt(timestamp),
                        uid: uid,
                        msg: msg,
                        id_cons: consId
                    }));
                }
                socket.emit('viewMessageReply', result);
            })
    }
    static processReadMessage(socket, data) {
        let fb = Firebase.getInstance();
        fb.databaseService.ref(`DMessage/${data.id_cons}/${data.uid}_has_new`).set(false);
    }
}

module.exports = Message