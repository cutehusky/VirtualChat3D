NetworkController = require("./NetworkController.js")
Firebase = require("./FirebaseDataModel.js")

class UserControl {
    static async processGetUserList(socket, data) {
        let fb = Firebase.getInstance();
        fb.databaseService.ref(`Account`)
            .once('value', async (data) => {
                let res = Object.entries(data.val()).map(([uid, { birthday, description, __, _, username }]) => ({
                    uid: uid,
                    birthday: birthday,
                    description: description,
                    username: username
                }));
                for (let i in res) {
                    await this.authService.getUser(res[i]['uid']).then((user) => {
                        res[i]['status'] = !user.disabled;
                    });
                }
                console.log(`sent${res}`)
                socket.emit('getUserListReply', res);
            });
    }
    
    static async processLockUser(socket, data) {
        network = NetworkController.getInstance();
        console.log("lock user " + data.uid);

        let fb = Firebase.getInstance();
        await fb.authService.updateUser(data.uid, {
            disabled: true
        })

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
        await fb.authService.updateUser(data.uid, {
            disabled: false
        });
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
        let userId = data.uid
        let fb = Firebase.getInstance();
        let frRef = fb.databaseService.ref(`Account/${userId}/Friend`);
        const frSnapshot = await frRef.get();
        if (frSnapshot.exists()) {
            const friendIds = Object.keys(frSnapshot.val());
            for (const id of friendIds) {
                await this.databaseService.ref(`Account/${id}/Friend/${userId}`).remove();
                await this.databaseService.ref(`DMessages/${friendIds[id]}`).remove();
            }
        }
        await fb.authService.deleteUser(userId);
        await fb.databaseService.ref(`Account/${userId}`).remove();
        if (userId in network.clientList) {
            network.clientList[userId].emit('logout', null);
            network.clientList[userId].disconnect();
            delete network.clientProcess[network.clientList[userId].id];
            delete network.clientList[userId];
            /*
            for (let key in network.clientList) {
                if (key !== data.uid)
                network.clientList[key].emit('hideRemoveUser', data);
            }
            */
        }
        socket.emit("removeUserReply", null);
    }
    static processCreateUser(socket, data) {
        let fb = Firebase.getInstance();
        fb.databaseService.ref(`Account/${data.uid}/username`).set(data.uid);
        fb.databaseService.ref(`Account/${data.uid}/description`).set("");
        fb.databaseService.ref(`Account/${data.uid}/birthday`).set(Date.now());
    }
}

module.exports = UserControl