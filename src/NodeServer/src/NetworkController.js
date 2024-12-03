const { Server } = require('socket.io');
const express = require('express');
const { createServer } = require('node:http');
Firebase = require("./FirebaseDataModel.js")

PORT = 3000

class NetworkController {
    static #instance = null;
    static getInstance() {
        if (NetworkController.#instance === null) {
            new NetworkController(PORT);
        }
        return NetworkController.#instance;
    }

    #io;
    #app;
    #server;
    #socketEventDict = {};
    clientList = {};
    constructor(port) {
        if (NetworkController.#instance) {
            throw new Error("illegal instantiation");
        }
        NetworkController.#instance = this;

        this.#app = express();
        this.#server = createServer(this.#app);
        this.#io = new Server(this.#server);
        this.#io.on('connection', (socket) => this.runEvents(socket));
        this.#server.listen(port, () => {
            console.log(`Server is up and running on http://localhost:${port}`);
        });
    }

    runEvents(socket) {
        console.log(`client: ${socket.id} connected`);
        socket.on('fetch', (data) => {
            let uid = Firebase.getInstance().verifyToken(data.token);
            this.clientList[uid] = socket;
            Firebase.getInstance().adminChecker(socket, uid);  
        })
        for (const [eventName, callback] of Object.entries(this.#socketEventDict)) {
            socket.on(eventName, (data) => callback(socket, data));
        }
        socket.on('disconnect', () => {
            console.log(`client: ${socket.id} disconnected`);
        })
    }

    SubscribeEvent(eventName, callback) {
        this.#socketEventDict[eventName] = callback;
    }
}

module.exports = NetworkController