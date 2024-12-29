NetworkController = require("./NetworkController.js")
fb = require("./FirebaseDataModel.js")
message = require("./Message.js")
friend = require("./Friend.js")
usercontrol = require("./UserControl.js")
info = require("./SystemInfoAnalytics.js")

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
network.SubscribeEvent('processLockUser', usercontrol.processLockUser);
network.SubscribeEvent('processUnlockUser', usercontrol.processUnlockUser);
network.SubscribeEvent('processRemoveUser', usercontrol.processRemoveUser);
network.SubscribeEvent('getUserList', usercontrol.processGetUserList);
network.SubscribeEvent('viewSystemInfo', info.processViewSystemInfo);
network.SubscribeEvent('createUser', usercontrol.processCreateUser);
/* new event need to be implemented
'sendFriendRequestReply'
'friendRequestAcceptReply'
'friendRequestRefuseReply'
'processRemoveFriendReply'
Note:
'processViewFriendList': only return friends
'processViewFriendRequestList': only return who send friend request to the current user
*/