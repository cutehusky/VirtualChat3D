NetworkController = require("./NetworkController.js")
Firebase = require("./FirebaseDataModel.js")

/*
Data format:
{
    uid: string
}
*/

class AdminController {
    static async processLockUser(socket, data) {
        network = NetworkController.getInstance();
        console.log("lock user " + data.uid);
        let fb = Firebase.getInstance();
        await fb.lockUser(data.uid);
        if (data.uid in network.clientList) {
            network.clientList[data.uid].emit('logout', null);
            network.clientList[data.uid].disconnect();
            delete network.clientProcess[network.clientList[data.uid].id];
            delete network.clientList[data.uid];
            /*
            for (let key in network.clientList) {
                if (key != data.uid)
                network.clientList[key].emit('hideLockUser', data);
            }
            */
        }
        socket.emit("lockUserReply", null);
    }
    static async processUnlockUser(socket, data) {
        network = NetworkController.getInstance();
        console.log("unlock user " + data.uid);
        let fb = Firebase.getInstance();
        await fb.unlockUser(data.uid);
        if (data.uid in network.clientList) {
            /*
            for (let key in network.clientList) {
                if (key !== data.uid)
                network.clientList[key].emit('unhideUnlockUser', data);
            }
            */
        }
        socket.emit("unlockUserReply", null);
    }
    static async processRemoveUser(socket, data) {
        network = NetworkController.getInstance();
        console.log("remove user " + data.uid);
        let fb = Firebase.getInstance();
        await fb.deleteUser(data.uid);
        if (data.uid in network.clientList) {
            network.clientList[data.uid].emit('logout', null);
            network.clientList[data.uid].disconnect();
            delete network.clientProcess[network.clientList[data.uid].id];
            delete network.clientList[data.uid];
            /*
            for (let key in network.clientList) {
                if (key !== data.uid)
                network.clientList[key].emit('hideRemoveUser', data);
            }
            */
        }
        socket.emit("removeUserReply", null);
    }
    static async processGetUserList(socket, data) {
        let fb = Firebase.getInstance();
        let res = await fb.getUserList(socket);
        socket.emit('getUserListReply', res);
    }
    static processCreateUser(socket, data) {
        let fb = Firebase.getInstance();
        fb.createUser(data.uid);
    }
    static async processViewSystemInfo(socket, data) {
        let fb = Firebase.getInstance();
        let nw = NetworkController.getInstance();
        let anlt = await fb.viewAnalyticSystemInfo();
        socket.emit('analytic', anlt);
        let sys = fb.viewSystemInfo();
        sys['active_user'] = Object.keys(nw.clientList).length;
        socket.emit('system', sys);
    }
}

module.exports = AdminController