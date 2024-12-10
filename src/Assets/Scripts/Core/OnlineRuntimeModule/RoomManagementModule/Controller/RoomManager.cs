using System.Collections.Generic;
using Core.MVC;
using Core.OnlineRuntimeModule.RoomManagementModule.Model;
using Core.OnlineRuntimeModule.RoomManagementModule.View;
using Core.UserAccountModule.Model;
using Firebase;
using Firebase.Database;
using QFramework;

namespace Core.OnlineRuntimeModule.RoomManagementModule.Controller
{
    public class RoomManager : ControllerBase
    {
        private HostRoomView _hostRoomView;
        private JoinRoomView _joinRoomView;

        public ViewBase OpenHostRoomView()
        {
            AppMain.Instance.CloseCurrentView();
            _hostRoomView.Display();
            _hostRoomView.Render(this.GetModel<RoomDataModel>());
            return _hostRoomView;
        }

        public ViewBase OpenJoinRoomView()
        {
            AppMain.Instance.CloseCurrentView();
            _joinRoomView.Display();
            _joinRoomView.Render(null);
            return _joinRoomView;
        }

        public void CreateRoom()
        {

        }

        public void EditRoomAccessType(string roomId)
        {

        }

        public void HostRoom(RoomData roomData)
        {
            this.GetModel<RoomDataModel>().CurrentHostRoomData = roomData;
        }

        public void DeleteRoom(string roomId)
        {
            this.GetModel<RoomDataModel>().DeleteRoom(this.GetModel<UserProfileDataModel>().UserProfileData.UserId, roomId);
        }

        public void LoadRoomsData()
        {
            this.GetModel<RoomDataModel>().FetchRoomsList(this.GetModel<UserProfileDataModel>().UserProfileData.UserId);
        }

        public override void OnInit(List<ViewBase> view)
        {
            base.OnInit(view);
            _hostRoomView = view[0] as HostRoomView;
            _joinRoomView = view[1] as JoinRoomView;
            _hostRoomView.OnHostRoom += HostRoom;
            _hostRoomView.OnDeleteRoom += DeleteRoom;
            _hostRoomView.OnEditRoom += (roomId) =>
            {

            };
        }
    }
}