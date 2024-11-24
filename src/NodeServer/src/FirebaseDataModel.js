const serviceAccount = require("../key.json");

class FirebaseDataModel {
    static #instance = null;
    #admin = require("firebase-admin");
    #authService;
    #databaseService;
    static getInstance() {
        if(FirebaseDataModel.#instance === null) {
            new FirebaseDataModel(PORT);
        }
        return FirebaseDataModel.#instance;
    }
    constructor() {
        if(FirebaseDataModel.#instance) {
            throw new Error("illegal instantiation");
        }
        FirebaseDataModel.#instance = this;
        this.#admin.initializeApp({
            credential: this.#admin.credential.cert(serviceAccount),
            databaseURL: "https://virtualchat3d-default-rtdb.asia-southeast1.firebasedatabase.app"
        });
        this.#authService = this.#admin.auth()
        this.#databaseService = this.#admin.database()
    }
    verifyToken(token) {
        this.#authService.verifyToken(token).then((uid) => {
            return uid;
        }).catch((error) => {
            return error;
        })
    }
    lockUser(userId) {
        
    }
    unlockUser(userId) {

    }
    deleteUser(userId) {

    }
    friendRemove(userId, targetId) {

    }
    friendRequest(userId, targetId) {

    }
    friendRequestAccept(userId, targetId) {

    }
    friendRequestRefuse(userId, targetId) {

    }
    getFriendList(userId) {

    }
    messageWrite(userId, targetId, consId,  messageData) {
        let msgRef = this.#databaseService.ref(`DMessage/${consId}`)
        msgRef.push({
            from: userId,
            to: targetId,
            msg:  messageData  
        })
    }
    getMessage(messageId) {

    }
}

module.exports = FirebaseDataModel