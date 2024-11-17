using QFramework;

namespace Core.MVC
{
    public abstract class ControllerBase: IController
    {
        /// <summary>
        /// Bind input action.
        /// </summary>
        /// <param name="view"></param>
        public abstract void OnInit(ViewBase view);
        
        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}