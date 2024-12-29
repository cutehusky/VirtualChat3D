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
    clientProcess = {};
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
        Firebase.getInstance();
    }

    runEvents(socket) {
        console.log(`client: ${socket.id} connected`);
        this.clientProcess[socket.id] = false;
        socket.on('fetch', (data) => {
            //let uid = Firebase.getInstance().verifyToken(data.token);
            console.log(`fetched uid from ${data.uid}`);

            if (this.clientList[data.uid] && this.clientList[data.uid].id != socket.id)
                this.clientList[data.uid].emit("logout", null);

            for (const uid of Object.keys(this.clientList)) {
                if (this.clientList[uid].id == socket.id) {
                    delete this.clientList[uid];
                    break;
                }
            }

            Firebase.getInstance().adminChecker(socket, data.uid);
            socket.emit('fetchReply', data);
            this.clientList[data.uid] = socket;
        })
        socket.on('clear', (data) => {
            //let uid = Firebase.getInstance().verifyToken(data.token);
            delete this.clientList[data.uid];
        })
        for (const [eventName, callback] of Object.entries(this.#socketEventDict)) {
            socket.on(eventName, (data) => callback(socket, data));
        }
        socket.on('disconnect', () => {
            console.log(`client: ${socket.id} disconnected`);
            for (const uid of Object.keys(this.clientList)) {
                if (this.clientList[uid].id == socket.id) {
                    delete this.clientList[uid];
                    break;
                }
            }
        })
    }

    SubscribeEvent(eventName, callback) {
        this.#socketEventDict[eventName] = (socket, data) => {
            if(this.clientProcess[socket.id] == false) {
                this.clientProcess[socket.id] = true;
                callback(socket, data);
                this.clientProcess[socket.id] = false;
            }
        }
    }
}

module.exports = NetworkController