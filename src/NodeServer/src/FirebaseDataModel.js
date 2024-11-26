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
            [Date.now()]: userId
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
    getFriendList(socket, userId) {
        this.#databaseService.ref(`Account/${userId}/FriendList`)
        .once('value', (data) => {
            let res = Object.entries(data.val()).map(([fid, id_cons]) => ({
                uid: fid,
                id_cons: id_cons,
            }));

            promises = []
            for(item in res) {
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
                socket.emit('viewFriendReply', res);
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