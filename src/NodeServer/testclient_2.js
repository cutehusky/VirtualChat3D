const io = require('socket.io-client');

const socket = io('http://localhost:3000');

socket.on('connect', () => {
  console.log('Connected to the server');
  socket.emit('fetch', {uid: '002'});
});

socket.on('disconnect', () => {
  console.log('Disconnected from the server');
});

socket.on('receivedMessage', (data) => {
  console.log(data)
})