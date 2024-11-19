using System.Collections.Generic;
using Core.CharacterCustomizationModule.View;
using Core.MVC;

namespace Core.CharacterCustomizationModule.Controller
{
    public class CharacterCustomizationController: ControllerBase
    {
        private ModelListView _modelListView;
        public void LoadModelFromExternal()
        {
            
        }

        public void SelectChatRoomModel(string id)
        {
            
        }
        
        public void SelectChatBotModel(string id)
        {
            
        }
        
        public override void OnInit(List<ViewBase> view)
        {
            _modelListView = view[0] as ModelListView;
        }

        public ViewBase OpenModelListView()
        {
            // CODE HERE
            return null;
        }
    }
}