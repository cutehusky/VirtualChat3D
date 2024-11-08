const { Server } = require('socket.io');

const express = require('express');

const { createServer } = require('node:http');

const app = express();
const server = createServer(app);
const io = new Server(server);

io.on('connection', (socket) => {
    console.log('a user connected');
    socket.on('messageClient2Server', (data) => {
        console.log(data);
        socket.emit('messageServer2Client', { Header: '', Value: 'hello from server' })
    });
    socket.on('disconnect', () => {
        console.log('user disconnected');
    });
});

server.listen(3000, () => {
    console.log('server running at http://localhost:3000');
});