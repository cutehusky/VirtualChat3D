using System.Collections.Generic;
using Core.MVC;
using Core.OnlineRuntimeModule.EnvironmentCustomize.Model;
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
            LoadRoomsData();
            this.GetModel<RoomDataModel>().CurrentHostRoomData = null;
            _hostRoomView.Display();
            _hostRoomView.currentActiveRoom = "";
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
            this.GetModel<RoomDataModel>().CreateRoom(
                this.GetModel<UserProfileDataModel>().UserProfileData.UserId,
                false, () =>
                {
                    _hostRoomView.RefreshList();
                });
        }

        public void EditRoomAccessType(string roomId)
        {

        }

        public void HostRoom(RoomData roomData)
        {
            if ( this.GetModel<RoomDataModel>().CurrentHostRoomData == null ||
                 roomData.RoomId != this.GetModel<RoomDataModel>().CurrentHostRoomData.RoomId)
                AppMain.Instance.EnvironmentObjectManager.LoadRoomEnvironmentData(roomData.RoomId,
                    () =>
                    {
                        this.GetModel<RoomDataModel>().CurrentHostRoomData = roomData;
                        _hostRoomView.currentActiveRoom = roomData.RoomId;
                        _hostRoomView.RefreshList();
                    });
            else
            {
                this.GetModel<RoomDataModel>().CurrentHostRoomData = null;
                _hostRoomView.currentActiveRoom = "";
                _hostRoomView.RefreshList(); 
            }
        }

        public void DeleteRoom(string roomId)
        {
            this.GetModel<RoomDataModel>().DeleteRoom(
                this.GetModel<UserProfileDataModel>().UserProfileData.UserId, roomId,
                () =>
                {
                    _hostRoomView.RefreshList();
                });
        }

        public void LoadRoomsData()
        {
            this.GetModel<RoomDataModel>().FetchRoomsList(
                this.GetModel<UserProfileDataModel>().UserProfileData.UserId, () =>
                {
                    _hostRoomView.RefreshList();
                });
        }

        public override void OnInit(List<ViewBase> view)
        {
            base.OnInit(view);
            _hostRoomView = view[0] as HostRoomView;
            _joinRoomView = view[1] as JoinRoomView;
            _hostRoomView.OnHostRoom += HostRoom;
            _hostRoomView.OnDeleteRoom += DeleteRoom;
            _hostRoomView.createRoom.onClick.AddListener(CreateRoom);
            _hostRoomView.OnEditRoom += (roomId) =>
            {
                AppMain.Instance.OpenEnvironmentEditView(roomId);
            };
        }
    }
}