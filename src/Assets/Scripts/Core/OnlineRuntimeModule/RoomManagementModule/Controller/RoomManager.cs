using System.Collections.Generic;
using Core.MVC;
using Core.OnlineRuntimeModule.RoomManagementModule.View;

namespace Core.OnlineRuntimeModule.RoomManagementModule.Controller
{
    public class RoomManager: ControllerBase
    {
        private HostRoomView _hostRoomView;
        private JoinRoomView _joinRoomView;

        public ViewBase OpenHostRoomView()
        {
            AppMain.Instance.CloseCurrentView();
            _hostRoomView.Display();
            _hostRoomView.Render(null);
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

        public void DeleteRoom(string roomId)
        {
            
        }
        
        public void LoadRoomsData()
        {
            
        }
        
        public override void OnInit(List<ViewBase> view)
        {
            base.OnInit(view);
            _hostRoomView = view[0] as HostRoomView;
            _joinRoomView = view[1] as JoinRoomView;
        }
    }
}