NetworkController = require("./NetworkController.js")
fb = require("./FirebaseDataModel.js")
message = require("./MessageController.js")
friend = require("./FriendController.js")
admin = require("./AdminController.js")

network = NetworkController.getInstance()
network.SubscribeEvent('sendMessage', message.processSendMessage);
network.SubscribeEvent('viewMessage', message.processViewMessage);
network.SubscribeEvent('readMessage', message.processReadMessage);
network.SubscribeEvent('sendFriendRequest', friend.processFriendRequest);
network.SubscribeEvent('friendRequestAccept', friend.processFriendRequestAccept);
network.SubscribeEvent('friendRequestRefuse', friend.processFriendRequestRefuse);
network.SubscribeEvent('processViewFriendList', friend.processViewFriendList);
network.SubscribeEvent('processViewFriendRequestList', friend.processViewFriendRequest);
network.SubscribeEvent('processRemoveFriend', friend.processRemoveFriend);
network.SubscribeEvent('processLockUser', admin.processLockUser);
network.SubscribeEvent('processUnlockUser', admin.processUnlockUser);
network.SubscribeEvent('processRemoveUser', admin.processRemoveUser);
network.SubscribeEvent('getUserList', admin.processGetUserList);
network.SubscribeEvent('createUser', admin.processCreateUser);
/* new event need to be implemented
'viewFriendReply'
'viewFriendRequestReply'
'sendFriendRequestReply'
'friendRequestAcceptReply'
'friendRequestRefuseReply'
'processRemoveFriendReply'
Note:
'processViewFriendList': only return friends
'processViewFriendRequestList': only return who send friend request to the current user
*/