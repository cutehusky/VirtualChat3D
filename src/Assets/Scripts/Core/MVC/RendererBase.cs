namespace Core.MVC
{
    /// <summary>
    /// Base class of Render component
    /// Used to render View
    /// </summary>
    public abstract class RendererBase
    {
        public abstract void Render(ViewBase view);
    }
}