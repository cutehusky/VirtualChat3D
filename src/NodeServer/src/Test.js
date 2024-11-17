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

function getRSAKeyFromXML(xmlPublicKey, id, callback) {
    // Parse the XML to extract Modulus and Exponent
    xml2js.parseString(xmlPublicKey, (err, result) => {
        if (err) {
            console.error('Error parsing XML:', err);
            return;
        }

        // Extract the Modulus and Exponent from the parsed XML
        const modulusBase64 = result.RSAKeyValue.Modulus[0];
        const exponentBase64 = result.RSAKeyValue.Exponent[0];

        // Decode base64 values to binary
        const modulus = new forge.jsbn.BigInteger(forge.util.bytesToHex(forge.util.decode64(modulusBase64)), 16);
        const exponent = new forge.jsbn.BigInteger(forge.util.bytesToHex(forge.util.decode64(exponentBase64)), 16);

        // Create a new public key object using forge
        const publicKey = forge.pki.setRsaPublicKey(modulus, exponent);

        // Convert the public key to PEM format in PKCS#8
        const pem = forge.pki.publicKeyToPem(publicKey);

        clientRSAPublicKey[id] = new NodeRSA(pem, {
            encryptionScheme: {
                scheme: 'pkcs1_oaep',
                hash: 'sha1',
            }
        });
        if (callback)
            callback();
    });
}

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
clientRSAPublicKey = {}

io.on('connection', (socket) => {
    console.log(`- user ${socket.id} connected`);
    socket.on('clientRSAPublicKey', (data) => {
        console.log(`- getting user ${socket.id} RSA public key`);
        getRSAKeyFromXML(data, socket.id, () => {
            let re = clientRSAPublicKey[socket.id].encrypt('hello from server', 'base64');
            socket.emit('clientRSAPublicKeyRep', {
                Header: 'RSA', Value: re
            });
        });
    });

    socket.on('clientAESKey', (data) => {
        console.log(`- getting user ${socket.id} AES key`);
        clientAESKey[socket.id] = serverKey.decrypt(data);
        let re = encryptAES128ECB('hello from server', clientAESKey[socket.id]).toString('base64');
        socket.emit('clientAESKeyRep', {
            Header: 'AES', Value: re
        });
    });

    socket.on('messageClient2ServerRSA', (data) => {
        console.log(`- get data from user ${socket.id}: ${data}`);
        console.log(`- decrypt data from user ${socket.id}: ${serverKey.decrypt(data, 'utf8')}`);

        let re = clientRSAPublicKey[socket.id].encrypt('hello from server', 'base64');
        socket.emit('messageServer2ClientRSA', {
            Header: '', Value: re
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
        if (clientRSAPublicKey.hasOwnProperty(socket.id))
            delete clientRSAPublicKey[socket.id];
        if (clientAESKey.hasOwnProperty(socket.id))
            delete clientAESKey[socket.id];
    });
});

server.listen(3000, () => {
    console.log('server running at http://localhost:3000');
});