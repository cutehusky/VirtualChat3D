const { Server } = require('socket.io');
const express = require('express');
const { createServer } = require('node:http');

const app = express();
const server = createServer(app);
const io = new Server(server);
const { initializeApp } = require('firebase-admin/app');


let userId2sockets = {}

function verifyAdmin(token) {
    return;
}

function checkUser(id) {
    return;
}

function checkBlock(id) {
    return;
}

function blockUser(id) {
    
}



io.on('connection', (socket) => {
    socket.on('disconnect', () => {

    });
    socket.on('send-message', (data) => {

    });
    socket.on('receive-user-id', (data) => {
        userId2sockets[data] = socket;
    });
});

server.listen(3000, () => {
    console.log('server running at http://localhost:3000');
});