const { Parser } = require("xml2js");
const serviceAccount = require("../key.json");

class FirebaseDataModel {
    static #instance = null;
    #admin = require("firebase-admin");
    #authService;
    #databaseService;
    static getInstance() {
        if (FirebaseDataModel.#instance === null) {
            new FirebaseDataModel(PORT);
        }
        return FirebaseDataModel.#instance;
    }
    constructor() {
        if (FirebaseDataModel.#instance) {
            throw new Error("illegal instantiation");
        }
        FirebaseDataModel.#instance = this;
        this.#admin.initializeApp({
            credential: this.#admin.credential.cert(serviceAccount),
            databaseURL: "https://virtualchat3d-default-rtdb.asia-southeast1.firebasedatabase.app"
        });
        this.#authService = this.#admin.auth();
        this.#databaseService = this.#admin.database();
    }
    verifyToken(token) {
        this.#authService.verifyToken(token).then((uid) => {
            return uid;
        }).catch((error) => {
            return error;
        })
    }
    createUser(userId) {
        this.#databaseService.ref(`Account/${userId}/username`).set(userId);
        this.#databaseService.ref(`Account/${userId}/description`).set("");
        this.#databaseService.ref(`Account/${userId}/birthday`).set(Date.now());
    }
    lockUser(userId) {
        this.#authService.updateUser(userId, {
            disabled: true
        });
    }
    unlockUser(userId) {
        this.#authService.updateUser(userId, {
            disabled: false
        });
    }
    deleteUser(userId) {
        let frRef = this.#databaseService.ref(`Account/${userId}/Friend`);
        if (frRef != null) {
            for (id in Object.keys(frRef.val())) {
                this.#databaseService.ref(`Account/${id}/Friend/${userId}`).remove();
            }
        }
        this.#authService.deleteUser(userId);
        this.#databaseService.ref(`Account/${userId}`).remove();
    }
    friendRemove(userId, targetId) {
        let cons_id = this.#databaseService.ref(`Account/${userId}/Friend/${targetId}`).val();
        this.#databaseService.ref(`DMessage/${cons_id}`).remove();
        this.#databaseService.ref(`Account/${userId}/Friend/${targetId}`).remove();
        this.#databaseService.ref(`Account/${targetId}/Friend/${userId}`).remove();
    }
    friendRequest(userId, targetId) {
        this.#databaseService.ref(`Account/${targetId}/FriendRequest`).set({
            [userId]: userId
        });
    }
    friendRequestAccept(userId, targetId) {
        this.#databaseService.ref(`Account/${userId}/FriendRequest/${targetId}`).remove();
        let consId = this.#databaseService.ref(`DMessage`).push().key;
        this.#databaseService.ref(`Account/${userId}/Friend/`).set({
            [targetId]: consId
        })
        this.#databaseService.ref(`Account/${targetId}/Friend/`).set({
            [userId]: consId
        })
    }
    friendRequestRefuse(userId, targetId) {
        this.#databaseService.ref(`Account/${userId}/FriendRequest/${targetId}`).remove();
    }
    getUserList(socket) {
        this.#databaseService.ref(`Account`)
            .once('value', (data) => {
                let res = Object.entries(data.val()).map(([uid, { birthday, description, __, _, username }]) => ({
                    uid: uid,
                    birthday: birthday,
                    description: description,
                    username: username
                }));
                console.log(`sent${res}`)
                socket.emit('getUserListReply', res);
            });
    }
    getFriendList(socket, userId) {
        this.#databaseService.ref(`Account/${userId}/friendList`)
            .once('value', (data) => {
                if (data.val() == null) {
                    socket.emit('viewFriendReply', []);
                    return;
                }
                let res = Object.entries(data.val()).map(([fid, id_cons]) => ({
                    uid: fid,
                    id_cons: id_cons,
                }));
                let promises = []
                for (let item in res) {
                    promises.push(
                        this.#databaseService.ref(`Account/${res[item]['uid']}`)
                            .once('value', (friend) => {
                                let val = friend.val();
                                res[item]['username'] = val['username'];
                                res[item]['birthday'] = val['birthday'];
                                res[item]['description'] = val['description'];
                            })
                    )

                }
                console.log('trying....')
                Promise.all(promises).then(() => {
                    socket.emit('viewFriendReply', res);
                });
            })
    }
    getFriendRequest(socket, userId) {
        this.#databaseService.ref(`Account/${userId}/FriendRequest`)
            .once('value', (data) => {
                if (data.val() == null) {
                    socket.emit('viewFriendRequestReply', []);
                    return;
                }
                let res = Object.values(data.val()).map((fid) => ({
                    uid: fid
                }));
                let promises = []
                for (let item of res) {
                    promises.push(
                        this.#databaseService.ref(`Account/${item['uid']}`)
                            .once('value', (friend) => {
                                let val = friend.val();
                                item['username'] = val['username'];
                                item['birthday'] = val['birthday'];
                                item['description'] = val['description'];
                            })
                    )
                }
                Promise.all(promises).then(() => {
                    socket.emit('viewFriendRequestReply', res);
                });
            })
    }
    messageWrite(data) {
        this.#databaseService.ref(`DMessage/${data.id_cons}/${data.fid}_has_new`).set(true);
        this.#databaseService.ref(`DMessage/${data.id_cons}/${data.timestamp}`)
            .set({
                uid: data.uid,
                msg: data.msg
            });
    }
    messageRead(data) {
        this.#databaseService.ref(`DMessage/${data.id_cons}/${data.uid}_has_new`).set(false);
    }
    getMessage(socket, consId) {
        this.#databaseService.ref(`/DMessage/${consId}`)
            .once('value', (data) => {
                const result = Object.entries(data.val())
                    .filter(([_, { uid }]) => uid).map(([timestamp, { msg, uid }]) => ({
                        timestamp: parseInt(timestamp),
                        uid: uid,
                        msg: msg,
                        id_cons: consId
                    }));
                socket.emit('viewMessageReply', result);
                console.log(result);
            })
    }
}

module.exports = FirebaseDataModel