const serviceAccount = require("../key.json");
const os = require('os');

class FirebaseDataModel {
    static #instance = null;
    #admin = require("firebase-admin");
    authService;
    databaseService;
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
        this.authService = this.#admin.auth();
        this.databaseService = this.#admin.database();
        this.PushIP();
    }

    verifyToken(token) {
        this.authService.verifyToken(token).then((uid) => {
            return uid;
        }).catch((error) => {
            return error;
        })
    }

    adminChecker(socket, uid) {
        this.databaseService.ref(`Admin/${uid}`).once('value', (data) => {
            socket.emit('fetchAdminReply', {
                res: (data.val() != null)
            });
        });
    }

    PushIP() {
        const interfaces = os.networkInterfaces();
        for (const interfaceName in interfaces) {
            if (interfaceName.indexOf("VMware Network") != -1) {
                continue;
            }
            for (const net of interfaces[interfaceName]) {
                if (net.family === 'IPv4' && !net.internal) {
                    this.databaseService.ref(`IP`).set(net.address);
                }
            }
        }
    }
}

module.exports = FirebaseDataModel