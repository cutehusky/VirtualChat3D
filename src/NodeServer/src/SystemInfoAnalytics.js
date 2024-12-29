const { google } = require('googleapis');
const NetworkController = require("./NetworkController.js")
const os = require('os');

class SystemInfoAnalytics {
    static async viewAnalyticSystemInfo() {
        const auth = new google.auth.GoogleAuth({
            keyFile: './key.json',
            scopes: ['https://www.googleapis.com/auth/analytics.readonly'],
        });

        const analyticsData = google.analyticsdata({
            version: 'v1beta',
            auth,
        });
        const propertyId = 'properties/462826853';
        try {
            const response = await analyticsData.properties
                .runReport({
                    property: propertyId,
                    requestBody: {
                        dimensions: [{ name: 'country' }],
                        metrics: [{ name: 'activeUsers' }],
                        dateRanges: [{ startDate: '30daysAgo', endDate: 'today' }],
                    },
                });
            const formattedData = response.data.rows.map(row => ({
                country: row.dimensionValues[0].value,
                activeUsers: parseInt(row.metricValues[0].value, 10),
            }));

            console.log('Formatted User Insights:', formattedData);
            return formattedData;
        } catch (error) {
            console.error('Error fetching user metrics:', error);
            throw error;
        }
    }
    static viewSystemInfo() {
        return ({
            cpu: os.cpus()[0]['model'],
            cpu_speed: os.cpus()[0]['speed'],
            ram: os.totalmem()
        });
    }

    static async processViewSystemInfo(socket, data) {
        let nw = NetworkController.getInstance();
        let anlt = SystemInfoAnalytics.viewAnalyticSystemInfo();
        socket.emit('analytic', anlt);

        let sys = SystemInfoAnalytics.viewSystemInfo();
        sys['active_user'] = Object.keys(nw.clientList).length;
        socket.emit('system', sys);
    }
}

module.exports = SystemInfoAnalytics