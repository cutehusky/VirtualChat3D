using System.Collections.Generic;
using QFramework;

namespace Core.MVC
{
    public abstract class ControllerBase: IController
    {
        public virtual void Update()
        {
            
        }
        
        /// <summary>
        /// Bind input action.
        /// </summary>
        /// <param name="view"></param>
        public virtual void OnInit(List<ViewBase> views)
        {
            foreach (var view in views)
            {
                if (view.footer)
                {
                    view.footer.host.onClick.AddListener(() =>
                    {
                        AppMain.Instance.OpenHostRoomView();
                    });

                    view.footer.users.onClick.AddListener(() =>
                    {
                        AppMain.Instance.OpenUserListView();
                    });

                    view.footer.friend.onClick.AddListener(() =>
                    {
                        AppMain.Instance.OpenFriendListView();
                    });

                    view.footer.join.onClick.AddListener(() =>
                    {
                        AppMain.Instance.OpenJoinRoomView();
                    });
                }

                if (view.header)
                {
                    view.header.avatar.onClick.AddListener(() =>
                    {
                        AppMain.Instance.OpenUserProfileView();
                    });
                    
                    view.header.analytic.onClick.AddListener(() =>
                    {
                        AppMain.Instance.OpenSystemMonitorView();
                    });
                    
                    view.header.modelCustomize.onClick.AddListener(() =>
                    {
                        AppMain.Instance.OpenModelListView();
                    });
                    view.header.chatBot.onClick.AddListener(() =>
                    {
                        AppMain.Instance.OpenChatBotView();
                    });
                }
            }
        }
        
        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}