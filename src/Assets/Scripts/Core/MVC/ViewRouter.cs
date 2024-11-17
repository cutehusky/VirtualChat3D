
using Core.UserAccountModule.View;
using UnityEngine.SceneManagement;

namespace Core.MVC
{
    public class ViewRouter: MonoSingletonControllerBase
    {
        private RootViewBase _rootView;
        private ViewBase _currentView;
        public override void OnInit()
        {
            SceneManager.sceneLoaded += GetReferenceOfViewOnScene;
        }

        private void GetReferenceOfViewOnScene(Scene scene, LoadSceneMode mode)
        {
            _rootView = FindFirstObjectByType(typeof(RootViewBase)) as RootViewBase;
            // init controller and bind input action here
        }

        public void OpenSignUpView()
        {
           
        }
        
        public void OpenUserProfileView()
        {
           
        }

        public void OpenLoginView()
        {
            
        }
    }
}