NetworkController = require("./NetworkController.js")
fb = require("./FirebaseDataModel.js")
message = require("./MessageController.js")
friend = require("./FriendController.js")

network = NetworkController.getInstance()
network.SubscribeEvent('sendMessage', message.processSendMessage);
network.SubscribeEvent('viewMessage', message.processViewMessage);
network.SubscribeEvent('readMessage', message.processReadMessage);
network.SubscribeEvent('sendFriendRequest', friend.processFriendRequest);
network.SubscribeEvent('friendRequestAccept', friend.processFriendRequestAccept);
network.SubscribeEvent('friendRequestRefuse', friend.processFriendRequestRefuse);
network.SubscribeEvent('processViewFriendList', friend.processViewFriendList);
network.SubscribeEvent('processRemoveFriend', friend.processRemoveFriend);