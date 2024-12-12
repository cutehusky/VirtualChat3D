NetworkController = require("./NetworkController.js")
Firebase = require("./FirebaseDataModel.js")

class MessageController {
    static processSendMessage(socket, data) {
        network = NetworkController.getInstance();
        data['timestamp'] = Date.now();
        if (data.fid in network.clientList) {
            network.clientList[data.fid].emit('receivedMessage', data);
        }
        socket.emit('sendMessageReply', data);
        let fb = Firebase.getInstance();
        fb.messageWrite(data);
    }
    static async processViewMessage(socket, data) {
        let fb = Firebase.getInstance();
        let result = await fb.getMessage(data.id_cons);
        socket.emit('viewMessageReply', result);
    }
    static processReadMessage(socket, data) {
        let fb = Firebase.getInstance();
        fb.messageRead(data);
    }
}

module.exports = MessageController