NetworkController = require("./NetworkController.js")
Firebase = require("./FirebaseDataModel.js")

/*
Data format:
{
    uid: string
}
*/

class AdminController {
    static processLockUser(socket, data) {
        network = NetworkController.getInstance();
        if (data.uid in network.clientList) {
            network.clientList[data.uid].emit('lockUser', data);
            for (let key in network.clientList) {
                if (key !== data.uid)
                    network.clientList[key].emit('hideLockUser', data);
            }
        }
        let fb = Firebase.getInstance();
        fb.lockUser(data.uid);
        network.clientList[data.uid].disconnect();
    }
    static processUnlockUser(socket, data) {
        network = NetworkController.getInstance();
        if (data.uid in network.clientList) {
            network.clientList[data.uid].emit('unlockUser', data);
            for (let key in network.clientList) {
                if (key !== data.uid)
                    network.clientList[key].emit('unhideUnlockUser', data);
            }
        }
        let fb = Firebase.getInstance();
        fb.unlockUser(data.uid);
    }
    static processRemoveUser(socket, data) {
        network = NetworkController.getInstance();
        if (data.uid in network.clientList) {
            network.clientList[data.uid].emit('removeUser', data);
            for (let key in network.clientList) {
                if (key !== data.uid)
                    network.clientList[key].emit('hideRemoveUser', data);
            }
        }
        let fb = Firebase.getInstance();
        fb.deleteUser(data.uid);
        network.clientList[data.uid].disconnect();
    }
    static processViewSystemInfo(socket, data) {
        
    }
}

module.exports = AdminController