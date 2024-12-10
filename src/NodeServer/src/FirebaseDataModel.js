const { Parser } = require("xml2js");
const { google } = require('googleapis');
const serviceAccount = require("../key.json");
const os = require('os');

class FirebaseDataModel {
    static #instance = null;
    #admin = require("firebase-admin");
    #authService;
    #databaseService;
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
        this.#authService = this.#admin.auth();
        this.#databaseService = this.#admin.database();
    }
    verifyToken(token) {
        this.#authService.verifyToken(token).then((uid) => {
            return uid;
        }).catch((error) => {
            return error;
        })
    }

    viewAnalyticSystemInfo() {
        const auth = new google.auth.GoogleAuth({
            keyFile: './virtualchat3d-firebase-adminsdk-udkda-0810e8cd7a.json',
            scopes: ['https://www.googleapis.com/auth/analytics.readonly'],
        });

        const analyticsData = google.analyticsdata({
            version: 'v1beta',
            auth,
        });

        const propertyId = 'properties/462826853';

        return analyticsData.properties
            .runReport({
                property: propertyId,
                requestBody: {
                    dimensions: [{ name: 'country' }],
                    metrics: [{ name: 'activeUsers' }],
                    dateRanges: [{ startDate: '30daysAgo', endDate: 'today' }],
                },
            })
            .then((response) => {
                const formattedData = response.data.rows.map(row => ({
                    country: row.dimensionValues[0].value,
                    activeUsers: parseInt(row.metricValues[0].value, 10),
                }));

                console.log('Formatted User Insights:', formattedData);
                return formattedData;
            })
            .catch((error) => {
                console.error('Error fetching user metrics:', error);
                throw error;
            });
    }

    viewSystemInfo() {
        return ({
            cpu: os.cpus()[0]['model'],
            cpu_speed: os.cpus()[0]['speed'],
            ram: os.totalmem()
        });
    }

    createUser(userId) {
        this.#databaseService.ref(`Account/${userId}/username`).set(userId);
        this.#databaseService.ref(`Account/${userId}/description`).set("");
        this.#databaseService.ref(`Account/${userId}/birthday`).set(Date.now());
    }

    async lockUser(userId) {
        await this.#authService.updateUser(userId, {
            disabled: true
        });
    }
    async unlockUser(userId) {
        await this.#authService.updateUser(userId, {
            disabled: false
        });
    }
    async deleteUser(userId) {
        let frRef = this.#databaseService.ref(`Account/${userId}/Friend`);
        const frSnapshot = await frRef.get();
        if (frSnapshot.exists()) {
            const friendIds = Object.keys(frSnapshot.val());
            for (const id of friendIds) {
                await this.#databaseService.ref(`Account/${id}/Friend/${userId}`).remove();
                await this.#databaseService.ref(`DMessages/${friendIds[id]}`).remove();
            }
        }
        await this.#authService.deleteUser(userId);
        await this.#databaseService.ref(`Account/${userId}`).remove();
    }
    async friendRemove(userId, targetId) {
        await this.#databaseService.ref(`Account/${userId}/Friend/${targetId}`)
            .once('value', (cons_id) => {
                console.log(cons_id.val());
                this.#databaseService.ref(`DMessage/${cons_id.val()}`).remove();
            });
        await this.#databaseService.ref(`Account/${userId}/Friend/${targetId}`).remove();
        await this.#databaseService.ref(`Account/${targetId}/Friend/${userId}`).remove();
    }
    async friendRequest(userId, targetId) {
        await this.#databaseService.ref(`Account/${targetId}/FriendRequest`).set({
            [userId]: userId
        });
    }
    async friendRequestAccept(userId, targetId) {
        await this.#databaseService.ref(`Account/${userId}/FriendRequest/${targetId}`).remove();
        let consId = (await this.#databaseService.ref(`DMessage`).push({
            [`${userId}_has_new`]: false,
            [`${targetId}_has_new`]: false
        })).key;
        await this.#databaseService.ref(`Account/${userId}/Friend/`).set({
            [targetId]: consId
        })
        await this.#databaseService.ref(`Account/${targetId}/Friend/`).set({
            [userId]: consId
        })
        await this.#databaseService.ref(`DMessage/${consId}/${targetId}_has_new`).set(true);
        await this.#databaseService.ref(`DMessage/${consId}/${userId}_has_new`).set(true);
    }
    async friendRequestRefuse(userId, targetId) {
        await this.#databaseService.ref(`Account/${userId}/FriendRequest/${targetId}`).remove();
    }
    getUserList(socket) {
        this.#databaseService.ref(`Account`)
            .once('value', async (data) => {
                let res = Object.entries(data.val()).map(([uid, { birthday, description, __, _, username }]) => ({
                    uid: uid,
                    birthday: birthday,
                    description: description,
                    username: username
                }));
                for (let i in res) {
                    await this.#authService.getUser(res[i]['uid']).then((user) => {
                        res[i]['status'] = !user.disabled;
                    });
                }
                console.log(`sent${res}`)
                socket.emit('getUserListReply', res);
            });
    }
    getFriendList(socket, userId) {
        this.#databaseService.ref(`Account/${userId}/Friend`)
            .once('value', (data) => {
                if (data.val() == null) {
                    console.log('testing 2');
                    socket.emit('viewFriendReply', []);
                    return;
                }
                let res = Object.entries(data.val()).map(([fid, id_cons]) => ({
                    uid: fid,
                    id_cons: id_cons,
                }));
                let promises = []
                for (let item in res) {
                    promises.push(
                        this.#databaseService.ref(`Account/${res[item]['uid']}`)
                            .once('value', (friend) => {
                                let val = friend.val();
                                res[item]['username'] = val['username'];
                                res[item]['birthday'] = val['birthday'];
                                res[item]['description'] = val['description'];
                            })
                    )

                }
                Promise.all(promises).then(() => {
                    socket.emit('viewFriendReply', res);
                });
            })
    }
    getFriendRequest(socket, userId) {
        this.#databaseService.ref(`Account/${userId}/FriendRequest`)
            .once('value', (data) => {
                if (data.val() == null) {
                    socket.emit('viewFriendRequestReply', []);
                    return;
                }
                let res = Object.values(data.val()).map((fid) => ({
                    uid: fid
                }));
                let promises = []
                for (let item of res) {
                    promises.push(
                        this.#databaseService.ref(`Account/${item['uid']}`)
                            .once('value', (friend) => {
                                let val = friend.val();
                                item['username'] = val['username'];
                                item['birthday'] = val['birthday'];
                                item['description'] = val['description'];
                            })
                    )
                }
                Promise.all(promises).then(() => {
                    socket.emit('viewFriendRequestReply', res);
                });
            })
    }
    messageWrite(data) {
        this.#databaseService.ref(`DMessage/${data.id_cons}/${data.fid}_has_new`).set(true);
        this.#databaseService.ref(`DMessage/${data.id_cons}/${data.timestamp}`)
            .set({
                uid: data.uid,
                msg: data.msg
            });
    }
    messageRead(data) {
        this.#databaseService.ref(`DMessage/${data.id_cons}/${data.uid}_has_new`).set(false);
    }
    getMessage(socket, consId) {
        this.#databaseService.ref(`/DMessage/${consId}`)
            .once('value', (data) => {
                if (data.val() == null) {
                    result = [];
                }
                let result = Object.entries(data.val())
                    .filter(([_, { uid }]) => uid);
                if (result == null) {
                    result = [];
                }
                else {
                    result = result.map(([timestamp, { msg, uid }]) => ({
                        timestamp: parseInt(timestamp),
                        uid: uid,
                        msg: msg,
                        id_cons: consId
                    }));
                }
                socket.emit('viewMessageReply', result);
                console.log(result);
            })
    }
    adminChecker(socket, uid) {
        this.#databaseService.ref(`Admin/${uid}`).once('value', (data) => {
            socket.emit('fetchAdminReply', {
                res: (data.val() != null)
            });
        });
    }
}

module.exports = FirebaseDataModel