NetworkController = require("./NetworkController.js")
fb = require("./FirebaseDataModel.js")
message = require("./MessageController.js")

network = NetworkController.getInstance()
network.SubscribeEvent('sendMessage', message.processSendMessage);
network.SubscribeEvent('viewMessage', message.processViewMessage);
