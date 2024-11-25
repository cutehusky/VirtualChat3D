NetworkController = require("./NetworkController.js")
Firebase = require("./FirebaseDataModel.js")

class MessageController {
    static processSendMessage(socket, data) {
        network = NetworkController.getInstance();
        data['timestamp'] = Date.now(); 
        if(data.fid in network.clientList) {
            socket.emit('sendMessageReply', data);
            network.clientList[data.fid].emit('receivedMessage', data);
        }
        let fb = Firebase.getInstance();
        fb.messageWrite(data);
    }
    static processViewMessage(socket, data) {
        let fb = Firebase.getInstance();
        fb.getMessage(socket, data.id_cons);
    }
    static processReadMessage(socket, data) {
        let fb = Firebase.getInstance();
        fb.messageRead(data);
    }
}

module.exports = MessageController