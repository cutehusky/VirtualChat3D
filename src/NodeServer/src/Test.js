const { Server } = require('socket.io');
const crypto = require('crypto');

const express = require('express');
const { createServer } = require('node:http');
const NodeRSA = require('node-rsa');
const fs = require('fs');
const xml2js = require('xml2js');
const forge = require('node-forge');

const app = express();
const server = createServer(app);
const io = new Server(server);

const serverKey = new NodeRSA(fs.readFileSync('server_private.key'), 'pkcs8', {
    encryptionScheme: {
        scheme: 'pkcs1_oaep',
        hash: 'sha1',
    }
});

// Encrypt the plaintext
function encryptAES128ECB(plaintext, key) {
    const cipher = crypto.createCipheriv('aes-128-ecb', key, null);
    cipher.setAutoPadding(true);
    const encrypted = Buffer.concat([cipher.update(plaintext, 'utf8'), cipher.final()]);
    return encrypted;
}

// Decrypt the ciphertext
function decryptAES128ECB(ciphertext, key) {
    const decipher = crypto.createDecipheriv('aes-128-ecb', key, null);
    decipher.setAutoPadding(true);
    const decrypted = Buffer.concat([decipher.update(ciphertext), decipher.final()]);
    return decrypted.toString('utf8');
}

clientAESKey = {}

io.on('connection', (socket) => {
    console.log(`- user ${socket.id} connected`);

    socket.on('clientAESKey', (data) => {
        console.log(`- getting user ${socket.id} AES key`);
        clientAESKey[socket.id] = serverKey.decrypt(data);
        let re = encryptAES128ECB('hello from server', clientAESKey[socket.id]).toString('base64');
        socket.emit('clientAESKeyRep', {
            Header: 'AES', Value: re
        });
    });

    socket.on('messageClient2ServerAES', (data) => {
        console.log(`- get data from user ${socket.id}: ${data}`);
        console.log(`- decrypt data from user ${socket.id}: ${decryptAES128ECB(Buffer.from(data, 'base64'), clientAESKey[socket.id])}`);

        let re = encryptAES128ECB('hello from server', clientAESKey[socket.id]).toString('base64');
        socket.emit('messageServer2ClientAES', {
            Header: '', Value: re
        });
    });

    socket.on('disconnect', () => {
        console.log(`- user ${socket.id} disconnected`);
        if (clientAESKey.hasOwnProperty(socket.id))
            delete clientAESKey[socket.id];
    });
});

server.listen(3000, () => {
    console.log('server running at http://localhost:3000');
});