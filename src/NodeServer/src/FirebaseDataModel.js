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
    constructor(config) {
        if(FirebaseDataModel.#instance) {
            throw new Error("illegal instantiation");
        }
        FirebaseDataModel.#instance = this;
        this.#admin.initializeApp({
            credential: admin.credential.cert(serviceAccount),
            databaseURL: "https://virtualchat3d-default-rtdb.asia-southeast1.firebasedatabase.app"
        });
        this.#authService = getAuth(this.#admin)
    }
    verifyToken(token) {
        this.#authService.verifyToken(token).then((uid) => {
            return uid;
        }).catch((error) => {
            return error
        })
    }
}