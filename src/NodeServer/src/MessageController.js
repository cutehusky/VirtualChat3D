NetworkController = require("./NetworkController.js")
Firebase = require("./FirebaseDataModel.js")

class MessageController {
    static processSendMessage(socket, data) {
        network = NetworkController.getInstance();
        if(data.fid in network.clientList) {
            network.clientList[data.fid].emit('receivedMessage', data);
        }
        fb = Firebase.getInstance();
        fb.messageWrite(data.uid, data.fid, data.id_cons, data.msg);
    }
    static processViewMessage(socket, data) {
        
    }
}

module.exports = MessageController