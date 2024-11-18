using System.Collections.Generic;
using QFramework;

namespace Core.MVC
{
    public abstract class ControllerBase: IController
    {
        /// <summary>
        /// Bind input action.
        /// </summary>
        /// <param name="view"></param>
        public virtual void OnInit(List<ViewBase> view) {}
        
        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}